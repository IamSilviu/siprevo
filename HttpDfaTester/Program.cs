using System;
using System.Collections.Generic;
using System.Text;
using Http.Message;

namespace HttpDfaTester
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Loading...");
			int start = Environment.TickCount;

			var dfa = new HttpMessageReader();
			dfa.LoadTables(@"..\..\..\HttpDfaCompiler\bin\Release\Http.Message.dfa");
			int loadTablesDelay = Environment.TickCount - start;
			start = Environment.TickCount;
			dfa.SetDefaultValue();
			dfa.Parse(new byte[] { 0 }, 0, 1);
			Console.WriteLine("Done (LoadTables {0} ms + JIT {1} ms)", loadTablesDelay, Environment.TickCount - start);

			var utf = new UTF8Encoding();

			var message0 = utf.GetBytes(
				"POST /enlighten/calais.asmx/Enlighten HTTP/1.1\r\n" +
				"Test: test\r\n" +
				"Host: api.opencalais.com\r\n" +
				"Content-Type: application/x-www-form-urlencoded\r\n" +
				"Content-Length: 123\r\n" +
				"Upgrade: first, second/567,     third\r\n" +
				"Referer: referer.com\r\n" +
				"\r\n");

			dfa.SetDefaultValue();
			int proccessed = dfa.Parse(message0, 0, message0.Length);
			dfa.SetArray(message0);

			Console.WriteLine("Total: {0}", message0.Length);
			Console.WriteLine("Proccessed: {0}", proccessed);
			Console.WriteLine("Final: {0}", dfa.IsFinal);
			Console.WriteLine("Error: {0}", dfa.IsError);

			Console.WriteLine("Method: {0}", dfa.Method);
			Console.WriteLine("Request-URI: |{0}|", dfa.RequestUri.ToString());
			Console.WriteLine("Host: |{0}|", dfa.Host.ToString());
			Console.WriteLine("Content-Type: |{0}|", dfa.ContentType.Value.ToString());
			Console.WriteLine("Content-Length: {0}", dfa.ContentLength);
			Console.WriteLine("Referer: |{0}|", dfa.Referer.ToString());

			Console.WriteLine("Upgrade");
			for (int i = 0; i < dfa.Count.UpgradeCount; i++)
				Console.WriteLine("  #{0} |{1}| + |{2}| = |{3}|", i, dfa.Upgrades[i].Name.ToString(), dfa.Upgrades[i].Version.ToString(), dfa.Upgrades[i].Value.ToString());

			TestSpeed(dfa, message0);
		}

		private static void TestSpeed(HttpMessageReader dfa, byte[] message)
		{
			Console.WriteLine("Testing speed");

			int repeat = 1000000;
			int start2 = Environment.TickCount;
			for (int i = 0; i < repeat; i++)
			{
				dfa.SetDefaultValue();
				dfa.Parse(message, 0, message.Length);
				dfa.SetArray(message);
			}
			int spent = Environment.TickCount - start2;

			Console.WriteLine("Parsed {0} times, {1} ms", repeat, spent);
		}
	}
}
