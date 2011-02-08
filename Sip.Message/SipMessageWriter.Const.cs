using System;

namespace Sip.Message
{
	public partial class SipMessageWriter
	{
		public static class C
		{
			public readonly static ByteArrayPart From = new ByteArrayPart(@"From");
			public readonly static ByteArrayPart From__ = new ByteArrayPart(@"From: ");
			public readonly static ByteArrayPart To = new ByteArrayPart(@"To");
			public readonly static ByteArrayPart To__ = new ByteArrayPart(@"To: ");
			public readonly static ByteArrayPart Call_ID = new ByteArrayPart(@"Call-ID");
			public readonly static ByteArrayPart Call_ID__ = new ByteArrayPart(@"Call-ID: ");
			public readonly static ByteArrayPart CSeq = new ByteArrayPart(@"CSeq");
			public readonly static ByteArrayPart Max_Forwards = new ByteArrayPart(@"Max-Forwards");
			public readonly static ByteArrayPart Expires = new ByteArrayPart(@"Expires");
			public readonly static ByteArrayPart Min_Expires = new ByteArrayPart( @"Min-Expires" );
			public readonly static ByteArrayPart Content_Length = new ByteArrayPart( @"Content-Length" );
			public readonly static ByteArrayPart Contact = new ByteArrayPart(@"Contact");
			public readonly static ByteArrayPart WWW_Authenticate = new ByteArrayPart(@"WWW-Authenticate");
			public readonly static ByteArrayPart Proxy_Authenticate = new ByteArrayPart(@"Proxy-Authenticate");
			public readonly static ByteArrayPart Authentication_Info = new ByteArrayPart(@"Authentication-Info");
			public readonly static ByteArrayPart Proxy_Authentication_Info = new ByteArrayPart(@"Proxy-Authentication-Info");
			public readonly static ByteArrayPart Date = new ByteArrayPart(@"Date");
			public readonly static ByteArrayPart Unsupported = new ByteArrayPart(@"Unsupported");
			public readonly static ByteArrayPart Route = new ByteArrayPart(@"Route");
			public readonly static ByteArrayPart RecordRoute = new ByteArrayPart(@"Record-Route");
			public readonly static ByteArrayPart Via = new ByteArrayPart(@"Via");
			public readonly static ByteArrayPart Via__SIP_2_0_ = new ByteArrayPart(@"Via: SIP/2.0/");
			public readonly static ByteArrayPart Content_Type = new ByteArrayPart(@"Content-Type");
			public readonly static ByteArrayPart Content_Type__ = new ByteArrayPart(@"Content-Type: ");
			public readonly static ByteArrayPart Supported = new ByteArrayPart(@"Supported");
			public readonly static ByteArrayPart Require = new ByteArrayPart(@"Require");
			public readonly static ByteArrayPart Subscription_State = new ByteArrayPart(@"Subscription-State");
			public readonly static ByteArrayPart Subscription_State__ = new ByteArrayPart(@"Subscription-State: ");
			public readonly static ByteArrayPart Content_Transfer_Encoding = new ByteArrayPart(@"Content-Transfer-Encoding");
			public readonly static ByteArrayPart SIP_ETag__ = new ByteArrayPart(@"SIP-ETag: ");
			public readonly static ByteArrayPart x_Error_Details = new ByteArrayPart(@"x-Error-Details");
			public readonly static ByteArrayPart Event__presence = new ByteArrayPart(@"Event: presence");

			public readonly static ByteArrayPart ACK = new ByteArrayPart(@"ACK");
			public readonly static ByteArrayPart BENOTIFY = new ByteArrayPart(@"BENOTIFY");
			public readonly static ByteArrayPart BYE = new ByteArrayPart(@"BYE");
			public readonly static ByteArrayPart CANCEL = new ByteArrayPart(@"CANCEL");
			public readonly static ByteArrayPart INFO = new ByteArrayPart(@"INFO");
			public readonly static ByteArrayPart INVITE = new ByteArrayPart(@"INVITE");
			public readonly static ByteArrayPart MESSAGE = new ByteArrayPart(@"MESSAGE");
			public readonly static ByteArrayPart NOTIFY = new ByteArrayPart(@"NOTIFY");
			public readonly static ByteArrayPart OPTIONS = new ByteArrayPart(@"OPTIONS");
			public readonly static ByteArrayPart REFER = new ByteArrayPart(@"REFER");
			public readonly static ByteArrayPart REGISTER = new ByteArrayPart(@"REGISTER");
			public readonly static ByteArrayPart SERVICE = new ByteArrayPart(@"SERVICE");
			public readonly static ByteArrayPart SUBSCRIBE = new ByteArrayPart(@"SUBSCRIBE");
			public readonly static ByteArrayPart PUBLISH = new ByteArrayPart(@"PUBLISH");

