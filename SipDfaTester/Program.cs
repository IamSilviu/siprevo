using System;
using System.Text;
using Sip.Message;

namespace SipDfaTester
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Loading...");
			var dfa = new SipMessageReader();
			dfa.LoadTables(@"..\..\..\Sip.Message\SipMessageReader.dfa");
			//dfa.LoadTables(@"..\..\..\SipDfaCompiler\bin\Debug\SipMessageReader.dfa");
			dfa.SetDefaultValue();
			dfa.Parse(new byte[] { 0 }, 0, 1);
			Console.WriteLine("Done");

			int proccessed = -1;
			var utf = new UTF8Encoding();

			var message1 = utf.GetBytes(
				"REGISTER sip:officesip.local;ms-received-cid=A123F SIP/2.0\r\n" +
				"Accept: media/submedis\r\n" +
				"Via: SIP/2.0/TCP 127.0.0.1:1800\r\n" +
				"Max-Forwards: 70\r\n" +
				"From: <sip:a@officesip.local>;tag=566ec054f8;epid=aaa6ef05f4\r\n" +
				"To: <sip:b@officesip.local>\r\n" +
				"Call-ID: 6743485a6e45407e903f75571a3a7af9\r\n" +
				"CSeq: 2 REGISTER\r\n" +
				"Contact: <sip:user@127.0.0.1:1801;transport=tcp;ms-opaque=0deb5c7e83>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace  ; +sip.instance=\"<urn:uuid:92794BFE-F550-5AC1-89B4-F1E4E8BCB878>\";expires=123\r\n" +
				"Contact: <sip:127.0.0.2:1802;transport=tcp;ms-opaque=0deb5c7e83>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace;+sip.instance=\"<urn:uuid:92794BFE-F550-5AC1-89B4-F1E4E8BCB878>\",<sip:127.0.0.3:1803;transport=tcp;ms-opaque=0deb5c7e83>;methods=\"INVITE\"\r\n" +
				"Contact: <sip:127.0.0.4:1804;transport=tcp;ms-opaque=0deb5c7e83>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace;+sip.instance=\"<urn:uuid:92794BFE-F550-5AC1-89B4-F1E4E8BCB878>\"\r\n" +
				"Custom: Value\r\n" +
				"Authorization: Digest username=\"jdoe1\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=f1f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"\r\n" +
				"\r\n");

			dfa.SetDefaultValue();

			proccessed = dfa.Parse(message1, 0, message1.Length);

			dfa.SetArray(message1);

			Console.WriteLine("Total: {0}", message1.Length);
			Console.WriteLine("Proccessed: {0}", proccessed);
			Console.WriteLine("Final: {0}", dfa.Final);
			Console.WriteLine("Method: {0}", dfa.Method);
			Console.WriteLine("SipVersion: {0}", dfa.SipVersion);
			Console.WriteLine("RequestUri.MsReceivedCid: {0}", dfa.RequestUri.MsReceivedCid.ToString());
			Console.WriteLine("Header #0: {0}", dfa.Headers[0].HeaderName.ToString());
			Console.WriteLine("Proxy Replace #0: |{0}|", dfa.Contact[0].ProxyReplace.ToString());
			Console.WriteLine("Proxy Replace #1: |{0}|", dfa.Contact[1].ProxyReplace.ToString());
			Console.WriteLine("Proxy Replace #2: |{0}|", dfa.Contact[2].ProxyReplace.ToString());
			Console.WriteLine("Proxy Replace #3: |{0}|", dfa.Contact[3].ProxyReplace.ToString());
			Console.WriteLine("User #0: |{0}|", dfa.Contact[0].AddrSpec2.User.ToString());
			Console.WriteLine("AddrSpec #0: |{0}| : {1}", dfa.Contact[0].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[0].AddrSpec2.Hostport.Port);
			Console.WriteLine("AddrSpec #1: |{0}| : {1}", dfa.Contact[1].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[1].AddrSpec2.Hostport.Port);
			Console.WriteLine("AddrSpec #2: |{0}| : {1}", dfa.Contact[2].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[2].AddrSpec2.Hostport.Port);
			Console.WriteLine("AddrSpec #3: |{0}| : {1}", dfa.Contact[3].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[3].AddrSpec2.Hostport.Port);

			Console.WriteLine("RequestUri: |{0}|", dfa.RequestUri.Hostport.Host.ToString());
			Console.WriteLine("ContactCount: {0}", dfa.Count.ContactCount);
			Console.WriteLine("AddrSpec #0: |{0}| : {1}", dfa.Contact[0].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[0].AddrSpec2.Hostport.Port);
			Console.WriteLine("AddrSpec #1: |{0}| : {1}", dfa.Contact[1].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[1].AddrSpec2.Hostport.Port);
			Console.WriteLine("AddrSpec #2: |{0}| : {1}", dfa.Contact[2].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[2].AddrSpec2.Hostport.Port);
			Console.WriteLine("AddrSpec #3: |{0}| : {1}", dfa.Contact[3].AddrSpec2.Hostport.Host.ToString(), dfa.Contact[3].AddrSpec2.Hostport.Port);
			Console.WriteLine("Expires 1: {0}", dfa.Contact[0].Expires);
			Console.WriteLine("Expires 2: {0}", dfa.Contact[1].Expires);
			Console.WriteLine("Star: {0}", dfa.IsContactStar);
			Console.WriteLine("Via: {2} = {0} : {1}", dfa.Via[0].SentBy.Host.ToString(), dfa.Via[0].SentBy.Port, dfa.Via[0].Transport.ToString());
			Console.WriteLine("CSeq: {0} {1}", dfa.CSeq.Value, dfa.CSeq.Method);
			Console.WriteLine("MaxForwards: {0}", dfa.MaxForwards);
			Console.WriteLine("From: {0} : {1} @ {2}", dfa.From.AddrSpec1.UriScheme.ToString(), dfa.From.AddrSpec1.User.ToString(), dfa.From.AddrSpec1.Hostport.Host.ToString());
			Console.WriteLine("To: {0}", dfa.To.AddrSpec1.Hostport.Host.ToString());
			Console.WriteLine("CallId: {0}", dfa.CallId.ToString());

			Console.WriteLine();
			Console.WriteLine("AUTHORIZATION:");
			Console.WriteLine("AuthScheme: {0}", dfa.Authorization.AuthScheme);
			Console.WriteLine("AuthAlgorithm: {0}", dfa.Authorization.AuthAlgorithm);
			Console.WriteLine("Nonce: |{0}|", dfa.Authorization.Nonce.ToString());
			Console.WriteLine("Username: |{0}|", dfa.Authorization.Username.ToString());
			Console.WriteLine("Realm: |{0}|", dfa.Authorization.Realm.ToString());
			Console.WriteLine("MessageQop: |{0}|", dfa.Authorization.MessageQop.ToString());
			Console.WriteLine("Cnonce: |{0}|", dfa.Authorization.Cnonce.ToString());
			Console.WriteLine("NonceCount: {0}", dfa.Authorization.NonceCount);
			Console.WriteLine("Opaque: |{0}|", dfa.Authorization.Opaque.ToString());
			Console.WriteLine("Response: |{0}|", dfa.Authorization.Response.ToString());
			Console.WriteLine();

			//Console.WriteLine("Headers: {0}", dfa.Count.HeaderCount);
			//for (int i = 0; i < dfa.Count.HeaderCount; i++)
			//    Console.WriteLine("{0}:\r\n|{1}|", dfa.Headers[i].Name.ToString(), dfa.Headers[i].Value.ToString());

			var message2 = utf.GetBytes(
				"BENOTIFY sip:officesip.local SIP/2.0\r\n" +
				"Contact: *\r\n" +
				"Require: token1, token2, token3\r\n" +
				"Require: token4\r\n" +
				"Proxy-Require: token1, token2, token3\r\n" +
				"\r\n");

			dfa.SetDefaultValue();
			dfa.SetArray(message2);
			proccessed = dfa.Parse(message2, 0, message2.Length);

			Console.WriteLine("--");
			Console.WriteLine("Total: {0}", message2.Length);
			Console.WriteLine("Proccessed: {0}", proccessed);
			Console.WriteLine("Final: {0}", dfa.Final);
			Console.WriteLine("Method: {0}", dfa.Method);
			Console.WriteLine("ContactCount: {0}", dfa.Count.ContactCount);
			Console.WriteLine("RequireCount: {0}", dfa.Count.RequireCount);
			Console.Write("Require: ");
			for (int i = 0; i <= dfa.Count.RequireCount; i++)
				Console.Write("{0}, ", dfa.Require[i].ToString());
			Console.WriteLine();
			Console.Write("Proxy-Require: ");
			for (int i = 0; i <= dfa.Count.ProxyRequireCount; i++)
				Console.Write("{0}, ", dfa.ProxyRequire[i].ToString());
			Console.WriteLine();
			Console.WriteLine("Star: {0}", dfa.IsContactStar);

			//Console.ReadKey();

			//Console.WriteLine("Testing speed");

			//int repeat = 100000;
			//int start = Environment.TickCount;
			//for (int i = 0; i < repeat; i++)
			//{
			//    dfa.SetDefaultValue();
			//    dfa.Parse(message1, 0, message1.Length);
			//}
			//int spent = Environment.TickCount - start;

			//Console.WriteLine("Parsed {0} times, {1} ms", repeat, spent);
		}
	}
}
