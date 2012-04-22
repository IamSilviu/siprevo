using System;
using System.Text;
using Base.Message;

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
			public readonly static ByteArrayPart Min_Expires = new ByteArrayPart(@"Min-Expires");
			public readonly static ByteArrayPart Content_Length = new ByteArrayPart(@"Content-Length");
			public readonly static ByteArrayPart Contact = new ByteArrayPart(@"Contact");
			public readonly static ByteArrayPart Contact___ = new ByteArrayPart(@"Contact: <");
			public readonly static ByteArrayPart WWW_Authenticate = new ByteArrayPart(@"WWW-Authenticate");
			public readonly static ByteArrayPart Proxy_Authenticate = new ByteArrayPart(@"Proxy-Authenticate");
			public readonly static ByteArrayPart Authentication_Info = new ByteArrayPart(@"Authentication-Info");
			public readonly static ByteArrayPart Proxy_Authentication_Info = new ByteArrayPart(@"Proxy-Authentication-Info");
			public readonly static ByteArrayPart Authorization = new ByteArrayPart(@"Authorization");
			public readonly static ByteArrayPart Proxy_Authorization = new ByteArrayPart(@"Proxy-Authorization");
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
			public readonly static ByteArrayPart Content_Transfer_Encoding__binary__ = new ByteArrayPart("Content-Transfer-Encoding: binary\r\n");
			public readonly static ByteArrayPart SIP_ETag__ = new ByteArrayPart(@"SIP-ETag: ");
			public readonly static ByteArrayPart x_Error_Details = new ByteArrayPart(@"x-Error-Details");
			public readonly static ByteArrayPart Event__presence = new ByteArrayPart(@"Event: presence");
			public readonly static ByteArrayPart Event__registration = new ByteArrayPart(@"Event: registration");
			public readonly static ByteArrayPart Allow__ = new ByteArrayPart(@"Allow: ");
			public readonly static ByteArrayPart Supported__ms_benotify__ = new ByteArrayPart("Supported: ms-benotify\r\n");
			public readonly static ByteArrayPart At = new ByteArrayPart(@"@");

			public readonly static ByteArrayPart __Digest_username__ = new ByteArrayPart(": Digest username=\"");
			public readonly static ByteArrayPart ___realm__ = new ByteArrayPart("\", realm=\"");
			public readonly static ByteArrayPart ___qop__ = new ByteArrayPart("\", qop=");
			public readonly static ByteArrayPart __algorithm_ = new ByteArrayPart(", algorithm=");
			public readonly static ByteArrayPart __uri__ = new ByteArrayPart(", uri=\"");
			public readonly static ByteArrayPart ___nonce__ = new ByteArrayPart("\", nonce=\"");
			public readonly static ByteArrayPart __nc_ = new ByteArrayPart("\", nc=");
			public readonly static ByteArrayPart __cnonce__ = new ByteArrayPart(", cnonce=\"");
			public readonly static ByteArrayPart ___opaque__ = new ByteArrayPart("\", opaque=\"");
			public readonly static ByteArrayPart ___response__ = new ByteArrayPart("\", response=\"");

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
			public readonly static ByteArrayPart CommaSpace = new ByteArrayPart(@", ");

			public readonly static ByteArrayPart tag = new ByteArrayPart(@"tag");
			public readonly static ByteArrayPart _tag_ = new ByteArrayPart(@";tag=");
			public readonly static ByteArrayPart received = new ByteArrayPart(@"received");
			public readonly static ByteArrayPart ms_received_port = new ByteArrayPart(@"ms-received-port");
			public readonly static ByteArrayPart ms_received_cid = new ByteArrayPart(@"ms-received-cid");
			public readonly static ByteArrayPart _ms_received_cid_ = new ByteArrayPart(@";ms-received-cid=");
			public readonly static ByteArrayPart expires = new ByteArrayPart(@"expires");
			public readonly static ByteArrayPart _expires_ = new ByteArrayPart(@";expires=");
			public readonly static ByteArrayPart Digest = new ByteArrayPart(@"Digest");
			public readonly static ByteArrayPart NTLM = new ByteArrayPart(@"NTLM");
			public readonly static ByteArrayPart Kerberos = new ByteArrayPart(@"Kerberos");
			public readonly static ByteArrayPart realm = new ByteArrayPart(@"realm");
			public readonly static ByteArrayPart nonce = new ByteArrayPart(@"nonce");
			public readonly static ByteArrayPart qop = new ByteArrayPart(@"qop");
			public readonly static ByteArrayPart auth = new ByteArrayPart(@"auth");
			public readonly static ByteArrayPart auth_int = new ByteArrayPart(@"auth-int");
			public readonly static ByteArrayPart algorithm = new ByteArrayPart(@"algorithm");
			public readonly static ByteArrayPart MD5 = new ByteArrayPart(@"MD5");
			public readonly static ByteArrayPart MD5_sess = new ByteArrayPart(@"MD5-sess");
			public readonly static ByteArrayPart stale = new ByteArrayPart(@"stale");
			public readonly static ByteArrayPart _true = new ByteArrayPart(@"true");
			public readonly static ByteArrayPart _false = new ByteArrayPart(@"false");
			public readonly static ByteArrayPart opaque = new ByteArrayPart(@"opaque");
			public readonly static ByteArrayPart targetname = new ByteArrayPart(@"targetname");
			public readonly static ByteArrayPart version = new ByteArrayPart(@"version");
			public readonly static ByteArrayPart snum = new ByteArrayPart(@"snum");
			public readonly static ByteArrayPart _snum__ = new ByteArrayPart(",snum=\"");
			public readonly static ByteArrayPart srand = new ByteArrayPart(@"srand");
			public readonly static ByteArrayPart _srand__ = new ByteArrayPart(",srand=\"");
			public readonly static ByteArrayPart rspauth = new ByteArrayPart(@"rspauth");
			public readonly static ByteArrayPart _rspauth__ = new ByteArrayPart(",rspauth=\"");
			public readonly static ByteArrayPart gssapi_data = new ByteArrayPart(@"gssapi-data");
			public readonly static ByteArrayPart lr = new ByteArrayPart(@"lr");
			public readonly static ByteArrayPart branch = new ByteArrayPart(@"branch");
			public readonly static ByteArrayPart _branch_ = new ByteArrayPart(@";branch=");
			public readonly static ByteArrayPart _branch_z9hG4bK = new ByteArrayPart(@";branch=z9hG4bK");
			public readonly static ByteArrayPart z9hG4bK_NO_TRANSACTION = new ByteArrayPart(@"z9hG4bK.N0.TRAN5ACT10N");
			public readonly static ByteArrayPart epid = new ByteArrayPart(@"epid");
			public readonly static ByteArrayPart _epid_ = new ByteArrayPart(@";epid=");
			public readonly static ByteArrayPart transport = new ByteArrayPart(@"transport");
			public readonly static ByteArrayPart maddr = new ByteArrayPart(@"maddr");
			public readonly static ByteArrayPart active = new ByteArrayPart(@"active");
			public readonly static ByteArrayPart pending = new ByteArrayPart(@"pending");
			public readonly static ByteArrayPart terminated = new ByteArrayPart(@"terminated");
			public readonly static ByteArrayPart __sip_instance___ = new ByteArrayPart(";+sip.instance=\"<");

			//public readonly static ByteArrayPart UDP = new ByteArrayPart(@"UDP");
			//public readonly static ByteArrayPart TCP = new ByteArrayPart(@"TCP");
			//public readonly static ByteArrayPart TLS = new ByteArrayPart(@"TLS");
			//public readonly static ByteArrayPart SCTP = new ByteArrayPart(@"SCTP");

			public readonly static ByteArrayPart udp = new ByteArrayPart(@"udp");
			public readonly static ByteArrayPart tcp = new ByteArrayPart(@"tcp");

			public readonly static ByteArrayPart sip = new ByteArrayPart(@"sip");
			public readonly static ByteArrayPart sips = new ByteArrayPart(@"sips");

			public readonly static ByteArrayPart eventlist = new ByteArrayPart(@"eventlist");
			public readonly static ByteArrayPart ms_benotify = new ByteArrayPart(@"ms-benotify");

			public readonly static ByteArrayPart _________0 = new ByteArrayPart("         0");

			public readonly static ByteArrayPart Content_Type__multipart_related_type___ = new ByteArrayPart("Content-Type: multipart/related;type=\"");
			public readonly static ByteArrayPart __boundary_OFFICESIP2011VITALIFOMINE__ = new ByteArrayPart("\";boundary=OFFICESIP2011VITALIFOMINE\r\n");
			public readonly static ByteArrayPart __OFFICESIP2011VITALIFOMINE__1 = new ByteArrayPart("--OFFICESIP2011VITALIFOMINE\r\n");
			public readonly static ByteArrayPart __OFFICESIP2011VITALIFOMINE__2 = new ByteArrayPart("--OFFICESIP2011VITALIFOMINE--");

			public readonly static byte[] i = Create(@"i");
			public readonly static byte[] _invalid = Create(@".invalid");

			public static byte[] Create(string text)
			{
				return Encoding.UTF8.GetBytes(text);
			}
		}
	}
}
