using System;
using System.Text;
using System.Net;

namespace Sip.Message
{
	public struct ByteArrayPart
		: IEquatable<ByteArrayPart>
	{
		public byte[] Bytes;
		public int Begin;
		public int End;

		public static readonly ByteArrayPart Invalid = new ByteArrayPart() { Bytes = null, Begin = int.MinValue, End = int.MinValue, };

		public ByteArrayPart(byte[] array, int offset, int length)
		{
			Bytes = new byte[length];
			Begin = 0;
			End = length;

			Buffer.BlockCopy(array, offset, Bytes, 0, length);
		}

		public ByteArrayPart(ByteArrayPart part)
			: this(part.Bytes, part.Offset, part.Length)
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
			int lenght = Length;

			if (lenght != bytes.Length)
				return false;

			for (int i = 0; i < lenght; i++)
				if (Bytes[Begin + i] != bytes[i])
					return false;

			return true;
		}

		public bool Equals(ByteArrayPart other)
		{
			return IsEqualValue(other);
		}

		public static bool operator ==(ByteArrayPart x, ByteArrayPart y)
		{
			return x.IsEqualValue(y);
		}

		public static bool operator !=(ByteArrayPart x, ByteArrayPart y)
		{
			return !x.IsEqualValue(y);
		}

		public override bool Equals(Object obj)
		{
			return obj is ByteArrayPart && IsEqualValue((ByteArrayPart)obj);
		}

		public override int GetHashCode()
		{
			int value = 0;
			int maxOffset = End - Begin - 1;

			if (maxOffset >= 0)
			{
				for (int i = 0; i <= 3; i++)
				{
					value <<= 8;
					value |= Bytes[Begin + maxOffset * i / 3];
				}
			}

			return value;
		}

		public bool IsValid
		{
			get { return Begin >= 0 && End >= 0; }
		}

		public bool IsNotEmpty
		{
			get { return Begin >= 0 && End >= 0 && Begin < End; }
		}

		public bool IsInvalid
		{
			get { return Begin < 0 || End < 0; }
			//get { return Bytes == null || Begin < 0 || End < 0; }
		}

		public bool StartsWith(byte[] bytes)
		{
			int length = bytes.Length;

			if (Length < length)
				return false;

			int startIndex = 0;
			int endIndex = startIndex + length;

			for (int i = Begin, j = startIndex; j < endIndex; i++, j++)
				if (Bytes[i] != bytes[j])
					return false;

			return true;
		}

		public bool EndWith(ByteArrayPart part)
		{
			int length = part.Length;

			if (Length < length)
				return false;

			for (int i = Begin + Length - part.Length, j = part.Begin; j < part.End; i++, j++)
				if (Bytes[i] != part.Bytes[j])
					return false;

			return true;
		}

		//public bool IsEmpty
		//{
		//    get { return Begin < 0 || End < 0; }
		//}

		//public byte[] Items
		//{
		//    get { return Bytes; }
		//}

		public int Offset
		{
			get { return Begin; }
		}

		public int Length
		{
			get { return End - Begin; }
		}

		public void SetDefaultValue()
		{
			Bytes = null;
			Begin = int.MinValue;
			End = int.MinValue;
		}

		public ByteArrayPart DeepCopy()
		{
			if (IsInvalid)
				return Invalid;

			var copy = new ByteArrayPart()
				{
					Bytes = new byte[Length],
					Begin = 0,
					End = Length,
				};

			Buffer.BlockCopy(Bytes, Offset, copy.Bytes, copy.Offset, Length);

			return copy;
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

		public ArraySegment<byte> ToArraySegment()
		{
			return new ArraySegment<byte>(Bytes, Offset, Length);
		}

		public void BlockCopyTo(byte[] bytes, int offset)
		{
			Buffer.BlockCopy(Bytes, Begin, bytes, offset, Length);
		}

		public void BlockCopyTo(byte[] bytes, ref int offset)
		{
			Buffer.BlockCopy(Bytes, Begin, bytes, offset, Length);
			offset += Length;
		}
	}
}
