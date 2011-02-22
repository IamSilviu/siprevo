using System;
using Sip.Message;
using NUnit.Framework;

namespace SipMessageTest
{
	[SetUpFixture]
	class LoadTables
	{
		[SetUp]
		public void Load()
		{
			var dfa = new SipMessageReader();
			dfa.LoadTables(@"..\..\..\Sip.Message\Sip.Message.dfa");
			dfa.SetDefaultValue();
			dfa.Parse(new byte[] { 0 }, 0, 1);
		}
	}
}
