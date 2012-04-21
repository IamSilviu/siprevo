using System;
using System.Net;
using System.Text;
using Base.Message;

namespace Sip.Message
{
	public static class Converters
	{
		private static readonly byte[][] transportParams;
		private static readonly byte[][] authSchemes;
		private static readonly byte[][] headerNames;

		static Converters()
		{
			transportParams = new byte[Enum.GetValues(typeof(Transports)).Length][];
			authSchemes = new byte[Enum.GetValues(typeof(AuthSchemes)).Length][];
			headerNames = new byte[Enum.GetValues(typeof(HeaderNames)).Length][];

			InitializeTransportParams();
			InitializeAuthSchemes();
			InitializeHeaderNames();

			Verify(transportParams, typeof(Transports));
			Verify(authSchemes, typeof(AuthSchemes));
			Verify(headerNames, typeof(HeaderNames));
		}

		public static ByteArrayPart ToByteArrayPart(this Methods method)
		{
			switch (method)
			{
				case Methods.Ackm:
					return SipMessageWriter.C.ACK;
				case Methods.Benotifym:
					return SipMessageWriter.C.BENOTIFY;
				case Methods.Byem:
					return SipMessageWriter.C.BYE;
				case Methods.Cancelm:
					return SipMessageWriter.C.CANCEL;
				case Methods.Infom:
					return SipMessageWriter.C.INFO;
				case Methods.Invitem:
					return SipMessageWriter.C.INVITE;
				case Methods.Messagem:
					return SipMessageWriter.C.MESSAGE;
				case Methods.Notifym:
					return SipMessageWriter.C.NOTIFY;
				case Methods.Optionsm:
					return SipMessageWriter.C.OPTIONS;
				case Methods.Referm:
					return SipMessageWriter.C.REFER;
				case Methods.Registerm:
					return SipMessageWriter.C.REGISTER;
				case Methods.Servicem:
					return SipMessageWriter.C.SERVICE;
				case Methods.Subscribem:
					return SipMessageWriter.C.SUBSCRIBE;
				case Methods.Publishm:
					return SipMessageWriter.C.PUBLISH;

				case Methods.Extension:
				case Methods.None:
					throw new ArgumentException();

				default:
					throw new NotImplementedException(method.ToString());
			}
		}

		public static ByteArrayPart ToByteArrayPart(this UriSchemes scheme)
		{
			switch (scheme)
			{
				case UriSchemes.Sip:
					return SipMessageWriter.C.sip;
				case UriSchemes.Sips:
					return SipMessageWriter.C.sips;

				case UriSchemes.Absolute:
				case UriSchemes.None:
					throw new ArgumentException();

				default:
					throw new NotImplementedException();
			}
		}

		public static ByteArrayPart ToByteArrayPart(this Transports transport)
		{
			switch (transport)
			{
				case Transports.Tcp:
					return SipMessageWriter.C.TCP;
				case Transports.Tls:
					return SipMessageWriter.C.TLS;
				case Transports.Udp:
					return SipMessageWriter.C.UDP;
				case Transports.Sctp:
					return SipMessageWriter.C.SCTP;

				case Transports.Other:
				case Transports.None:
					throw new ArgumentException();

				default:
					throw new NotImplementedException();
			}
		}

		public static ByteArrayPart ToByteArrayPart(this AuthQops qop)
		{
			switch (qop)
			{
				case AuthQops.Auth:
					return SipMessageWriter.C.auth;
				case AuthQops.AuthInt:
					return SipMessageWriter.C.auth_int;

				default:
					throw new NotImplementedException();
			}
		}
		public static ByteArrayPart ToByteArrayPart(this AuthAlgorithms algorithm)
		{
			switch (algorithm)
			{
				case AuthAlgorithms.Md5:
					return SipMessageWriter.C.MD5;
				case AuthAlgorithms.Md5Sess:
					return SipMessageWriter.C.MD5_sess;

				case AuthAlgorithms.Other:
				case AuthAlgorithms.None:
					throw new ArgumentException();

				default:
					throw new NotImplementedException();
			}
		}

		public static byte[] ToTransportParamUtf8Bytes(this Transports transport)
		{
			return transportParams[(int)transport];
		}

		public static byte[] ToUtf8Bytes(this AuthSchemes schemes)
		{
			return authSchemes[(int)schemes];
		}

		public static byte[] ToUtf8Bytes(this HeaderNames headerName)
		{
			return headerNames[(int)headerName];
		}

