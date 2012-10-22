using System;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using Sip.Message;
using NUnit.Framework;

namespace SipMessageTest
{
	[TestFixture]
	class SipMessageReaderOptimized2Test
	{
		[SetUp]
		public void Destroy_old_readers()
		{
			GC.Collect();
			GC.WaitForFullGCComplete();
			GC.Collect();
		}

		[Test]
		public void n1_It_should_reserve_new_slot_for_each_parser()
		{
			for (int qty = 1; qty <= 1024 * 32; qty *= 2)
			{
				var readers = CreateReaders(qty);
				DestroyReaders(readers, 0, true);
			}
		}

		[Test]
		public void n2_It_should_reserve_new_slot_for_each_parser()
		{
			for (int qty = 1; qty <= 2048 * 32 / 2; qty *= 2)
			{
				var readers = CreateReaders(qty);
				DestroyReaders(readers, qty / 2, true);
			}
		}

		[Test]
		public void n3_multithreading()
		{
			int run = 1;
			int count = 1;
			const int max = 64;
			var exceptions = new List<Exception>();
			var locker = new ReaderWriterLockSlim();

			locker.EnterWriteLock();

			for (int i = 0; i < max; i++)
			{
				ThreadPool.QueueUserWorkItem((_) =>
				{
					Interlocked.Increment(ref count);
					locker.EnterReadLock();
					locker.ExitReadLock();

					try
					{
						while (Thread.VolatileRead(ref run) != 0)
						{
							DestroyReaders(
								CreateReaders(16), 0, false);
						}
					}
					catch (Exception ex)
					{
						exceptions.Add(ex);
					}

					Interlocked.Increment(ref count);
				});
			}

			while (Thread.VolatileRead(ref count) < max)
				Thread.Sleep(100);
			count = 1;
			locker.ExitWriteLock();


			Thread.Sleep(60000);
			run = 0;


			while (Thread.VolatileRead(ref count) < max)
				Thread.Sleep(1000);


			if (exceptions.Count > 0)
				throw exceptions[0];
		}

		private SipMessageReader[] CreateReaders(int qty)
		{
			int expectedIndex = -1;
			var readers = new SipMessageReader[qty];

			for (int i = 0; i < qty; i++, expectedIndex += (expectedIndex >= 0) ? 1 : 0)
			{
				readers[i] = new SipMessageReader();
				if (expectedIndex >= 0)
					Assert.AreEqual(expectedIndex, readers[i].Index);

				readers[i].SetDefaultValue();
			}

			return readers;
		}

		private void DestroyReaders(SipMessageReader[] readers, int skip, bool validate)
		{
			for (int i = 0; i < readers.Length; i++)
				Assert.IsFalse(SipMessageReader.IsArraySlotAvailable(readers[i].Index), "Slot #" + readers[i].Index);

			for (int i = skip; i < readers.Length; i++)
			{
				int index = readers[i].Index;

				readers[i].Dispose();
				if (validate)
					Assert.IsTrue(SipMessageReader.IsArraySlotAvailable(index), "Slot #" + index);
			}
		}
	}
}
