using System;
using System.Text;
using Server.Memory;

namespace Sip.Message
{
	public interface IByteArrayPart
		: IDefaultValue
	{
		byte[] Items { get; }
		int Offset { get; }
		int Length { get; }
		bool IsEmpty { get; }
	}

	public struct ByteArrayPartRef
		: IByteArrayPart
	{
		private static UTF8Encoding utf = new UTF8Encoding();

		public byte[] Bytes;
		public int Begin;
		public int End;

		public bool IsValid
		{
			get { return Begin >= 0 && End >= 0; }
		}

		public bool IsInvalid
		{
			get { return Bytes == null || Begin < 0 || End < 0; }
		}

		public new string ToString()
		{
			if (Bytes == null || Begin < 0 || End < 0)
				return null;

			lock (utf)
				return utf.GetString(Bytes, Offset, Length);
		}

		#region IByteArrayPart

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

		#endregion

		#region IDefaultValue

		public void SetDefaultValue()
		{
			Bytes = null;
			Begin = -1;
			End = -1;
		}

		#endregion
	}

	class ByteArrayPart
		: IByteArrayPart
	{
		private static UTF8Encoding _utf = new UTF8Encoding();
		private byte[] _array;

		public ByteArrayPart(byte[] array, int offset, int length)
		{
			_array = new byte[length];
			Array.Copy(array, offset, _array, 0, length);
		}

		public ByteArrayPart(IByteArrayPart part)
			: this(part.Items, part.Offset, part.Length)
		{
		}

		public ByteArrayPart(string text)
		{
			lock (_utf)
				_array = _utf.GetBytes(text);
		}

		public ByteArrayPart(char simbol)
		{
			lock (_utf)
				_array = _utf.GetBytes(new char[] { simbol,});
		}

		#region IByteArrayPart

		public byte[] Items
		{
			get { return _array; }
		}

		public int Offset
		{
			get { return 0; }
		}

		public int Length
		{
			get { return _array.Length; }
		}

		public bool IsEmpty
		{
			get { return false; }
		}

		#endregion

		#region IDefaultValue

		public void SetDefaultValue()
		{
		}
		
		#endregion
	}
}
