using System;
using System.Text;
using Sip.Message;
using NUnit.Framework;

namespace SipMessageTest
{
	[TestFixture]
	class ByteArrayPartTest
	{
		[Test]
		public void It_should_trim_sws_at_begin()
		{
			var part1 = new ByteArrayPart("\r\n X");
			part1.TrimStartSws();

			var part2 = new ByteArrayPart(" \r\n X");
			part2.TrimStartSws();

			var part3 = new ByteArrayPart("  \r\n      X");
			part3.TrimStartSws();

			var part4 = new ByteArrayPart("  \r\n ");
			part4.TrimStartSws();

			var part5 = new ByteArrayPart("  \r\n      ");
			part5.TrimStartSws();

			var part6 = new ByteArrayPart("  X");
			part6.TrimStartSws();

			Assert.AreEqual("X", part1.ToString());
			Assert.AreEqual("X", part2.ToString());
			Assert.AreEqual("X", part3.ToString());
			Assert.AreEqual("", part4.ToString());
			Assert.AreEqual("", part5.ToString());
			Assert.AreEqual("X", part6.ToString());
		}

		[Test]
		public void It_should_trim_sws_at_end()
		{
			var part1 = new ByteArrayPart("X\r\n ");
			part1.TrimEndSws();

			var part2 = new ByteArrayPart("X \r\n ");
			part2.TrimEndSws();

			var part3 = new ByteArrayPart("X  \r\n      ");
			part3.TrimEndSws();

			var part4 = new ByteArrayPart("  \r\n ");
			part4.TrimEndSws();

			var part5 = new ByteArrayPart("  \r\n      ");
			part5.TrimEndSws();

			var part6 = new ByteArrayPart("X  ");
			part6.TrimEndSws();

			var part7 = new ByteArrayPart("");
			part7.TrimEndSws();

			Assert.AreEqual("X", part1.ToString());
			Assert.AreEqual("X", part2.ToString());
			Assert.AreEqual("X", part3.ToString());
			Assert.AreEqual("", part4.ToString());
			Assert.AreEqual("", part5.ToString());
			Assert.AreEqual("X", part6.ToString());
			Assert.AreEqual("", part7.ToString());
		}
	}
}
