
namespace Sip.Message
{
	static class SipMessage
	{
		static SipMessage()
		{
			BufferManager = new BufferManager();
		}

		public static IBufferManager BufferManager { get; set; }
	}
}
