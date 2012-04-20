using System.Text;
using Base.Message;

namespace Sip.Message
{
	public static class SipMessage
	{
		public static readonly byte[] MagicCookie = Encoding.UTF8.GetBytes(@"z9hG4bK");

		static SipMessage()
		{
			BufferManager = new BufferManager();
		}

		public static IBufferManager BufferManager { get; set; }
	}
}
