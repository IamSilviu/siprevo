#if OPTIMIZED

using System;
using System.Text;
using System.Net;

namespace Sip.Message
{
	public partial struct BeginEnd
		: IEquatable<BeginEnd>, IEquatable<ByteArrayPart>
		, IEquatable<byte[]>
	{
		public int Begin;
		public int End;

		public byte[] Bytes
		{
			get { return SipMessageReader.Bytes; }
		}

		public int Offset
		{
			get { return Begin; }
		}

		public int Length
		{
			get { return IsInvalid ? 0 : End - Begin; }
		}

		public void SetDefaultValue()
		{
			Begin = int.MinValue;
			End = int.MinValue;
		}

		public bool IsValid
		{
			get { return Begin >= 0 && End >= 0; }
		}

		public bool IsInvalid
		{
			get { return Begin < 0 || End < 0; }
		}

		public static implicit operator ByteArrayPart(BeginEnd be)
		{
			return new ByteArrayPart() { Bytes = be.Bytes, Begin = be.Begin, End = be.End, };
		}

		public new string ToString()
		{
			if (Bytes == null || Begin < 0 || End < 0)
				return null;

			return Encoding.UTF8.GetString(Bytes, Offset, Length);
		}

		public IPAddress ToIpAddress()
		{
			IPAddress ip = IPAddress.None;

			if (IsValid == true)
				IPAddress.TryParse(ToString(), out ip);

			return ip;
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

		#region IEquatable<...>, GetHashCode()

		public bool Equals(BeginEnd y)
		{
			return Equals(y.Bytes, y.Begin, y.Length);
		}

		public bool Equals(ByteArrayPart y)
		{
			return Equals(y.Bytes, y.Begin, y.Length);
		}

		public bool Equals(byte[] bytes)
		{
			return Equals(bytes, 0, bytes.Length);
		}

		public static bool operator ==(BeginEnd x, BeginEnd y)
		{
			return x.Equals(y);
		}

		public static bool operator ==(BeginEnd x, ByteArrayPart y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(BeginEnd x, BeginEnd y)
		{
			return !x.Equals(y);
		}

		public static bool operator !=(BeginEnd x, ByteArrayPart y)
		{
			return !x.Equals(y);
		}

		private bool Equals(byte[] bytes, int startIndex, int length)
		{
			if (Length != length)
				return false;

			int endIndex = startIndex + length;
			for (int i = Begin, j = startIndex; j < endIndex; i++, j++)
				if (Bytes[i] != bytes[j])
					return false;

			return true;
		}

		public override bool Equals(Object obj)
		{
			return obj is ByteArrayPart && Equals((ByteArrayPart)obj)
				|| obj is BeginEnd && Equals((BeginEnd)obj)
				|| obj is byte[] && Equals(obj as byte[]);
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

		#endregion

		#region TrimStartSws(), TrimEndSws(), TrimSws(), TrimStartComma()

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
	}
}

#endif