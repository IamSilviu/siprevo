#if OPTIMIZED2
using System;
using System.Threading;

namespace Sip.Message
{
	partial class SipMessageReader
	{
		private readonly static object sync = new object();
		private readonly static int[] used = new int[8192];
		private static int count = 0;

		public int Index
		{
			get { return index; }
		}

		public static bool IsArraySlotAvailable(int index)
		{
			int i = index / 32;
			int bit = 1 << (index % 32);

			return (used[i] & bit) == 0;
		}

		private void ReleaseArraySlot()
		{
			bytes[index] = null;

			int i = index / 32;
			int bit = 1 << (index % 32);

			int used32;

			do { used32 = Thread.VolatileRead(ref used[i]); }
			while (Interlocked.CompareExchange(ref used[i], used32 & ~bit, used32) != used32);

			Interlocked.Decrement(ref count);
		}

		private void AcquireArraySlot()
		{
			int length = bytes.Length;

			for (; ; )
			{
				if (count < (length / 2) || length == (used.Length * 32))
				{
					//if (startIndex < 0)
					//	startIndex = (Thread.CurrentThread.ManagedThreadId % length) / 32;

					for (int i = 0; i < length / 32; i++)
					{
						int used32 = Thread.VolatileRead(ref used[i]);

						if (used32 != -1)
							for (int j = 0, bit = 1; j < 32; j++, bit <<= 1)
								if ((used32 & bit) == 0)
								{
									if (Interlocked.CompareExchange(ref used[i], used32 | bit, used32) != used32)
										break;

									Interlocked.Increment(ref count);
									index = i * 32 + j;
									return;
								}
					}
				}
				else
				{
					lock (sync)
					{
						if (length < bytes.Length)
							length = bytes.Length;

						if ((length / 2 - 16) <= count)
						{
							byte[][] newBytes = new byte[Math.Min(length * 2, used.Length * 32)][];
							Array.Copy(bytes, newBytes, length);
							Interlocked.Exchange<byte[][]>(ref bytes, newBytes);
						}
					}
				}
			}
		}
	}
}
#endif