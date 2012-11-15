#if BASEMESSAGE
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Base.Message
{
	public abstract class ByteArrayWriter
	{
		protected int end;
		protected int begin;
		protected ArraySegment<byte> segment;

		public ByteArrayWriter(ArraySegment<byte> segment)
			: this(0, segment)
		{
		}

		public ByteArrayWriter(int reservAtBegin, ArraySegment<byte> segment)
		{
			this.end = this.begin = reservAtBegin;
			this.segment = segment;
		}

		protected abstract void Reallocate(ref ArraySegment<byte> segment, int extraSize);

		//// temporary - should be removed for optimization
		//~ByteArrayWriter()
		//{
		//    SipMessage.BufferManager.Free(ref segment);
		//}

		//public void Dispose()
		//{
		//    SipMessage.BufferManager.Free(ref segment);
		//    GC.SuppressFinalize(this);
		//}

		public ArraySegment<byte> Detach()
		{
			var oldSegment = segment;
			segment = new ArraySegment<byte>();
			return oldSegment;
		}

		public ArraySegment<byte> Segment
		{
			get { return segment; }
		}

		public int Count
		{
			get { return end - begin; }
		}

		public int Offset
		{
			get { return segment.Offset + begin; }
		}

		public int End
		{
			get { return segment.Offset + end; }
		}

		public int OffsetOffset
		{
			get { return begin; }
		}

		public byte[] Buffer
		{
			get { return segment.Array; }
		}

		public void AddCount(int length)
		{
			end += length;
		}

		public ByteArrayPart ToByteArrayPart()
		{
			return new ByteArrayPart()
			{
				Bytes = segment.Array,
				Begin = segment.Offset + begin,
				End = segment.Offset + end
			};
		}

		public void Write(ByteArrayPart part)
		{
			ValidateCapacity(part.Length);

			System.Buffer.BlockCopy(part.Bytes, part.Offset, segment.Array, segment.Offset + end, part.Length);
			end += part.Length;
		}

		public void Write(ArraySegment<byte> source)
		{
			if (source.Count > 0 && source.Array != null)
			{
				ValidateCapacity(source.Count);

				System.Buffer.BlockCopy(source.Array, source.Offset, segment.Array, segment.Offset + end, source.Count);
				end += source.Count;
			}
		}

		public void Write(string value)
		{
			int count = Encoding.UTF8.GetByteCount(value);

			ValidateCapacity(count);

			Encoding.UTF8.GetBytes(value, 0, value.Length, segment.Array, segment.Offset + end);
			end += count;
		}

		public void Write(Int32 value)
		{
			ValidateCapacity(11);

			if (value < 0)
			{
				segment.Array[segment.Offset + end++] = (byte)0x2D;
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
					segment.Array[segment.Offset + end++] = (byte)(0x30 + digit);

				value %= denominator;
			}

			segment.Array[segment.Offset + end++] = (byte)(0x30 + value);
		}

		public void Write(byte value)
		{
			ValidateCapacity(3);

			bool print = false;

			for (byte denominator = 100; denominator >= 10; denominator /= 10)
			{
				byte digit = (byte)(value / denominator);

				if (print = print || digit > 0)
					segment.Array[segment.Offset + end++] = (byte)(0x30 + digit);

				value %= denominator;
			}

			segment.Array[segment.Offset + end++] = (byte)(0x30 + value);
		}

		public void Write(byte[] bytes)
		{
			ValidateCapacity(bytes.Length);

			System.Buffer.BlockCopy(bytes, 0, segment.Array, segment.Offset + end, bytes.Length);
			end += bytes.Length;
		}

		public void Write(IPEndPoint endpoint)
		{
			Write(endpoint.Address);
			segment.Array[segment.Offset + end++] = (byte)0x3A;
			Write(endpoint.Port);
		}

		//public void WriteAsTag(IPEndPoint endpoint)
		//{
		//    ValidateCapacity(10);
		//    segment.Array[segment.Offset + end++] = (byte)0x69;

		//    if (endpoint.Address.AddressFamily == AddressFamily.InterNetwork)
		//    {
		//        WriteAsHex(new ArraySegment<byte>(endpoint.Address.GetAddressBytes()));
		//    }
		//    else
		//    {
		//        var bytes = Encoding.UTF8.GetBytes(endpoint.Address.ToString());
		//        for (int i = 0; i < bytes.Length; i++)
		//            if (bytes[i] != 0x3A)
		//                WriteAsHex(bytes[i]);
		//    }

		//    Write(endpoint.Port);
		//}

		public void Write(IPAddress address)
		{
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				var bytes = address.GetAddressBytes();

				Write(bytes[0]);
				segment.Array[segment.Offset + end++] = (byte)0x2E;
				Write(bytes[1]);
				segment.Array[segment.Offset + end++] = (byte)0x2E;
				Write(bytes[2]);
				segment.Array[segment.Offset + end++] = (byte)0x2E;
				Write(bytes[3]);
			}
			else
			{
				Write(Encoding.UTF8.GetBytes(address.ToString()));
			}
		}

		public void WriteAsHex8(int value)
		{
			ValidateCapacity(8);

			end += 8;

			for (int i = 1; i < 9; i++, value >>= 4)
				segment.Array[segment.Offset + end - i] = GetLowerHexChar((byte)(value & 0x0f));
		}

		public void WriteAsHex(ArraySegment<byte> data)
		{
			ValidateCapacity(data.Count * 2);

			for (int i = 0; i < data.Count; i++)
			{
				segment.Array[segment.Offset + end] = GetLowerHexChar((byte)(data.Array[data.Offset + i] >> 4));
				end++;

				segment.Array[segment.Offset + end] = GetLowerHexChar((byte)(data.Array[data.Offset + i] & 0x0f));
				end++;
			}
		}

		public void WriteAsHex(byte data)
		{
			ValidateCapacity(2);

			segment.Array[segment.Offset + end] = GetLowerHexChar((byte)(data >> 4));
			end++;

			segment.Array[segment.Offset + end] = GetLowerHexChar((byte)(data & 0x0f));
			end++;
		}

		public void WriteAsBase64(ArraySegment<byte> data)
		{
			ValidateCapacity(Base64Encoding.GetEncodedLength(data.Count));

			end += Base64Encoding.Encode(data, segment.Array, segment.Offset + end);
		}

		public ArraySegment<byte> GetBytesForCustomWrite(int size)
		{
			ValidateCapacity(size);

			end += size;

			return new ArraySegment<byte>(
				segment.Array, segment.Offset + end - size, segment.Offset + end);
		}

		public void ValidateCapacity(int extraSize)
		{
			if ((end + extraSize) > segment.Count)
				Reallocate(ref segment, extraSize);
		}

		public static byte GetLowerHexChar(byte digit)
		{
			switch (digit)
			{
				case 0: return 0x30;
				case 1: return 0x31;
				case 2: return 0x32;
				case 3: return 0x33;
				case 4: return 0x34;
				case 5: return 0x35;
				case 6: return 0x36;
				case 7: return 0x37;
				case 8: return 0x38;
				case 9: return 0x39;

				case 10: return 0x61;
				case 11: return 0x62;
				case 12: return 0x63;
				case 13: return 0x64;
				case 14: return 0x65;
				case 15: return 0x66;

				default:
					throw new ArgumentOutOfRangeException(@"digit");
			}
		}

		#region public WriteToTop methods

		public void WriteToTop(ByteArrayPart part)
		{
			WriteToTop(part, -1);
		}

		public void WriteToTop(ByteArrayPart part, int ignoreAfter)
		{
			int length = (ignoreAfter > 0 && ignoreAfter < part.Length) ? ignoreAfter : part.Length;

			ValidateCapacityToTop(length);

			begin -= length;
			System.Buffer.BlockCopy(part.Bytes, part.Offset, segment.Array, segment.Offset + begin, length);
		}

		public void WriteToTop(Int32 value)
		{
			ValidateCapacityToTop(11);

			if (value < 0)
			{
				WriteToTop((UInt32)(-value));
				segment.Array[segment.Offset + --begin] = (byte)0x2D;
			}
			else
			{
				WriteToTop((UInt32)value);
			}
		}

		public void WriteToTop(UInt32 value)
		{
			ValidateCapacityToTop(10);
			ReversWrite(value, ref begin);
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
			if (begin < extraSize)
				throw new ArgumentOutOfRangeException(@"Not enougth space was reserved at begin");
		}

		#endregion

		#region public void Write(params object[] parts) {...}

		public void Write(byte[] part1, byte[] part2)
		{
			Write(part1);
			Write(part2);
		}

		public void Write(byte[] part1, byte[] part2, byte[] part3)
		{
			Write(part1);
			Write(part2);
			Write(part3);
		}

		public void Write(byte[] part1, byte[] part2, byte[] part3, byte[] part4)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
		}

		public void Write(byte[] part1, byte[] part2, byte[] part3, byte[] part4, byte[] part5)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
		}

		public void Write(byte[] part1, byte[] part2, byte[] part3, byte[] part4, byte[] part5, byte[] part6)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
			Write(part6);
		}

		public void Write(ByteArrayPart part1, ByteArrayPart part2)
		{
			Write(part1);
			Write(part2);
		}

		public void Write(ByteArrayPart part1, int part2)
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

		public void Write(ByteArrayPart part1, ByteArrayPart part2, int part3)
		{
			Write(part1);
			Write(part2);
			Write(part3);
		}

		public void Write(ByteArrayPart part1, int part2, ByteArrayPart part3)
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
			return Encoding.UTF8.GetString(segment.Array, segment.Offset + begin, Count);
		}
	}
}

#endif