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

		[Test]
		public void It_should_test_equality_by_value()
		{
			var part1 = new ByteArrayPart(@"1234567890");
			var part2 = new ByteArrayPart(@"1234567890");
			var part3 = new ByteArrayPart(@"123456789-");
			var part4 = new ByteArrayPart(@"123456789");
			var part5 = new ByteArrayPart(@"12345678901");

			Assert.IsTrue(part1.IsEqualValue(part2));
			Assert.IsFalse(part1.IsEqualValue(part3));
			Assert.IsFalse(part1.IsEqualValue(part4));
			Assert.IsFalse(part1.IsEqualValue(part5));

			Assert.IsTrue(part1 == part2);
			Assert.IsTrue(part1 != part3);
			Assert.IsTrue(part1 != part4);
			Assert.IsTrue(part1 != part5);

			var bytes2 = Encoding.UTF8.GetBytes(@"1234567890");
			var bytes3 = Encoding.UTF8.GetBytes(@"123456789-");
			var bytes4 = Encoding.UTF8.GetBytes(@"123456789");
			var bytes5 = Encoding.UTF8.GetBytes(@"12345678901");

			Assert.IsTrue(part1.IsEqualValue(bytes2));
			Assert.IsFalse(part1.IsEqualValue(bytes3));
			Assert.IsFalse(part1.IsEqualValue(bytes4));
			Assert.IsFalse(part1.IsEqualValue(bytes5));
		}

		[Test]
		public void It_should_return_valid_hash_code()
		{
			var part1 = new ByteArrayPart();
			Assert.AreEqual(0, part1.GetHashCode());

			var part2 = new ByteArrayPart(new byte[] { 0, 1, 2, 3, }, 0, 4);
			Assert.AreEqual(0x00010203, part2.GetHashCode());

			var part3 = new ByteArrayPart(new byte[] { 0, 1, 2, 3, 4, 5, 6, }, 0, 7);
			Assert.AreEqual(0x00020406, part3.GetHashCode());

			var part4 = new ByteArrayPart(new byte[] { 0, 1, 2, 3, 4, 5, }, 0, 6);
			Assert.AreEqual(0x00010305, part4.GetHashCode());
		}
	}
}
