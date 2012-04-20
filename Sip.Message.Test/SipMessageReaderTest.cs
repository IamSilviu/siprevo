using System;
using System.Text;
using Sip.Message;
using NUnit.Framework;

namespace SipMessageTest
{
	[TestFixture]
	public class SipMessageReaderTest
	{
		[Test]
		public void It_should_parse_sip_version()
		{
			Assert.AreEqual(20, ParseAll("ACK sip:domain SIP/2.0\r\n\r\n").SipVersion);
			Assert.AreEqual(31, ParseAll("ACK sip:domain SIP/3.1\r\n\r\n").SipVersion);
		}

		[Test]
		public void It_should_parse_method_in_request()
		{
			Assert.AreEqual(Methods.Ackm, ParseAll("ACK sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Benotifym, ParseAll("BENOTIFY sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Byem, ParseAll("BYE sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Cancelm, ParseAll("CANCEL sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Infom, ParseAll("INFO sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Invitem, ParseAll("INVITE sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Messagem, ParseAll("MESSAGE sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Notifym, ParseAll("NOTIFY sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Optionsm, ParseAll("OPTIONS sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Referm, ParseAll("REFER sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Registerm, ParseAll("REGISTER sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Servicem, ParseAll("SERVICE sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Subscribem, ParseAll("SUBSCRIBE sip:domain SIP/2.0\r\n\r\n").Method);
			Assert.AreEqual(Methods.Publishm, ParseAll("PUBLISH sip:domain SIP/2.0\r\n\r\n").Method);

			Assert.AreEqual(Methods.Extension, ParseAll("UNKNOW sip:domain SIP/2.0\r\n\r\n").Method);

			//Assert.AreEqual(Methods.None, ParseAll("SIP/2.0 200 OK\r\n\r\n").Method);
		}

		[Test]
		public void It_should_parse_status_code_in_response()
		{
			Assert.IsTrue(ParseAll("REGISTER sip:domain SIP/2.0\r\n\r\n").StatusCode.Value < 0);
			Assert.AreEqual(401, ParseAll("SIP/2.0 401 Unauthorized\r\n\r\n").StatusCode.Value);
		}

		[Test]
		public void It_should_convert_parsed_status_code_to_enum_value()
		{
			Assert.IsTrue(ParseAll("REGISTER sip:domain SIP/2.0\r\n\r\n").StatusCode.Code == StatusCodes.None);
			Assert.IsTrue(ParseAll("SIP/2.0 401 Unauthorized\r\n\r\n").StatusCode.Code == StatusCodes.Unauthorized);
			Assert.IsTrue(ParseAll("SIP/2.0 199 NotDefined\r\n\r\n").StatusCode.Code == StatusCodes.Trying);
			Assert.IsTrue(ParseAll("SIP/2.0 999 NotDefined\r\n\r\n").StatusCode.Code == StatusCodes.None);
		}

		[Test]
		public void It_should_parse_user_in_sip_uri()
		{
			Assert.AreEqual("user", ParseAll("ACK sip:user:password@domain SIP/2.0\r\n\r\n").RequestUri.User.ToString());
			Assert.AreEqual("user", ParseAll("ACK sip:user@domain SIP/2.0\r\n\r\n").RequestUri.User.ToString());
			Assert.AreEqual("user", ParseAll("ACK sip:user:password@domain:5060 SIP/2.0\r\n\r\n").RequestUri.User.ToString());
			Assert.IsTrue(ParseAll("ACK sip:domain SIP/2.0\r\n\r\n").RequestUri.User.IsInvalid);
			Assert.IsTrue(ParseAll("ACK sip:domain:5060;lr SIP/2.0\r\n\r\n").RequestUri.User.IsInvalid);
		}

		[Test]
		public void It_should_parse_user_in_sips_uri()
		{
			Assert.AreEqual("user", ParseAll("ACK sips:user:password@domain SIP/2.0\r\n\r\n").RequestUri.User.ToString());
			Assert.AreEqual("user", ParseAll("ACK sips:user@domain SIP/2.0\r\n\r\n").RequestUri.User.ToString());
			Assert.AreEqual("user", ParseAll("ACK sips:user:password@domain:5060 SIP/2.0\r\n\r\n").RequestUri.User.ToString());
			Assert.IsTrue(ParseAll("ACK sips:domain SIP/2.0\r\n\r\n").RequestUri.User.IsInvalid);
			Assert.IsTrue(ParseAll("ACK sips:domain:5060;lr SIP/2.0\r\n\r\n").RequestUri.User.IsInvalid);
		}

		[Test]
		public void It_should_parse_Contact_star()
		{
			Assert.IsTrue(ParseAll("ACK sip:domain SIP/2.0\r\nContact: *\r\n\r\n").Contact[0].IsStar);
			Assert.False(ParseAll("ACK sip:domain SIP/2.0\r\nContact: sip:abc\r\n\r\n").Contact[0].IsStar);
		}

		[Test]
		public void It_should_parse_Require_header()
		{
			var dfa = ParseHeaders(
				"Require: token1, token2, token3",
				"Require: token4");

			Assert.AreEqual(4, dfa.Count.RequireCount);
			Assert.AreEqual("token1", dfa.Require[0].ToString());
			Assert.AreEqual("token2", dfa.Require[1].ToString());
			Assert.AreEqual("token3", dfa.Require[2].ToString());
			Assert.AreEqual("token4", dfa.Require[3].ToString());
		}

		[Test]
		public void It_should_parse_Proxy_Require_header()
		{
			var dfa = ParseHeaders(
				"Proxy-Require: token1, token2, token3",
				"Proxy-Require: token4");

			Assert.AreEqual(4, dfa.Count.ProxyRequireCount);
			Assert.AreEqual("token1", dfa.ProxyRequire[0].ToString());
			Assert.AreEqual("token2", dfa.ProxyRequire[1].ToString());
			Assert.AreEqual("token3", dfa.ProxyRequire[2].ToString());
			Assert.AreEqual("token4", dfa.ProxyRequire[3].ToString());
		}

		[Test]
		public void It_should_parse_Call_ID_header()
		{
			Assert.AreEqual("123", ParseHeader("Call-ID: 123").CallId.ToString());
			Assert.AreEqual("123", ParseHeader("Call-ID:123").CallId.ToString());
			Assert.AreEqual("123", ParseHeader("Call-ID:    123").CallId.ToString());
			Assert.AreEqual("1", ParseHeader("Call-ID: 1").CallId.ToString());
		}

		[Test]
		public void It_should_parse_Max_Forwards_header()
		{
			Assert.AreEqual(70, ParseHeader("Max-Forwards: 70").MaxForwards);
			Assert.IsTrue(ParseHeader("No-Max-Forwards: 70").MaxForwards < 0);
		}

		[Test]
		public void It_should_parse_lr_param_in_uri()
		{
			Assert.IsTrue(ParseHeader("Record-Route: <sip:127.0.0.1:5060;lr>").RecordRoute[0].AddrSpec.HasLr);
			Assert.IsFalse(ParseHeader("Record-Route: <sip:127.0.0.1:5060>").RecordRoute[0].AddrSpec.HasLr);
		}

		[Test]
		public void It_should_ignore_invalid_lr_param_in_uri()
		{
			Assert.IsFalse(ParseHeader("Record-Route: <sip:127.0.0.1:5060;lrwrong>").RecordRoute[0].AddrSpec.HasLr);
		}

		[Test]
		public void It_should_parse_ms_received_param_in_uri()
		{
			Assert.AreEqual("A123F", ParseAll("X sip:domain;ms-received-cid=A123F SIP/2.0\r\n\r\n").RequestUri.MsReceivedCid.ToString());
			Assert.IsTrue(ParseAll("X sips:user:password@domain SIP/2.0\r\n\r\n").RequestUri.MsReceivedCid.IsInvalid);
		}

		[Test]
		public void It_should_parse_transport_param_in_uri()
		{
			Assert.AreEqual(Transports.Tcp, ParseAll("X sip:domain;transport=tcp;lr SIP/2.0\r\n\r\n").RequestUri.Transport);
			Assert.AreEqual(Transports.Tcp, ParseAll("X sip:domain;transport=tcp SIP/2.0\r\n\r\n").RequestUri.Transport);
			Assert.AreEqual(Transports.Udp, ParseAll("X sip:domain;transport=udp SIP/2.0\r\n\r\n").RequestUri.Transport);
			Assert.AreEqual(Transports.Tls, ParseAll("X sip:domain;transport=tls SIP/2.0\r\n\r\n").RequestUri.Transport);
			Assert.AreEqual(Transports.Sctp, ParseAll("X sip:domain;transport=sctp SIP/2.0\r\n\r\n").RequestUri.Transport);
			Assert.AreEqual(Transports.None, ParseAll("X sip:domain SIP/2.0\r\n\r\n").RequestUri.Transport);
		}

		[Test]
		public void It_should_parse_proxy_replace_param_in_uri()
		{
			var dfa = ParseHeaders(
				"Contact: <sip:domain>;proxy=replace  ; next=param;next=param",
				"Contact: <sip:domain>;  proxy=replace,<sip:domain>;proxy=replace",
				"Contact: <sip:domain>;proxy=replace",
				"Contact: <sip:domain>;proxy=replacewrong");

			Assert.AreEqual(5, dfa.Count.ContactCount);
			Assert.AreEqual(";proxy=replace", dfa.Contact[0].ProxyReplace.ToString());
			Assert.AreEqual(";  proxy=replace", dfa.Contact[1].ProxyReplace.ToString());
			Assert.AreEqual(";proxy=replace", dfa.Contact[2].ProxyReplace.ToString());
			Assert.AreEqual(";proxy=replace", dfa.Contact[3].ProxyReplace.ToString());
			Assert.IsTrue(dfa.Contact[4].ProxyReplace.IsInvalid);
		}

		[Test]
		public void It_should_parse_Contact_expires_param()
		{
			var dfa = ParseHeaders(
				"Contact: <sip:domain>;expires=123",
				"Contact: <sip:domain>;expires=0",
				"Contact: <sip:domain>");

			Assert.AreEqual(3, dfa.Count.ContactCount);
			Assert.AreEqual(123, dfa.Contact[0].Expires);
			Assert.AreEqual(0, dfa.Contact[1].Expires);
			Assert.IsTrue(dfa.Contact[2].Expires < 0);
		}

		[Test]
		public void It_should_parse_Contact_addrspec1_and_addrspec2()
		{
			var dfa = ParseHeaders(
				"Contact: <sip:domain1>",
				"Contact: sip:domain2");

			Assert.AreEqual(2, dfa.Count.ContactCount);
			Assert.AreEqual("domain1", dfa.Contact[0].AddrSpec.Hostport.Host.ToString());
			Assert.AreEqual("domain2", dfa.Contact[1].AddrSpec.Hostport.Host.ToString());
		}

		[Test]
		public void It_should_parse_Route_comma_plus_value_range()
		{
			var dfa = ParseHeaders(
				"Route: <sip:domain1>, <sip:domain2>  ,<sip:domain3>",
				"Route: <sip:domain4>");

			Assert.AreEqual(4, dfa.Count.RouteCount);
			Assert.AreEqual(" <sip:domain1>", dfa.Route[0].CommaAndValue.ToString());
			Assert.AreEqual(", <sip:domain2>  ", dfa.Route[1].CommaAndValue.ToString());
			Assert.AreEqual(",<sip:domain3>", dfa.Route[2].CommaAndValue.ToString());
			Assert.AreEqual(" <sip:domain4>", dfa.Route[3].CommaAndValue.ToString());
		}

		[Test]
		public void It_should_parse_Record_Route_comma_plus_value_range()
		{
			var dfa = ParseHeaders(
				"Record-Route: <sip:domain1>, <sip:domain2>  ,<sip:domain3>",
				"Record-Route: <sip:domain4>");

			Assert.AreEqual(4, dfa.Count.RecordRouteCount);
			Assert.AreEqual(" <sip:domain1>", dfa.RecordRoute[0].CommaAndValue.ToString());
			Assert.AreEqual(", <sip:domain2>  ", dfa.RecordRoute[1].CommaAndValue.ToString());
			Assert.AreEqual(",<sip:domain3>", dfa.RecordRoute[2].CommaAndValue.ToString());
			Assert.AreEqual(" <sip:domain4>", dfa.RecordRoute[3].CommaAndValue.ToString());
		}

		[Test]
		public void It_should_parse_Via_comma_plus_value_range()
		{
			var dfa = ParseHeaders(
				"Via:SIP/2.0/TCP 127.0.0.1:1801,SIP/2.0/TCP 127.0.0.2:1802,SIP/2.0/TCP 127.0.0.3:1803",
				"Via:  SIP/2.0/TCP 127.0.0.4:1804  ,  SIP/2.0/TCP 127.0.0.5:1805  ,  SIP/2.0/TCP 127.0.0.6:1806");

			Assert.AreEqual(6, dfa.Count.ViaCount);
			Assert.AreEqual("SIP/2.0/TCP 127.0.0.1:1801", dfa.Via[0].CommaAndValue.ToString());
			Assert.AreEqual(",SIP/2.0/TCP 127.0.0.2:1802", dfa.Via[1].CommaAndValue.ToString());
			Assert.AreEqual(",SIP/2.0/TCP 127.0.0.3:1803", dfa.Via[2].CommaAndValue.ToString());
			Assert.AreEqual("  SIP/2.0/TCP 127.0.0.4:1804", dfa.Via[3].CommaAndValue.ToString());
			Assert.AreEqual(",  SIP/2.0/TCP 127.0.0.5:1805", dfa.Via[4].CommaAndValue.ToString());
			Assert.AreEqual(",  SIP/2.0/TCP 127.0.0.6:1806", dfa.Via[5].CommaAndValue.ToString());
		}

		[Test]
		public void It_should_parse_Content_Type_header()
		{
			{
				var dfa = ParseHeader("Content-Type: application/msrtc-adrl-categorylist+xml");
				Assert.AreEqual("application", dfa.ContentType.Type.ToString());
				Assert.AreEqual("msrtc-adrl-categorylist+xml", dfa.ContentType.Subtype.ToString());
			}
			{
				var dfa = ParseHeader("Content-Type: type / subtype");
				Assert.AreEqual("type", dfa.ContentType.Type.ToString());
				Assert.AreEqual("subtype", dfa.ContentType.Subtype.ToString());
			}
			{
				var dfa = ParseHeader("Content-Type:           type         /               subtype");
				Assert.AreEqual("type", dfa.ContentType.Type.ToString());
				Assert.AreEqual("subtype", dfa.ContentType.Subtype.ToString());
			}
		}

		[Test]
		public void It_should_parse_Authorization_header_for_Digest()
		{
			var dfa = ParseHeader("Authorization: Digest username=\"jdoe1\", realm=\"officesip.local\", qop=auth, algorithm=MD5, uri=\"sip:officesip.local\", nonce=\"50c148849f3d4e069105f8a80471b5d1\", nc=00000f1f, cnonce=\"53186537273641419345563711231350\", opaque=\"f2cb4f2013d74a73b9e86603fb56b969\", response=\"2bb45befa63ca772b840501502df102d\"");

			var auth = dfa.Authorization[0];

			Assert.AreEqual(AuthSchemes.Digest, auth.AuthScheme);
			Assert.AreEqual(AuthAlgorithms.Md5, auth.AuthAlgorithm);
			Assert.AreEqual("50c148849f3d4e069105f8a80471b5d1", auth.Nonce.ToString());
			Assert.AreEqual("jdoe1", auth.Username.ToString());
			Assert.AreEqual("officesip.local", auth.Realm.ToString());
			Assert.AreEqual("auth", auth.MessageQop.ToString());
			Assert.AreEqual("53186537273641419345563711231350", auth.Cnonce.ToString());
			Assert.AreEqual(0xf1f, auth.NonceCount);
			Assert.AreEqual("00000f1f", auth.NonceCountBytes.ToString());
			Assert.AreEqual("f2cb4f2013d74a73b9e86603fb56b969", auth.Opaque.ToString());
			Assert.AreEqual("auth", auth.MessageQop.ToString());
			Assert.AreEqual("2bb45befa63ca772b840501502df102d", auth.Response.ToString());
		}

		[Test]
		public void It_should_parse_Authorization_header_for_NTLM()
		{
			var dfa = ParseHeader("Authorization: NTLM qop=\"auth\", realm=\"OfficeSIP Server\", targetname=\"officesip.local\", gssapi-data=\"12345\", version=3");

			var auth = dfa.Authorization[0];

			Assert.AreEqual(AuthSchemes.Ntlm, auth.AuthScheme);
			Assert.AreEqual("auth", auth.MessageQop.ToString());
			Assert.AreEqual("OfficeSIP Server", auth.Realm.ToString());
			Assert.AreEqual("auth", auth.MessageQop.ToString());
			Assert.AreEqual("12345", auth.GssapiData.ToString());
			Assert.AreEqual(3, auth.Version);
			Assert.AreEqual("officesip.local", auth.Targetname.ToString());
		}

		[Test]
		public void It_should_parse_Authorization_header_for_NTLM_signature()
		{
			var dfa = ParseHeader("Authorization: NTLM qop=\"auth\", realm=\"OfficeSIP Server\", opaque=\"05cee767\", targetname=\"officesip.local\", crand=\"3c1c4c7b\", cnum=\"21\", response=\"0100000061646d69de3324fd36334466\"");

			var auth = dfa.Authorization[0];

			Assert.AreEqual(AuthSchemes.Ntlm, auth.AuthScheme);
			Assert.AreEqual("auth", auth.MessageQop.ToString());
			Assert.AreEqual("OfficeSIP Server", auth.Realm.ToString());
			Assert.AreEqual("auth", auth.MessageQop.ToString());
			Assert.AreEqual("05cee767", auth.Opaque.ToString());
			Assert.AreEqual("officesip.local", auth.Targetname.ToString());
			Assert.AreEqual(0x3c1c4c7b, auth.Crand);
			Assert.AreEqual(21, auth.Cnum);
			Assert.AreEqual("0100000061646d69de3324fd36334466", auth.Response.ToString());
		}

		[Test]
		public void It_should_parse_Authorization_header_for_NTLM_signature_crand_and_cnum_without_qoute()
		{
			var dfa = ParseHeader("Authorization: NTLM crand=3c1c4c7b, cnum=1234567");

			var auth = dfa.Authorization[0];

			Assert.AreEqual(AuthSchemes.Ntlm, auth.AuthScheme);
			Assert.AreEqual(0x3c1c4c7b, auth.Crand);
			Assert.AreEqual(1234567, auth.Cnum);
		}

		[Test]
		public void It_should_parse_Authorization_header_with_empty_gssapi_data()
		{
			Assert.IsTrue(ParseHeader("Authorization: NTLM gssapi-data=\"\"")
				.Authorization[0].GssapiData.IsInvalid);
		}

		[Test]
		public void It_should_parse_SIP_ETag()
		{
			Assert.AreEqual("1234567890", ParseHeader("SIP-ETag: 1234567890").SipEtag.ToString());
		}

		[Test]
		public void It_should_parse_SIP_If_Match()
		{
			Assert.AreEqual("1234567890", ParseHeader("SIP-If-Match: 1234567890").SipIfMatch.ToString());
		}

		[Test]
		public void It_should_parse_Event()
		{
			Assert.AreEqual("presence", ParseHeader("Event: presence").Event.EventType.ToString());
			Assert.AreEqual("presence", ParseHeader("Event: presence; id = 1234").Event.EventType.ToString());
			Assert.AreEqual("1234", ParseHeader("Event: presence; id = 1234").Event.Id.ToString());
		}

		[Test]
		public void It_should_parse_User_Agent()
		{
			Assert.AreEqual("abc", ParseHeader("User-Agent: abc").UserAgent.Product.ToString());
			Assert.AreEqual("a.b*c", ParseHeader("User-Agent: a.b*c").UserAgent.Product.ToString());
			Assert.AreEqual("abc", ParseHeader("User-Agent: abc/123").UserAgent.Product.ToString());
			Assert.AreEqual("123", ParseHeader("User-Agent: abc/123").UserAgent.Version.ToString());
		}

		[Test]
		public void It_should_return_correct_parsed_count_when_error()
		{
			var dfa = new SipMessageReader();
			{
				var message = Encoding.UTF8.GetBytes("\r\n\r\n");

				dfa.SetDefaultValue();
				int parsed = dfa.Parse(message, 0, message.Length);

				Assert.AreEqual(0, parsed);
			}
			{
				var message = Encoding.UTF8.GetBytes("REG@@@@");

				dfa.SetDefaultValue();
				Assert.AreEqual(3, dfa.Parse(message, 0, message.Length));

				dfa.SetDefaultValue();
				Assert.AreEqual(3, dfa.Parse(message, 0, 4));
			}
		}

		[Test]
		public void It_should_parse_WWW_Authenticate_Digest()
		{
			var reader = ParseHeaders(
				"WWW-Authenticate: Digest realm=\"realm1\",nonce=\"11111\",qop=\"auth\",algorithm=MD5,stale=false,opaque=\"opaque1\"",
				"WWW-Authenticate: Digest realm=\"realm2\",nonce=\"22222\",qop=\"auth,auth-int\",algorithm=MD5,stale=true,opaque=\"opaque2\""
				);

			Assert.AreEqual(2, reader.Count.WwwAuthenticateCount);

			Assert.AreEqual(AuthSchemes.Digest, reader.WwwAuthenticate[0].AuthScheme);
			Assert.AreEqual(AuthSchemes.Digest, reader.WwwAuthenticate[1].AuthScheme);

			Assert.AreEqual("realm1", reader.WwwAuthenticate[0].Realm.ToString());
			Assert.AreEqual("realm2", reader.WwwAuthenticate[1].Realm.ToString());

			Assert.AreEqual("11111", reader.WwwAuthenticate[0].Nonce.ToString());
			Assert.AreEqual("22222", reader.WwwAuthenticate[1].Nonce.ToString());

			Assert.AreEqual("auth", reader.WwwAuthenticate[0].Qop.ToString());
			Assert.AreEqual("auth,auth-int", reader.WwwAuthenticate[1].Qop.ToString());

			Assert.AreEqual(AuthAlgorithms.Md5, reader.WwwAuthenticate[0].AuthAlgorithm);
			Assert.AreEqual(AuthAlgorithms.Md5, reader.WwwAuthenticate[1].AuthAlgorithm);

			Assert.AreEqual(false, reader.WwwAuthenticate[0].Stale);
			Assert.AreEqual(true, reader.WwwAuthenticate[1].Stale);

			Assert.AreEqual("opaque1", reader.WwwAuthenticate[0].Opaque.ToString());
			Assert.AreEqual("opaque2", reader.WwwAuthenticate[1].Opaque.ToString());
		}

		[Test]
		public void It_should_parse_message_example_1()
		{
			var message1 = UTF8Encoding.UTF8.GetBytes(
				"REGISTER sip:officesip.local SIP/2.0\r\n" +
				"Via: SIP/2.0/TCP 127.0.0.1:2260\r\n" +
				"Max-Forwards: 70\r\n" +
				"From: <sip:demchenko@officesip.local>;tag=9c0e7f8549;epid=fc68f4ccf1\r\n" +
				"To: <sip:demchenko@officesip.local>\r\n" +
				"Call-ID: 78cda9b3461a453e916f6ac7c7560198\r\n" +
				"CSeq: 1 REGISTER\r\n" +
				"Contact: <sip:127.0.0.1:2260;transport=tcp;ms-opaque=e24420fcd5>;methods=\"INVITE, MESSAGE, INFO, OPTIONS, BYE, CANCEL, NOTIFY, ACK, REFER, BENOTIFY\";proxy=replace;+sip.instance=\"<urn:uuid:02C51493-0E21-59BD-83E4-28EA883A80DA>\"\r\n" +
				"User-Agent: UCCAPI/2.0.6362.67\r\n" +
				"Supported: gruu-10, adhoclist, msrtc-event-categories\r\n" +
				"Supported: ms-forking\r\n" +
				"ms-keep-alive: UAC;hop-hop=yes\r\n" +
				"Event: registration\r\n" +
				"Content-Length: 0\r\n" +
				"\r\n");

			Assert.AreEqual(0x0000029d, message1.Length);

			var message2 = new byte[0x10000000];
			var offset = 0x00020000;

			Buffer.BlockCopy(message1, 0, message2, offset, message1.Length);

			var dfa = new SipMessageReader();
			dfa.SetDefaultValue();

			int parsed = dfa.Parse(message2, offset, message1.Length);
			dfa.SetArray(message2);

			Assert.AreEqual(message1.Length, parsed);
			Assert.IsTrue(dfa.Final);
			Assert.AreEqual(Methods.Registerm, dfa.Method);
		}

		private SipMessageReader ParseHeader(string header)
		{
			return ParseAll("DUMMY sip:domain SIP/2.0\r\n" + header + "\r\n\r\n");
		}

		private SipMessageReader ParseHeaders(params string[] headers)
		{
			string middle = "";
			foreach (var header in headers)
				middle += header + "\r\n";

			return ParseAll("DUMMY sip:domain SIP/2.0\r\n" + middle + "\r\n");
		}

		private SipMessageReader ParseAll(string message)
		{
			var dfa = new SipMessageReader();
			var bytes = UTF8Encoding.UTF8.GetBytes(message);

			dfa.SetDefaultValue();

			if (dfa.Parse(bytes, 0, bytes.Length) != bytes.Length)
				throw new Exception("It should parse all message, but message parsed partially");

			if (dfa.IsFinal == false)
				throw new Exception("It should goto to final state");

			dfa.SetArray(bytes);

			return dfa;
		}
	}
}
