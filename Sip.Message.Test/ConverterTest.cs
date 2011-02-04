using System;
using Sip.Message;
using NUnit.Framework;

namespace SipMessageTest
{
	[TestFixture]
	public class ConverterTest
	{
		[Test]
		public void It_should_convert_all_method_enums_to_ByteArrayPart()
		{
			try
			{
				foreach (Methods method in Enum.GetValues(typeof(Methods)))
					if (method != Methods.None && method != Methods.Extension)
						method.ToByteArrayPart();
			}
			catch (ArgumentOutOfRangeException ex)
			{
				Assert.Fail(ex.ToString());
			}

			try
			{
				Methods.None.ToByteArrayPart();
			}
			catch (ArgumentException)
			{
				try
				{
					Methods.Extension.ToByteArrayPart();
				}
				catch (ArgumentException)
				{
					return;
				}
			}

			Assert.Fail("ArgumentException expected for Methods.None and Methods.Extension");
		}

		[Test]
		public void It_should_define_HeaderMask_for_each_HeaderName()
		{
			try
			{
				foreach (HeaderNames name in Enum.GetValues(typeof(HeaderNames)))
					name.ToMask();
			}
			catch (ArgumentOutOfRangeException ex)
			{
				Assert.Fail(ex.ToString());
			}
		}
	}
}
