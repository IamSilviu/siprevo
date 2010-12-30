using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Sip.Message
{
	public class ByteArrayWriter
	{
		protected int count;
		protected int offsetOffset;
		private ArraySegment<byte> segment;

		public ByteArrayWriter(int size)
		{
			segment = SipMessage.BufferManager.Allocate(size);
		}

		public ByteArrayWriter(int reservAtBegin, int size)
		{
			count = offsetOffset = reservAtBegin;
			segment = SipMessage.BufferManager.Allocate(size);
		}

		public ArraySegment<byte> Segment
		{
			get { return segment; }
		}

		public int Count
		{
			get { return count - offsetOffset; }
		}

		public int Offset
		{
			get { return segment.Offset + offsetOffset; }
		}

		public ByteArrayPart ToByteArrayPart()
		{
			return new ByteArrayPart()
			{
				Bytes = segment.Array,
				Begin = segment.Offset + offsetOffset,
				End = segment.Offset + count
			};
		}

		public void Write(ByteArrayPart part)
		{
			ValidateCapacity(part.Length);

			Buffer.BlockCopy(part.Bytes, part.Offset, segment.Array, segment.Offset + count, part.Length);
			count += part.Length;
		}

		public void Write(Int32 value)
		{
			ValidateCapacity(11);

			if (value < 0)
			{
				segment.Array[segment.Offset + count++] = (byte)0x2D;
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
					segment.Array[segment.Offset + count++] = (byte)(0x30 + digit);

				value %= denominator;
			}

			segment.Array[segment.Offset + count++] = (byte)(0x30 + value);
		}

		public void Write(byte value)
		{
			ValidateCapacity(3);

			bool print = false;

			for (byte denominator = 100; denominator >= 10; denominator /= 10)
			{
				byte digit = (byte)(value / denominator);

				if (print = print || digit > 0)
					segment.Array[segment.Offset + count++] = (byte)(0x30 + digit);

				value %= denominator;
			}

			segment.Array[segment.Offset + count++] = (byte)(0x30 + value);
		}

		public void Write(byte[] bytes)
		{
			ValidateCapacity(bytes.Length);

			Buffer.BlockCopy(bytes, 0, segment.Array, segment.Offset + count, bytes.Length);
			count += bytes.Length;
		}

		public void Write(IPAddress address)
		{
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				var bytes = address.GetAddressBytes();

				Write(bytes[0]);
				segment.Array[segment.Offset + count++] = (byte)0x2E;
				Write(bytes[1]);
				segment.Array[segment.Offset + count++] = (byte)0x2E;
				Write(bytes[2]);
				segment.Array[segment.Offset + count++] = (byte)0x2E;
				Write(bytes[3]);
			}
			else
			{
				Write(Encoding.UTF8.GetBytes(address.ToString()));
			}
		}

		public void ValidateCapacity(int extraSize)
		{
			if ((count + extraSize) > segment.Count)
				SipMessage.BufferManager.Reallocate(ref segment, extraSize);
		}

		public void WriteToTop(ByteArrayPart part)
		{
			WriteToTop(part, -1);
		}

		public void WriteToTop(ByteArrayPart part, int ignoreAfter)
		{
			int length = (ignoreAfter > 0 && ignoreAfter < part.Length) ? ignoreAfter : part.Length;

			ValidateCapacityToTop(length);

			offsetOffset -= length;
			Buffer.BlockCopy(part.Bytes, part.Offset, segment.Array, segment.Offset + offsetOffset, length);
		}

		public void WriteToTop(Int32 value)
		{
			ValidateCapacityToTop(11);

			if (value < 0)
			{
				WriteToTop((UInt32)(-value));
				segment.Array[segment.Offset + --offsetOffset] = (byte)0x2D;
			}
			else
			{
				WriteToTop((UInt32)value);
			}
		}

		public void WriteToTop(UInt32 value)
		{
			ValidateCapacityToTop(10);
			ReversWrite(value, ref offsetOffset);
		}

		protected void ReversWrite(UInt32 value, ref int position)
		{
			do
			{
				byte digit = (byte)(value % 10);

				segment.Array[segment.Offset + --position] = (byte)(0x30 + digit);

				value /= 10;
			}
			while (value > 0);
		}

		public void ValidateCapacityToTop(int extraSize)
		{
			if (offsetOffset < extraSize)
				throw new ArgumentOutOfRangeException(@"Not enougth space was reserved at begin");
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

		public void Write(ByteArrayPart part1, ByteArrayPart part2, int part3, ByteArrayPart part4, ByteArrayPart part5, ByteArrayPart part6)
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

		public override string ToString()
		{
			return Encoding.UTF8.GetString(segment.Array, segment.Offset + offsetOffset, Count);
		}
	}
}
