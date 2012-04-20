#if BASEMESSAGE
using System;

namespace Base.Message
{
	/// <summary>
	/// based on Org.BouncyCastle.Utilities.Encoders from http://www.bouncycastle.org/
	/// </summary>
	public static class Base64Encoding
	{
		private static readonly byte[] encodingTable =
		{
			(byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', (byte)'G',
			(byte)'H', (byte)'I', (byte)'J', (byte)'K', (byte)'L', (byte)'M', (byte)'N',
			(byte)'O', (byte)'P', (byte)'Q', (byte)'R', (byte)'S', (byte)'T', (byte)'U',
			(byte)'V', (byte)'W', (byte)'X', (byte)'Y', (byte)'Z',
			(byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', (byte)'f', (byte)'g',
			(byte)'h', (byte)'i', (byte)'j', (byte)'k', (byte)'l', (byte)'m', (byte)'n',
			(byte)'o', (byte)'p', (byte)'q', (byte)'r', (byte)'s', (byte)'t', (byte)'u',
			(byte)'v', (byte)'w', (byte)'x', (byte)'y', (byte)'z',
			(byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6',
			(byte)'7', (byte)'8', (byte)'9',
			(byte)'+', (byte)'/'
		};

		private static readonly byte padding = (byte)'=';

		public static int GetEncodedLength(int length)
		{
			int modulus = length % 3;
			int dataLength = (length - modulus);

			return (dataLength / 3) * 4 + ((modulus == 0) ? 0 : 4);
		}

		public static int Encode(ArraySegment<byte> segment, byte[] output, int outputOffset)
		{
			return Encode(segment.Array, segment.Offset, segment.Count, output, outputOffset);
		}

		public static int Encode(byte[] input, int inputOffset, int length, byte[] output, int outputOffset)
		{
			int modulus = length % 3;
			int dataLength = (length - modulus);
			int a1, a2, a3;

			for (int i = inputOffset; i < inputOffset + dataLength; i += 3)
			{
				a1 = input[i] & 0xff;
				a2 = input[i + 1] & 0xff;
				a3 = input[i + 2] & 0xff;

				output[outputOffset++] = encodingTable[(int)((uint)a1 >> 2) & 0x3f];
				output[outputOffset++] = encodingTable[((a1 << 4) | (int)((uint)a2 >> 4)) & 0x3f];
				output[outputOffset++] = encodingTable[((a2 << 2) | (int)((uint)a3 >> 6)) & 0x3f];
				output[outputOffset++] = encodingTable[a3 & 0x3f];
			}


			int b1, b2, b3;
			int d1, d2;

			switch (modulus)
			{
				case 0:
					break;
				case 1:
					d1 = input[inputOffset + dataLength] & 0xff;
					b1 = (d1 >> 2) & 0x3f;
					b2 = (d1 << 4) & 0x3f;

					output[outputOffset++] = encodingTable[b1];
					output[outputOffset++] = encodingTable[b2];
					output[outputOffset++] = padding;
					output[outputOffset++] = padding;
					break;
				case 2:
					d1 = input[inputOffset + dataLength] & 0xff;
					d2 = input[inputOffset + dataLength + 1] & 0xff;

					b1 = (d1 >> 2) & 0x3f;
					b2 = ((d1 << 4) | (d2 >> 4)) & 0x3f;
					b3 = (d2 << 2) & 0x3f;

					output[outputOffset++] = encodingTable[b1];
					output[outputOffset++] = encodingTable[b2];
					output[outputOffset++] = encodingTable[b3];
					output[outputOffset++] = padding;
					break;
			}

			return (dataLength / 3) * 4 + ((modulus == 0) ? 0 : 4);
		}
	}
}

#endif