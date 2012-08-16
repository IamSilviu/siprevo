using System;
using System.Collections.Generic;
using Base.Message;

namespace Http.Message
{
	public partial class HttpMessageWriter
		: ByteArrayWriter
		, IDisposable
	{
		public HttpMessageWriter()
			: this(0, 2048)
		{
		}

		public HttpMessageWriter(int size)
			: this(0, size)
		{
		}

		public HttpMessageWriter(int reservAtBegin, int size)
			: base(reservAtBegin, HttpMessage.BufferManager.Allocate(size))
		{
		}

		//// temporary - should be removed for optimization
		//~SipMessageWriter()
		//{
		//    SipMessage.BufferManager.Free(ref segment);
		//}

		public void Dispose()
		{
			HttpMessage.BufferManager.Free(ref segment);
			//GC.SuppressFinalize(this);
		}

		protected override void Reallocate(ref ArraySegment<byte> segment, int extraSize)
		{
			HttpMessage.BufferManager.Reallocate(ref segment, extraSize);
		}

		public void WriteStatusLine(StatusCodes statusCode)
		{
			Write(C.HTTP_1_1_);
			Write((int)statusCode);
			Write(C.SP);
			Write(statusCode.GetReason());
			Write(C.CRLF);
		}

		public void WriteCRLF()
		{
			Write(C.CRLF);
		}

		public void WriteContentLength(int length)
		{
			Write(C.Content_Length__);
			Write(length);
			Write(C.CRLF);
		}

		public void WriteConnectionClose()
		{
			Write(C.Connection__close__);
		}

		public void WriteContentType(ContentType contentType)
		{
			if (contentType != ContentType.None)
			{
				Write(C.Content_Type__);

				switch (contentType)
				{
					case ContentType.TextHtmlUtf8:
						Write(C.text_html__charset_utf_8__);
						break;
					case ContentType.TextHtml:
						Write(C.text_html__);
						break;
					case ContentType.TextJavascript:
						Write(C.text_javascript__);
						break;
					case ContentType.TextPlain:
						Write(C.text_plain__);
						break;
					case ContentType.TextXml:
						Write(C.text_xml__);
						break;
					case ContentType.TextCss:
						Write(C.text_css__);
						break;
					case ContentType.ImageGif:
						Write(C.image_gif__);
						break;
					case ContentType.ImageJpeg:
						Write(C.image_jpeg__);
						break;
					case ContentType.ImagePng:
						Write(C.image_png__);
						break;
					case ContentType.ImageTiff:
						Write(C.image_tiff__);
						break;
					case ContentType.ApplicationJson:
						Write(C.application_json__);
						break;
					case ContentType.ApplicationXml:
						Write(C.application_xml__);
						break;
					case ContentType.ApplicationJavascript:
						Write(C.application_javascript__);
						break;
					default:
						throw new ArgumentOutOfRangeException(contentType.ToString());
				}
			}
		}

		public void WriteContentTypeHtmlUtf8()
		{
			Write(C.Content_Type__text_html__charset_utf_8__);
		}

		public void WriteConnectionUpgrade()
		{
			Write(C.Connection__Upgrade__);
		}

		public void WriteUpgradeWebsocket()
		{
			Write(C.Upgrade__websocket__);
		}

		public void WriteSecWebSocketAccept(byte[] value)
		{
			Write(C.Sec_WebSocket_Accept__);
			WriteAsBase64(new ArraySegment<byte>(value));
			Write(C.CRLF);
		}

		public void WriteSecWebSocketProtocol(byte[] protocol)
		{
			Write(C.Sec_WebSocket_Protocol__);
			Write(protocol);
			Write(C.CRLF);
		}

		public void WriteSetCookie(byte[] name, int value)
		{
			Write(C.Set_Cookie__);
			Write(name);
			Write(C.EQUAL);
			Write(value);
			Write(C.CRLF);
		}

		public void WriteCacheControlNoCache()
		{
			Write(C.Cache_Control__no_cache__);
		}

		public void WriteAuthenticateDigest(byte[] realm, int nonce1, int nonce2, int nonce3, int nonce4)
		//, bool authint, bool stale, int opaque)
		{
			Write(C.WWW_Authenticate__Digest_);

			Write(C.realm__, realm, C.DQUOTE);
			
			Write(C.__nonce__);
			WriteAsHex8(nonce1);
			WriteAsHex8(nonce2);
			WriteAsHex8(nonce3);
			WriteAsHex8(nonce4);
			Write(C.DQUOTE);

			Write(C.__algorithm_MD5);

			//, C.COMMA);
			//Write(C.qop, C.EQUAL, C.DQUOTE, C.auth);
			//if (authint)
			//    Write(C.COMMA, C.auth_int);
			//Write(C.DQUOTE, C.COMMA);
			//Write(C.algorithm, C.EQUAL, C.MD5, C.COMMA);
			//Write(C.stale, C.EQUAL, stale ? C._true : C._false, C.COMMA);
			//Write(C.opaque, C.EQUAL, C.DQUOTE);
			//WriteAsHex8(opaque);
			//Write(C.DQUOTE);

			Write(C.CRLF);
		}
	}
}
