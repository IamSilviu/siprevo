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
	}
}
