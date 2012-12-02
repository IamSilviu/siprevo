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
					case ContentType.ApplicationXcapCapsXml:
						Write(C.application_xcap_caps_xml__);
						break;
					case ContentType.ApplicationResourceListsXml:
						Write(C.application_resource_lists_xml__);
						break;
					case ContentType.ApplicationRlsServicesXml:
						Write(C.application_rls_services_xml__);
						break;
					case ContentType.ApplicationAuthPolicyXml:
						Write(C.application_auth_policy_xml__);
						break;
					case ContentType.ApplicationXcapErrorXml:
						Write(C.application_xcap_error_xml__);
						break;
					case ContentType.ApplicationPidfXml:
						Write(C.application_pidf_xml__);
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

		public void WriteAuthenticateDigest(bool proxy, ByteArrayPart realm, int nonce1, int nonce2, int nonce3, int nonce4, bool authint, bool stale, int opaque)
		{
			Write(proxy ? C.Proxy_Authenticate : C.WWW_Authenticate, C.HCOLON, C.SP, C.Digest, C.SP);

			Write(C.realm, C.EQUAL, C.DQUOTE);
			Write(realm);
			Write(C.DQUOTE, C.COMMA);
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
			Write(C.stale, C.EQUAL, stale ? C.@true : C.@false, C.COMMA);
			Write(C.opaque, C.EQUAL, C.DQUOTE);
			WriteAsHex8(opaque);
			Write(C.DQUOTE);

			Write(C.CRLF);
		}

		public void WriteXErrorDetails(byte[] details)
		{
			Write(C.x_Error_Details__);
			Write(details);
			Write(C.CRLF);
		}

		public void WriteAccessControlAllowOrigin(bool anyOrNone)
		{
			Write(C.Access_Control_Allow_Origin__);
			Write(anyOrNone ? C.STAR : C.@null);
			Write(C.CRLF);
		}

		public void WriteAccessControlAllowCredentials(bool allow)
		{
			Write(C.Access_Control_Allow_Credentials__);
			Write(allow ? C.@true : C.@false);
			Write(C.CRLF);
		}

		public void WriteAccessControlMaxAge(int age)
		{
			Write(C.Access_Control_Max_Age__);
			Write(age);
			Write(C.CRLF);
		}

		#region public void WriteAccessControlAllowMethods(...)

		public void WriteAccessControlAllowMethods(Methods method1)
		{
			Write(C.Access_Control_Allow_Methods__);
			Write(method1.ToUtf8Bytes());
			Write(C.CRLF);
		}

		public void WriteAccessControlAllowMethods(Methods method1, Methods method2)
		{
			Write(C.Access_Control_Allow_Methods__);
			Write(method1.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method2.ToUtf8Bytes());
			Write(C.CRLF);
		}

		public void WriteAccessControlAllowMethods(Methods method1, Methods method2, Methods method3)
		{
			Write(C.Access_Control_Allow_Methods__);
			Write(method1.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method2.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method3.ToUtf8Bytes());
			Write(C.CRLF);
		}

		public void WriteAccessControlAllowMethods(Methods method1, Methods method2, Methods method3, Methods method4)
		{
			Write(C.Access_Control_Allow_Methods__);
			Write(method1.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method2.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method3.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method4.ToUtf8Bytes());
			Write(C.CRLF);
		}

		public void WriteAccessControlAllowMethods(Methods method1, Methods method2, Methods method3, Methods method4, Methods method5)
		{
			Write(C.Access_Control_Allow_Methods__);
			Write(method1.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method2.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method3.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method4.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method5.ToUtf8Bytes());
			Write(C.CRLF);
		}

		public void WriteAccessControlAllowMethods(params Methods[] methods)
		{
			Write(C.Access_Control_Allow_Methods__);
			for (int i = 0; i < methods.Length; i++)
			{
				if (i > 0)
					Write(C.COMMA);
				Write(methods[i].ToUtf8Bytes());
			}
			Write(C.CRLF);
		}

		#endregion

		public void WriteAccessControlAllowHeaders(byte[] value)
		{
			Write(C.Access_Control_Allow_Headers__);
			Write(value);
			Write(C.CRLF);
		}

		public void WriteAccessControlExposeHeaders(byte[] value)
		{
			Write(C.Access_Control_Expose_Headers__);
			Write(value);
			Write(C.CRLF);
		}

		#region public void WriteAllow(...)

		public void WriteAllow(Methods method1)
		{
			Write(C.Allow__);
			Write(method1.ToUtf8Bytes());
			Write(C.CRLF);
		}

		public void WriteAllow(Methods method1, Methods method2)
		{
			Write(C.Allow__);
			Write(method1.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method2.ToUtf8Bytes());
			Write(C.CRLF);
		}

		public void WriteAllow(Methods method1, Methods method2, Methods method3)
		{
			Write(C.Allow__);
			Write(method1.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method2.ToUtf8Bytes());
			Write(C.COMMA);
			Write(method3.ToUtf8Bytes());
			Write(C.CRLF);
		}

		public void WriteAllow(params Methods[] methods)
		{
			Write(C.Allow__);
			for (int i = 0; i < methods.Length; i++)
			{
				if (i > 0)
					Write(C.COMMA);
				Write(methods[i].ToUtf8Bytes());
			}
			Write(C.CRLF);
		}

		#endregion

		public void WriteEtag(int etag)
		{
			Write(C.ETag__);
			Write(C.DQUOTE);
			WriteAsHex8(etag);
			Write(C.DQUOTE);
			Write(C.CRLF);
		}

		public void WriteLocation(bool httpOrHttps, ByteArrayPart host, int port, byte[] extra)
		{
			Write(C.Location__);
			Write(httpOrHttps ? C.http___ : C.https___);
			if (host.IsValid)
			{
				Write(host);
				if (port > 0)
				{
					Write(C.HCOLON);
					Write(port);
				}
			}
			if (extra != null)
				Write(extra);
			Write(C.CRLF);
		}

		public void WriteResponse(StatusCodes statusCode)
		{
			WriteStatusLine(statusCode);
			WriteContentLength(0);
			WriteCRLF();
		}
	}
}