		public static ByteArrayPart ToByteArrayPart(this string text)
		{
			return new ByteArrayPart(text);
		}

		public static UriSchemes ToScheme(this Transports transport)
		{
			switch (transport)
			{
				case Transports.Tls:
					return UriSchemes.Sips;

				case Transports.Udp:
				case Transports.Tcp:
				case Transports.Sctp:
					return UriSchemes.Sip;

				default:
					throw new ArgumentException(@"Can not convert Transports to UriSchemes: " + transport.ToString());
			}
		}

		private static void InitializeTransportParams()
		{
			transportParams[(int)Transports.None] = new byte[0];
			transportParams[(int)Transports.Other] = new byte[0];
			transportParams[(int)Transports.Tcp] = Encoding.UTF8.GetBytes(@"tcp");
			transportParams[(int)Transports.Tls] = Encoding.UTF8.GetBytes(@"tls");
			transportParams[(int)Transports.Udp] = Encoding.UTF8.GetBytes(@"udp");
			transportParams[(int)Transports.Ws] = Encoding.UTF8.GetBytes(@"ws");
			// ws transport parameter used for wss transport
			// http://tools.ietf.org/html/draft-ibc-sipcore-sip-websocket-01
			transportParams[(int)Transports.Wss] = Encoding.UTF8.GetBytes(@"ws");
			transportParams[(int)Transports.Sctp] = Encoding.UTF8.GetBytes(@"sctp");
			transportParams[(int)Transports.TlsSctp] = Encoding.UTF8.GetBytes(@"sctp");
		}

		private static void InitializeAuthSchemes()
		{
			authSchemes[(int)AuthSchemes.None] = new byte[0];
			authSchemes[(int)AuthSchemes.Digest] = Encoding.UTF8.GetBytes(@"Digest");
			authSchemes[(int)AuthSchemes.Kerberos] = Encoding.UTF8.GetBytes(@"Kerberos");
			authSchemes[(int)AuthSchemes.Ntlm] = Encoding.UTF8.GetBytes(@"NTLM");
			authSchemes[(int)AuthSchemes.TlsDsk] = Encoding.UTF8.GetBytes(@"TLS-DSK");
		}

