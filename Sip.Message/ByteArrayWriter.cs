using System;
using System.Text;
using Server.Memory;

namespace Sip.Message
{
	public class ByteArrayWriter
	{
		protected byte[] _array;
		protected int _length;

		public ByteArrayWriter()
		{
		}

		public void SetArray(byte[] array)
		{
			_array = array;
			_length = 0;
		}

		public void Write(IByteArrayPart part)
		{
			Array.Copy(part.Items, part.Offset, _array, _length, part.Length);
			_length += part.Length;
		}

		public void Write(int value)
		{
			int divider = 1;
			while (divider < value)
				divider *= 10;
			divider /= 10;

			while (divider > 0)
			{
				_array[_length++] = (byte)(value / divider);
				value %= divider;
				divider /= 10;
			}
		}

		public void Write(params IByteArrayPart[] parts)
		{
			for (int i = 0; i < parts.Length; i++)
				Write(parts[i]);
		}

		public void WriteIf(params IByteArrayPart[] parts)
		{
			for (int i = 0; i < parts.Length; i++)
				if (parts[i] == null || parts[i].IsEmpty)
					return;

			for (int i = 0; i < parts.Length; i++)
				Write(parts[i]);
		}

		public void Write(IByteArrayPart part1, IByteArrayPart part2, int part3, params IByteArrayPart[] parts)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(parts);
		}

		public void Write(IByteArrayPart part1, IByteArrayPart part2, IByteArrayPart part3, int part4, IByteArrayPart part5)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
		}

		public void Write(IByteArrayPart part1, IByteArrayPart part2, IByteArrayPart part3, int part4, IByteArrayPart part5, IByteArrayPart part6, IByteArrayPart part7)
		{
			Write(part1);
			Write(part2);
			Write(part3);
			Write(part4);
			Write(part5);
			Write(part6);
			Write(part7);
		}
	}
}