			public readonly static ByteArrayPart SIP_2_0 = new ByteArrayPart(@"SIP/2.0");
			public readonly static ByteArrayPart SP = new ByteArrayPart(' ');
			public readonly static ByteArrayPart CRLF = new ByteArrayPart("\r\n");
			public readonly static ByteArrayPart DQUOTE = new ByteArrayPart('"');
			public readonly static ByteArrayPart HCOLON = new ByteArrayPart(':');
			public readonly static ByteArrayPart LAQUOT = new ByteArrayPart('<');
			public readonly static ByteArrayPart RAQUOT = new ByteArrayPart('>');
			public readonly static ByteArrayPart EQUAL = new ByteArrayPart('=');
			public readonly static ByteArrayPart SEMI = new ByteArrayPart(';');
			public readonly static ByteArrayPart COMMA = new ByteArrayPart(',');
			public readonly static ByteArrayPart SLASH = new ByteArrayPart('/');
			public readonly static ByteArrayPart BACKSLASH = new ByteArrayPart('\\');

			public readonly static ByteArrayPart tag = new ByteArrayPart(@"tag");
			public readonly static ByteArrayPart _tag_ = new ByteArrayPart(@";tag=");
			public readonly static ByteArrayPart received = new ByteArrayPart(@"received");
			public readonly static ByteArrayPart ms_received_port = new ByteArrayPart(@"ms-received-port");
			public readonly static ByteArrayPart ms_received_cid = new ByteArrayPart(@"ms-received-cid");
			public readonly static ByteArrayPart expires = new ByteArrayPart(@"expires");
			public readonly static ByteArrayPart _expires_ = new ByteArrayPart(@";expires=");
			public readonly static ByteArrayPart Digest = new ByteArrayPart(@"Digest");
			public readonly static ByteArrayPart NTLM = new ByteArrayPart(@"NTLM");
			public readonly static ByteArrayPart Kerberos = new ByteArrayPart(@"Kerberos");
			public readonly static ByteArrayPart realm = new ByteArrayPart( @"realm" );
			public readonly static ByteArrayPart nonce = new ByteArrayPart(@"nonce");
			public readonly static ByteArrayPart qop = new ByteArrayPart(@"qop");
			public readonly static ByteArrayPart auth = new ByteArrayPart(@"auth");
			public readonly static ByteArrayPart auth_int = new ByteArrayPart( @"auth-int" );
			public readonly static ByteArrayPart algorithm = new ByteArrayPart(@"algorithm");
			public readonly static ByteArrayPart MD5 = new ByteArrayPart(@"MD5");
			public readonly static ByteArrayPart stale = new ByteArrayPart(@"stale");
			public readonly static ByteArrayPart _true = new ByteArrayPart(@"true");
			public readonly static ByteArrayPart _false = new ByteArrayPart(@"false");
			public readonly static ByteArrayPart opaque = new ByteArrayPart(@"opaque");
			public readonly static ByteArrayPart targetname = new ByteArrayPart(@"targetname");
			public readonly static ByteArrayPart version = new ByteArrayPart(@"version");
			public readonly static ByteArrayPart snum = new ByteArrayPart(@"snum");
			public readonly static ByteArrayPart srand = new ByteArrayPart(@"srand");
			public readonly static ByteArrayPart rspauth = new ByteArrayPart(@"rspauth");
			public readonly static ByteArrayPart gssapi_data = new ByteArrayPart(@"gssapi-data");
			public readonly static ByteArrayPart lr = new ByteArrayPart(@"lr");
			public readonly static ByteArrayPart branch = new ByteArrayPart(@"branch");
			public readonly static ByteArrayPart _branch_ = new ByteArrayPart(@";branch=");
			public readonly static ByteArrayPart epid = new ByteArrayPart(@"epid");
			public readonly static ByteArrayPart transport = new ByteArrayPart(@"transport");
			public readonly static ByteArrayPart maddr = new ByteArrayPart(@"maddr");
			public readonly static ByteArrayPart active = new ByteArrayPart(@"active");
			public readonly static ByteArrayPart pending = new ByteArrayPart(@"pending");
			public readonly static ByteArrayPart terminated = new ByteArrayPart(@"terminated");

			public readonly static ByteArrayPart UDP = new ByteArrayPart(@"UDP");
			public readonly static ByteArrayPart TCP = new ByteArrayPart(@"TCP");
			public readonly static ByteArrayPart TLS = new ByteArrayPart(@"TLS");
			public readonly static ByteArrayPart SCTP = new ByteArrayPart(@"SCTP");

			public readonly static ByteArrayPart udp = new ByteArrayPart(@"udp");
			public readonly static ByteArrayPart tcp = new ByteArrayPart(@"tcp");

			public readonly static ByteArrayPart sip = new ByteArrayPart(@"sip");
			public readonly static ByteArrayPart sips = new ByteArrayPart(@"sips");

			public readonly static ByteArrayPart eventlist = new ByteArrayPart(@"eventlist");
			public readonly static ByteArrayPart ms_benotify = new ByteArrayPart(@"ms-benotify");

			public readonly static ByteArrayPart _________0 = new ByteArrayPart("         0");
		}
	}
}
