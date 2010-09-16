using System;
using Server.Memory;

namespace Sip.Message
{
	public abstract partial class SipMessageWriter
		: IDefaultValue
	{
		protected static class H
		{
			public readonly static IByteArrayPart From = new ByteArrayPart(@"From");
			public readonly static IByteArrayPart To = new ByteArrayPart(@"To");
			public readonly static IByteArrayPart CallId = new ByteArrayPart(@"Call-ID");
			public readonly static IByteArrayPart CSeq = new ByteArrayPart(@"CSeq");
			public readonly static IByteArrayPart MaxForwards = new ByteArrayPart(@"Max-Forwards");

			public readonly static IByteArrayPart Register = new ByteArrayPart(@"REGISTER");
			public readonly static IByteArrayPart Invite = new ByteArrayPart(@"INVITE");
			public readonly static IByteArrayPart Options = new ByteArrayPart(@"OPTIONS");
			public readonly static IByteArrayPart Notify = new ByteArrayPart(@"NOTIFY");
			public readonly static IByteArrayPart Cancel = new ByteArrayPart(@"CANCEL");
			public readonly static IByteArrayPart Bye = new ByteArrayPart(@"BYE");
			public readonly static IByteArrayPart Ack = new ByteArrayPart(@"ACK");
			public readonly static IByteArrayPart Subscribe = new ByteArrayPart(@"SUBSCRIBE");

			public readonly static IByteArrayPart SipVersion = new ByteArrayPart(@"SIP/2.0");
			public readonly static IByteArrayPart SP = new ByteArrayPart(' ');
			public readonly static IByteArrayPart CLRF = new ByteArrayPart("\r\n");
			public readonly static IByteArrayPart DQUOTE = new ByteArrayPart('"');
			public readonly static IByteArrayPart HCOLON = new ByteArrayPart(':');
			public readonly static IByteArrayPart LAQUOT = new ByteArrayPart('<');
			public readonly static IByteArrayPart RAQUOT = new ByteArrayPart('>');

			public static IByteArrayPart GetMethod(Methods method)
			{
				switch (method)
				{
					case Methods.Ackm:
						return Ack;
					case Methods.Byem:
						return Bye;
					case Methods.Cancelm:
						return Cancel;
					case Methods.Invitem:
						return Invite;
					case Methods.Notifym:
						return Notify;
					case Methods.Optionsm:
						return Options;
					case Methods.Registerm:
						return Register;
					case Methods.Subscribem:
						return Subscribe;
					case Methods.Extension:
					case Methods.None:
						throw new ArgumentException();
					default:
						throw new NotImplementedException();
				}
			}
		}
	}
}
