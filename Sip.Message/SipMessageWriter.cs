using System;
using System.Net;
using Base.Message;

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
		, IDisposable
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
					return ByteArrayPart.Invalid;

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
		protected Range fromEpid;
		protected Range toEpid;

		public SipMessageWriter()
			: this(128, 2048)
		{
		}

		public SipMessageWriter(int size)
			: this(128, size)
		{
		}

		public SipMessageWriter(int reservAtBegin, int size)
			: base(reservAtBegin, SipMessage.BufferManager.Allocate(size))
		{
			InitializeProperties();
		}

		// temporary - should be removed for optimization
		~SipMessageWriter()
		{
			SipMessage.BufferManager.Free(ref segment);
		}

		public void Dispose()
		{
			SipMessage.BufferManager.Free(ref segment);
			GC.SuppressFinalize(this);
		}

		protected override void Reallocate(ref ArraySegment<byte> segment, int extraSize)
		{
			SipMessage.BufferManager.Reallocate(ref segment, extraSize);
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
			fromEpid = new Range();
			toEpid = new Range();
		}

		public Methods Method { get; protected set; }
		public int StatusCode { get; private set; }
		public int CSeq { get; protected set; }
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

		public ByteArrayPart FromEpid
		{
			get { return fromEpid.ToByteArrayPart(Buffer, Segment.Offset); }
		}

		public ByteArrayPart ToEpid
		{
			get { return toEpid.ToByteArrayPart(Buffer, Segment.Offset); }
		}

		public ByteArrayPart Epid
		{
			get { return IsRequest ? ToEpid : FromEpid; }
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

		public void WriteHeader(HeaderNames name, ByteArrayPart value)
		{
			Write(name.ToUtf8Bytes());
			Write(C.HCOLON, C.SP, value, C.CRLF);
		}

		public void WriteHeader(Header header)
		{
			Write(header.Name, C.HCOLON, header.Value, C.CRLF);
		}

		public void WriteHeader(SipMessageReader reader, int index)
		{
			Write(reader.Headers[index].Name, C.HCOLON);

			int headerBegin = reader.Headers[index].Value.Begin;

			switch (reader.Headers[index].HeaderName)
			{
				case HeaderNames.CallId:
					callId = CreateRange(reader.CallId, headerBegin);
					break;

				case HeaderNames.From:
					fromAddrspec = CreateRange(reader.From.AddrSpec.Value, headerBegin);
					fromTag = CreateRange(reader.From.Tag, headerBegin);
					fromEpid = CreateRange(reader.From.Epid, headerBegin);
					break;

				case HeaderNames.To:
					toAddrspec = CreateRange(reader.To.AddrSpec.Value, headerBegin);
					toTag = CreateRange(reader.To.Tag, headerBegin);
					toEpid = CreateRange(reader.To.Epid, headerBegin);
					break;

				case HeaderNames.CSeq:
					Method = reader.CSeq.Method;
					CSeq = reader.CSeq.Value;
					break;
			}

			Write(reader.Headers[index].Value, C.CRLF);
		}

		public void WriteToHeader(SipMessageReader reader, int index, ByteArrayPart epid1)
		{
			if (reader.To.Epid.IsNotEmpty || epid1.Length <= 0)
				WriteHeader(reader, index);
			else
			{
				Write(C.To, C.HCOLON);

				int headerBegin = reader.Headers[index].Value.Begin;

				toAddrspec = CreateRange(reader.To.AddrSpec.Value, headerBegin);
				toTag = CreateRange(reader.To.Tag, headerBegin);

				Write(reader.Headers[index].Value);

				Write(C._epid_);
				toEpid = new Range(end, epid1.Length);
				Write(epid1);

				Write(C.CRLF);
			}
		}

		private Range CreateRange(ByteArrayPart part, int headerBegin)
		{
			if (part.IsValid)
				return new Range(end + part.Begin - headerBegin, part.Length);
			return new Range();
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
			Method = method;

			Write(method.ToByteArrayPart(), C.SP, requestUri, C.SP, C.SIP_2_0, C.CRLF);
		}

		public void WriteRequestLine(Methods method, UriSchemes scheme, ByteArrayPart user, ByteArrayPart domain)
		{
			Method = method;

			Write(method.ToByteArrayPart(), C.SP);
			Write(scheme.ToByteArrayPart(), C.HCOLON, user, C.At, domain);
			Write(C.SP, C.SIP_2_0, C.CRLF);
		}

		public void WriteRequestLine(Methods method, Transports transport, IPEndPoint endPoint)
		{
			Method = method;

			Write(method.ToByteArrayPart(), C.SP, C.sip, C.HCOLON);
			Write(endPoint);
			Write(C.SEMI, C.transport, C.EQUAL);
			Write(transport.ToTransportParamUtf8Bytes());
			Write(C.SP, C.SIP_2_0, C.CRLF);
		}

		public void WriteCRLF()
		{
			Write(C.CRLF);
		}
		//public void WriteContacts(SipMessageReader reader)
		//{
		//    for (int i = 0; i < reader.Count.ContactCount; i++)
		//        WriteContact(reader.Contact[i]);
		//}

		public void WriteContact(ByteArrayPart addrSpec, ByteArrayPart sipInstance, int expires)
		{
			Write(C.Contact___, addrSpec, C.RAQUOT);

			if (sipInstance.IsValid)
				Write(C.__sip_instance___, sipInstance, C.RAQUOT, C.DQUOTE);

			Write(C._expires_, expires);
			Write(C.CRLF);
		}

		//public void WriteContact(SipMessageReader.ContactStruct contact)
		//{
		//    Write(C.Contact, C.HCOLON, new ByteArrayPart()
		//    {
		//        Bytes = contact.Value.Bytes,
		//        Begin = contact.Value.Begin,
		//        End = contact.AddrSpec1.Value.End
		//    });
		//    if ((contact.AddrSpec1.Maddr.IsValid == false) && (contact.AddrSpec1.xMaddrIP != IPAddress.None))
		//    {
		//        Write(C.SEMI, C.maddr, C.EQUAL, contact.AddrSpec1.xMaddrIP);
		//    }
		//    if ((contact.AddrSpec1.MsReceivedCid.IsValid == false) && (contact.AddrSpec1.xMsReceivedCid.IsValid == true))
		//    {
		//        Write(C.SEMI, C.ms_received_cid, C.EQUAL);
		//        Write(contact.AddrSpec1.xMsReceivedCid);
		//    }

		//    if ((contact._RemoveProxy == true) && (contact.ProxyReplace.IsValid == true))
		//    {
		//        Write(new ByteArrayPart()
		//        {
		//            Bytes = contact.Value.Bytes,
		//            Begin = contact.AddrSpec1.Value.End,
		//            End = contact.ProxyReplace.Begin
		//        }, new ByteArrayPart()
		//        {
		//            Bytes = contact.Value.Bytes,
		//            Begin = contact.ProxyReplace.End,
		//            End = contact.Value.End
		//        });
		//    }
		//    else
		//    {
		//        Write(new ByteArrayPart()
		//        {
		//            Bytes = contact.Value.Bytes,
		//            Begin = contact.AddrSpec1.Value.End,
		//            End = contact.Value.End
		//        });
		//    }

		//    if (contact.Expires != int.MinValue)
		//    {
		//        Write(C.SEMI, C.expires, C.EQUAL);
		//        Write(contact.Expires);
		//    }
		//    Write(C.CRLF);
		//}

		public void WriteContact(ByteArrayPart hostport, Transports transport)
		{
			Write(C.Contact, C.HCOLON, C.SP, C.LAQUOT, hostport);
			if (transport != Transports.None)
				Write(C.SEMI, C.transport, C.EQUAL, transport == Transports.Udp ? C.udp : C.tcp);
			Write(C.RAQUOT, C.CRLF);
		}

		public void WriteContact(IPEndPoint endPoint, Transports transport)
		{
			WriteContact(endPoint, transport, ByteArrayPart.Invalid);
		}

		public void WriteContact(IPEndPoint endPoint, Transports transport, ByteArrayPart sipInstance)
		{
			WriteContact(ByteArrayPart.Invalid, endPoint, transport, sipInstance);

			//Write(C.Contact, C.HCOLON, C.SP, C.LAQUOT, C.sip, C.HCOLON);
			//Write(endPoint);
			//if (transport != Transports.None)
			//{
			//    Write(C.SEMI, C.transport, C.EQUAL);
			//    Write(transport.ToLowerUtf8Bytes());
			//}
			//Write(C.RAQUOT);
			//if (sipInstance.IsValid)
			//    Write(C.__sip_instance___, sipInstance, C.RAQUOT, C.DQUOTE);
			//Write(C.CRLF);
		}

		public void WriteContact(ByteArrayPart user, IPEndPoint endPoint, Transports transport, ByteArrayPart sipInstance)
		{
			Write(C.Contact, C.HCOLON, C.SP, C.LAQUOT, C.sip, C.HCOLON);

			if (user.IsValid)
				Write(user, C.At);

			Write(endPoint, transport);

			if (transport != Transports.None)
			{
				Write(C.SEMI, C.transport, C.EQUAL);
				Write(transport.ToTransportParamUtf8Bytes());
			}

			Write(C.RAQUOT);

			if (sipInstance.IsValid)
				Write(C.__sip_instance___, sipInstance, C.RAQUOT, C.DQUOTE);

			Write(C.CRLF);
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
			CSeq = number;
			Method = method;

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
					toEpid = new Range(end, epid1.Length);
					Write(epid1);
				}
				Write(C.CRLF);
			}
		}

		//public void WriteVia(Header header, SipMessageReader.ViaStruct via)
		//{
		//    if (header.Name.IsValid == true)
		//    {
		//        Write(header.Name, C.HCOLON, header.Value);
		//        if (via.ReceivedIP != IPAddress.None)
		//        {
		//            Write(C.SEMI, C.received, C.EQUAL, via.ReceivedIP);
		//        }
		//        if (via.MsReceivedPort != int.MinValue)
		//        {
		//            Write(C.SEMI, C.ms_received_port, C.EQUAL);
		//            Write(via.MsReceivedPort);
		//        }
		//        if (via.MsReceivedCid.IsValid == true && via.MsReceivedCid.Bytes != null)
		//        {
		//            Write(C.SEMI, C.ms_received_cid, C.EQUAL);
		//            Write(via.MsReceivedCid);
		//        }
		//        Write(C.CRLF);
		//    }
		//}

		public void WriteVia(Transports transport, ByteArrayPart host, int port, ByteArrayPart branch)
		{
			Write(C.Via, C.HCOLON, C.SP, C.SIP_2_0, C.SLASH);
			Write(transport.ToUtf8Bytes());
			Write(C.SP, host, C.HCOLON);
			Write(port);
			Write(C.SEMI, C.branch, C.EQUAL, branch, C.CRLF);
		}

		//public void WriteAuthenticateDigest(bool proxy, ByteArrayPart realm, ByteArrayPart nonce, bool authint, bool stale, ByteArrayPart opaque)
		//{
		//    Write(proxy ? C.Proxy_Authenticate : C.WWW_Authenticate, C.HCOLON, C.SP, C.Digest, C.SP);

		//    Write(C.realm, C.EQUAL, C.DQUOTE, realm, C.DQUOTE, C.COMMA);
		//    Write(C.nonce, C.EQUAL, C.DQUOTE, nonce, C.DQUOTE, C.COMMA);
		//    Write(C.qop, C.EQUAL, C.DQUOTE, C.auth);
		//    if (authint)
		//        Write(C.COMMA, C.auth_int);
		//    Write(C.DQUOTE, C.COMMA);
		//    Write(C.algorithm, C.EQUAL, C.MD5, C.COMMA);
		//    Write(C.stale, C.EQUAL, stale ? C._true : C._false, C.COMMA);
		//    Write(C.opaque, C.EQUAL, C.DQUOTE, opaque, C.DQUOTE);

		//    Write(C.CRLF);
		//}

		public void WriteAuthenticateDigest(bool proxy, ByteArrayPart realm, int nonce1, int nonce2, int nonce3, int nonce4, bool authint, bool stale, int opaque)
		{
			Write(proxy ? C.Proxy_Authenticate : C.WWW_Authenticate, C.HCOLON, C.SP, C.Digest, C.SP);

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

		//public void WriteMsAuthentication(HeaderNames header, AuthSchemes scheme, ByteArrayPart targetname, ByteArrayPart realm, bool version, bool crlf)
		//{
		//    ByteArrayPart name;

		//    switch (header)
		//    {
		//        case HeaderNames.ProxyAuthenticate:
		//            name = C.Proxy_Authenticate;
		//            break;

		//        case HeaderNames.WwwAuthenticate:
		//            name = C.WWW_Authenticate;
		//            break;

		//        case HeaderNames.AuthenticationInfo:
		//            name = C.Authentication_Info;
		//            break;

		//        case HeaderNames.ProxyAuthenticationInfo:
		//            name = C.Proxy_Authentication_Info;
		//            break;

		//        default:
		//            throw new ArgumentException();
		//    }

		//    Write(name, C.HCOLON, C.SP, scheme == AuthSchemes.Ntlm ? C.NTLM : C.Kerberos, C.SP);

		//    if (scheme == AuthSchemes.Kerberos)
		//        Write(C.targetname, C.EQUAL, C.DQUOTE, C.sip, C.SLASH, targetname, C.DQUOTE, C.COMMA);
		//    else
		//        Write(C.targetname, C.EQUAL, C.DQUOTE, targetname, C.DQUOTE, C.COMMA);

		//    Write(C.realm, C.EQUAL, C.DQUOTE, realm, C.DQUOTE);

		//    if (version == true)
		//        Write(C.COMMA, C.version, C.EQUAL, 3);

		//    Write(crlf == true ? C.CRLF : C.COMMA);
		//}

		//public void WriteMsAuthenticationInfo(ByteArrayPart opaque, int snum, ByteArrayPart srand, ByteArrayPart rspauth)
		//{
		//    Write(C.opaque, C.EQUAL, C.DQUOTE, opaque, C.DQUOTE, C.COMMA);
		//    Write(C.qop, C.EQUAL, C.DQUOTE, C.auth, C.DQUOTE, C.COMMA);
		//    Write(C.snum, C.EQUAL, C.DQUOTE, snum, C.DQUOTE, C.COMMA);
		//    Write(C.srand, C.EQUAL, C.DQUOTE, srand, C.DQUOTE, C.COMMA);
		//    Write(C.rspauth, C.EQUAL, C.DQUOTE, rspauth, C.DQUOTE, C.CRLF);
		//}

		//public void WriteMsAuthentication(ByteArrayPart opaque, ByteArrayPart gssapiData)
		//{
		//    Write(C.opaque, C.EQUAL, C.DQUOTE, opaque, C.DQUOTE, C.COMMA);
		//    Write(C.gssapi_data, C.EQUAL, C.DQUOTE, gssapiData, C.DQUOTE, C.CRLF);
		//}

		public void WriteAuthenticationInfo(bool proxy, AuthSchemes scheme, ByteArrayPart targetname, ByteArrayPart realm, int opaque, int snum, int srand, ArraySegment<byte> rspauth)
		{
			Write(proxy ? C.Proxy_Authentication_Info : C.Authentication_Info, C.HCOLON, C.SP,
				scheme == AuthSchemes.Ntlm ? C.NTLM : C.Kerberos, C.SP);

			Write(C.targetname, C.EQUAL, C.DQUOTE);
			if (scheme == AuthSchemes.Kerberos)
				Write(C.sip, C.SLASH);
			Write(targetname, C.DQUOTE, C.COMMA);

			Write(C.realm, C.EQUAL, C.DQUOTE, realm, C.DQUOTE);

			Write(C.COMMA, C.opaque, C.EQUAL, C.DQUOTE);
			WriteAsHex8(opaque);
			Write(C.DQUOTE);

			Write(C.COMMA, C.qop, C.EQUAL, C.DQUOTE, C.auth, C.DQUOTE);

			Write(C._snum__, snum, C.DQUOTE);

			Write(C._srand__);
			WriteAsHex8(srand);
			Write(C.DQUOTE);

			Write(C._rspauth__);
			WriteAsHex(rspauth);
			Write(C.DQUOTE);

			Write(C.CRLF);
		}

		public void WriteAuthenticateMs(bool proxy, AuthSchemes scheme, ByteArrayPart targetname, ByteArrayPart realm, int opaque)
		{
			Write(proxy ? C.Proxy_Authenticate : C.WWW_Authenticate, C.HCOLON, C.SP, scheme == AuthSchemes.Ntlm ? C.NTLM : C.Kerberos, C.SP);

			Write(C.targetname, C.EQUAL, C.DQUOTE);
			if (scheme == AuthSchemes.Kerberos)
				Write(C.sip, C.SLASH);
			Write(targetname, C.DQUOTE, C.COMMA);

			Write(C.realm, C.EQUAL, C.DQUOTE, realm, C.DQUOTE);
			Write(C.COMMA, C.version, C.EQUAL, 3);

			Write(C.COMMA, C.opaque, C.EQUAL, C.DQUOTE);
			WriteAsHex8(opaque);
			Write(C.DQUOTE, C.CRLF);
		}

		public void WriteAuthenticateMs(bool proxy, AuthSchemes scheme, ByteArrayPart targetname, ByteArrayPart realm, int opaque, ArraySegment<byte> gssapiData)
		{
			Write(proxy ? C.Proxy_Authenticate : C.WWW_Authenticate, C.HCOLON, C.SP, scheme == AuthSchemes.Ntlm ? C.NTLM : C.Kerberos, C.SP);

			Write(C.targetname, C.EQUAL, C.DQUOTE);
			if (scheme == AuthSchemes.Kerberos)
				Write(C.sip, C.SLASH);
			Write(targetname, C.DQUOTE, C.COMMA);

			Write(C.realm, C.EQUAL, C.DQUOTE, realm, C.DQUOTE);
			Write(C.COMMA, C.version, C.EQUAL, 3);

			Write(C.COMMA, C.opaque, C.EQUAL, C.DQUOTE);
			WriteAsHex8(opaque);
			Write(C.DQUOTE, C.COMMA);

			Write(C.gssapi_data, C.EQUAL, C.DQUOTE);
			WriteAsBase64(gssapiData);
			Write(C.DQUOTE, C.CRLF);
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

		public void WriteRecordRoute(Transports transport, IPEndPoint endpoint, ArraySegment<byte> msReceivedCid)
		{
			var scheme = (transport == Transports.Tls) ? UriSchemes.Sips : UriSchemes.Sip;

			Write(C.RecordRoute, C.HCOLON, C.SP, C.LAQUOT, scheme.ToByteArrayPart(), C.HCOLON);
			Write(endpoint, transport);
			Write(C._ms_received_cid_);
			Write(msReceivedCid);
			Write(C.SEMI, C.lr, C.RAQUOT, C.CRLF);
		}

		public void WriteRecordRoute(Transports transport, IPEndPoint endpoint)
		{
			var scheme = (transport == Transports.Tls) ? UriSchemes.Sips : UriSchemes.Sip;

			Write(C.RecordRoute, C.HCOLON, C.SP, C.LAQUOT, scheme.ToByteArrayPart(), C.HCOLON);
			Write(endpoint, transport);
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

		public void WriteXErrorDetails(byte[] details)
		{
			Write(C.x_Error_Details, C.HCOLON, C.SP);
			Write(details);
			Write(C.CRLF);
		}

		public void WriteXErrorDetails(ByteArrayPart details)
		{
			Write(C.x_Error_Details, C.HCOLON, C.SP, details, C.CRLF);
		}

		public void WriteXErrorDetails(ByteArrayPart details1, byte[] details2)
		{
			Write(C.x_Error_Details, C.HCOLON, C.SP, details1);
			if (details2 != null && details2.Length > 0)
			{
				Write(C.CommaSpace);
				Write(details2);
			}
			Write(C.CRLF);
		}

		public void WriteTo(ByteArrayPart uri, ByteArrayPart tag)
		{
			Write(C.To__, C.LAQUOT, uri, C.RAQUOT);
			if (tag.IsValid)
				Write(C._tag_, tag);
			Write(C.CRLF);
		}

		public void WriteTo2(ByteArrayPart user, ByteArrayPart domain, ByteArrayPart tag)
		{
			Write(C.To__, C.LAQUOT, C.sip, C.HCOLON, user, C.At, domain, C.RAQUOT);
			if (tag.IsValid)
				Write(C._tag_, tag);
			Write(C.CRLF);
		}

		public void WriteToRaw(ByteArrayPart user, ByteArrayPart domain, ByteArrayPart raw)
		{
			Write(C.To__, C.LAQUOT, C.sip, C.HCOLON, user, C.At, domain, C.RAQUOT);
			Write(raw, C.CRLF);
		}

		public void WriteTo(ByteArrayPart uri, ByteArrayPart tag, ByteArrayPart epid1)
		{
			Write(C.To__, C.LAQUOT);
			toAddrspec = new Range(end, uri.Length);
			Write(uri, C.RAQUOT, C._tag_);
			toTag = new Range(end, tag.Length);
			Write(tag);
			if (epid1.IsNotEmpty)
			{
				Write(C._epid_);
				toEpid = new Range(end, epid1.Length);
				Write(epid1);
			}
			Write(C.CRLF);
		}

		public void WriteTo(ByteArrayPart uri)
		{
			Write(C.To__, C.LAQUOT);
			toAddrspec = new Range(end, uri.Length);
			Write(uri, C.RAQUOT, C.CRLF);
		}

		public void WriteFrom(ByteArrayPart uri, ByteArrayPart tag)
		{
			WriteFrom(uri, tag, ByteArrayPart.Invalid);
		}

		public void WriteFrom(ByteArrayPart uri, ByteArrayPart tag, ByteArrayPart epid)
		{
			Write(C.From__, C.LAQUOT);
			fromAddrspec = new Range(end, uri.Length);
			Write(uri, C.RAQUOT, C._tag_);
			fromTag = new Range(end, tag.Length);
			Write(tag);
			if (epid.IsValid)
				Write(C._epid_, epid);
			Write(C.CRLF);
		}

		public void WriteFrom(ByteArrayPart uri, int tag)
		{
			Write(C.From__, C.LAQUOT);
			fromAddrspec = new Range(end, uri.Length);
			Write(uri, C.RAQUOT, C._tag_);
			fromTag = new Range(end, 8);
			WriteAsHex8(tag);
			Write(C.CRLF);
		}

		public void WriteFromRaw(ByteArrayPart displayName, ByteArrayPart uri, ByteArrayPart raw)
		{
			Write(C.From__, C.DQUOTE, displayName, C.DQUOTE, C.SP, C.LAQUOT, uri, C.RAQUOT, raw);
			Write(C.CRLF);
		}

		public void WriteCallId(ByteArrayPart callIdValue)
		{
			Write(C.Call_ID__);
			callId = new Range(end, callIdValue.Length);
			Write(callIdValue, C.CRLF);
		}

		public void WriteCallId(IPAddress localAddreess, int random)
		{
			Write(C.Call_ID__);
			int start = end;
			WriteAsHex8(random);
			Write(C.At);
			Write(localAddreess);
			callId = new Range(start, end);
			Write(C.CRLF);
		}

		public void WriteEventPresence()
		{
			Write(C.Event__presence, C.CRLF);
		}

		public void WriteEventRegistration()
		{
			Write(C.Event__registration, C.CRLF);
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
			Write(C.Via__SIP_2_0_);
			Write(transport.ToUtf8Bytes());
			Write(C.SP);
			Write(endpoint, transport);
			Write(C._branch_z9hG4bK);
			WriteAsHex8(branch);
			Write(C.CRLF);
		}

		public void WriteVia(Transports transport, IPEndPoint endpoint)
		{
			Write(C.Via__SIP_2_0_);
			Write(transport.ToUtf8Bytes());
			Write(C.SP);
			Write(endpoint, transport);
			Write(C._branch_);
			Write(C.z9hG4bK_NO_TRANSACTION);
			Write(C.CRLF);
		}

		public void WriteVia(Transports transport, IPEndPoint endpoint, int branch, ArraySegment<byte> msRecivedCid)
		{
			Write(C.Via__SIP_2_0_);
			Write(transport.ToUtf8Bytes());
			Write(C.SP);
			Write(endpoint, transport);
			Write(C._branch_z9hG4bK);
			WriteAsHex8(branch);
			Write(C._ms_received_cid_);
			Write(msRecivedCid);
			Write(C.CRLF);
		}

		public void Write(IPEndPoint endpoint, Transports transport)
		{
			if (transport != Transports.Ws && transport != Transports.Wss)
				Write(endpoint);
			else
			{
				Write(C.i);
				Write((UInt32)endpoint.GetHashCode());
				Write(C._invalid);
			}
		}

		public void WriteDigestAuthorization(HeaderNames header, ByteArrayPart username, ByteArrayPart realm, AuthQops qop,
			AuthAlgorithms algorithm, ByteArrayPart uri, ByteArrayPart nonce, int nc, int cnonce, ByteArrayPart opaque, byte[] response)
		{
			if (header == HeaderNames.Authorization)
				Write(C.Authorization);
			else if (header == HeaderNames.ProxyAuthorization)
				Write(C.Proxy_Authorization);
			else
				throw new ArgumentException(@"HeaderNames");

			Write(C.__Digest_username__);
			Write(username);
			Write(C.___realm__);
			Write(realm);
			if (qop != AuthQops.None)
			{
				Write(C.___qop__);
				Write(qop.ToByteArrayPart());
			}
			else
				Write(C.DQUOTE);
			Write(C.__algorithm_);
			Write(algorithm.ToByteArrayPart());
			Write(C.__uri__);
			Write(uri);
			Write(C.___nonce__);
			Write(nonce);
			if (qop != AuthQops.None)
			{
				Write(C.__nc_);
				Write(nc);
				Write(C.__cnonce__);
				WriteAsHex8(cnonce);
			}
			Write(C.___opaque__);
			if (opaque.IsValid)
				Write(opaque);
			Write(C.___response__);
			Write(response);
			Write(C.DQUOTE);
			Write(C.CRLF);
		}

		public void WriteSupportedMsBenotify()
		{
			Write(C.Supported__ms_benotify__);
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