		private static void InitializeHeaderNames()
		{
			headerNames[(int)HeaderNames.None] = new byte[0];
			headerNames[(int)HeaderNames.Extension] = Encoding.UTF8.GetBytes(@"Extension");
			headerNames[(int)HeaderNames.ContentType] = Encoding.UTF8.GetBytes(@"Content-Type");
			headerNames[(int)HeaderNames.ContentEncoding] = Encoding.UTF8.GetBytes(@"Content-Encoding");
			headerNames[(int)HeaderNames.From] = Encoding.UTF8.GetBytes(@"From");
			headerNames[(int)HeaderNames.CallId] = Encoding.UTF8.GetBytes(@"Call-ID");
			headerNames[(int)HeaderNames.Supported] = Encoding.UTF8.GetBytes(@"Supported");
			headerNames[(int)HeaderNames.ContentLength] = Encoding.UTF8.GetBytes(@"Content-Length");
			headerNames[(int)HeaderNames.Contact] = Encoding.UTF8.GetBytes(@"Contact");
			headerNames[(int)HeaderNames.Event] = Encoding.UTF8.GetBytes(@"Event");
			headerNames[(int)HeaderNames.Expires] = Encoding.UTF8.GetBytes(@"Expires");
			headerNames[(int)HeaderNames.Subject] = Encoding.UTF8.GetBytes(@"Subject");
			headerNames[(int)HeaderNames.To] = Encoding.UTF8.GetBytes(@"To");
			headerNames[(int)HeaderNames.AllowEvents] = Encoding.UTF8.GetBytes(@"Allow-Events");
			headerNames[(int)HeaderNames.Via] = Encoding.UTF8.GetBytes(@"Via");
			headerNames[(int)HeaderNames.CSeq] = Encoding.UTF8.GetBytes(@"CSeq");
			headerNames[(int)HeaderNames.Date] = Encoding.UTF8.GetBytes(@"Date");
			headerNames[(int)HeaderNames.Allow] = Encoding.UTF8.GetBytes(@"Allow");
			headerNames[(int)HeaderNames.Route] = Encoding.UTF8.GetBytes(@"Route");
			headerNames[(int)HeaderNames.Accept] = Encoding.UTF8.GetBytes(@"Accept");
			headerNames[(int)HeaderNames.Server] = Encoding.UTF8.GetBytes(@"Server");
			headerNames[(int)HeaderNames.Require] = Encoding.UTF8.GetBytes(@"Require");
			headerNames[(int)HeaderNames.Warning] = Encoding.UTF8.GetBytes(@"Warning");
			headerNames[(int)HeaderNames.Priority] = Encoding.UTF8.GetBytes(@"Priority");
			headerNames[(int)HeaderNames.ReplyTo] = Encoding.UTF8.GetBytes(@"Reply-To");
			headerNames[(int)HeaderNames.SipEtag] = Encoding.UTF8.GetBytes(@"Sip-Etag");
			headerNames[(int)HeaderNames.CallInfo] = Encoding.UTF8.GetBytes(@"Call-Info");
			headerNames[(int)HeaderNames.Timestamp] = Encoding.UTF8.GetBytes(@"Timestamp");
			headerNames[(int)HeaderNames.AlertInfo] = Encoding.UTF8.GetBytes(@"Alert-Info");
			headerNames[(int)HeaderNames.ErrorInfo] = Encoding.UTF8.GetBytes(@"Error-Info");
			headerNames[(int)HeaderNames.UserAgent] = Encoding.UTF8.GetBytes(@"User-Agent");
			headerNames[(int)HeaderNames.InReplyTo] = Encoding.UTF8.GetBytes(@"In-Reply-To");
			headerNames[(int)HeaderNames.MinExpires] = Encoding.UTF8.GetBytes(@"Min-Expires");
			headerNames[(int)HeaderNames.RetryAfter] = Encoding.UTF8.GetBytes(@"Retry-After");
			headerNames[(int)HeaderNames.Unsupported] = Encoding.UTF8.GetBytes(@"Unsupported");
			headerNames[(int)HeaderNames.MaxForwards] = Encoding.UTF8.GetBytes(@"Max-Forwards");
			headerNames[(int)HeaderNames.MimeVersion] = Encoding.UTF8.GetBytes(@"Mime-Version");
			headerNames[(int)HeaderNames.Organization] = Encoding.UTF8.GetBytes(@"Organization");
			headerNames[(int)HeaderNames.RecordRoute] = Encoding.UTF8.GetBytes(@"Record-Route");
			headerNames[(int)HeaderNames.SipIfMatch] = Encoding.UTF8.GetBytes(@"Sip-If-Match");
			headerNames[(int)HeaderNames.Authorization] = Encoding.UTF8.GetBytes(@"Authorization");
			headerNames[(int)HeaderNames.ProxyRequire] = Encoding.UTF8.GetBytes(@"Proxy-Require");
			headerNames[(int)HeaderNames.AcceptEncoding] = Encoding.UTF8.GetBytes(@"Accept-Encoding");
			headerNames[(int)HeaderNames.AcceptLanguage] = Encoding.UTF8.GetBytes(@"Accept-Language");
			headerNames[(int)HeaderNames.ContentLanguage] = Encoding.UTF8.GetBytes(@"Content-Language");
			headerNames[(int)HeaderNames.WwwAuthenticate] = Encoding.UTF8.GetBytes(@"WWW-Authenticate");
			headerNames[(int)HeaderNames.ProxyAuthenticate] = Encoding.UTF8.GetBytes(@"Proxy-Authenticate");
			headerNames[(int)HeaderNames.SubscriptionState] = Encoding.UTF8.GetBytes(@"Subscription-State");
			headerNames[(int)HeaderNames.AuthenticationInfo] = Encoding.UTF8.GetBytes(@"Authentication-Info");
			headerNames[(int)HeaderNames.ContentDisposition] = Encoding.UTF8.GetBytes(@"Content-Disposition");
			headerNames[(int)HeaderNames.ProxyAuthorization] = Encoding.UTF8.GetBytes(@"Proxy-Authorization");
			headerNames[(int)HeaderNames.ProxyAuthenticationInfo] = Encoding.UTF8.GetBytes(@"Proxy-Authentication-Info");
		}

		private static void Verify(byte[][] values, Type type)
		{
			int length = Enum.GetValues(type).Length;

			for (int i = 0; i < length; i++)
				if (values[i] == null)
					throw new InvalidProgramException(
						string.Format(@"Converter value {0} not defined for type {1}", i, type.Name));
		}
	}
}
