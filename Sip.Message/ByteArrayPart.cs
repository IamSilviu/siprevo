using System;
using System.Text;
using System.Net;

namespace Sip.Message
{
	public struct ByteArrayPart
	{
		public byte[] Bytes;
		public int Begin;
		public int End;

		public ByteArrayPart(byte[] array, int offset, int length)
		{
			Bytes = new byte[length];
			Begin = 0;
			End = length;

			Buffer.BlockCopy(array, offset, Bytes, 0, length);
		}

		public ByteArrayPart(ByteArrayPart part)
			: this(part.Items, part.Offset, part.Length)
		{
		}

		public ByteArrayPart(string text)
		{
			Bytes = Encoding.UTF8.GetBytes(text);
			Begin = 0;
			End = Bytes.Length;
		}

		public ByteArrayPart(char simbol)
		{
			Bytes = Encoding.UTF8.GetBytes(new char[] { simbol, });
			Begin = 0;
			End = Bytes.Length;
		}

		public bool IsValid
		{
			get { return Begin >= 0 && End >= 0; }
		}

		public bool IsInvalid
		{
			get { return Bytes == null || Begin < 0 || End < 0; }
		}

		public byte[] Items
		{
			get { return Bytes; }
		}

		public int Offset
		{
			get { return Begin; }
		}

		public int Length
		{
			get { return End - Begin; }
		}

		public bool IsEmpty
		{
			get { return Begin < 0 || End < 0; }
		}

		public void SetDefaultValue()
		{
			Bytes = null;
			Begin = int.MinValue;
			End = int.MinValue;
		}

		public IPAddress ToIpAddress()
		{
			IPAddress ip = IPAddress.None;

			if (IsValid == true)
				IPAddress.TryParse(ToString(), out ip);

			return ip;
		}

		public new string ToString()
		{
			if (Bytes == null || Begin < 0 || End < 0)
				return null;

			return Encoding.UTF8.GetString(Bytes, Offset, Length);
		}
	}
}
