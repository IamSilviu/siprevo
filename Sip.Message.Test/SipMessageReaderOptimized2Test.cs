using System;
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
		public void It_should_reserve_new_slot_for_each_parser()
		{
			for (int qty = 1; qty <= 256; qty *= 2)
			{
				var readers = CreateReaders(qty, -1);

				var dictionary = new Dictionary<int, bool>(qty);
				foreach (var reader in readers)
				{
					Assert.False(SipMessageReader.IsArraySlotAvailable(reader.Index));
					dictionary[reader.Index] = true;
				}

				DestroyReaders(readers);
			}
		}

		private SipMessageReader[] CreateReaders(int qty, int expectedIndex)
		{
			var readers = new SipMessageReader[qty];

			for (int i = 0; i < qty; i++, expectedIndex += (expectedIndex >= 0) ? 1 : 0)
			{
				readers[i] = new SipMessageReader();
				if (expectedIndex >= 0)
					Assert.AreEqual(expectedIndex, readers[i].Index);
			}

			return readers;
		}

		private void DestroyReaders(SipMessageReader[] readers)
		{
			for (int i = 0; i < readers.Length; i++)
				Assert.IsFalse(SipMessageReader.IsArraySlotAvailable(readers[i].Index), "Slot #" + readers[i].Index);

			for (int i = 2; i < readers.Length; i++)
			{
				int index = readers[i].Index;

				readers[i].Dispose();
				Assert.IsTrue(SipMessageReader.IsArraySlotAvailable(index), "Slot #" + index);
			}
		}
	}
}
