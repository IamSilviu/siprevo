using System;
using Base.Message;

namespace Http.Message
{
	public class HttpMessage
	{
		static HttpMessage()
		{
			BufferManager = new BufferManager();
		}

		public static IBufferManager BufferManager { get; set; }
	}
}
