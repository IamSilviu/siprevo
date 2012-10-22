using System;
using System.Text;

namespace Http.Message
{
	public partial class HttpMessageWriter
	{
		public static class C
		{
			public readonly static byte[] HTTP_1_0_ = Create(@"HTTP/1.0 ");
			public readonly static byte[] HTTP_1_1_ = Create(@"HTTP/1.1 ");

			public readonly static byte[] Connection__Upgrade__ = Create("Connection: Upgrade\r\n");
			public readonly static byte[] Upgrade__websocket__ = Create("Upgrade: websocket\r\n");
			public readonly static byte[] Connection__close__ = Create("Connection: close\r\n");
			public readonly static byte[] Content_Type__text_html__charset_utf_8__ = Create("Content-Type: text/html; charset=utf-8\r\n");

			public readonly static byte[] Sec_WebSocket_Accept__ = Create("Sec-WebSocket-Accept: ");
			public readonly static byte[] Sec_WebSocket_Protocol__ = Create("Sec-WebSocket-Protocol: ");
			public readonly static byte[] Content_Length__ = Create("Content-Length: ");
			public readonly static byte[] Set_Cookie__ = Create("Set-Cookie: ");
			public readonly static byte[] Content_Type__ = Create("Content-Type: ");
			public readonly static byte[] Cache_Control__no_cache__ = Create("Cache-Control: no-cache\r\n");
			public readonly static byte[] x_Error_Details__ = Create(@"x-Error-Details: ");

			public readonly static byte[] WWW_Authenticate__Digest_ = Create("WWW-Authenticate: Digest ");
			public readonly static byte[] realm__ = Create("realm=\"");
			public readonly static byte[] __nonce__ = Create(" ,nonce=\"");
			public readonly static byte[] __algorithm_MD5 = Create(" ,algorithm=MD5");


			public readonly static byte[] text_html__charset_utf_8__ = Create("text/html; charset=utf-8\r\n");
			public readonly static byte[] text_html__ = Create("text/html\r\n");
			public readonly static byte[] text_javascript__ = Create("text/javascript\r\n");
			public readonly static byte[] text_plain__ = Create("text/plain\r\n");
			public readonly static byte[] text_xml__ = Create("text/xml\r\n");
			public readonly static byte[] text_css__ = Create("text/css\r\n");
			public readonly static byte[] image_gif__ = Create("image/gif\r\n");
			public readonly static byte[] image_jpeg__ = Create("image/jpeg\r\n");
			public readonly static byte[] image_png__ = Create("image/png\r\n");
			public readonly static byte[] image_tiff__ = Create("image/tiff\r\n");
			public readonly static byte[] application_json__ = Create("application/json\r\n");
			public readonly static byte[] application_xml__ = Create("application/xml\r\n");
			public readonly static byte[] application_javascript__ = Create("application/javascript\r\n");
			public readonly static byte[] application_xcap_caps_xml__ = Create("application/xcap-caps+xml\r\n");
			public readonly static byte[] application_resource_lists_xml__ = Create("application/resource-lists+xml\r\n");
			public readonly static byte[] application_rls_services_xml__ = Create("application/rls-services+xml\r\n");
			public readonly static byte[] application_auth_policy_xml__ = Create("application/auth-policy+xml\r\n");
			public readonly static byte[] application_xcap_error_xml__ = Create("application/xcap-error+xml\r\n");

			public readonly static byte[] SP = Create(' ');
			public readonly static byte[] CRLF = Create("\r\n");
			public readonly static byte[] DQUOTE = Create('"');
			public readonly static byte[] HCOLON = Create(':');
			public readonly static byte[] LAQUOT = Create('<');
			public readonly static byte[] RAQUOT = Create('>');
			public readonly static byte[] EQUAL = Create('=');
			public readonly static byte[] SEMI = Create(';');
			public readonly static byte[] COMMA = Create(',');
			public readonly static byte[] SLASH = Create('/');
			public readonly static byte[] BACKSLASH = Create('\\');

			public readonly static byte[] Digest = Create(@"Digest");
			public readonly static byte[] NTLM = Create(@"NTLM");
			public readonly static byte[] Kerberos = Create(@"Kerberos");
			public readonly static byte[] WWW_Authenticate = Create(@"WWW-Authenticate");
			public readonly static byte[] Proxy_Authenticate = Create(@"Proxy-Authenticate");
			public readonly static byte[] realm = Create(@"realm");
			public readonly static byte[] nonce = Create(@"nonce");
			public readonly static byte[] qop = Create(@"qop");
			public readonly static byte[] auth = Create(@"auth");
			public readonly static byte[] auth_int = Create(@"auth-int");
			public readonly static byte[] algorithm = Create(@"algorithm");
			public readonly static byte[] MD5 = Create(@"MD5");
			public readonly static byte[] MD5_sess = Create(@"MD5-sess");
			public readonly static byte[] stale = Create(@"stale");
			public readonly static byte[] _true = Create(@"true");
			public readonly static byte[] _false = Create(@"false");
			public readonly static byte[] opaque = Create(@"opaque");

			public static byte[] Create(string text)
			{
				return Encoding.UTF8.GetBytes(text);
			}

			public static byte[] Create(char simbol)
			{
				return Encoding.UTF8.GetBytes(@"" + simbol);
			}
		}
	}
}