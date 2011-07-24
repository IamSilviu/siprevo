using System;
using System.Net;
using System.Text;

namespace Sip.Message
{
	public static class Converters
	{
		private static readonly byte[][] lowerTransport;

		static Converters()
		{
			lowerTransport = new byte[Enum.GetValues(typeof(Transports)).Length][];

			InitializeLowerTransport();

			Verify(lowerTransport, typeof(Transports));
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

		public static byte[] ToLowerUtf8Bytes(this Transports transport)
		{
			return lowerTransport[(int)transport];
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

		private static void InitializeLowerTransport()
		{
			lowerTransport[(int)Transports.None] = new byte[0];
			lowerTransport[(int)Transports.Other] = new byte[0];
			lowerTransport[(int)Transports.Sctp] = Encoding.UTF8.GetBytes(@"sctp");
			lowerTransport[(int)Transports.Tcp] = Encoding.UTF8.GetBytes(@"tcp");
			lowerTransport[(int)Transports.Tls] = Encoding.UTF8.GetBytes(@"tls");
			lowerTransport[(int)Transports.Udp] = Encoding.UTF8.GetBytes(@"udp");
		}

		private static void Verify(byte[][] values, Type type)
		{
			int length = Enum.GetValues(typeof(Transports)).Length;

			for (int i = 0; i < length; i++)
				if (values[i] == null)
					throw new InvalidProgramException(
						string.Format(@"Converter value {0} not defined for type {1}", i, type.Name));
		}
	}
}
