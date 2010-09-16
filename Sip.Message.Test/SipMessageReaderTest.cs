using System.Text;
using Sip.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sip.Message.Test
{
	[TestClass()]
	public class SipMessageReaderTest
	{
		UTF8Encoding _utf = new UTF8Encoding();

		[TestMethod()]
		public void ParseTest()
		{
			//SipMessageReader reader = new SipMessageReader();
			//reader.SetDefaultValues();
			//byte[] bytes = _utf.GetBytes("INVITE sip:callee@domain.com SIP/2.0\r\nCSeq:25 INVITE\r\nMax-Forwards :70\r\n");
			//reader.Parse(bytes, 0, bytes.Length);

			//Assert.AreEqual<Methods>(Methods.Invitem, reader.FirstLine.Method);
			//Assert.AreEqual<Methods>(Methods.Invitem, reader.Cseq.Method);
			//Assert.AreEqual(25, reader.Cseq.Value);
			//Assert.AreEqual(20, reader.FirstLine.SipVersion);
			//Assert.AreEqual(70, reader.MaxForwards.Value);
			//Assert.AreEqual("sip:callee@domain.com", reader.FirstLine.GetRequestUriString(bytes));

			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}
	}
}
