using System;
using System.Net;
using System.Text;
using Sip.Message;
using NUnit.Framework;

namespace SipMessageTest
{
	[TestFixture]
	public class ByteArrayWriterTest
	{
		[Test]
		public void It_should_write_byte_array()
		{
			var writer = new ByteArrayWriter(2);

			var block1 = new byte[] { 0, 1, };
			writer.Write(block1);

			Assert.AreEqual(writer.Segment.Array, block1);

			writer.Write(block1);
			Assert.AreEqual(writer.Segment.Array, new byte[] { 0, 1, 0, 1, });
			Assert.AreEqual(writer.Count, 4);
		}

		[Test]
		public void It_should_write_ByteArrayPart()
		{
			var writer = new ByteArrayWriter(2);

			writer.Write(new ByteArrayPart(new byte[] { 0, 1, 2, 3, 4, 5, }, 2, 2));

			Assert.AreEqual(writer.Segment.Array, new byte[] { 2, 3, });
			Assert.AreEqual(writer.Count, 2);
		}

		[Test]
		public void It_should_write_UInt32()
		{
			var writer = new ByteArrayWriter(11);

			writer.Write(UInt32.MinValue);
			writer.Write(UInt32.MaxValue);

			var actual = GetWritedArrayPart(writer);
			Assert.AreEqual(new byte[] { 48, 52, 50, 57, 52, 57, 54, 55, 50, 57, 53, }, actual);
		}

		[Test]
		public void It_should_write_Int32()
		{
			var expeted = Encoding.UTF8.GetBytes("-214748364802147483647-1234567890");

			var writer = new ByteArrayWriter(expeted.Length);

			writer.Write(Int32.MinValue);
			writer.Write(0);
			writer.Write(Int32.MaxValue);
			writer.Write(-1234567890);

			var actual = GetWritedArrayPart(writer);
			Assert.AreEqual(expeted, actual);
		}

		[Test]
		public void It_should_write_to_top_Int32()
		{
			var expeted = Encoding.UTF8.GetBytes("-214748364802147483647-1234567890");

			var writer = new ByteArrayWriter(256, 1024);

			writer.WriteToTop(-1234567890);
			writer.WriteToTop(Int32.MaxValue);
			writer.WriteToTop(0);
			writer.WriteToTop(Int32.MinValue);

			var actual = GetWritedArrayPart(writer);
			Assert.AreEqual(expeted, actual);
		}

		[Test]
		public void It_should_write_IPAddressV4()
		{
			var expeted = Encoding.UTF8.GetBytes("192.168.1.2");

			var writer = new ByteArrayWriter(expeted.Length);

			writer.Write(IPAddress.Parse("192.168.1.2"));

			var actual = GetWritedArrayPart(writer);
			Assert.AreEqual(expeted, actual);
		}

		[Test]
		public void It_should_write_IPAddressV6()
		{
			var expeted = Encoding.UTF8.GetBytes("fe80::202:b3ff:fe1e:8329");

			var writer = new ByteArrayWriter(expeted.Length);

			writer.Write(IPAddress.Parse("fe80::202:b3ff:fe1e:8329"));

			var actual = GetWritedArrayPart(writer);
			Assert.AreEqual(expeted, actual);
		}

		[Test]
		public void It_should_write_byte()
		{
			var expeted = Encoding.UTF8.GetBytes("0123255");

			var writer = new ByteArrayWriter(1024);

			writer.Write((byte)byte.MinValue);
			writer.Write((byte)123);
			writer.Write((byte)byte.MaxValue);

			var actual = GetWritedArrayPart(writer);
			Assert.AreEqual(expeted, actual);
		}

		private byte[] GetWritedArrayPart(ByteArrayWriter writer)
		{
			var bytes = new byte[writer.Count];

			Buffer.BlockCopy(writer.Segment.Array, writer.Offset, bytes, 0, bytes.Length);

			return bytes;
		}
	}
}
