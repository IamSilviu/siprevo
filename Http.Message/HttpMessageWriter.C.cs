using System;
using System.Text;

namespace Http.Message
{
	public partial class HttpMessageWriter
	{
		public static class C
		{
			public readonly static byte[] SP = Create(@" ");
			public readonly static byte[] EQUAL = Create(@"=");
			public readonly static byte[] CRLF = Create("\r\n");

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

			public readonly static byte[] text_html__charset_utf_8__ = Create("text/html; charset=utf-8\r\n");
			public readonly static byte[] text_html__ = Create("text/html\r\n");
			public readonly static byte[] text_javascript__ = Create("text/javascript\r\n");
			public readonly static byte[] text_plain__ = Create("text/plain\r\n");
			public readonly static byte[] text_css__ = Create("text/css\r\n");
			public readonly static byte[] image_gif__ = Create("image/gif\r\n");
			public readonly static byte[] image_jpeg__ = Create("image/jpeg\r\n");
			public readonly static byte[] image_png__ = Create("image/png\r\n");
			public readonly static byte[] image_tiff__ = Create("image/tiff\r\n");

			public static byte[] Create(string text)
			{
				return Encoding.UTF8.GetBytes(text);
			}
		}
	}
}