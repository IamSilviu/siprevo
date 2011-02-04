using System;
using System.Net;

namespace Sip.Message
{
	public static class Converters
	{
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

		public static ByteArrayPart ToByteArrayPart(this string text)
		{
			return new ByteArrayPart(text);
		}
	}
}
