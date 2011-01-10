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

		public bool IsEqualValue(ByteArrayPart y)
		{
			if (Length != y.Length)
				return false;

			for (int i = 0; i < Length; i++)
				if (Bytes[Begin + i] != y.Bytes[y.Begin + i])
					return false;

			return true;
		}

		public bool IsEqualValue(byte[] bytes)
		{
			if (Length != bytes.Length)
				return false;

			for (int i = 0; i < Length; i++)
				if (Bytes[Begin + i] != bytes[i])
					return false;

			return true;
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

		#region Trim methods

		const byte Cl = 0x0a;
		const byte Cr = 0x0d;
		const byte Space = 0x20;
		const byte Comma = 0x2c;

		public void TrimStartSws()
		{
			while (Begin < End && Bytes[Begin] == Space)
				Begin++;

			if (Begin + 2 < End && Bytes[Begin] == Cr && Bytes[Begin + 1] == Cl && Bytes[Begin + 2] == Space)
				Begin += 3;

			while (Begin < End && Bytes[Begin] == Space)
				Begin++;
		}

		public void TrimEndSws()
		{
			if (Begin < End && Bytes[End - 1] == Space)
			{
				while (Begin < End && Bytes[End - 1] == Space)
					End--;

				if (Begin <= End - 2 && Bytes[End - 2] == Cr && Bytes[End - 1] == Cl)
					End -= 2;

				while (Begin < End && Bytes[End - 1] == Space)
					End--;
			}
		}

		public void TrimSws()
		{
			TrimStartSws();
			TrimEndSws();
		}

		public void TrimStartComma()
		{
			if (Begin < End && Bytes[Begin] == Comma)
				Begin++;
		}

		#endregion

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
