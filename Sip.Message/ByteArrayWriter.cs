using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Sip.Message
{
	public class ByteArrayWriter
	{
		protected int _count;
		protected ArraySegment<byte> segment;

		public ByteArrayWriter()
		{
			segment = SipMessage.BufferManager.Allocate(2048);
		}

		public ByteArrayWriter(int size)
		{
			segment = SipMessage.BufferManager.Allocate(size);
		}

		public ArraySegment<byte> Segment
		{
			get { return segment; }
		}

		public int Count
		{
			get { return _count; }
		}

		public ByteArrayPart ToByteArrayPartRef()
		{
			return new ByteArrayPart()
			{
				Bytes = segment.Array,
				Begin = segment.Offset,
				End = segment.Offset + _count
			};
		}

		public void Write(ByteArrayPart part)
		{
			ValidateCapacity(part.Length);

			Buffer.BlockCopy(part.Bytes, part.Offset, segment.Array, segment.Offset + _count, part.Length);
			_count += part.Length;
		}

		public void Write(Int32 value)
		{
			ValidateCapacity(11);

			if (value < 0)
			{
				segment.Array[segment.Offset + _count++] = (byte)0x2D;
				Write((UInt32)(-value));
			}
			else
			{
				Write((UInt32)value);
			}
		}

		public void Write(UInt32 value)
		{
			ValidateCapacity(10);

			bool print = false;

			for (UInt32 denominator = 1000000000; denominator >= 10; denominator /= 10)
			{
				byte digit = (byte)(value / denominator);

				if (print = print || digit > 0)
					segment.Array[segment.Offset + _count++] = (byte)(0x30 + digit);

				value %= denominator;
			}

			segment.Array[segment.Offset + _count++] = (byte)(0x30 + value);
		}

		public void Write(byte value)
		{
			ValidateCapacity(3);

			bool print = false;

			for (byte denominator = 100; denominator >= 10; denominator /= 10)
			{
				byte digit = (byte)(value / denominator);

				if (print = print || digit > 0)
					segment.Array[segment.Offset + _count++] = (byte)(0x30 + digit);

				value %= denominator;
			}

			segment.Array[segment.Offset + _count++] = (byte)(0x30 + value);
		}

		public void Write(byte[] bytes)
		{
			ValidateCapacity(bytes.Length);

			Buffer.BlockCopy(bytes, 0, segment.Array, segment.Offset + _count, bytes.Length);
			_count += bytes.Length;
		}

		public void Write(IPAddress address)
		{
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				var bytes = address.GetAddressBytes();

				Write(bytes[0]);
				segment.Array[segment.Offset + _count++] = (byte)0x2E;
				Write(bytes[1]);
				segment.Array[segment.Offset + _count++] = (byte)0x2E;
				Write(bytes[2]);
				segment.Array[segment.Offset + _count++] = (byte)0x2E;
				Write(bytes[3]);
			}
			else
			{
				Write(Encoding.UTF8.GetBytes(address.ToString()));
			}
		}

		public void ValidateCapacity(int extraSize)
		{
			if ((_count + extraSize) > segment.Count)
				SipMessage.BufferManager.Reallocate(ref segment, extraSize);
		}

		#region public void Write(params object[] parts) {...}

		public void Write(ByteArrayPart part1, ByteArrayPart part2)
		{
			Write(part1);
			Write(part2);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3)
		{
			Write(part1);
			Write(part2);
			Write(part3);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, ByteArrayPart part4)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, int part3, ByteArrayPart part4)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, int part4)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, IPAddress part4)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, int part4, ByteArrayPart part5)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, ByteArrayPart part4, ByteArrayPart part5)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, ByteArrayPart part4, ByteArrayPart part5, ByteArrayPart part6)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
			Write(part6);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, int part4, ByteArrayPart part5, ByteArrayPart part6)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
			Write(part6);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, int part4, ByteArrayPart part5, ByteArrayPart part6, ByteArrayPart part7)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
			Write(part6);
			Write(part7);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, ByteArrayPart part4, ByteArrayPart part5, ByteArrayPart part6, ByteArrayPart part7)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
			Write(part6);
			Write(part7);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, ByteArrayPart part4, ByteArrayPart part5, ByteArrayPart part6, ByteArrayPart part7, ByteArrayPart part8)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
			Write(part6);
			Write(part7);
			Write(part8);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2, ByteArrayPart part3, ByteArrayPart part4, ByteArrayPart part5, ByteArrayPart part6, ByteArrayPart part7, ByteArrayPart part8, ByteArrayPart part9)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
			Write(part6);
			Write(part7);
			Write(part8);
			Write(part9);
		}

		#endregion
	}
}
