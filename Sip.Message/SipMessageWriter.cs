using System;
using System.Net;

namespace Sip.Message
{
	public enum SubscriptionStates
	{
		Active,
		Pending,
		Terminated,
	}

	public partial class SipMessageWriter
		: ByteArrayWriter
	{
		#region struct Range {...}

		protected struct Range
		{
			public Range(int offset, int length)
			{
				Offset = offset;
				Length = length;
			}

			public Range(int offset, ByteArrayPart part)
			{
				Offset = offset;
				Length = part.Length;
			}

			public readonly int Offset;
			public readonly int Length;

			public ByteArrayPart ToByteArrayPart(byte[] bytes, int offset)
			{
				if (Length == 0)
					return new ByteArrayPart();

				return new ByteArrayPart()
				{
					Bytes = bytes,
					Begin = offset + Offset,
					End = offset + Offset + Length,
				};
			}
		}

		#endregion

		protected Range callId;
		protected Range fromAddrspec;
		protected Range fromTag;
		protected Range toAddrspec;
		protected Range toTag;
		protected Range epid;

		public SipMessageWriter()
			: base(128, 2048)
		{
			InitializeProperties();
		}

		public SipMessageWriter(int size)
			: base(128, size)
		{
			InitializeProperties();
		}

		#region Properties

		public void InitializeProperties()
		{
			Method = Methods.None;
			StatusCode = 0;
			CSeq = 0;
			Expires = int.MinValue;

			callId = new Range();
			fromAddrspec = new Range();
			fromTag = new Range();
			toAddrspec = new Range();
			toTag = new Range();
			epid = new Range();
		}

		public Methods Method { get; private set; }
		public int StatusCode { get; private set; }
		public int CSeq { get; private set; }
		public int Expires { get; private set; }

		public bool IsRequest
		{
			get { return StatusCode == 0; }
		}

		public bool IsResponse
		{
			get { return StatusCode > 0; }
		}

		public ByteArrayPart CallId
		{
			get { return callId.ToByteArrayPart(Buffer, Segment.Offset); }
		}

		public ByteArrayPart FromAddrspec
		{
			get { return fromAddrspec.ToByteArrayPart(Buffer, Segment.Offset); }
		}

		public ByteArrayPart FromTag
		{
			get { return fromTag.ToByteArrayPart(Buffer, Segment.Offset); }
		}

		public ByteArrayPart ToAddrspec
		{
			get { return toAddrspec.ToByteArrayPart(Buffer, Segment.Offset); }
		}

		public ByteArrayPart ToTag
		{
			get { return toTag.ToByteArrayPart(Buffer, Segment.Offset); }
		}

		public ByteArrayPart Epid
		{
			get { return epid.ToByteArrayPart(Buffer, Segment.Offset); }
		}

		#endregion

		#region Custom Headers Writer

		public event Action<SipMessageWriter> WriteCustomHeadersEvent;

		public void WriteCustomHeaders()
		{
			var handler = WriteCustomHeadersEvent;
			if (handler != null)
				handler(this);
		}

		#endregion

		public void WriteHeader(Header header)
		{
			if (header.IsRemoved == false)
				Write(header.Name, C.HCOLON, header.Value, C.CRLF);
		}

		public void WriteHeaderName(ByteArrayPart name, bool sp)
		{
			Write(name, C.HCOLON);
			if (sp == true)
				Write(C.SP);
		}

		public void WriteStatusLine(int statusCode, ByteArrayPart responsePhrase)
		{
			StatusCode = statusCode;

			Write(C.SIP_2_0, C.SP, statusCode, C.SP);
			if (responsePhrase.IsValid)
				Write(responsePhrase);

			Write(C.CRLF);
		}

		public void WriteRequestLine(Methods method, ByteArrayPart requestUri)
		{
			Write(method.ToByteArrayPart(), C.SP, requestUri, C.SP, C.SIP_2_0, C.CRLF);
		}

		public void WriteCRLF()
		{
			Write(C.CRLF);
		}
		public void WriteContacts(SipMessageReader reader)
		{
			for (int i = 0; i < reader.Count.ContactCount; i++)
				WriteContact(reader.Contact[i]);
		}

		public void WriteContact(SipMessageReader.ContactStruct contact)
		{
			Write(C.Contact, C.HCOLON, new ByteArrayPart()
			{
				Bytes = contact.Value.Bytes,
				Begin = contact.Value.Begin,
				End = contact.AddrSpec1.Value.End
			});
			if ((contact.AddrSpec1.Maddr.IsValid == false) && (contact.AddrSpec1.xMaddrIP != IPAddress.None))
			{
				Write(C.SEMI, C.maddr, C.EQUAL, contact.AddrSpec1.xMaddrIP);
			}
			if ((contact.AddrSpec1.MsReceivedCid.IsValid == false) && (contact.AddrSpec1.xMsReceivedCid.IsValid == true))
			{
				Write(C.SEMI, C.ms_received_cid, C.EQUAL);
				Write(contact.AddrSpec1.xMsReceivedCid);
			}

			if ((contact._RemoveProxy == true) && (contact.ProxyReplace.IsValid == true))
			{
				Write(new ByteArrayPart()
				{
					Bytes = contact.Value.Bytes,
					Begin = contact.AddrSpec1.Value.End,
					End = contact.ProxyReplace.Begin
				}, new ByteArrayPart()
				{
					Bytes = contact.Value.Bytes,
					Begin = contact.ProxyReplace.End,
					End = contact.Value.End
				});
			}
			else
			{
				Write(new ByteArrayPart()
				{
					Bytes = contact.Value.Bytes,
					Begin = contact.AddrSpec1.Value.End,
					End = contact.Value.End
				});
			}

			if (contact.Expires != int.MinValue)
			{
				Write(C.SEMI, C.expires, C.EQUAL);
				Write(contact.Expires);
			}
			Write(C.CRLF);
		}

		public void WriteContact(ByteArrayPart hostport, Transports transport)
		{
			Write(C.Contact, C.HCOLON, C.SP, C.LAQUOT, hostport);
			if (transport != Transports.None)
				Write(C.SEMI, C.transport, C.EQUAL, transport == Transports.Udp ? C.udp : C.tcp);
			Write(C.RAQUOT, C.CRLF);
		}

		public void WriteExpires(int expires)
		{
			Expires = expires;
			Write(C.Expires, C.HCOLON, C.SP, expires, C.CRLF);
		}

		public void WriteMinExpires(int expires)
		{
			Write(C.Min_Expires, C.HCOLON, C.SP, expires, C.CRLF);
		}

		public void WriteContentLength(int length)
		{
			Write(C.Content_Length, C.HCOLON, C.SP, length, C.CRLF);
		}

		public void WriteCseq(int number, Methods method)
		{
			Write(C.CSeq, C.HCOLON, C.SP, number, C.SP, method.ToByteArrayPart(), C.CRLF);
		}

		public void WriteTo(Header header, ByteArrayPart tag, ByteArrayPart ctag)
		{
			if (header.Name.IsValid == true)
			{
				Write(header.Name, C.HCOLON);
				if (ctag.IsValid == true)
				{
					Write(new ByteArrayPart()
					{
						Bytes = header.Value.Bytes,
						Begin = header.Value.Begin,
						End = ctag.Begin
					},
					tag,
					new ByteArrayPart()
					{
						Bytes = header.Value.Bytes,
						Begin = ctag.End,
						End = header.Value.End
					});
				}
				else
				{
					Write(header.Value);
					if (tag.IsValid == true)
					{
						Write(C.SEMI, C.tag, C.EQUAL, tag);
					}
				}
				Write(C.CRLF);
			}
		}

		public void WriteFrom(Sip.Message.Header header, ByteArrayPart tag)
		{
			if (header.Name.IsValid == true)
			{
				Write(header.Name, C.HCOLON, header.Value);
				if (tag.IsValid == true)
				{
					Write(C.SEMI, C.tag, C.EQUAL, tag);
				}
				Write(C.CRLF);
			}
		}

		public void WriteTo(Sip.Message.Header header, ByteArrayPart epid1)
		{
			if (header.Name.IsValid == true)
			{
				Write(header.Name, C.HCOLON, header.Value);
				if (epid1.IsValid == true)
				{
					Write(C.SEMI, C.epid, C.EQUAL);
					epid = new Range(end, epid1.Length);
					Write(epid1);
				}
				Write(C.CRLF);
			}
		}

		public void WriteVia(Header header, SipMessageReader.ViaStruct via)
		{
			if (header.Name.IsValid == true)
			{
				Write(header.Name, C.HCOLON, header.Value);
				if (via.ReceivedIP != IPAddress.None)
				{
					Write(C.SEMI, C.received, C.EQUAL, via.ReceivedIP);
				}
				if (via.MsReceivedPort != int.MinValue)
				{
					Write(C.SEMI, C.ms_received_port, C.EQUAL);
					Write(via.MsReceivedPort);
				}
				if (via.MsReceivedCid.IsValid == true)
				{
					Write(C.SEMI, C.ms_received_cid, C.EQUAL);
					Write(via.MsReceivedCid);
				}
				Write(C.CRLF);
			}
		}

		public void WriteVia(Transports transport, ByteArrayPart host, int port, ByteArrayPart branch)
		{
			Write(C.Via, C.HCOLON, C.SP, C.SIP_2_0, C.SLASH, transport.ToByteArrayPart(), C.SP, host, C.HCOLON);
			Write(port);
			Write(C.SEMI, C.branch, C.EQUAL, branch, C.CRLF);
		}

		public void WriteAuthenticateDigest(bool wwwOrProxy, ByteArrayPart realm, ByteArrayPart nonce, bool authint, bool stale, ByteArrayPart opaque)
		{
			Write(wwwOrProxy ? C.WWW_Authenticate : C.Proxy_Authenticate, C.HCOLON, C.SP, C.Digest, C.SP);

			Write(C.realm, C.EQUAL, C.DQUOTE, realm, C.DQUOTE, C.COMMA);
			Write(C.nonce, C.EQUAL, C.DQUOTE, nonce, C.DQUOTE, C.COMMA);
			Write(C.qop, C.EQUAL, C.DQUOTE, C.auth);
			if (authint)
				Write(C.COMMA, C.auth_int);
			Write(C.DQUOTE, C.COMMA);
			Write(C.algorithm, C.EQUAL, C.MD5, C.COMMA);
			Write(C.stale, C.EQUAL, stale ? C._true : C._false, C.COMMA);
			Write(C.opaque, C.EQUAL, C.DQUOTE, opaque, C.DQUOTE);

			Write(C.CRLF);
		}

		public void WriteAuthenticateDigest(bool wwwOrProxy, ByteArrayPart realm, int nonce1, int nonce2, int nonce3, int nonce4, bool authint, bool stale, int opaque)
		{
			Write(wwwOrProxy ? C.WWW_Authenticate : C.Proxy_Authenticate, C.HCOLON, C.SP, C.Digest, C.SP);

			Write(C.realm, C.EQUAL, C.DQUOTE, realm, C.DQUOTE, C.COMMA);
			Write(C.nonce, C.EQUAL, C.DQUOTE);
			WriteAsHex8(nonce1);
			WriteAsHex8(nonce2);
			WriteAsHex8(nonce3);
			WriteAsHex8(nonce4);
			Write(C.DQUOTE, C.COMMA);
			Write(C.qop, C.EQUAL, C.DQUOTE, C.auth);
			if (authint)
				Write(C.COMMA, C.auth_int);
			Write(C.DQUOTE, C.COMMA);
			Write(C.algorithm, C.EQUAL, C.MD5, C.COMMA);
			Write(C.stale, C.EQUAL, stale ? C._true : C._false, C.COMMA);
			Write(C.opaque, C.EQUAL, C.DQUOTE);
			WriteAsHex8(opaque);
			Write(C.DQUOTE);

			Write(C.CRLF);
		}

		public void WriteMsAuthentication(HeaderNames header, AuthSchemes scheme, ByteArrayPart targetname, ByteArrayPart realm, bool version, bool crlf)
		{
			ByteArrayPart name;

			switch (header)
			{
				case HeaderNames.ProxyAuthenticate:
					name = C.Proxy_Authenticate;
					break;

				case HeaderNames.WwwAuthenticate:
					name = C.WWW_Authenticate;
					break;

				case HeaderNames.AuthenticationInfo:
					name = C.Authentication_Info;
					break;

				case HeaderNames.ProxyAuthenticationInfo:
					name = C.Proxy_Authentication_Info;
					break;

				default:
					throw new ArgumentException();
			}

			Write(name, C.HCOLON, C.SP, scheme == AuthSchemes.Ntlm ? C.NTLM : C.Kerberos, C.SP);

			if (scheme == AuthSchemes.Kerberos)
				Write(C.targetname, C.EQUAL, C.DQUOTE, C.sip, C.SLASH, targetname, C.DQUOTE, C.COMMA);
			else
				Write(C.targetname, C.EQUAL, C.DQUOTE, targetname, C.DQUOTE, C.COMMA);

			Write(C.realm, C.EQUAL, C.DQUOTE, realm, C.DQUOTE);

			if (version == true)
				Write(C.COMMA, C.version, C.EQUAL, 3);

			Write(crlf == true ? C.CRLF : C.COMMA);
		}

		public void WriteMsAuthenticationInfo(ByteArrayPart opaque, int snum, ByteArrayPart srand, ByteArrayPart rspauth)
		{
			Write(C.opaque, C.EQUAL, C.DQUOTE, opaque, C.DQUOTE, C.COMMA);
			Write(C.qop, C.EQUAL, C.DQUOTE, C.auth, C.DQUOTE, C.COMMA);
			Write(C.snum, C.EQUAL, C.DQUOTE, snum, C.DQUOTE, C.COMMA);
			Write(C.srand, C.EQUAL, C.DQUOTE, srand, C.DQUOTE, C.COMMA);
			Write(C.rspauth, C.EQUAL, C.DQUOTE, rspauth, C.DQUOTE, C.CRLF);
		}

		public void WriteMsAuthentication(ByteArrayPart opaque, ByteArrayPart gssapiData)
		{
			Write(C.opaque, C.EQUAL, C.DQUOTE, opaque, C.DQUOTE, C.COMMA);
			Write(C.gssapi_data, C.EQUAL, C.DQUOTE, gssapiData, C.DQUOTE, C.CRLF);
		}

		public void WriteDate(DateTime date)
		{
			Write(C.Date, C.HCOLON, C.SP, date.ToString(@"R").ToByteArrayPart(), C.CRLF);
		}

		public void WriteMaxForwards(int hop)
		{
			Write(C.Max_Forwards, C.HCOLON, C.SP, hop, C.CRLF);
		}

		public void WriteUnsupported(ByteArrayPart value)
		{
			Write(C.Unsupported, C.HCOLON, C.SP, value);
		}

		public void WriteUnsupportedValue(ByteArrayPart value)
		{
			Write(C.COMMA, C.SP, value);
		}

		public void WriteRoute(ByteArrayPart value)
		{
			Write(C.Route, C.HCOLON, C.SP, C.LAQUOT, value, C.RAQUOT, C.CRLF);
		}

		public void WriteRecordRoute(UriSchemes scheme, ByteArrayPart host, int port)
		{
			Write(C.RecordRoute, C.HCOLON, C.SP, C.LAQUOT, scheme.ToByteArrayPart(), C.HCOLON, host, C.HCOLON);
			Write(port);
			Write(C.SEMI, C.lr, C.RAQUOT, C.CRLF);
		}

		public void WriteContentType(ByteArrayPart type, ByteArrayPart subtype, ByteArrayPart parameters)
		{
			Write(C.Content_Type, C.HCOLON, C.SP);
			Write(type);
			Write(C.SLASH);
			Write(subtype);
			Write(parameters);
			Write(C.CRLF);
		}

		public void WriteSupported(ByteArrayPart options)
		{
			Write(C.Supported, C.HCOLON, C.SP, options, C.CRLF);
		}

		public void WriteRequire(ByteArrayPart requires)
		{
			Write(C.Require, C.HCOLON, C.SP, requires, C.CRLF);
		}

		public void WriteSubscriptionState(ByteArrayPart substate, int expires)
		{
			Write(C.Subscription_State, C.HCOLON, C.SP, substate, C.SEMI, C.expires, C.EQUAL);
			Write(expires);
			Write(C.CRLF);
		}


		// vf:...

		public void WriteStatusLine(StatusCodes statusCode)
		{
			StatusCode = (int)statusCode;

			Write(C.SIP_2_0, C.SP, (int)statusCode, C.SP, statusCode.GetReason(), C.CRLF);
		}

		public void WriteStatusLine(StatusCodes statusCode, ByteArrayPart reason)
		{
			StatusCode = (int)statusCode;

			Write(C.SIP_2_0, C.SP, (int)statusCode, C.SP, reason, C.CRLF);
		}

		public void WriteHeaderWithTag(Header header, ByteArrayPart tag)
		{
			Write(header.Name, C.HCOLON, header.Value, C.SEMI, C.tag, C.EQUAL, tag, C.CRLF);
		}

		public ByteArrayPart GenerateTag()
		{
			return new ByteArrayPart(Guid.NewGuid().ToString().Replace(@"-", @"").ToLower());
		}

		private int contentLengthEnd = -1;

		public void WriteContentLength()
		{
			Write(C.Content_Length, C.HCOLON, C._________0);
			contentLengthEnd = end;
			Write(C.CRLF);
		}

		public void RewriteContentLength(int value)
		{
			if (contentLengthEnd < 0)
				throw new InvalidOperationException(@"WriteContentLength must be called before RewriteContentLength");

			ReversWrite((uint)value, ref contentLengthEnd);
			contentLengthEnd = -1;
		}

		public void RewriteContentLength()
		{
			if (contentLengthEnd < 0)
				throw new InvalidOperationException(@"WriteContentLength must be called before RewriteContentLength");

			ReversWrite((uint)(end - contentLengthEnd - C.CRLF.Length * 2), ref contentLengthEnd);
			contentLengthEnd = -1;
		}

		public void WriteXErrorDetails(ByteArrayPart details)
		{
			Write(C.x_Error_Details, C.HCOLON, C.SP, details, C.CRLF);
		}

		public void WriteTo(ByteArrayPart uri, ByteArrayPart tag)
		{
			Write(C.To__, C.LAQUOT, uri, C.RAQUOT, C._tag_, tag, C.CRLF);
		}

		public void WriteTo(ByteArrayPart uri, ByteArrayPart tag, ByteArrayPart epid1)
		{
			Write(C.To__, C.LAQUOT);
			toAddrspec = new Range(end, uri.Length);
			Write(uri, C.RAQUOT, C._tag_);
			toTag = new Range(end, tag.Length);
			Write(tag, C._epid_);
			epid = new Range(end, epid1.Length);
			Write(epid1, C.CRLF);
		}

		public void WriteFrom(ByteArrayPart uri, ByteArrayPart tag)
		{
			Write(C.From__, C.LAQUOT);
			fromAddrspec = new Range(end, uri.Length);
			Write(uri, C.RAQUOT, C._tag_);
			fromTag = new Range(end, tag.Length);
			Write(tag, C.CRLF);
		}

		public void WriteCallId(ByteArrayPart callIdValue)
		{
			Write(C.Call_ID__);
			callId = new Range(end, callIdValue.Length);
			Write(callIdValue, C.CRLF);
		}

		public void WriteEventPresence()
		{
			Write(C.Event__presence, C.CRLF);
		}

		public void WriteSubscriptionState(int expires)
		{
			Write(C.Subscription_State__);

			if (expires > 0)
				Write(C.active, C._expires_, expires);
			else
				Write(C.terminated);

			Write(C.CRLF);
		}

		public void WriteSubscriptionState(SubscriptionStates state, int expires)
		{
			Write(C.Subscription_State__);

			switch (state)
			{
				case SubscriptionStates.Active: Write(C.active, C._expires_, expires); break;
				case SubscriptionStates.Pending: Write(C.pending, C._expires_, expires); break;
				case SubscriptionStates.Terminated: Write(C.terminated); break;
			}

			Write(C.CRLF);
		}

		public void WriteContentType(ByteArrayPart type, ByteArrayPart subtype)
		{
			Write(C.Content_Type__, type, C.SLASH, subtype, C.CRLF);
		}

		public void WriteContentType(ByteArrayPart value)
		{
			Write(C.Content_Type__, value, C.CRLF);
		}

		public void WriteVia(Transports transport, IPEndPoint endpoint, int branch)
		{
			Write(C.Via__SIP_2_0_, transport.ToByteArrayPart(), C.SP);
			Write(endpoint);
			Write(C._branch_z9hG4bK);
			WriteAsHex8(branch);
			Write(C.CRLF);
		}

		public void WriteAllow(Methods[] methods)
		{
			Write(C.Allow__);

			Write(methods[0].ToByteArrayPart());
			for (int i = 1; i < methods.Length; i++)
			{
				Write(C.CommaSpace);
				Write(methods[i].ToByteArrayPart());
			}

			Write(C.CRLF);
		}

		public void WriteContentTransferEncodingBinary()
		{
			Write(C.Content_Transfer_Encoding__binary__);
		}

		public void WriteSipEtag(int value)
		{
			Write(C.SIP_ETag__);
			WriteAsHex8(value);
			Write(C.CRLF);
		}

		public void WriteContentTypeMultipart(ByteArrayPart contentType)
		{
			Write(C.Content_Type__multipart_related_type___, contentType, C.__boundary_OFFICESIP2011VITALIFOMINE__);
		}

		public void WriteBoundary()
		{
			Write(C.__OFFICESIP2011VITALIFOMINE__1);
		}

		public void WriteBoundaryEnd()
		{
			Write(C.__OFFICESIP2011VITALIFOMINE__2);
		}
	}
}
