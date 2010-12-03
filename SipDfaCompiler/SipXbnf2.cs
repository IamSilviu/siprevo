using System;
using System.Collections.Generic;
using Fsm;

namespace SipDfaCompiler
{
	class MarkRuleEventArgs: EventArgs
	{
		public MarkRuleEventArgs(State start, List<string> rulenames)
		{
			Start = start;
			Rulenames = rulenames;
		}
		public State Start { get; set; }
		public List<string> Rulenames { get; private set; }
	}
	class ChangeRuleEventArgs: EventArgs
	{
		public ChangeRuleEventArgs(State[] states, List<string> rulenames)
		{
			States = states;
			Rulenames = rulenames;
		}
		public State[] States { get; set; }
		public List<string> Rulenames { get; private set; }
	}
	class SipXbnf
	{
		public event EventHandler<MarkRuleEventArgs> MarkRule;
		public event EventHandler<ChangeRuleEventArgs> ChangeConcatanation;
		private State OnMarkRule(State start, List<string> rulenames)
		{
			if (MarkRule != null)
			{
				var args = new MarkRuleEventArgs(start, rulenames);
				MarkRule(this, args);
				return args.Start;
			}
			return start;
		}
		private State[] OnChangeConcatanation(List<string> rulenames, params State[] states)
		{
			if (ChangeConcatanation != null)
			{
				var args = new ChangeRuleEventArgs(states, rulenames);
				ChangeConcatanation(this, args);
				return args.States;
			}
			return states;
		}
		private State FromString(string charval, List<string> rulenames)
		{
			rulenames.Insert(0, charval);
			State rule = (State)charval;
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getalphanum0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetALPHA(rulenames),GetDIGIT(rulenames));
			return rule;
		}
		public State Getreserved0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString(";",rulenames),FromString("/",rulenames),FromString("?",rulenames),FromString(":",rulenames),FromString("@",rulenames),FromString("&",rulenames),FromString("=",rulenames),FromString("+",rulenames),FromString("$",rulenames),FromString(",",rulenames));
			return rule;
		}
		public State Getunreserved0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getalphanum(rulenames),Getmark(rulenames));
			return rule;
		}
		public State Getmark0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("-",rulenames),FromString("_",rulenames),FromString(".",rulenames),FromString("!",rulenames),FromString("~",rulenames),FromString("*",rulenames),FromString("'",rulenames),FromString("(",rulenames),FromString(")",rulenames));
			return rule;
		}
		public State Getescaped0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("%",rulenames),GetHEXDIG(rulenames),GetHEXDIG(rulenames)));
			return rule;
		}
		public State GetLWS0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.NoCloneOption(State.NoCloneConcatanation(State.Repeat(-1,-1,GetWSP(rulenames)),GetCRLF(rulenames))),State.Repeat(1,-1,GetWSP(rulenames))));
			return rule;
		}
		public State GetSWS0(List<string> rulenames)
		{
			State rule = State.NoCloneOption(GetLWS(rulenames));
			return rule;
		}
		public State GetHCOLON0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(-1,-1,(State.NoCloneAlternation(GetSP(rulenames),GetHTAB(rulenames)))),FromString(":",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetTEXT_UTF8_TRIM0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(1,-1,GetTEXT_UTF8char(rulenames)),State.Repeat(-1,-1,(State.NoCloneConcatanation(State.Repeat(-1,-1,GetLWS(rulenames)),GetTEXT_UTF8char(rulenames))))));
			return rule;
		}
		public State GetTEXT_UTF8char0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((0x21.To(0x7E)),GetUTF8_NONASCII(rulenames));
			return rule;
		}
		public State GetUTF8_NONASCII0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(0xC0.To(0x0DF)),State.Repeat(1,-1,GetUTF8_CONT(rulenames)))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(0xE0.To(0x0EF)),State.Repeat(2,-1,GetUTF8_CONT(rulenames)))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(0xF0.To(0x0F7)),State.Repeat(3,-1,GetUTF8_CONT(rulenames)))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(0xF8.To(0x0FB)),State.Repeat(4,-1,GetUTF8_CONT(rulenames)))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(0xFC.To(0x0FD)),State.Repeat(5,-1,GetUTF8_CONT(rulenames)))));
			return rule;
		}
		public State GetUTF8_CONT0(List<string> rulenames)
		{
			State rule = (0x80.To(0x0BF));
			return rule;
		}
		public State GetLHEX0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetDIGIT(rulenames),(0x61.To(0x66)));
			return rule;
		}
		public State Gettoken0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,(State.NoCloneAlternation(Getalphanum(rulenames),FromString("-",rulenames),FromString(".",rulenames),FromString("!",rulenames),FromString("%",rulenames),FromString("*",rulenames),FromString("_",rulenames),FromString("+",rulenames),FromString("`",rulenames),FromString("'",rulenames),FromString("~",rulenames))));
			return rule;
		}
		public State Getseparators0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("(",rulenames),FromString(")",rulenames),FromString("<",rulenames),FromString(">",rulenames),FromString("@",rulenames),FromString(",",rulenames),FromString(";",rulenames),FromString(":",rulenames),FromString("\\",rulenames),GetDQUOTE(rulenames),FromString("/",rulenames),FromString("[",rulenames),FromString("]",rulenames),FromString("?",rulenames),FromString("=",rulenames),FromString("{",rulenames),FromString("}",rulenames),GetSP(rulenames),GetHTAB(rulenames));
			return rule;
		}
		public State Getword0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,(State.NoCloneAlternation(Getalphanum(rulenames),FromString("-",rulenames),FromString(".",rulenames),FromString("!",rulenames),FromString("%",rulenames),FromString("*",rulenames),FromString("_",rulenames),FromString("+",rulenames),FromString("`",rulenames),FromString("'",rulenames),FromString("~",rulenames),FromString("(",rulenames),FromString(")",rulenames),FromString("<",rulenames),FromString(">",rulenames),FromString(":",rulenames),FromString("\\",rulenames),GetDQUOTE(rulenames),FromString("/",rulenames),FromString("[",rulenames),FromString("]",rulenames),FromString("?",rulenames),FromString("{",rulenames),FromString("}",rulenames))));
			return rule;
		}
		public State GetSTAR0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString("*",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetSLASH0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString("/",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetEQUAL0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString("=",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetLPAREN0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString("(",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetRPAREN0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString(")",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetRAQUOT0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString(">",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetLAQUOT0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString("<",rulenames)));
			return rule;
		}
		public State GetCOMMA0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString(",",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetSEMI0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString(";",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetCOLON0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),FromString(":",rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State GetLDQUOT0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),GetDQUOTE(rulenames)));
			return rule;
		}
		public State GetRDQUOT0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetDQUOTE(rulenames),GetSWS(rulenames)));
			return rule;
		}
		public State Getcomment0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetLPAREN(rulenames),State.Repeat(-1,-1,(State.NoCloneAlternation(Getctext(rulenames),Getquoted_pair(rulenames)))),GetRPAREN(rulenames)));
			return rule;
		}
		public State Getctext0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((0x21.To(0x27)),(0x2A.To(0x5B)),(0x5D.To(0x7E)),GetUTF8_NONASCII(rulenames),GetLWS(rulenames));
			return rule;
		}
		public State Getquoted_string0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSWS(rulenames),GetDQUOTE(rulenames),State.Repeat(-1,-1,(State.NoCloneAlternation(Getqdtext(rulenames),Getquoted_pair(rulenames)))),GetDQUOTE(rulenames)));
			return rule;
		}
		public State Getqdtext0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetLWS(rulenames),0x21,(0x23.To(0x5B)),(0x5D.To(0x7E)),GetUTF8_NONASCII(rulenames));
			return rule;
		}
		public State GetALPHA0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((0x41.To(0x5A)),(0x61.To(0x7A)));
			return rule;
		}
		public State GetBIT0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("0",rulenames),FromString("1",rulenames));
			return rule;
		}
		public State GetCHAR0(List<string> rulenames)
		{
			State rule = (0x01.To(0x7F));
			return rule;
		}
		public State GetCR0(List<string> rulenames)
		{
			State rule = 0x0D;
			return rule;
		}
		public State GetCRLF0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetCR(rulenames),GetLF(rulenames)));
			return rule;
		}
		public State GetCTL0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((0x00.To(0x1F)),0x7F);
			return rule;
		}
		public State GetDIGIT0(List<string> rulenames)
		{
			State rule = (0x30.To(0x39));
			return rule;
		}
		public State GetDQUOTE0(List<string> rulenames)
		{
			State rule = 0x22;
			return rule;
		}
		public State GetHEXDIG0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetDIGIT(rulenames),FromString("A",rulenames),FromString("B",rulenames),FromString("C",rulenames),FromString("D",rulenames),FromString("E",rulenames),FromString("F",rulenames));
			return rule;
		}
		public State GetHTAB0(List<string> rulenames)
		{
			State rule = 0x09;
			return rule;
		}
		public State GetLF0(List<string> rulenames)
		{
			State rule = 0x0A;
			return rule;
		}
		public State GetLWSP0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(GetWSP(rulenames),State.NoCloneConcatanation(GetCRLF(rulenames),GetWSP(rulenames)))));
			return rule;
		}
		public State GetOCTET0(List<string> rulenames)
		{
			State rule = (0x00.To(0x0FF));
			return rule;
		}
		public State GetSP0(List<string> rulenames)
		{
			State rule = 0x20;
			return rule;
		}
		public State GetVCHAR0(List<string> rulenames)
		{
			State rule = (0x21.To(0x7E));
			return rule;
		}
		public State GetWSP0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetSP(rulenames),GetHTAB(rulenames));
			return rule;
		}
		public State Getquoted_pair0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("\\",rulenames),(State.NoCloneAlternation((0x00.To(0x09)),(0x0B.To(0x0C)),(0x0E.To(0x7F))))));
			return rule;
		}
		public State GetSIP_URI0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("sip:",rulenames),State.NoCloneOption(Getuserinfo(rulenames)),Gethostport(rulenames),Geturi_parameters(rulenames),State.NoCloneOption(Getheaders(rulenames))));
			return rule;
		}
		public State GetSIPS_URI0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("sips:",rulenames),State.NoCloneOption(Getuserinfo(rulenames)),Gethostport(rulenames),Geturi_parameters(rulenames),State.NoCloneOption(Getheaders(rulenames))));
			return rule;
		}
		public State Getuserinfo0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(Getuser(rulenames)),State.NoCloneOption(State.NoCloneConcatanation(FromString(":",rulenames),Getpassword(rulenames))),FromString("@",rulenames)));
			return rule;
		}
		public State Getuser0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,(State.NoCloneAlternation(Getunreserved(rulenames),Getescaped(rulenames),Getuser_unreserved(rulenames))));
			return rule;
		}
		public State Getuser_unreserved0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("&",rulenames),FromString("=",rulenames),FromString("+",rulenames),FromString("$",rulenames),FromString(",",rulenames),FromString(";",rulenames),FromString("?",rulenames),FromString("/",rulenames));
			return rule;
		}
		public State Getpassword0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(Getunreserved(rulenames),Getescaped(rulenames),FromString("&",rulenames),FromString("=",rulenames),FromString("+",rulenames),FromString("$",rulenames),FromString(",",rulenames))));
			return rule;
		}
		public State Gethostport0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gethost(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(":",rulenames),Getport(rulenames)))));
			return rule;
		}
		public State Gethost0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gethostname(rulenames),GetIPv4address(rulenames),GetIPv6reference(rulenames));
			return rule;
		}
		public State Gethostname0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(-1,-1,(State.NoCloneConcatanation(Getdomainlabel(rulenames),FromString(".",rulenames)))),Gettoplabel(rulenames),State.NoCloneOption(FromString(".",rulenames))));
			return rule;
		}
		public State Getdomainlabel0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getalphanum(rulenames),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getalphanum(rulenames),State.Repeat(-1,-1,(State.NoCloneAlternation(Getalphanum(rulenames),FromString("-",rulenames)))),Getalphanum(rulenames))));
			return rule;
		}
		public State Gettoplabel0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetALPHA(rulenames),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetALPHA(rulenames),State.Repeat(-1,-1,(State.NoCloneAlternation(Getalphanum(rulenames),FromString("-",rulenames)))),Getalphanum(rulenames))));
			return rule;
		}
		public State GetIPv4address0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(1,3,GetDIGIT(rulenames)),FromString(".",rulenames),State.Repeat(1,3,GetDIGIT(rulenames)),FromString(".",rulenames),State.Repeat(1,3,GetDIGIT(rulenames)),FromString(".",rulenames),State.Repeat(1,3,GetDIGIT(rulenames))));
			return rule;
		}
		public State GetIPv6reference0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("[",rulenames),GetIPv6address(rulenames),FromString("]",rulenames)));
			return rule;
		}
		public State GetIPv6address0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gethexpart(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(":",rulenames),GetIPv4address(rulenames)))));
			return rule;
		}
		public State Gethexpart0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gethexseq(rulenames),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gethexseq(rulenames),FromString("::",rulenames),State.NoCloneOption(Gethexseq(rulenames)))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("::",rulenames),State.NoCloneOption(Gethexseq(rulenames)))));
			return rule;
		}
		public State Gethexseq0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gethex4(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(":",rulenames),Gethex4(rulenames))))));
			return rule;
		}
		public State Gethex40(List<string> rulenames)
		{
			State rule = State.Repeat(1,4,GetHEXDIG(rulenames));
			return rule;
		}
		public State Getport0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Geturi_parameters0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),Geturi_parameter(rulenames))));
			return rule;
		}
		public State Geturi_parameter0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gettransport_param(rulenames),Getuser_param(rulenames),Getmethod_param(rulenames),Getttl_param(rulenames),Getmaddr_param(rulenames),Getlr_param(rulenames),Getother_param(rulenames));
			return rule;
		}
		public State Gettransport_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("transport=",rulenames),(State.NoCloneAlternation(FromString("udp",rulenames),FromString("tcp",rulenames),FromString("sctp",rulenames),FromString("tls",rulenames),Getother_transport(rulenames)))));
			return rule;
		}
		public State Getother_transport0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getuser_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("user=",rulenames),(State.NoCloneAlternation(FromString("phone",rulenames),FromString("ip",rulenames),Getother_user(rulenames)))));
			return rule;
		}
		public State Getother_user0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getmethod_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("method=",rulenames),GetMethod(rulenames)));
			return rule;
		}
		public State Getttl_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("ttl=",rulenames),Getttl(rulenames)));
			return rule;
		}
		public State Getmaddr_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("maddr=",rulenames),Gethost(rulenames)));
			return rule;
		}
		public State Getlr_param0(List<string> rulenames)
		{
			State rule = FromString("lr",rulenames);
			return rule;
		}
		public State Getother_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getpname(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),Getpvalue(rulenames)))));
			return rule;
		}
		public State Getpname0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,Getparamchar(rulenames));
			return rule;
		}
		public State Getpvalue0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,Getparamchar(rulenames));
			return rule;
		}
		public State Getparamchar0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getparam_unreserved(rulenames),Getunreserved(rulenames),Getescaped(rulenames));
			return rule;
		}
		public State Getparam_unreserved0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("[",rulenames),FromString("]",rulenames),FromString("/",rulenames),FromString(":",rulenames),FromString("&",rulenames),FromString("+",rulenames),FromString("$",rulenames));
			return rule;
		}
		public State Getheaders0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("?",rulenames),Getheader(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString("&",rulenames),Getheader(rulenames))))));
			return rule;
		}
		public State Getheader0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gethname(rulenames),FromString("=",rulenames),Gethvalue(rulenames)));
			return rule;
		}
		public State Gethname0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,(State.NoCloneAlternation(Gethnv_unreserved(rulenames),Getunreserved(rulenames),Getescaped(rulenames))));
			return rule;
		}
		public State Gethvalue0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(Gethnv_unreserved(rulenames),Getunreserved(rulenames),Getescaped(rulenames))));
			return rule;
		}
		public State Gethnv_unreserved0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("[",rulenames),FromString("]",rulenames),FromString("/",rulenames),FromString("?",rulenames),FromString(":",rulenames),FromString("+",rulenames),FromString("$",rulenames));
			return rule;
		}
		public State GetSIP_message0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetRequest(rulenames),GetResponse(rulenames));
			return rule;
		}
		public State GetRequest0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetRequest_Line(rulenames),State.Repeat(-1,-1,(Getmessage_header(rulenames))),GetCRLF(rulenames),State.NoCloneOption(Getmessage_body(rulenames))));
			return rule;
		}
		public State GetRequest_Line0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetMethod(rulenames),GetSP(rulenames),GetRequest_URI(rulenames),GetSP(rulenames),GetSIP_Version(rulenames),GetCRLF(rulenames)));
			return rule;
		}
		public State GetRequest_URI0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetSIP_URI(rulenames),GetSIPS_URI(rulenames),GetabsoluteURI(rulenames));
			return rule;
		}
		public State GetabsoluteURI0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getscheme(rulenames),FromString(":",rulenames),(State.NoCloneAlternation(Gethier_part(rulenames),Getopaque_part(rulenames)))));
			return rule;
		}
		public State Gethier_part0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(Getnet_path(rulenames),Getabs_path(rulenames))),State.NoCloneOption(State.NoCloneConcatanation(FromString("?",rulenames),Getquery(rulenames)))));
			return rule;
		}
		public State Getnet_path0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("//",rulenames),Getauthority(rulenames),State.NoCloneOption(Getabs_path(rulenames))));
			return rule;
		}
		public State Getabs_path0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("/",rulenames),Getpath_segments(rulenames)));
			return rule;
		}
		public State Getopaque_part0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Geturic_no_slash(rulenames),State.Repeat(-1,-1,Geturic(rulenames))));
			return rule;
		}
		public State Geturic0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getreserved(rulenames),Getunreserved(rulenames),Getescaped(rulenames));
			return rule;
		}
		public State Geturic_no_slash0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getunreserved(rulenames),Getescaped(rulenames),FromString(";",rulenames),FromString("?",rulenames),FromString(":",rulenames),FromString("@",rulenames),FromString("&",rulenames),FromString("=",rulenames),FromString("+",rulenames),FromString("$",rulenames),FromString(",",rulenames));
			return rule;
		}
		public State Getpath_segments0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getsegment(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString("/",rulenames),Getsegment(rulenames))))));
			return rule;
		}
		public State Getsegment0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(-1,-1,Getpchar(rulenames)),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),Getparam(rulenames))))));
			return rule;
		}
		public State Getparam0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,Getpchar(rulenames));
			return rule;
		}
		public State Getpchar0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getunreserved(rulenames),Getescaped(rulenames),FromString(":",rulenames),FromString("@",rulenames),FromString("&",rulenames),FromString("=",rulenames),FromString("+",rulenames),FromString("$",rulenames),FromString(",",rulenames));
			return rule;
		}
		public State Getscheme0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetALPHA(rulenames),State.Repeat(-1,-1,(State.NoCloneAlternation(GetALPHA(rulenames),GetDIGIT(rulenames),FromString("+",rulenames),FromString("-",rulenames),FromString(".",rulenames))))));
			return rule;
		}
		public State Getauthority0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getsrvr(rulenames),Getreg_name(rulenames));
			return rule;
		}
		public State Getsrvr0(List<string> rulenames)
		{
			State rule = State.NoCloneOption(State.NoCloneConcatanation(State.NoCloneOption(State.NoCloneConcatanation(Getuserinfo(rulenames),FromString("@",rulenames))),Gethostport(rulenames)));
			return rule;
		}
		public State Getreg_name0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,(State.NoCloneAlternation(Getunreserved(rulenames),Getescaped(rulenames),FromString("$",rulenames),FromString(",",rulenames),FromString(";",rulenames),FromString(":",rulenames),FromString("@",rulenames),FromString("&",rulenames),FromString("=",rulenames),FromString("+",rulenames))));
			return rule;
		}
		public State Getquery0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,Geturic(rulenames));
			return rule;
		}
		public State GetSIP_Version0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("SIP",rulenames),FromString("/",rulenames),State.Repeat(1,-1,GetDIGIT(rulenames)),FromString(".",rulenames),State.Repeat(1,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State Getmessage_header0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(GetAccept(rulenames),GetAccept_Encoding(rulenames),GetAccept_Language(rulenames),GetAlert_Info(rulenames),GetAllow(rulenames),GetAuthentication_Info(rulenames),GetAuthorization(rulenames),GetCall_ID(rulenames),GetCall_Info(rulenames),GetContact(rulenames),GetContent_Disposition(rulenames),GetContent_Encoding(rulenames),GetContent_Language(rulenames),GetContent_Length(rulenames),GetContent_Type(rulenames),GetCSeq(rulenames),GetDate(rulenames),GetError_Info(rulenames),GetExpires(rulenames),GetFrom(rulenames),GetIn_Reply_To(rulenames),GetMax_Forwards(rulenames),GetMIME_Version(rulenames),GetMin_Expires(rulenames),GetOrganization(rulenames),GetPriority(rulenames),GetProxy_Authenticate(rulenames),GetProxy_Authorization(rulenames),GetProxy_Require(rulenames),GetRecord_Route(rulenames),GetReply_To(rulenames),GetRequire(rulenames),GetRetry_After(rulenames),GetRoute(rulenames),GetServer(rulenames),GetSubject(rulenames),GetSupported(rulenames),GetTimestamp(rulenames),GetTo(rulenames),GetUnsupported(rulenames),GetUser_Agent(rulenames),GetVia(rulenames),GetWarning(rulenames),GetWWW_Authenticate(rulenames),Getextension_header(rulenames))),GetCRLF(rulenames)));
			return rule;
		}
		public State GetINVITEm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x49,0x4E,0x56,0x49,0x54,0x45,});
			return rule;
		}
		public State GetACKm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x41,0x43,0x4B,});
			return rule;
		}
		public State GetOPTIONSm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x4F,0x50,0x54,0x49,0x4F,0x4E,0x53,});
			return rule;
		}
		public State GetBYEm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x42,0x59,0x45,});
			return rule;
		}
		public State GetCANCELm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x43,0x41,0x4E,0x43,0x45,0x4C,});
			return rule;
		}
		public State GetREGISTERm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x52,0x45,0x47,0x49,0x53,0x54,0x45,0x52,});
			return rule;
		}
		public State GetMethod0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetINVITEm(rulenames),GetACKm(rulenames),GetOPTIONSm(rulenames),GetBYEm(rulenames),GetCANCELm(rulenames),GetREGISTERm(rulenames),Getextension_method(rulenames));
			return rule;
		}
		public State Getextension_method0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetResponse0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetStatus_Line(rulenames),State.Repeat(-1,-1,(Getmessage_header(rulenames))),GetCRLF(rulenames),State.NoCloneOption(Getmessage_body(rulenames))));
			return rule;
		}
		public State GetStatus_Line0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetSIP_Version(rulenames),GetSP(rulenames),GetStatus_Code(rulenames),GetSP(rulenames),GetReason_Phrase(rulenames),GetCRLF(rulenames)));
			return rule;
		}
		public State GetStatus_Code0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetInformational(rulenames),GetRedirection(rulenames),GetSuccess(rulenames),GetClient_Error(rulenames),GetServer_Error(rulenames),GetGlobal_Failure(rulenames),Getextension_code(rulenames));
			return rule;
		}
		public State Getextension_code0(List<string> rulenames)
		{
			State rule = State.Repeat(3,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State GetReason_Phrase0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(Getreserved(rulenames),Getunreserved(rulenames),Getescaped(rulenames),GetUTF8_NONASCII(rulenames),GetUTF8_CONT(rulenames),GetSP(rulenames),GetHTAB(rulenames))));
			return rule;
		}
		public State GetInformational0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("100",rulenames),FromString("180",rulenames),FromString("181",rulenames),FromString("182",rulenames),FromString("183",rulenames));
			return rule;
		}
		public State GetSuccess0(List<string> rulenames)
		{
			State rule = FromString("200",rulenames);
			return rule;
		}
		public State GetRedirection0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("300",rulenames),FromString("301",rulenames),FromString("302",rulenames),FromString("305",rulenames),FromString("380",rulenames));
			return rule;
		}
		public State GetClient_Error0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("400",rulenames),FromString("401",rulenames),FromString("402",rulenames),FromString("403",rulenames),FromString("404",rulenames),FromString("405",rulenames),FromString("406",rulenames),FromString("407",rulenames),FromString("408",rulenames),FromString("410",rulenames),FromString("413",rulenames),FromString("414",rulenames),FromString("415",rulenames),FromString("416",rulenames),FromString("420",rulenames),FromString("421",rulenames),FromString("423",rulenames),FromString("480",rulenames),FromString("481",rulenames),FromString("482",rulenames),FromString("483",rulenames),FromString("484",rulenames),FromString("485",rulenames),FromString("486",rulenames),FromString("487",rulenames),FromString("488",rulenames),FromString("491",rulenames),FromString("493",rulenames));
			return rule;
		}
		public State GetServer_Error0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("500",rulenames),FromString("501",rulenames),FromString("502",rulenames),FromString("503",rulenames),FromString("504",rulenames),FromString("505",rulenames),FromString("513",rulenames));
			return rule;
		}
		public State GetGlobal_Failure0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("600",rulenames),FromString("603",rulenames),FromString("604",rulenames),FromString("606",rulenames));
			return rule;
		}
		public State GetAccept0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Accept",rulenames),GetHCOLON(rulenames),State.NoCloneOption(State.NoCloneRepeatBy(Getaccept_range(rulenames),GetCOMMA(rulenames)))));
			return rule;
		}
		public State Getaccept_range0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getmedia_range(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getaccept_param(rulenames))))));
			return rule;
		}
		public State Getmedia_range0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("*/*",rulenames),(State.NoCloneConcatanation(Getm_type(rulenames),GetSLASH(rulenames),FromString("*",rulenames))),(State.NoCloneConcatanation(Getm_type(rulenames),GetSLASH(rulenames),Getm_subtype(rulenames))))),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getm_parameter(rulenames))))));
			return rule;
		}
		public State Getaccept_param0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("q",rulenames),GetEQUAL(rulenames),Getqvalue(rulenames))),Getgeneric_param(rulenames));
			return rule;
		}
		public State Getqvalue0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("0",rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(".",rulenames),State.Repeat(0,3,GetDIGIT(rulenames)))))),(State.NoCloneConcatanation(FromString("1",rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(".",rulenames),State.Repeat(0,3,(FromString("0",rulenames))))))));
			return rule;
		}
		public State Getgeneric_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(GetEQUAL(rulenames),Getgen_value(rulenames)))));
			return rule;
		}
		public State Getgen_value0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gettoken(rulenames),Gethost(rulenames),Getquoted_string(rulenames));
			return rule;
		}
		public State GetAccept_Encoding0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Accept-Encoding",rulenames),GetHCOLON(rulenames),State.NoCloneOption(State.NoCloneRepeatBy(Getencoding(rulenames),GetCOMMA(rulenames)))));
			return rule;
		}
		public State Getencoding0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getcodings(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getaccept_param(rulenames))))));
			return rule;
		}
		public State Getcodings0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getcontent_coding(rulenames),FromString("*",rulenames));
			return rule;
		}
		public State Getcontent_coding0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetAccept_Language0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Accept-Language",rulenames),GetHCOLON(rulenames),State.NoCloneOption(State.NoCloneRepeatBy(Getlanguage(rulenames),GetCOMMA(rulenames)))));
			return rule;
		}
		public State Getlanguage0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getlanguage_range(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getaccept_param(rulenames))))));
			return rule;
		}
		public State Getlanguage_range0(List<string> rulenames)
		{
			State rule = (State.NoCloneAlternation((State.NoCloneConcatanation(State.Repeat(1,8,GetALPHA(rulenames)),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString("-",rulenames),State.Repeat(1,8,GetALPHA(rulenames))))))),FromString("*",rulenames)));
			return rule;
		}
		public State GetAlert_Info0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Alert-Info",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getalert_param(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getalert_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetLAQUOT(rulenames),GetabsoluteURI(rulenames),GetRAQUOT(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getgeneric_param(rulenames))))));
			return rule;
		}
		public State GetAllow0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Allow",rulenames),GetHCOLON(rulenames),State.NoCloneOption(State.NoCloneRepeatBy(GetMethod(rulenames),GetCOMMA(rulenames)))));
			return rule;
		}
		public State GetAuthorization0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Authorization",rulenames),GetHCOLON(rulenames),Getcredentials(rulenames)));
			return rule;
		}
		public State Getcredentials0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("Digest",rulenames),GetLWS(rulenames),Getdigest_response(rulenames))),Getother_response(rulenames));
			return rule;
		}
		public State Getdigest_response0(List<string> rulenames)
		{
			State rule = State.NoCloneRepeatBy(Getdig_resp(rulenames),GetCOMMA(rulenames));
			return rule;
		}
		public State Getdig_resp0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getusername(rulenames),Getrealm(rulenames),Getnonce(rulenames),Getdigest_uri(rulenames),Getdresponse(rulenames),Getalgorithm(rulenames),Getcnonce(rulenames),Getopaque(rulenames),Getmessage_qop(rulenames),Getnonce_count(rulenames),Getauth_param(rulenames));
			return rule;
		}
		public State Getusername0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("username",rulenames),GetEQUAL(rulenames),Getusername_value(rulenames)));
			return rule;
		}
		public State Getusername_value0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getdigest_uri0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("uri",rulenames),GetEQUAL(rulenames),GetLDQUOT(rulenames),Getdigest_uri_value(rulenames),GetRDQUOT(rulenames)));
			return rule;
		}
		public State Getdigest_uri_value0(List<string> rulenames)
		{
			State rule = GetRequest_URI(rulenames);
			return rule;
		}
		public State Getmessage_qop0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("qop",rulenames),GetEQUAL(rulenames),Getqop_value(rulenames)));
			return rule;
		}
		public State Getcnonce0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("cnonce",rulenames),GetEQUAL(rulenames),Getcnonce_value(rulenames)));
			return rule;
		}
		public State Getcnonce_value0(List<string> rulenames)
		{
			State rule = Getnonce_value(rulenames);
			return rule;
		}
		public State Getnonce_count0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("nc",rulenames),GetEQUAL(rulenames),Getnc_value(rulenames)));
			return rule;
		}
		public State Getnc_value0(List<string> rulenames)
		{
			State rule = State.Repeat(8,-1,GetLHEX(rulenames));
			return rule;
		}
		public State Getdresponse0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("response",rulenames),GetEQUAL(rulenames),Getrequest_digest(rulenames)));
			return rule;
		}
		public State Getrequest_digest0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetLDQUOT(rulenames),State.Repeat(32,-1,GetLHEX(rulenames)),GetRDQUOT(rulenames)));
			return rule;
		}
		public State Getauth_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getauth_param_name(rulenames),GetEQUAL(rulenames),(State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames)))));
			return rule;
		}
		public State Getauth_param_name0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getother_response0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getauth_scheme(rulenames),GetLWS(rulenames),State.NoCloneRepeatBy(Getauth_param(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getauth_scheme0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetAuthentication_Info0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Authentication-Info",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getainfo(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getainfo0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getnextnonce(rulenames),Getmessage_qop(rulenames),Getresponse_auth(rulenames),Getcnonce(rulenames),Getnonce_count(rulenames));
			return rule;
		}
		public State Getnextnonce0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("nextnonce",rulenames),GetEQUAL(rulenames),Getnonce_value(rulenames)));
			return rule;
		}
		public State Getresponse_auth0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("rspauth",rulenames),GetEQUAL(rulenames),Getresponse_digest(rulenames)));
			return rule;
		}
		public State Getresponse_digest0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetLDQUOT(rulenames),State.Repeat(-1,-1,GetLHEX(rulenames)),GetRDQUOT(rulenames)));
			return rule;
		}
		public State GetCall_ID0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Call-ID",rulenames),FromString("i",rulenames))),GetHCOLON(rulenames),Getcallid(rulenames)));
			return rule;
		}
		public State Getcallid0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getword(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("@",rulenames),Getword(rulenames)))));
			return rule;
		}
		public State GetCall_Info0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Call-Info",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getinfo(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getinfo0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetLAQUOT(rulenames),GetabsoluteURI(rulenames),GetRAQUOT(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getinfo_param(rulenames))))));
			return rule;
		}
		public State Getinfo_param0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("purpose",rulenames),GetEQUAL(rulenames),(State.NoCloneAlternation(FromString("icon",rulenames),FromString("info",rulenames),FromString("card",rulenames),Gettoken(rulenames))))),Getgeneric_param(rulenames));
			return rule;
		}
		public State GetContact0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Contact",rulenames),FromString("m",rulenames))),GetHCOLON(rulenames),(State.NoCloneAlternation(GetSTAR(rulenames),(State.NoCloneRepeatBy(Getcontact_param(rulenames),GetCOMMA(rulenames)))))));
			return rule;
		}
		public State Getcontact_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(Getname_addr(rulenames),Getaddr_spec(rulenames))),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getcontact_params(rulenames))))));
			return rule;
		}
		public State Getname_addr0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.NoCloneOption(Getdisplay_name(rulenames)),GetLAQUOT(rulenames),Getaddr_spec(rulenames),GetRAQUOT(rulenames)));
			return rule;
		}
		public State Getaddr_spec0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetSIP_URI(rulenames),GetSIPS_URI(rulenames),GetabsoluteURI(rulenames));
			return rule;
		}
		public State Getdisplay_name0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(State.Repeat(-1,-1,(State.NoCloneConcatanation(Gettoken(rulenames),GetLWS(rulenames)))),Getquoted_string(rulenames));
			return rule;
		}
		public State Getcontact_params0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getc_p_q(rulenames),Getc_p_expires(rulenames),Getcontact_extension(rulenames));
			return rule;
		}
		public State Getc_p_q0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("q",rulenames),GetEQUAL(rulenames),Getqvalue(rulenames)));
			return rule;
		}
		public State Getc_p_expires0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("expires",rulenames),GetEQUAL(rulenames),Getdelta_seconds(rulenames)));
			return rule;
		}
		public State Getcontact_extension0(List<string> rulenames)
		{
			State rule = Getgeneric_param(rulenames);
			return rule;
		}
		public State Getdelta_seconds0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State GetContent_Disposition0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-Disposition",rulenames),GetHCOLON(rulenames),Getdisp_type(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getdisp_param(rulenames))))));
			return rule;
		}
		public State Getdisp_type0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("render",rulenames),FromString("session",rulenames),FromString("icon",rulenames),FromString("alert",rulenames),Getdisp_extension_token(rulenames));
			return rule;
		}
		public State Getdisp_param0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gethandling_param(rulenames),Getgeneric_param(rulenames));
			return rule;
		}
		public State Gethandling_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("handling",rulenames),GetEQUAL(rulenames),(State.NoCloneAlternation(FromString("optional",rulenames),FromString("required",rulenames),Getother_handling(rulenames)))));
			return rule;
		}
		public State Getother_handling0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getdisp_extension_token0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetContent_Encoding0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Content-Encoding",rulenames),FromString("e",rulenames))),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getcontent_coding(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State GetContent_Language0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-Language",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getlanguage_tag(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getlanguage_tag0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getprimary_tag(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString("-",rulenames),Getsubtag(rulenames))))));
			return rule;
		}
		public State Getprimary_tag0(List<string> rulenames)
		{
			State rule = State.Repeat(1,8,GetALPHA(rulenames));
			return rule;
		}
		public State Getsubtag0(List<string> rulenames)
		{
			State rule = State.Repeat(1,8,GetALPHA(rulenames));
			return rule;
		}
		public State GetContent_Length0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Content-Length",rulenames),FromString("l",rulenames))),GetHCOLON(rulenames),State.Repeat(1,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State GetContent_Type0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Content-Type",rulenames),FromString("c",rulenames))),GetHCOLON(rulenames),Getmedia_type(rulenames)));
			return rule;
		}
		public State Getmedia_type0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getm_type(rulenames),GetSLASH(rulenames),Getm_subtype(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getm_parameter(rulenames))))));
			return rule;
		}
		public State Getm_type0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getdiscrete_type(rulenames),Getcomposite_type(rulenames));
			return rule;
		}
		public State Getdiscrete_type0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("text",rulenames),FromString("image",rulenames),FromString("audio",rulenames),FromString("video",rulenames),FromString("application",rulenames),Getextension_token(rulenames));
			return rule;
		}
		public State Getcomposite_type0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("message",rulenames),FromString("multipart",rulenames),Getextension_token(rulenames));
			return rule;
		}
		public State Getextension_token0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getietf_token(rulenames),Getx_token(rulenames));
			return rule;
		}
		public State Getietf_token0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getx_token0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("x-",rulenames),Gettoken(rulenames)));
			return rule;
		}
		public State Getm_subtype0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getextension_token(rulenames),Getiana_token(rulenames));
			return rule;
		}
		public State Getiana_token0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getm_parameter0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getm_attribute(rulenames),GetEQUAL(rulenames),Getm_value(rulenames)));
			return rule;
		}
		public State Getm_attribute0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getm_value0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames));
			return rule;
		}
		public State GetCSeq0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("CSeq",rulenames),GetHCOLON(rulenames),State.Repeat(1,-1,GetDIGIT(rulenames)),GetLWS(rulenames),GetMethod(rulenames)));
			return rule;
		}
		public State GetDate0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Date",rulenames),GetHCOLON(rulenames),GetSIP_date(rulenames)));
			return rule;
		}
		public State GetSIP_date0(List<string> rulenames)
		{
			State rule = Getrfc1123_date(rulenames);
			return rule;
		}
		public State Getrfc1123_date0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getwkday(rulenames),FromString(",",rulenames),GetSP(rulenames),Getdate1(rulenames),GetSP(rulenames),Gettime(rulenames),GetSP(rulenames),FromString("GMT",rulenames)));
			return rule;
		}
		public State Getdate10(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(2,-1,GetDIGIT(rulenames)),GetSP(rulenames),Getmonth(rulenames),GetSP(rulenames),State.Repeat(4,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State Gettime0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(2,-1,GetDIGIT(rulenames)),FromString(":",rulenames),State.Repeat(2,-1,GetDIGIT(rulenames)),FromString(":",rulenames),State.Repeat(2,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State Getwkday0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("Mon",rulenames),FromString("Tue",rulenames),FromString("Wed",rulenames),FromString("Thu",rulenames),FromString("Fri",rulenames),FromString("Sat",rulenames),FromString("Sun",rulenames));
			return rule;
		}
		public State Getmonth0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("Jan",rulenames),FromString("Feb",rulenames),FromString("Mar",rulenames),FromString("Apr",rulenames),FromString("May",rulenames),FromString("Jun",rulenames),FromString("Jul",rulenames),FromString("Aug",rulenames),FromString("Sep",rulenames),FromString("Oct",rulenames),FromString("Nov",rulenames),FromString("Dec",rulenames));
			return rule;
		}
		public State GetError_Info0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Error-Info",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Geterror_uri(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Geterror_uri0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetLAQUOT(rulenames),GetabsoluteURI(rulenames),GetRAQUOT(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getgeneric_param(rulenames))))));
			return rule;
		}
		public State GetExpires0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Expires",rulenames),GetHCOLON(rulenames),Getdelta_seconds(rulenames)));
			return rule;
		}
		public State GetFrom0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("From",rulenames),FromString("f",rulenames))),GetHCOLON(rulenames),Getfrom_spec(rulenames)));
			return rule;
		}
		public State Getfrom_spec0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(Getname_addr(rulenames),Getaddr_spec(rulenames))),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getfrom_param(rulenames))))));
			return rule;
		}
		public State Getfrom_param0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gettag_param(rulenames),Getgeneric_param(rulenames));
			return rule;
		}
		public State Gettag_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("tag",rulenames),GetEQUAL(rulenames),Gettoken(rulenames)));
			return rule;
		}
		public State GetIn_Reply_To0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("In-Reply-To",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getcallid(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State GetMax_Forwards0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Max-Forwards",rulenames),GetHCOLON(rulenames),State.Repeat(1,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State GetMIME_Version0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("MIME-Version",rulenames),GetHCOLON(rulenames),State.Repeat(1,-1,GetDIGIT(rulenames)),FromString(".",rulenames),State.Repeat(1,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State GetMin_Expires0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Min-Expires",rulenames),GetHCOLON(rulenames),Getdelta_seconds(rulenames)));
			return rule;
		}
		public State GetOrganization0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Organization",rulenames),GetHCOLON(rulenames),State.NoCloneOption(GetTEXT_UTF8_TRIM(rulenames))));
			return rule;
		}
		public State GetPriority0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Priority",rulenames),GetHCOLON(rulenames),Getpriority_value(rulenames)));
			return rule;
		}
		public State Getpriority_value0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("emergency",rulenames),FromString("urgent",rulenames),FromString("normal",rulenames),FromString("non-urgent",rulenames),Getother_priority(rulenames));
			return rule;
		}
		public State Getother_priority0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetProxy_Authenticate0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Proxy-Authenticate",rulenames),GetHCOLON(rulenames),Getchallenge(rulenames)));
			return rule;
		}
		public State Getchallenge0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("Digest",rulenames),GetLWS(rulenames),State.NoCloneRepeatBy(Getdigest_cln(rulenames),GetCOMMA(rulenames)))),Getother_challenge(rulenames));
			return rule;
		}
		public State Getother_challenge0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getauth_scheme(rulenames),GetLWS(rulenames),State.NoCloneRepeatBy(Getauth_param(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getdigest_cln0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getrealm(rulenames),Getdomain(rulenames),Getnonce(rulenames),Getopaque(rulenames),Getstale(rulenames),Getalgorithm(rulenames),Getqop_options(rulenames),Getauth_param(rulenames));
			return rule;
		}
		public State Getrealm0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("realm",rulenames),GetEQUAL(rulenames),Getrealm_value(rulenames)));
			return rule;
		}
		public State Getrealm_value0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getdomain0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("domain",rulenames),GetEQUAL(rulenames),GetLDQUOT(rulenames),GetURI(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(State.Repeat(1,-1,GetSP(rulenames)),GetURI(rulenames)))),GetRDQUOT(rulenames)));
			return rule;
		}
		public State GetURI0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetabsoluteURI(rulenames),Getabs_path(rulenames));
			return rule;
		}
		public State Getnonce0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("nonce",rulenames),GetEQUAL(rulenames),Getnonce_value(rulenames)));
			return rule;
		}
		public State Getnonce_value0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getopaque0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("opaque",rulenames),GetEQUAL(rulenames),Getquoted_string(rulenames)));
			return rule;
		}
		public State Getstale0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("stale",rulenames),GetEQUAL(rulenames),(State.NoCloneAlternation(FromString("true",rulenames),FromString("false",rulenames)))));
			return rule;
		}
		public State Getalgorithm0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("algorithm",rulenames),GetEQUAL(rulenames),(State.NoCloneAlternation(FromString("MD5",rulenames),FromString("MD5-sess",rulenames),Gettoken(rulenames)))));
			return rule;
		}
		public State Getqop_options0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("qop",rulenames),GetEQUAL(rulenames),GetLDQUOT(rulenames),Getqop_value(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(",",rulenames),Getqop_value(rulenames)))),GetRDQUOT(rulenames)));
			return rule;
		}
		public State Getqop_value0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("auth",rulenames),FromString("auth-int",rulenames),Gettoken(rulenames));
			return rule;
		}
		public State GetProxy_Authorization0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Proxy-Authorization",rulenames),GetHCOLON(rulenames),Getcredentials(rulenames)));
			return rule;
		}
		public State GetProxy_Require0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Proxy-Require",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getoption_tag(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getoption_tag0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetRecord_Route0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Record-Route",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getrec_route(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getrec_route0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getname_addr(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getrr_param(rulenames))))));
			return rule;
		}
		public State Getrr_param0(List<string> rulenames)
		{
			State rule = Getgeneric_param(rulenames);
			return rule;
		}
		public State GetReply_To0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Reply-To",rulenames),GetHCOLON(rulenames),Getrplyto_spec(rulenames)));
			return rule;
		}
		public State Getrplyto_spec0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(Getname_addr(rulenames),Getaddr_spec(rulenames))),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getrplyto_param(rulenames))))));
			return rule;
		}
		public State Getrplyto_param0(List<string> rulenames)
		{
			State rule = Getgeneric_param(rulenames);
			return rule;
		}
		public State GetRequire0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Require",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getoption_tag(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State GetRetry_After0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Retry-After",rulenames),GetHCOLON(rulenames),Getdelta_seconds(rulenames),State.NoCloneOption(Getcomment(rulenames)),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getretry_param(rulenames))))));
			return rule;
		}
		public State Getretry_param0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("duration",rulenames),GetEQUAL(rulenames),Getdelta_seconds(rulenames))),Getgeneric_param(rulenames));
			return rule;
		}
		public State GetRoute0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Route",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getroute_param(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getroute_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getname_addr(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getrr_param(rulenames))))));
			return rule;
		}
		public State GetServer0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Server",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getserver_val(rulenames),GetLWS(rulenames))));
			return rule;
		}
		public State Getserver_val0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getproduct(rulenames),Getcomment(rulenames));
			return rule;
		}
		public State Getproduct0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(GetSLASH(rulenames),Getproduct_version(rulenames)))));
			return rule;
		}
		public State Getproduct_version0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetSubject0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Subject",rulenames),FromString("s",rulenames))),GetHCOLON(rulenames),State.NoCloneOption(GetTEXT_UTF8_TRIM(rulenames))));
			return rule;
		}
		public State GetSupported0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Supported",rulenames),FromString("k",rulenames))),GetHCOLON(rulenames),State.NoCloneOption(State.NoCloneRepeatBy(Getoption_tag(rulenames),GetCOMMA(rulenames)))));
			return rule;
		}
		public State GetTimestamp0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Timestamp",rulenames),GetHCOLON(rulenames),State.Repeat(1,-1,(GetDIGIT(rulenames))),State.NoCloneOption(State.NoCloneConcatanation(FromString(".",rulenames),State.Repeat(-1,-1,(GetDIGIT(rulenames))))),State.NoCloneOption(State.NoCloneConcatanation(GetLWS(rulenames),Getdelay(rulenames)))));
			return rule;
		}
		public State Getdelay0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(-1,-1,(GetDIGIT(rulenames))),State.NoCloneOption(State.NoCloneConcatanation(FromString(".",rulenames),State.Repeat(-1,-1,(GetDIGIT(rulenames)))))));
			return rule;
		}
		public State GetTo0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("To",rulenames),FromString("t",rulenames))),GetHCOLON(rulenames),(State.NoCloneAlternation(Getname_addr(rulenames),Getaddr_spec(rulenames))),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getto_param(rulenames))))));
			return rule;
		}
		public State Getto_param0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gettag_param(rulenames),Getgeneric_param(rulenames));
			return rule;
		}
		public State GetUnsupported0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Unsupported",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getoption_tag(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State GetUser_Agent0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("User-Agent",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getserver_val(rulenames),GetLWS(rulenames))));
			return rule;
		}
		public State GetVia0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Via",rulenames),FromString("v",rulenames))),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getvia_parm(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getvia_parm0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getsent_protocol(rulenames),GetLWS(rulenames),Getsent_by(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getvia_params(rulenames))))));
			return rule;
		}
		public State Getvia_params0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getvia_ttl(rulenames),Getvia_maddr(rulenames),Getvia_received(rulenames),Getvia_branch(rulenames),Getvia_extension(rulenames));
			return rule;
		}
		public State Getvia_ttl0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("ttl",rulenames),GetEQUAL(rulenames),Getttl(rulenames)));
			return rule;
		}
		public State Getvia_maddr0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("maddr",rulenames),GetEQUAL(rulenames),Gethost(rulenames)));
			return rule;
		}
		public State Getvia_received0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("received",rulenames),GetEQUAL(rulenames),(State.NoCloneAlternation(GetIPv4address(rulenames),GetIPv6address(rulenames)))));
			return rule;
		}
		public State Getvia_branch0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("branch",rulenames),GetEQUAL(rulenames),Gettoken(rulenames)));
			return rule;
		}
		public State Getvia_extension0(List<string> rulenames)
		{
			State rule = Getgeneric_param(rulenames);
			return rule;
		}
		public State Getsent_protocol0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getprotocol_name(rulenames),GetSLASH(rulenames),Getprotocol_version(rulenames),GetSLASH(rulenames),Gettransport(rulenames)));
			return rule;
		}
		public State Getprotocol_name0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("SIP",rulenames),Gettoken(rulenames));
			return rule;
		}
		public State Getprotocol_version0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Gettransport0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("UDP",rulenames),FromString("TCP",rulenames),FromString("TLS",rulenames),FromString("SCTP",rulenames),Getother_transport(rulenames));
			return rule;
		}
		public State Getsent_by0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gethost(rulenames),State.NoCloneOption(State.NoCloneConcatanation(GetCOLON(rulenames),Getport(rulenames)))));
			return rule;
		}
		public State Getttl0(List<string> rulenames)
		{
			State rule = State.Repeat(1,3,GetDIGIT(rulenames));
			return rule;
		}
		public State GetWarning0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Warning",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getwarning_value(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getwarning_value0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getwarn_code(rulenames),GetSP(rulenames),Getwarn_agent(rulenames),GetSP(rulenames),Getwarn_text(rulenames)));
			return rule;
		}
		public State Getwarn_code0(List<string> rulenames)
		{
			State rule = State.Repeat(3,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Getwarn_agent0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gethostport(rulenames),Getpseudonym(rulenames));
			return rule;
		}
		public State Getwarn_text0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getpseudonym0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetWWW_Authenticate0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("WWW-Authenticate",rulenames),GetHCOLON(rulenames),Getchallenge(rulenames)));
			return rule;
		}
		public State Getextension_header0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getheader_name(rulenames),GetHCOLON(rulenames),Getheader_value(rulenames)));
			return rule;
		}
		public State Getheader_name0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getheader_value0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(GetTEXT_UTF8char(rulenames),GetUTF8_CONT(rulenames),GetLWS(rulenames))));
			return rule;
		}
		public State Getmessage_body0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,GetOCTET(rulenames));
			return rule;
		}
		public State GetSUBSCRIBEm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x53,0x55,0x42,0x53,0x43,0x52,0x49,0x42,0x45,});
			return rule;
		}
		public State GetNOTIFYm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x4E,0x4F,0x54,0x49,0x46,0x59,});
			return rule;
		}
		public State Getextension_method1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetSUBSCRIBEm(rulenames),GetNOTIFYm(rulenames),Gettoken(rulenames));
			return rule;
		}
		public State GetEvent0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Event",rulenames),FromString("o",rulenames))),GetHCOLON(rulenames),Getevent_type(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getevent_param(rulenames))))));
			return rule;
		}
		public State Getevent_type0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getevent_package(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(".",rulenames),Getevent_template(rulenames))))));
			return rule;
		}
		public State Getevent_package0(List<string> rulenames)
		{
			State rule = Gettoken_nodot(rulenames);
			return rule;
		}
		public State Getevent_template0(List<string> rulenames)
		{
			State rule = Gettoken_nodot(rulenames);
			return rule;
		}
		public State Gettoken_nodot0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,(State.NoCloneAlternation(Getalphanum(rulenames),FromString("-",rulenames),FromString("!",rulenames),FromString("%",rulenames),FromString("*",rulenames),FromString("_",rulenames),FromString("+",rulenames),FromString("`",rulenames),FromString("'",rulenames),FromString("~",rulenames))));
			return rule;
		}
		public State Getevent_param0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getgeneric_param(rulenames),(State.NoCloneConcatanation(FromString("id",rulenames),GetEQUAL(rulenames),Gettoken(rulenames))));
			return rule;
		}
		public State GetAllow_Events0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("Allow-Events",rulenames),FromString("u",rulenames))),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getevent_type(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State GetSubscription_State0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Subscription-State",rulenames),GetHCOLON(rulenames),Getsubstate_value(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(GetSEMI(rulenames),Getsubexp_params(rulenames))))));
			return rule;
		}
		public State Getsubstate_value0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("active",rulenames),FromString("pending",rulenames),FromString("terminated",rulenames),Getextension_substate(rulenames));
			return rule;
		}
		public State Getextension_substate0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getsubexp_params0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("reason",rulenames),GetEQUAL(rulenames),Getevent_reason_value(rulenames))),(State.NoCloneConcatanation(FromString("expires",rulenames),GetEQUAL(rulenames),Getdelta_seconds(rulenames))),(State.NoCloneConcatanation(FromString("retry-after",rulenames),GetEQUAL(rulenames),Getdelta_seconds(rulenames))),Getgeneric_param(rulenames));
			return rule;
		}
		public State Getevent_reason_value0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("deactivated",rulenames),FromString("probation",rulenames),FromString("rejected",rulenames),FromString("timeout",rulenames),FromString("giveup",rulenames),FromString("noresource",rulenames),Getevent_reason_extension(rulenames));
			return rule;
		}
		public State Getevent_reason_extension0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getextension_header1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetEvent(rulenames),GetAllow_Events(rulenames),GetSubscription_State(rulenames));
			return rule;
		}
		public State GetSERVICEm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x53,0x45,0x52,0x56,0x49,0x43,0x45,});
			return rule;
		}
		public State GetBENOTIFYm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x42,0x45,0x4E,0x4F,0x54,0x49,0x46,0x59,});
			return rule;
		}
		public State GetMESSAGEm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x4D,0x45,0x53,0x53,0x41,0x47,0x45,});
			return rule;
		}
		public State GetINFOm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x49,0x4E,0x46,0x4F,});
			return rule;
		}
		public State GetREFERm0(List<string> rulenames)
		{
			State rule = (State)(new byte[] {0x52,0x45,0x46,0x45,0x52,});
			return rule;
		}
		public State Getextension_method2(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetSERVICEm(rulenames),GetBENOTIFYm(rulenames),GetMESSAGEm(rulenames),GetINFOm(rulenames),GetREFERm(rulenames));
			return rule;
		}
		public State Getfrom_param1(List<string> rulenames)
		{
			State rule = Getepid_param(rulenames);
			return rule;
		}
		public State Getto_param1(List<string> rulenames)
		{
			State rule = Getepid_param(rulenames);
			return rule;
		}
		public State Getepid_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("epid=",rulenames),Getepid_param_value(rulenames)));
			return rule;
		}
		public State Getepid_param_value0(List<string> rulenames)
		{
			State rule = State.Repeat(1,16,Gettokenchar(rulenames));
			return rule;
		}
		public State Gettokenchar0(List<string> rulenames)
		{
			State rule = (State.NoCloneAlternation(Getalphanum(rulenames),FromString("-",rulenames),FromString(".",rulenames),FromString("!",rulenames),FromString("%",rulenames),FromString("*",rulenames),FromString("_",rulenames),FromString("+",rulenames),FromString("`",rulenames),FromString("'",rulenames),FromString("~",rulenames)));
			return rule;
		}
		public State Getcontact_params1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getc_p_proxy(rulenames),Getc_p_instance(rulenames));
			return rule;
		}
		public State Getc_p_proxy0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("proxy=",rulenames),FromString("replace",rulenames)));
			return rule;
		}
		public State Getc_p_instance0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("+sip.instance",rulenames),GetEQUAL(rulenames),GetLDQUOT(rulenames),FromString("<",rulenames),Getinstance_val(rulenames),FromString(">",rulenames),GetRDQUOT(rulenames)));
			return rule;
		}
		public State Getinstance_val0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,Geturic(rulenames));
			return rule;
		}
		public State Geturi_parameter1(List<string> rulenames)
		{
			State rule = Getms_received_cid_param(rulenames);
			return rule;
		}
		public State Getms_received_cid_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("ms-received-cid=",rulenames),Getpvalue(rulenames)));
			return rule;
		}
		public State Getcredentials1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("NTLM",rulenames),GetLWS(rulenames),Getmsspi_response(rulenames))),(State.NoCloneConcatanation(FromString("Kerberos",rulenames),GetLWS(rulenames),Getmsspi_response(rulenames))),(State.NoCloneConcatanation(FromString("TLS-DSK",rulenames),GetLWS(rulenames),Getmsspi_response(rulenames))));
			return rule;
		}
		public State Getmsspi_response0(List<string> rulenames)
		{
			State rule = State.NoCloneRepeatBy(Getmsspi_resp(rulenames),GetCOMMA(rulenames));
			return rule;
		}
		public State Getmsspi_resp0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getqop_options(rulenames),Getrealm(rulenames),Getopaque(rulenames),Getversion(rulenames),Gettargetname(rulenames),Getgssapi_data(rulenames),Getcrand(rulenames),Getcnum(rulenames),Getmsspi_resp_data(rulenames));
			return rule;
		}
		public State Getcnum0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("cnum",rulenames),GetEQUAL(rulenames),Getcnum_value(rulenames)));
			return rule;
		}
		public State Getcnum_value0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Getcrand0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("crand",rulenames),GetEQUAL(rulenames),Getcrand_val(rulenames)));
			return rule;
		}
		public State Getcrand_val0(List<string> rulenames)
		{
			State rule = State.Repeat(8,-1,GetLHEX(rulenames));
			return rule;
		}
		public State Getmsspi_resp_data0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("response",rulenames),GetEQUAL(rulenames),Getmsspi_resp_data_value(rulenames)));
			return rule;
		}
		public State Getmsspi_resp_data_value0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getchallenge1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("NTLM",rulenames),GetLWS(rulenames),State.NoCloneRepeatBy(Getmsspi_cln(rulenames),GetCOMMA(rulenames)))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Kerberos",rulenames),GetLWS(rulenames),State.NoCloneRepeatBy(Getmsspi_cln(rulenames),GetCOMMA(rulenames)))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("TLS-DSK",rulenames),GetLWS(rulenames),State.NoCloneRepeatBy(Getmsspi_cln(rulenames),GetCOMMA(rulenames)))));
			return rule;
		}
		public State Getmsspi_cln0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getrealm(rulenames),Getopaque(rulenames),Gettargetname(rulenames),Getgssapi_data(rulenames),Getversion(rulenames));
			return rule;
		}
		public State Gettargetname0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("targetname",rulenames),GetEQUAL(rulenames),Gettarget_value(rulenames)));
			return rule;
		}
		public State Gettarget_value0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetDQUOTE(rulenames),(State.NoCloneAlternation(Getntlm_target_val(rulenames),(State.NoCloneConcatanation(FromString("sip/",rulenames),Getkerberos_target_val(rulenames))),Gettls_dsk_target_val(rulenames))),GetDQUOTE(rulenames)));
			return rule;
		}
		public State Getntlm_target_val0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getkerberos_target_val0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Gettls_dsk_target_val0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getgssapi_data0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("gssapi-data",rulenames),GetEQUAL(rulenames),Getgssapi_data_value(rulenames)));
			return rule;
		}
		public State Getgssapi_data_value0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getversion0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("version",rulenames),GetEQUAL(rulenames),Getversion_value(rulenames)));
			return rule;
		}
		public State Getversion_value0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State GetProxy_Authentication_Info0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Proxy-Authentication-Info",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getainfo(rulenames),GetCOMMA(rulenames))));
			return rule;
		}
		public State Getainfo1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getnextnonce(rulenames),Getmessage_qop(rulenames),Getresponse_auth(rulenames),Getcnonce(rulenames),Getnonce_count(rulenames),FromString("NTLM",rulenames),FromString("Kerberos",rulenames),Getsnum(rulenames),Getsrand(rulenames),Getrealm(rulenames),Gettargetname(rulenames),Getopaque(rulenames));
			return rule;
		}
		public State Getsnum0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("snum",rulenames),GetEQUAL(rulenames),Getsnum_value(rulenames)));
			return rule;
		}
		public State Getsnum_value0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Getsrand0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("srand",rulenames),GetEQUAL(rulenames),Getsrand_value(rulenames)));
			return rule;
		}
		public State Getsrand_value0(List<string> rulenames)
		{
			State rule = State.Repeat(8,-1,GetLHEX(rulenames));
			return rule;
		}
		public State Getextension_header2(List<string> rulenames)
		{
			State rule = GetProxy_Authentication_Info(rulenames);
			return rule;
		}
		public State Getmessage_headerX0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(GetAccept(rulenames),GetAccept_Encoding(rulenames),GetAccept_Language(rulenames),GetAlert_Info(rulenames),GetAllow(rulenames),GetAuthentication_Info(rulenames),GetAuthorization(rulenames),GetCall_ID(rulenames),GetCall_Info(rulenames),GetContent_Disposition(rulenames),GetContent_Encoding(rulenames),GetContent_Language(rulenames),GetContent_Length(rulenames),GetContent_Type(rulenames),GetCSeq(rulenames),GetDate(rulenames),GetError_Info(rulenames),GetExpires(rulenames),GetIn_Reply_To(rulenames),GetMax_Forwards(rulenames),GetMIME_Version(rulenames),GetMin_Expires(rulenames),GetOrganization(rulenames),GetPriority(rulenames),GetProxy_Authenticate(rulenames),GetProxy_Authorization(rulenames),GetProxy_Require(rulenames),GetReply_To(rulenames),GetRequire(rulenames),GetRetry_After(rulenames),GetServer(rulenames),GetSubject(rulenames),GetSupported(rulenames),GetTimestamp(rulenames),GetUnsupported(rulenames),GetUser_Agent(rulenames),GetWarning(rulenames),GetWWW_Authenticate(rulenames),GetContact(rulenames),GetFrom(rulenames),GetRecord_Route(rulenames),GetRoute(rulenames),GetTo(rulenames),GetVia(rulenames),Getextension_header(rulenames))),GetCRLF(rulenames)));
			return rule;
		}
		public State Getmessage_headerZ0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(GetRecord_Route(rulenames),GetRoute(rulenames),GetVia(rulenames),GetCall_ID(rulenames),Getextension_header(rulenames))),GetCRLF(rulenames)));
			return rule;
		}
		public State Getmessage_headerW0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(GetVia(rulenames)),GetCRLF(rulenames)));
			return rule;
		}
		public State GetSIP_messageX0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(GetRequest_Line(rulenames),GetStatus_Line(rulenames))),State.Repeat(-1,-1,(Getmessage_headerX(rulenames))),GetCRLF(rulenames)));
			return rule;
		}
		public State Getalphanum(List<string> rulenames)
		{
			rulenames.Insert(0, "alphanum");
			State rule = State.NoCloneAlternation(Getalphanum0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getreserved(List<string> rulenames)
		{
			rulenames.Insert(0, "reserved");
			State rule = State.NoCloneAlternation(Getreserved0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getunreserved(List<string> rulenames)
		{
			rulenames.Insert(0, "unreserved");
			State rule = State.NoCloneAlternation(Getunreserved0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmark(List<string> rulenames)
		{
			rulenames.Insert(0, "mark");
			State rule = State.NoCloneAlternation(Getmark0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getescaped(List<string> rulenames)
		{
			rulenames.Insert(0, "escaped");
			State rule = State.NoCloneAlternation(Getescaped0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLWS(List<string> rulenames)
		{
			rulenames.Insert(0, "LWS");
			State rule = State.NoCloneAlternation(GetLWS0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSWS(List<string> rulenames)
		{
			rulenames.Insert(0, "SWS");
			State rule = State.NoCloneAlternation(GetSWS0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetHCOLON(List<string> rulenames)
		{
			rulenames.Insert(0, "HCOLON");
			State rule = State.NoCloneAlternation(GetHCOLON0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetTEXT_UTF8_TRIM(List<string> rulenames)
		{
			rulenames.Insert(0, "TEXT-UTF8-TRIM");
			State rule = State.NoCloneAlternation(GetTEXT_UTF8_TRIM0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetTEXT_UTF8char(List<string> rulenames)
		{
			rulenames.Insert(0, "TEXT-UTF8char");
			State rule = State.NoCloneAlternation(GetTEXT_UTF8char0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetUTF8_NONASCII(List<string> rulenames)
		{
			rulenames.Insert(0, "UTF8-NONASCII");
			State rule = State.NoCloneAlternation(GetUTF8_NONASCII0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetUTF8_CONT(List<string> rulenames)
		{
			rulenames.Insert(0, "UTF8-CONT");
			State rule = State.NoCloneAlternation(GetUTF8_CONT0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLHEX(List<string> rulenames)
		{
			rulenames.Insert(0, "LHEX");
			State rule = State.NoCloneAlternation(GetLHEX0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettoken(List<string> rulenames)
		{
			rulenames.Insert(0, "token");
			State rule = State.NoCloneAlternation(Gettoken0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getseparators(List<string> rulenames)
		{
			rulenames.Insert(0, "separators");
			State rule = State.NoCloneAlternation(Getseparators0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getword(List<string> rulenames)
		{
			rulenames.Insert(0, "word");
			State rule = State.NoCloneAlternation(Getword0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSTAR(List<string> rulenames)
		{
			rulenames.Insert(0, "STAR");
			State rule = State.NoCloneAlternation(GetSTAR0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSLASH(List<string> rulenames)
		{
			rulenames.Insert(0, "SLASH");
			State rule = State.NoCloneAlternation(GetSLASH0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetEQUAL(List<string> rulenames)
		{
			rulenames.Insert(0, "EQUAL");
			State rule = State.NoCloneAlternation(GetEQUAL0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLPAREN(List<string> rulenames)
		{
			rulenames.Insert(0, "LPAREN");
			State rule = State.NoCloneAlternation(GetLPAREN0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRPAREN(List<string> rulenames)
		{
			rulenames.Insert(0, "RPAREN");
			State rule = State.NoCloneAlternation(GetRPAREN0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRAQUOT(List<string> rulenames)
		{
			rulenames.Insert(0, "RAQUOT");
			State rule = State.NoCloneAlternation(GetRAQUOT0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLAQUOT(List<string> rulenames)
		{
			rulenames.Insert(0, "LAQUOT");
			State rule = State.NoCloneAlternation(GetLAQUOT0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCOMMA(List<string> rulenames)
		{
			rulenames.Insert(0, "COMMA");
			State rule = State.NoCloneAlternation(GetCOMMA0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSEMI(List<string> rulenames)
		{
			rulenames.Insert(0, "SEMI");
			State rule = State.NoCloneAlternation(GetSEMI0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCOLON(List<string> rulenames)
		{
			rulenames.Insert(0, "COLON");
			State rule = State.NoCloneAlternation(GetCOLON0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLDQUOT(List<string> rulenames)
		{
			rulenames.Insert(0, "LDQUOT");
			State rule = State.NoCloneAlternation(GetLDQUOT0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRDQUOT(List<string> rulenames)
		{
			rulenames.Insert(0, "RDQUOT");
			State rule = State.NoCloneAlternation(GetRDQUOT0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcomment(List<string> rulenames)
		{
			rulenames.Insert(0, "comment");
			State rule = State.NoCloneAlternation(Getcomment0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getctext(List<string> rulenames)
		{
			rulenames.Insert(0, "ctext");
			State rule = State.NoCloneAlternation(Getctext0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getquoted_string(List<string> rulenames)
		{
			rulenames.Insert(0, "quoted-string");
			State rule = State.NoCloneAlternation(Getquoted_string0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getqdtext(List<string> rulenames)
		{
			rulenames.Insert(0, "qdtext");
			State rule = State.NoCloneAlternation(Getqdtext0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetALPHA(List<string> rulenames)
		{
			rulenames.Insert(0, "ALPHA");
			State rule = State.NoCloneAlternation(GetALPHA0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetBIT(List<string> rulenames)
		{
			rulenames.Insert(0, "BIT");
			State rule = State.NoCloneAlternation(GetBIT0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCHAR(List<string> rulenames)
		{
			rulenames.Insert(0, "CHAR");
			State rule = State.NoCloneAlternation(GetCHAR0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCR(List<string> rulenames)
		{
			rulenames.Insert(0, "CR");
			State rule = State.NoCloneAlternation(GetCR0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCRLF(List<string> rulenames)
		{
			rulenames.Insert(0, "CRLF");
			State rule = State.NoCloneAlternation(GetCRLF0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCTL(List<string> rulenames)
		{
			rulenames.Insert(0, "CTL");
			State rule = State.NoCloneAlternation(GetCTL0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetDIGIT(List<string> rulenames)
		{
			rulenames.Insert(0, "DIGIT");
			State rule = State.NoCloneAlternation(GetDIGIT0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetDQUOTE(List<string> rulenames)
		{
			rulenames.Insert(0, "DQUOTE");
			State rule = State.NoCloneAlternation(GetDQUOTE0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetHEXDIG(List<string> rulenames)
		{
			rulenames.Insert(0, "HEXDIG");
			State rule = State.NoCloneAlternation(GetHEXDIG0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetHTAB(List<string> rulenames)
		{
			rulenames.Insert(0, "HTAB");
			State rule = State.NoCloneAlternation(GetHTAB0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLF(List<string> rulenames)
		{
			rulenames.Insert(0, "LF");
			State rule = State.NoCloneAlternation(GetLF0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLWSP(List<string> rulenames)
		{
			rulenames.Insert(0, "LWSP");
			State rule = State.NoCloneAlternation(GetLWSP0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetOCTET(List<string> rulenames)
		{
			rulenames.Insert(0, "OCTET");
			State rule = State.NoCloneAlternation(GetOCTET0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSP(List<string> rulenames)
		{
			rulenames.Insert(0, "SP");
			State rule = State.NoCloneAlternation(GetSP0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetVCHAR(List<string> rulenames)
		{
			rulenames.Insert(0, "VCHAR");
			State rule = State.NoCloneAlternation(GetVCHAR0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetWSP(List<string> rulenames)
		{
			rulenames.Insert(0, "WSP");
			State rule = State.NoCloneAlternation(GetWSP0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getquoted_pair(List<string> rulenames)
		{
			rulenames.Insert(0, "quoted-pair");
			State rule = State.NoCloneAlternation(Getquoted_pair0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSIP_URI(List<string> rulenames)
		{
			rulenames.Insert(0, "SIP-URI");
			State rule = State.NoCloneAlternation(GetSIP_URI0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSIPS_URI(List<string> rulenames)
		{
			rulenames.Insert(0, "SIPS-URI");
			State rule = State.NoCloneAlternation(GetSIPS_URI0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getuserinfo(List<string> rulenames)
		{
			rulenames.Insert(0, "userinfo");
			State rule = State.NoCloneAlternation(Getuserinfo0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getuser(List<string> rulenames)
		{
			rulenames.Insert(0, "user");
			State rule = State.NoCloneAlternation(Getuser0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getuser_unreserved(List<string> rulenames)
		{
			rulenames.Insert(0, "user-unreserved");
			State rule = State.NoCloneAlternation(Getuser_unreserved0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpassword(List<string> rulenames)
		{
			rulenames.Insert(0, "password");
			State rule = State.NoCloneAlternation(Getpassword0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethostport(List<string> rulenames)
		{
			rulenames.Insert(0, "hostport");
			State rule = State.NoCloneAlternation(Gethostport0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethost(List<string> rulenames)
		{
			rulenames.Insert(0, "host");
			State rule = State.NoCloneAlternation(Gethost0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethostname(List<string> rulenames)
		{
			rulenames.Insert(0, "hostname");
			State rule = State.NoCloneAlternation(Gethostname0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdomainlabel(List<string> rulenames)
		{
			rulenames.Insert(0, "domainlabel");
			State rule = State.NoCloneAlternation(Getdomainlabel0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettoplabel(List<string> rulenames)
		{
			rulenames.Insert(0, "toplabel");
			State rule = State.NoCloneAlternation(Gettoplabel0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIPv4address(List<string> rulenames)
		{
			rulenames.Insert(0, "IPv4address");
			State rule = State.NoCloneAlternation(GetIPv4address0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIPv6reference(List<string> rulenames)
		{
			rulenames.Insert(0, "IPv6reference");
			State rule = State.NoCloneAlternation(GetIPv6reference0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIPv6address(List<string> rulenames)
		{
			rulenames.Insert(0, "IPv6address");
			State rule = State.NoCloneAlternation(GetIPv6address0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethexpart(List<string> rulenames)
		{
			rulenames.Insert(0, "hexpart");
			State rule = State.NoCloneAlternation(Gethexpart0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethexseq(List<string> rulenames)
		{
			rulenames.Insert(0, "hexseq");
			State rule = State.NoCloneAlternation(Gethexseq0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethex4(List<string> rulenames)
		{
			rulenames.Insert(0, "hex4");
			State rule = State.NoCloneAlternation(Gethex40(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getport(List<string> rulenames)
		{
			rulenames.Insert(0, "port");
			State rule = State.NoCloneAlternation(Getport0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Geturi_parameters(List<string> rulenames)
		{
			rulenames.Insert(0, "uri-parameters");
			State rule = State.NoCloneAlternation(Geturi_parameters0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Geturi_parameter(List<string> rulenames)
		{
			rulenames.Insert(0, "uri-parameter");
			State rule = State.NoCloneAlternation(Geturi_parameter0(rulenames), Geturi_parameter1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettransport_param(List<string> rulenames)
		{
			rulenames.Insert(0, "transport-param");
			State rule = State.NoCloneAlternation(Gettransport_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getother_transport(List<string> rulenames)
		{
			rulenames.Insert(0, "other-transport");
			State rule = State.NoCloneAlternation(Getother_transport0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getuser_param(List<string> rulenames)
		{
			rulenames.Insert(0, "user-param");
			State rule = State.NoCloneAlternation(Getuser_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getother_user(List<string> rulenames)
		{
			rulenames.Insert(0, "other-user");
			State rule = State.NoCloneAlternation(Getother_user0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmethod_param(List<string> rulenames)
		{
			rulenames.Insert(0, "method-param");
			State rule = State.NoCloneAlternation(Getmethod_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getttl_param(List<string> rulenames)
		{
			rulenames.Insert(0, "ttl-param");
			State rule = State.NoCloneAlternation(Getttl_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmaddr_param(List<string> rulenames)
		{
			rulenames.Insert(0, "maddr-param");
			State rule = State.NoCloneAlternation(Getmaddr_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getlr_param(List<string> rulenames)
		{
			rulenames.Insert(0, "lr-param");
			State rule = State.NoCloneAlternation(Getlr_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getother_param(List<string> rulenames)
		{
			rulenames.Insert(0, "other-param");
			State rule = State.NoCloneAlternation(Getother_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpname(List<string> rulenames)
		{
			rulenames.Insert(0, "pname");
			State rule = State.NoCloneAlternation(Getpname0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpvalue(List<string> rulenames)
		{
			rulenames.Insert(0, "pvalue");
			State rule = State.NoCloneAlternation(Getpvalue0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getparamchar(List<string> rulenames)
		{
			rulenames.Insert(0, "paramchar");
			State rule = State.NoCloneAlternation(Getparamchar0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getparam_unreserved(List<string> rulenames)
		{
			rulenames.Insert(0, "param-unreserved");
			State rule = State.NoCloneAlternation(Getparam_unreserved0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getheaders(List<string> rulenames)
		{
			rulenames.Insert(0, "headers");
			State rule = State.NoCloneAlternation(Getheaders0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getheader(List<string> rulenames)
		{
			rulenames.Insert(0, "header");
			State rule = State.NoCloneAlternation(Getheader0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethname(List<string> rulenames)
		{
			rulenames.Insert(0, "hname");
			State rule = State.NoCloneAlternation(Gethname0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethvalue(List<string> rulenames)
		{
			rulenames.Insert(0, "hvalue");
			State rule = State.NoCloneAlternation(Gethvalue0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethnv_unreserved(List<string> rulenames)
		{
			rulenames.Insert(0, "hnv-unreserved");
			State rule = State.NoCloneAlternation(Gethnv_unreserved0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSIP_message(List<string> rulenames)
		{
			rulenames.Insert(0, "SIP-message");
			State rule = State.NoCloneAlternation(GetSIP_message0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRequest(List<string> rulenames)
		{
			rulenames.Insert(0, "Request");
			State rule = State.NoCloneAlternation(GetRequest0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRequest_Line(List<string> rulenames)
		{
			rulenames.Insert(0, "Request-Line");
			State rule = State.NoCloneAlternation(GetRequest_Line0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRequest_URI(List<string> rulenames)
		{
			rulenames.Insert(0, "Request-URI");
			State rule = State.NoCloneAlternation(GetRequest_URI0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetabsoluteURI(List<string> rulenames)
		{
			rulenames.Insert(0, "absoluteURI");
			State rule = State.NoCloneAlternation(GetabsoluteURI0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethier_part(List<string> rulenames)
		{
			rulenames.Insert(0, "hier-part");
			State rule = State.NoCloneAlternation(Gethier_part0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnet_path(List<string> rulenames)
		{
			rulenames.Insert(0, "net-path");
			State rule = State.NoCloneAlternation(Getnet_path0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getabs_path(List<string> rulenames)
		{
			rulenames.Insert(0, "abs-path");
			State rule = State.NoCloneAlternation(Getabs_path0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getopaque_part(List<string> rulenames)
		{
			rulenames.Insert(0, "opaque-part");
			State rule = State.NoCloneAlternation(Getopaque_part0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Geturic(List<string> rulenames)
		{
			rulenames.Insert(0, "uric");
			State rule = State.NoCloneAlternation(Geturic0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Geturic_no_slash(List<string> rulenames)
		{
			rulenames.Insert(0, "uric-no-slash");
			State rule = State.NoCloneAlternation(Geturic_no_slash0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpath_segments(List<string> rulenames)
		{
			rulenames.Insert(0, "path-segments");
			State rule = State.NoCloneAlternation(Getpath_segments0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsegment(List<string> rulenames)
		{
			rulenames.Insert(0, "segment");
			State rule = State.NoCloneAlternation(Getsegment0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getparam(List<string> rulenames)
		{
			rulenames.Insert(0, "param");
			State rule = State.NoCloneAlternation(Getparam0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpchar(List<string> rulenames)
		{
			rulenames.Insert(0, "pchar");
			State rule = State.NoCloneAlternation(Getpchar0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getscheme(List<string> rulenames)
		{
			rulenames.Insert(0, "scheme");
			State rule = State.NoCloneAlternation(Getscheme0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getauthority(List<string> rulenames)
		{
			rulenames.Insert(0, "authority");
			State rule = State.NoCloneAlternation(Getauthority0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsrvr(List<string> rulenames)
		{
			rulenames.Insert(0, "srvr");
			State rule = State.NoCloneAlternation(Getsrvr0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getreg_name(List<string> rulenames)
		{
			rulenames.Insert(0, "reg-name");
			State rule = State.NoCloneAlternation(Getreg_name0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getquery(List<string> rulenames)
		{
			rulenames.Insert(0, "query");
			State rule = State.NoCloneAlternation(Getquery0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSIP_Version(List<string> rulenames)
		{
			rulenames.Insert(0, "SIP-Version");
			State rule = State.NoCloneAlternation(GetSIP_Version0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmessage_header(List<string> rulenames)
		{
			rulenames.Insert(0, "message-header");
			State rule = State.NoCloneAlternation(Getmessage_header0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetINVITEm(List<string> rulenames)
		{
			rulenames.Insert(0, "INVITEm");
			State rule = State.NoCloneAlternation(GetINVITEm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetACKm(List<string> rulenames)
		{
			rulenames.Insert(0, "ACKm");
			State rule = State.NoCloneAlternation(GetACKm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetOPTIONSm(List<string> rulenames)
		{
			rulenames.Insert(0, "OPTIONSm");
			State rule = State.NoCloneAlternation(GetOPTIONSm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetBYEm(List<string> rulenames)
		{
			rulenames.Insert(0, "BYEm");
			State rule = State.NoCloneAlternation(GetBYEm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCANCELm(List<string> rulenames)
		{
			rulenames.Insert(0, "CANCELm");
			State rule = State.NoCloneAlternation(GetCANCELm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetREGISTERm(List<string> rulenames)
		{
			rulenames.Insert(0, "REGISTERm");
			State rule = State.NoCloneAlternation(GetREGISTERm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetMethod(List<string> rulenames)
		{
			rulenames.Insert(0, "Method");
			State rule = State.NoCloneAlternation(GetMethod0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_method(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-method");
			State rule = State.NoCloneAlternation(Getextension_method0(rulenames), Getextension_method1(rulenames), Getextension_method2(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetResponse(List<string> rulenames)
		{
			rulenames.Insert(0, "Response");
			State rule = State.NoCloneAlternation(GetResponse0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetStatus_Line(List<string> rulenames)
		{
			rulenames.Insert(0, "Status-Line");
			State rule = State.NoCloneAlternation(GetStatus_Line0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetStatus_Code(List<string> rulenames)
		{
			rulenames.Insert(0, "Status-Code");
			State rule = State.NoCloneAlternation(GetStatus_Code0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_code(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-code");
			State rule = State.NoCloneAlternation(Getextension_code0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetReason_Phrase(List<string> rulenames)
		{
			rulenames.Insert(0, "Reason-Phrase");
			State rule = State.NoCloneAlternation(GetReason_Phrase0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetInformational(List<string> rulenames)
		{
			rulenames.Insert(0, "Informational");
			State rule = State.NoCloneAlternation(GetInformational0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSuccess(List<string> rulenames)
		{
			rulenames.Insert(0, "Success");
			State rule = State.NoCloneAlternation(GetSuccess0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRedirection(List<string> rulenames)
		{
			rulenames.Insert(0, "Redirection");
			State rule = State.NoCloneAlternation(GetRedirection0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetClient_Error(List<string> rulenames)
		{
			rulenames.Insert(0, "Client-Error");
			State rule = State.NoCloneAlternation(GetClient_Error0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetServer_Error(List<string> rulenames)
		{
			rulenames.Insert(0, "Server-Error");
			State rule = State.NoCloneAlternation(GetServer_Error0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetGlobal_Failure(List<string> rulenames)
		{
			rulenames.Insert(0, "Global-Failure");
			State rule = State.NoCloneAlternation(GetGlobal_Failure0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAccept(List<string> rulenames)
		{
			rulenames.Insert(0, "Accept");
			State rule = State.NoCloneAlternation(GetAccept0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getaccept_range(List<string> rulenames)
		{
			rulenames.Insert(0, "accept-range");
			State rule = State.NoCloneAlternation(Getaccept_range0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmedia_range(List<string> rulenames)
		{
			rulenames.Insert(0, "media-range");
			State rule = State.NoCloneAlternation(Getmedia_range0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getaccept_param(List<string> rulenames)
		{
			rulenames.Insert(0, "accept-param");
			State rule = State.NoCloneAlternation(Getaccept_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getqvalue(List<string> rulenames)
		{
			rulenames.Insert(0, "qvalue");
			State rule = State.NoCloneAlternation(Getqvalue0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getgeneric_param(List<string> rulenames)
		{
			rulenames.Insert(0, "generic-param");
			State rule = State.NoCloneAlternation(Getgeneric_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getgen_value(List<string> rulenames)
		{
			rulenames.Insert(0, "gen-value");
			State rule = State.NoCloneAlternation(Getgen_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAccept_Encoding(List<string> rulenames)
		{
			rulenames.Insert(0, "Accept-Encoding");
			State rule = State.NoCloneAlternation(GetAccept_Encoding0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getencoding(List<string> rulenames)
		{
			rulenames.Insert(0, "encoding");
			State rule = State.NoCloneAlternation(Getencoding0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcodings(List<string> rulenames)
		{
			rulenames.Insert(0, "codings");
			State rule = State.NoCloneAlternation(Getcodings0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcontent_coding(List<string> rulenames)
		{
			rulenames.Insert(0, "content-coding");
			State rule = State.NoCloneAlternation(Getcontent_coding0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAccept_Language(List<string> rulenames)
		{
			rulenames.Insert(0, "Accept-Language");
			State rule = State.NoCloneAlternation(GetAccept_Language0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getlanguage(List<string> rulenames)
		{
			rulenames.Insert(0, "language");
			State rule = State.NoCloneAlternation(Getlanguage0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getlanguage_range(List<string> rulenames)
		{
			rulenames.Insert(0, "language-range");
			State rule = State.NoCloneAlternation(Getlanguage_range0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAlert_Info(List<string> rulenames)
		{
			rulenames.Insert(0, "Alert-Info");
			State rule = State.NoCloneAlternation(GetAlert_Info0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getalert_param(List<string> rulenames)
		{
			rulenames.Insert(0, "alert-param");
			State rule = State.NoCloneAlternation(Getalert_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAllow(List<string> rulenames)
		{
			rulenames.Insert(0, "Allow");
			State rule = State.NoCloneAlternation(GetAllow0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAuthorization(List<string> rulenames)
		{
			rulenames.Insert(0, "Authorization");
			State rule = State.NoCloneAlternation(GetAuthorization0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcredentials(List<string> rulenames)
		{
			rulenames.Insert(0, "credentials");
			State rule = State.NoCloneAlternation(Getcredentials0(rulenames), Getcredentials1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdigest_response(List<string> rulenames)
		{
			rulenames.Insert(0, "digest-response");
			State rule = State.NoCloneAlternation(Getdigest_response0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdig_resp(List<string> rulenames)
		{
			rulenames.Insert(0, "dig-resp");
			State rule = State.NoCloneAlternation(Getdig_resp0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getusername(List<string> rulenames)
		{
			rulenames.Insert(0, "username");
			State rule = State.NoCloneAlternation(Getusername0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getusername_value(List<string> rulenames)
		{
			rulenames.Insert(0, "username-value");
			State rule = State.NoCloneAlternation(Getusername_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdigest_uri(List<string> rulenames)
		{
			rulenames.Insert(0, "digest-uri");
			State rule = State.NoCloneAlternation(Getdigest_uri0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdigest_uri_value(List<string> rulenames)
		{
			rulenames.Insert(0, "digest-uri-value");
			State rule = State.NoCloneAlternation(Getdigest_uri_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmessage_qop(List<string> rulenames)
		{
			rulenames.Insert(0, "message-qop");
			State rule = State.NoCloneAlternation(Getmessage_qop0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcnonce(List<string> rulenames)
		{
			rulenames.Insert(0, "cnonce");
			State rule = State.NoCloneAlternation(Getcnonce0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcnonce_value(List<string> rulenames)
		{
			rulenames.Insert(0, "cnonce-value");
			State rule = State.NoCloneAlternation(Getcnonce_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnonce_count(List<string> rulenames)
		{
			rulenames.Insert(0, "nonce-count");
			State rule = State.NoCloneAlternation(Getnonce_count0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnc_value(List<string> rulenames)
		{
			rulenames.Insert(0, "nc-value");
			State rule = State.NoCloneAlternation(Getnc_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdresponse(List<string> rulenames)
		{
			rulenames.Insert(0, "dresponse");
			State rule = State.NoCloneAlternation(Getdresponse0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrequest_digest(List<string> rulenames)
		{
			rulenames.Insert(0, "request-digest");
			State rule = State.NoCloneAlternation(Getrequest_digest0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getauth_param(List<string> rulenames)
		{
			rulenames.Insert(0, "auth-param");
			State rule = State.NoCloneAlternation(Getauth_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getauth_param_name(List<string> rulenames)
		{
			rulenames.Insert(0, "auth-param-name");
			State rule = State.NoCloneAlternation(Getauth_param_name0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getother_response(List<string> rulenames)
		{
			rulenames.Insert(0, "other-response");
			State rule = State.NoCloneAlternation(Getother_response0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getauth_scheme(List<string> rulenames)
		{
			rulenames.Insert(0, "auth-scheme");
			State rule = State.NoCloneAlternation(Getauth_scheme0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAuthentication_Info(List<string> rulenames)
		{
			rulenames.Insert(0, "Authentication-Info");
			State rule = State.NoCloneAlternation(GetAuthentication_Info0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getainfo(List<string> rulenames)
		{
			rulenames.Insert(0, "ainfo");
			State rule = State.NoCloneAlternation(Getainfo0(rulenames), Getainfo1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnextnonce(List<string> rulenames)
		{
			rulenames.Insert(0, "nextnonce");
			State rule = State.NoCloneAlternation(Getnextnonce0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getresponse_auth(List<string> rulenames)
		{
			rulenames.Insert(0, "response-auth");
			State rule = State.NoCloneAlternation(Getresponse_auth0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getresponse_digest(List<string> rulenames)
		{
			rulenames.Insert(0, "response-digest");
			State rule = State.NoCloneAlternation(Getresponse_digest0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCall_ID(List<string> rulenames)
		{
			rulenames.Insert(0, "Call-ID");
			State rule = State.NoCloneAlternation(GetCall_ID0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcallid(List<string> rulenames)
		{
			rulenames.Insert(0, "callid");
			State rule = State.NoCloneAlternation(Getcallid0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCall_Info(List<string> rulenames)
		{
			rulenames.Insert(0, "Call-Info");
			State rule = State.NoCloneAlternation(GetCall_Info0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getinfo(List<string> rulenames)
		{
			rulenames.Insert(0, "info");
			State rule = State.NoCloneAlternation(Getinfo0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getinfo_param(List<string> rulenames)
		{
			rulenames.Insert(0, "info-param");
			State rule = State.NoCloneAlternation(Getinfo_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContact(List<string> rulenames)
		{
			rulenames.Insert(0, "Contact");
			State rule = State.NoCloneAlternation(GetContact0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcontact_param(List<string> rulenames)
		{
			rulenames.Insert(0, "contact-param");
			State rule = State.NoCloneAlternation(Getcontact_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getname_addr(List<string> rulenames)
		{
			rulenames.Insert(0, "name-addr");
			State rule = State.NoCloneAlternation(Getname_addr0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getaddr_spec(List<string> rulenames)
		{
			rulenames.Insert(0, "addr-spec");
			State rule = State.NoCloneAlternation(Getaddr_spec0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdisplay_name(List<string> rulenames)
		{
			rulenames.Insert(0, "display-name");
			State rule = State.NoCloneAlternation(Getdisplay_name0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcontact_params(List<string> rulenames)
		{
			rulenames.Insert(0, "contact-params");
			State rule = State.NoCloneAlternation(Getcontact_params0(rulenames), Getcontact_params1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getc_p_q(List<string> rulenames)
		{
			rulenames.Insert(0, "c-p-q");
			State rule = State.NoCloneAlternation(Getc_p_q0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getc_p_expires(List<string> rulenames)
		{
			rulenames.Insert(0, "c-p-expires");
			State rule = State.NoCloneAlternation(Getc_p_expires0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcontact_extension(List<string> rulenames)
		{
			rulenames.Insert(0, "contact-extension");
			State rule = State.NoCloneAlternation(Getcontact_extension0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdelta_seconds(List<string> rulenames)
		{
			rulenames.Insert(0, "delta-seconds");
			State rule = State.NoCloneAlternation(Getdelta_seconds0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContent_Disposition(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-Disposition");
			State rule = State.NoCloneAlternation(GetContent_Disposition0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdisp_type(List<string> rulenames)
		{
			rulenames.Insert(0, "disp-type");
			State rule = State.NoCloneAlternation(Getdisp_type0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdisp_param(List<string> rulenames)
		{
			rulenames.Insert(0, "disp-param");
			State rule = State.NoCloneAlternation(Getdisp_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethandling_param(List<string> rulenames)
		{
			rulenames.Insert(0, "handling-param");
			State rule = State.NoCloneAlternation(Gethandling_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getother_handling(List<string> rulenames)
		{
			rulenames.Insert(0, "other-handling");
			State rule = State.NoCloneAlternation(Getother_handling0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdisp_extension_token(List<string> rulenames)
		{
			rulenames.Insert(0, "disp-extension-token");
			State rule = State.NoCloneAlternation(Getdisp_extension_token0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContent_Encoding(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-Encoding");
			State rule = State.NoCloneAlternation(GetContent_Encoding0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContent_Language(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-Language");
			State rule = State.NoCloneAlternation(GetContent_Language0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getlanguage_tag(List<string> rulenames)
		{
			rulenames.Insert(0, "language-tag");
			State rule = State.NoCloneAlternation(Getlanguage_tag0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getprimary_tag(List<string> rulenames)
		{
			rulenames.Insert(0, "primary-tag");
			State rule = State.NoCloneAlternation(Getprimary_tag0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsubtag(List<string> rulenames)
		{
			rulenames.Insert(0, "subtag");
			State rule = State.NoCloneAlternation(Getsubtag0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContent_Length(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-Length");
			State rule = State.NoCloneAlternation(GetContent_Length0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContent_Type(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-Type");
			State rule = State.NoCloneAlternation(GetContent_Type0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmedia_type(List<string> rulenames)
		{
			rulenames.Insert(0, "media-type");
			State rule = State.NoCloneAlternation(Getmedia_type0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getm_type(List<string> rulenames)
		{
			rulenames.Insert(0, "m-type");
			State rule = State.NoCloneAlternation(Getm_type0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdiscrete_type(List<string> rulenames)
		{
			rulenames.Insert(0, "discrete-type");
			State rule = State.NoCloneAlternation(Getdiscrete_type0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcomposite_type(List<string> rulenames)
		{
			rulenames.Insert(0, "composite-type");
			State rule = State.NoCloneAlternation(Getcomposite_type0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_token(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-token");
			State rule = State.NoCloneAlternation(Getextension_token0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getietf_token(List<string> rulenames)
		{
			rulenames.Insert(0, "ietf-token");
			State rule = State.NoCloneAlternation(Getietf_token0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getx_token(List<string> rulenames)
		{
			rulenames.Insert(0, "x-token");
			State rule = State.NoCloneAlternation(Getx_token0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getm_subtype(List<string> rulenames)
		{
			rulenames.Insert(0, "m-subtype");
			State rule = State.NoCloneAlternation(Getm_subtype0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getiana_token(List<string> rulenames)
		{
			rulenames.Insert(0, "iana-token");
			State rule = State.NoCloneAlternation(Getiana_token0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getm_parameter(List<string> rulenames)
		{
			rulenames.Insert(0, "m-parameter");
			State rule = State.NoCloneAlternation(Getm_parameter0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getm_attribute(List<string> rulenames)
		{
			rulenames.Insert(0, "m-attribute");
			State rule = State.NoCloneAlternation(Getm_attribute0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getm_value(List<string> rulenames)
		{
			rulenames.Insert(0, "m-value");
			State rule = State.NoCloneAlternation(Getm_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetCSeq(List<string> rulenames)
		{
			rulenames.Insert(0, "CSeq");
			State rule = State.NoCloneAlternation(GetCSeq0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetDate(List<string> rulenames)
		{
			rulenames.Insert(0, "Date");
			State rule = State.NoCloneAlternation(GetDate0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSIP_date(List<string> rulenames)
		{
			rulenames.Insert(0, "SIP-date");
			State rule = State.NoCloneAlternation(GetSIP_date0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrfc1123_date(List<string> rulenames)
		{
			rulenames.Insert(0, "rfc1123-date");
			State rule = State.NoCloneAlternation(Getrfc1123_date0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdate1(List<string> rulenames)
		{
			rulenames.Insert(0, "date1");
			State rule = State.NoCloneAlternation(Getdate10(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettime(List<string> rulenames)
		{
			rulenames.Insert(0, "time");
			State rule = State.NoCloneAlternation(Gettime0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getwkday(List<string> rulenames)
		{
			rulenames.Insert(0, "wkday");
			State rule = State.NoCloneAlternation(Getwkday0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmonth(List<string> rulenames)
		{
			rulenames.Insert(0, "month");
			State rule = State.NoCloneAlternation(Getmonth0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetError_Info(List<string> rulenames)
		{
			rulenames.Insert(0, "Error-Info");
			State rule = State.NoCloneAlternation(GetError_Info0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Geterror_uri(List<string> rulenames)
		{
			rulenames.Insert(0, "error-uri");
			State rule = State.NoCloneAlternation(Geterror_uri0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetExpires(List<string> rulenames)
		{
			rulenames.Insert(0, "Expires");
			State rule = State.NoCloneAlternation(GetExpires0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetFrom(List<string> rulenames)
		{
			rulenames.Insert(0, "From");
			State rule = State.NoCloneAlternation(GetFrom0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getfrom_spec(List<string> rulenames)
		{
			rulenames.Insert(0, "from-spec");
			State rule = State.NoCloneAlternation(Getfrom_spec0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getfrom_param(List<string> rulenames)
		{
			rulenames.Insert(0, "from-param");
			State rule = State.NoCloneAlternation(Getfrom_param0(rulenames), Getfrom_param1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettag_param(List<string> rulenames)
		{
			rulenames.Insert(0, "tag-param");
			State rule = State.NoCloneAlternation(Gettag_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIn_Reply_To(List<string> rulenames)
		{
			rulenames.Insert(0, "In-Reply-To");
			State rule = State.NoCloneAlternation(GetIn_Reply_To0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetMax_Forwards(List<string> rulenames)
		{
			rulenames.Insert(0, "Max-Forwards");
			State rule = State.NoCloneAlternation(GetMax_Forwards0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetMIME_Version(List<string> rulenames)
		{
			rulenames.Insert(0, "MIME-Version");
			State rule = State.NoCloneAlternation(GetMIME_Version0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetMin_Expires(List<string> rulenames)
		{
			rulenames.Insert(0, "Min-Expires");
			State rule = State.NoCloneAlternation(GetMin_Expires0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetOrganization(List<string> rulenames)
		{
			rulenames.Insert(0, "Organization");
			State rule = State.NoCloneAlternation(GetOrganization0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetPriority(List<string> rulenames)
		{
			rulenames.Insert(0, "Priority");
			State rule = State.NoCloneAlternation(GetPriority0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpriority_value(List<string> rulenames)
		{
			rulenames.Insert(0, "priority-value");
			State rule = State.NoCloneAlternation(Getpriority_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getother_priority(List<string> rulenames)
		{
			rulenames.Insert(0, "other-priority");
			State rule = State.NoCloneAlternation(Getother_priority0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetProxy_Authenticate(List<string> rulenames)
		{
			rulenames.Insert(0, "Proxy-Authenticate");
			State rule = State.NoCloneAlternation(GetProxy_Authenticate0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getchallenge(List<string> rulenames)
		{
			rulenames.Insert(0, "challenge");
			State rule = State.NoCloneAlternation(Getchallenge0(rulenames), Getchallenge1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getother_challenge(List<string> rulenames)
		{
			rulenames.Insert(0, "other-challenge");
			State rule = State.NoCloneAlternation(Getother_challenge0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdigest_cln(List<string> rulenames)
		{
			rulenames.Insert(0, "digest-cln");
			State rule = State.NoCloneAlternation(Getdigest_cln0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrealm(List<string> rulenames)
		{
			rulenames.Insert(0, "realm");
			State rule = State.NoCloneAlternation(Getrealm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrealm_value(List<string> rulenames)
		{
			rulenames.Insert(0, "realm-value");
			State rule = State.NoCloneAlternation(Getrealm_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdomain(List<string> rulenames)
		{
			rulenames.Insert(0, "domain");
			State rule = State.NoCloneAlternation(Getdomain0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetURI(List<string> rulenames)
		{
			rulenames.Insert(0, "URI");
			State rule = State.NoCloneAlternation(GetURI0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnonce(List<string> rulenames)
		{
			rulenames.Insert(0, "nonce");
			State rule = State.NoCloneAlternation(Getnonce0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnonce_value(List<string> rulenames)
		{
			rulenames.Insert(0, "nonce-value");
			State rule = State.NoCloneAlternation(Getnonce_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getopaque(List<string> rulenames)
		{
			rulenames.Insert(0, "opaque");
			State rule = State.NoCloneAlternation(Getopaque0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getstale(List<string> rulenames)
		{
			rulenames.Insert(0, "stale");
			State rule = State.NoCloneAlternation(Getstale0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getalgorithm(List<string> rulenames)
		{
			rulenames.Insert(0, "algorithm");
			State rule = State.NoCloneAlternation(Getalgorithm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getqop_options(List<string> rulenames)
		{
			rulenames.Insert(0, "qop-options");
			State rule = State.NoCloneAlternation(Getqop_options0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getqop_value(List<string> rulenames)
		{
			rulenames.Insert(0, "qop-value");
			State rule = State.NoCloneAlternation(Getqop_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetProxy_Authorization(List<string> rulenames)
		{
			rulenames.Insert(0, "Proxy-Authorization");
			State rule = State.NoCloneAlternation(GetProxy_Authorization0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetProxy_Require(List<string> rulenames)
		{
			rulenames.Insert(0, "Proxy-Require");
			State rule = State.NoCloneAlternation(GetProxy_Require0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getoption_tag(List<string> rulenames)
		{
			rulenames.Insert(0, "option-tag");
			State rule = State.NoCloneAlternation(Getoption_tag0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRecord_Route(List<string> rulenames)
		{
			rulenames.Insert(0, "Record-Route");
			State rule = State.NoCloneAlternation(GetRecord_Route0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrec_route(List<string> rulenames)
		{
			rulenames.Insert(0, "rec-route");
			State rule = State.NoCloneAlternation(Getrec_route0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrr_param(List<string> rulenames)
		{
			rulenames.Insert(0, "rr-param");
			State rule = State.NoCloneAlternation(Getrr_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetReply_To(List<string> rulenames)
		{
			rulenames.Insert(0, "Reply-To");
			State rule = State.NoCloneAlternation(GetReply_To0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrplyto_spec(List<string> rulenames)
		{
			rulenames.Insert(0, "rplyto-spec");
			State rule = State.NoCloneAlternation(Getrplyto_spec0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrplyto_param(List<string> rulenames)
		{
			rulenames.Insert(0, "rplyto-param");
			State rule = State.NoCloneAlternation(Getrplyto_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRequire(List<string> rulenames)
		{
			rulenames.Insert(0, "Require");
			State rule = State.NoCloneAlternation(GetRequire0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRetry_After(List<string> rulenames)
		{
			rulenames.Insert(0, "Retry-After");
			State rule = State.NoCloneAlternation(GetRetry_After0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getretry_param(List<string> rulenames)
		{
			rulenames.Insert(0, "retry-param");
			State rule = State.NoCloneAlternation(Getretry_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRoute(List<string> rulenames)
		{
			rulenames.Insert(0, "Route");
			State rule = State.NoCloneAlternation(GetRoute0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getroute_param(List<string> rulenames)
		{
			rulenames.Insert(0, "route-param");
			State rule = State.NoCloneAlternation(Getroute_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetServer(List<string> rulenames)
		{
			rulenames.Insert(0, "Server");
			State rule = State.NoCloneAlternation(GetServer0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getserver_val(List<string> rulenames)
		{
			rulenames.Insert(0, "server-val");
			State rule = State.NoCloneAlternation(Getserver_val0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getproduct(List<string> rulenames)
		{
			rulenames.Insert(0, "product");
			State rule = State.NoCloneAlternation(Getproduct0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getproduct_version(List<string> rulenames)
		{
			rulenames.Insert(0, "product-version");
			State rule = State.NoCloneAlternation(Getproduct_version0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSubject(List<string> rulenames)
		{
			rulenames.Insert(0, "Subject");
			State rule = State.NoCloneAlternation(GetSubject0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSupported(List<string> rulenames)
		{
			rulenames.Insert(0, "Supported");
			State rule = State.NoCloneAlternation(GetSupported0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetTimestamp(List<string> rulenames)
		{
			rulenames.Insert(0, "Timestamp");
			State rule = State.NoCloneAlternation(GetTimestamp0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdelay(List<string> rulenames)
		{
			rulenames.Insert(0, "delay");
			State rule = State.NoCloneAlternation(Getdelay0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetTo(List<string> rulenames)
		{
			rulenames.Insert(0, "To");
			State rule = State.NoCloneAlternation(GetTo0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getto_param(List<string> rulenames)
		{
			rulenames.Insert(0, "to-param");
			State rule = State.NoCloneAlternation(Getto_param0(rulenames), Getto_param1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetUnsupported(List<string> rulenames)
		{
			rulenames.Insert(0, "Unsupported");
			State rule = State.NoCloneAlternation(GetUnsupported0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetUser_Agent(List<string> rulenames)
		{
			rulenames.Insert(0, "User-Agent");
			State rule = State.NoCloneAlternation(GetUser_Agent0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetVia(List<string> rulenames)
		{
			rulenames.Insert(0, "Via");
			State rule = State.NoCloneAlternation(GetVia0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getvia_parm(List<string> rulenames)
		{
			rulenames.Insert(0, "via-parm");
			State rule = State.NoCloneAlternation(Getvia_parm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getvia_params(List<string> rulenames)
		{
			rulenames.Insert(0, "via-params");
			State rule = State.NoCloneAlternation(Getvia_params0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getvia_ttl(List<string> rulenames)
		{
			rulenames.Insert(0, "via-ttl");
			State rule = State.NoCloneAlternation(Getvia_ttl0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getvia_maddr(List<string> rulenames)
		{
			rulenames.Insert(0, "via-maddr");
			State rule = State.NoCloneAlternation(Getvia_maddr0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getvia_received(List<string> rulenames)
		{
			rulenames.Insert(0, "via-received");
			State rule = State.NoCloneAlternation(Getvia_received0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getvia_branch(List<string> rulenames)
		{
			rulenames.Insert(0, "via-branch");
			State rule = State.NoCloneAlternation(Getvia_branch0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getvia_extension(List<string> rulenames)
		{
			rulenames.Insert(0, "via-extension");
			State rule = State.NoCloneAlternation(Getvia_extension0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsent_protocol(List<string> rulenames)
		{
			rulenames.Insert(0, "sent-protocol");
			State rule = State.NoCloneAlternation(Getsent_protocol0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getprotocol_name(List<string> rulenames)
		{
			rulenames.Insert(0, "protocol-name");
			State rule = State.NoCloneAlternation(Getprotocol_name0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getprotocol_version(List<string> rulenames)
		{
			rulenames.Insert(0, "protocol-version");
			State rule = State.NoCloneAlternation(Getprotocol_version0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettransport(List<string> rulenames)
		{
			rulenames.Insert(0, "transport");
			State rule = State.NoCloneAlternation(Gettransport0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsent_by(List<string> rulenames)
		{
			rulenames.Insert(0, "sent-by");
			State rule = State.NoCloneAlternation(Getsent_by0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getttl(List<string> rulenames)
		{
			rulenames.Insert(0, "ttl");
			State rule = State.NoCloneAlternation(Getttl0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetWarning(List<string> rulenames)
		{
			rulenames.Insert(0, "Warning");
			State rule = State.NoCloneAlternation(GetWarning0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getwarning_value(List<string> rulenames)
		{
			rulenames.Insert(0, "warning-value");
			State rule = State.NoCloneAlternation(Getwarning_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getwarn_code(List<string> rulenames)
		{
			rulenames.Insert(0, "warn-code");
			State rule = State.NoCloneAlternation(Getwarn_code0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getwarn_agent(List<string> rulenames)
		{
			rulenames.Insert(0, "warn-agent");
			State rule = State.NoCloneAlternation(Getwarn_agent0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getwarn_text(List<string> rulenames)
		{
			rulenames.Insert(0, "warn-text");
			State rule = State.NoCloneAlternation(Getwarn_text0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpseudonym(List<string> rulenames)
		{
			rulenames.Insert(0, "pseudonym");
			State rule = State.NoCloneAlternation(Getpseudonym0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetWWW_Authenticate(List<string> rulenames)
		{
			rulenames.Insert(0, "WWW-Authenticate");
			State rule = State.NoCloneAlternation(GetWWW_Authenticate0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_header(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-header");
			State rule = State.NoCloneAlternation(Getextension_header0(rulenames), Getextension_header1(rulenames), Getextension_header2(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getheader_name(List<string> rulenames)
		{
			rulenames.Insert(0, "header-name");
			State rule = State.NoCloneAlternation(Getheader_name0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getheader_value(List<string> rulenames)
		{
			rulenames.Insert(0, "header-value");
			State rule = State.NoCloneAlternation(Getheader_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmessage_body(List<string> rulenames)
		{
			rulenames.Insert(0, "message-body");
			State rule = State.NoCloneAlternation(Getmessage_body0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSUBSCRIBEm(List<string> rulenames)
		{
			rulenames.Insert(0, "SUBSCRIBEm");
			State rule = State.NoCloneAlternation(GetSUBSCRIBEm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetNOTIFYm(List<string> rulenames)
		{
			rulenames.Insert(0, "NOTIFYm");
			State rule = State.NoCloneAlternation(GetNOTIFYm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetEvent(List<string> rulenames)
		{
			rulenames.Insert(0, "Event");
			State rule = State.NoCloneAlternation(GetEvent0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getevent_type(List<string> rulenames)
		{
			rulenames.Insert(0, "event-type");
			State rule = State.NoCloneAlternation(Getevent_type0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getevent_package(List<string> rulenames)
		{
			rulenames.Insert(0, "event-package");
			State rule = State.NoCloneAlternation(Getevent_package0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getevent_template(List<string> rulenames)
		{
			rulenames.Insert(0, "event-template");
			State rule = State.NoCloneAlternation(Getevent_template0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettoken_nodot(List<string> rulenames)
		{
			rulenames.Insert(0, "token-nodot");
			State rule = State.NoCloneAlternation(Gettoken_nodot0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getevent_param(List<string> rulenames)
		{
			rulenames.Insert(0, "event-param");
			State rule = State.NoCloneAlternation(Getevent_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAllow_Events(List<string> rulenames)
		{
			rulenames.Insert(0, "Allow-Events");
			State rule = State.NoCloneAlternation(GetAllow_Events0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSubscription_State(List<string> rulenames)
		{
			rulenames.Insert(0, "Subscription-State");
			State rule = State.NoCloneAlternation(GetSubscription_State0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsubstate_value(List<string> rulenames)
		{
			rulenames.Insert(0, "substate-value");
			State rule = State.NoCloneAlternation(Getsubstate_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_substate(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-substate");
			State rule = State.NoCloneAlternation(Getextension_substate0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsubexp_params(List<string> rulenames)
		{
			rulenames.Insert(0, "subexp-params");
			State rule = State.NoCloneAlternation(Getsubexp_params0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getevent_reason_value(List<string> rulenames)
		{
			rulenames.Insert(0, "event-reason-value");
			State rule = State.NoCloneAlternation(Getevent_reason_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getevent_reason_extension(List<string> rulenames)
		{
			rulenames.Insert(0, "event-reason-extension");
			State rule = State.NoCloneAlternation(Getevent_reason_extension0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSERVICEm(List<string> rulenames)
		{
			rulenames.Insert(0, "SERVICEm");
			State rule = State.NoCloneAlternation(GetSERVICEm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetBENOTIFYm(List<string> rulenames)
		{
			rulenames.Insert(0, "BENOTIFYm");
			State rule = State.NoCloneAlternation(GetBENOTIFYm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetMESSAGEm(List<string> rulenames)
		{
			rulenames.Insert(0, "MESSAGEm");
			State rule = State.NoCloneAlternation(GetMESSAGEm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetINFOm(List<string> rulenames)
		{
			rulenames.Insert(0, "INFOm");
			State rule = State.NoCloneAlternation(GetINFOm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetREFERm(List<string> rulenames)
		{
			rulenames.Insert(0, "REFERm");
			State rule = State.NoCloneAlternation(GetREFERm0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getepid_param(List<string> rulenames)
		{
			rulenames.Insert(0, "epid-param");
			State rule = State.NoCloneAlternation(Getepid_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getepid_param_value(List<string> rulenames)
		{
			rulenames.Insert(0, "epid-param-value");
			State rule = State.NoCloneAlternation(Getepid_param_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettokenchar(List<string> rulenames)
		{
			rulenames.Insert(0, "tokenchar");
			State rule = State.NoCloneAlternation(Gettokenchar0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getc_p_proxy(List<string> rulenames)
		{
			rulenames.Insert(0, "c-p-proxy");
			State rule = State.NoCloneAlternation(Getc_p_proxy0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getc_p_instance(List<string> rulenames)
		{
			rulenames.Insert(0, "c-p-instance");
			State rule = State.NoCloneAlternation(Getc_p_instance0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getinstance_val(List<string> rulenames)
		{
			rulenames.Insert(0, "instance-val");
			State rule = State.NoCloneAlternation(Getinstance_val0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getms_received_cid_param(List<string> rulenames)
		{
			rulenames.Insert(0, "ms-received-cid-param");
			State rule = State.NoCloneAlternation(Getms_received_cid_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmsspi_response(List<string> rulenames)
		{
			rulenames.Insert(0, "msspi-response");
			State rule = State.NoCloneAlternation(Getmsspi_response0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmsspi_resp(List<string> rulenames)
		{
			rulenames.Insert(0, "msspi-resp");
			State rule = State.NoCloneAlternation(Getmsspi_resp0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcnum(List<string> rulenames)
		{
			rulenames.Insert(0, "cnum");
			State rule = State.NoCloneAlternation(Getcnum0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcnum_value(List<string> rulenames)
		{
			rulenames.Insert(0, "cnum-value");
			State rule = State.NoCloneAlternation(Getcnum_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcrand(List<string> rulenames)
		{
			rulenames.Insert(0, "crand");
			State rule = State.NoCloneAlternation(Getcrand0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcrand_val(List<string> rulenames)
		{
			rulenames.Insert(0, "crand-val");
			State rule = State.NoCloneAlternation(Getcrand_val0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmsspi_resp_data(List<string> rulenames)
		{
			rulenames.Insert(0, "msspi-resp-data");
			State rule = State.NoCloneAlternation(Getmsspi_resp_data0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmsspi_resp_data_value(List<string> rulenames)
		{
			rulenames.Insert(0, "msspi-resp-data-value");
			State rule = State.NoCloneAlternation(Getmsspi_resp_data_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmsspi_cln(List<string> rulenames)
		{
			rulenames.Insert(0, "msspi-cln");
			State rule = State.NoCloneAlternation(Getmsspi_cln0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettargetname(List<string> rulenames)
		{
			rulenames.Insert(0, "targetname");
			State rule = State.NoCloneAlternation(Gettargetname0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettarget_value(List<string> rulenames)
		{
			rulenames.Insert(0, "target-value");
			State rule = State.NoCloneAlternation(Gettarget_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getntlm_target_val(List<string> rulenames)
		{
			rulenames.Insert(0, "ntlm-target-val");
			State rule = State.NoCloneAlternation(Getntlm_target_val0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getkerberos_target_val(List<string> rulenames)
		{
			rulenames.Insert(0, "kerberos-target-val");
			State rule = State.NoCloneAlternation(Getkerberos_target_val0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettls_dsk_target_val(List<string> rulenames)
		{
			rulenames.Insert(0, "tls-dsk-target-val");
			State rule = State.NoCloneAlternation(Gettls_dsk_target_val0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getgssapi_data(List<string> rulenames)
		{
			rulenames.Insert(0, "gssapi-data");
			State rule = State.NoCloneAlternation(Getgssapi_data0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getgssapi_data_value(List<string> rulenames)
		{
			rulenames.Insert(0, "gssapi-data-value");
			State rule = State.NoCloneAlternation(Getgssapi_data_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getversion(List<string> rulenames)
		{
			rulenames.Insert(0, "version");
			State rule = State.NoCloneAlternation(Getversion0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getversion_value(List<string> rulenames)
		{
			rulenames.Insert(0, "version-value");
			State rule = State.NoCloneAlternation(Getversion_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetProxy_Authentication_Info(List<string> rulenames)
		{
			rulenames.Insert(0, "Proxy-Authentication-Info");
			State rule = State.NoCloneAlternation(GetProxy_Authentication_Info0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsnum(List<string> rulenames)
		{
			rulenames.Insert(0, "snum");
			State rule = State.NoCloneAlternation(Getsnum0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsnum_value(List<string> rulenames)
		{
			rulenames.Insert(0, "snum-value");
			State rule = State.NoCloneAlternation(Getsnum_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsrand(List<string> rulenames)
		{
			rulenames.Insert(0, "srand");
			State rule = State.NoCloneAlternation(Getsrand0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsrand_value(List<string> rulenames)
		{
			rulenames.Insert(0, "srand-value");
			State rule = State.NoCloneAlternation(Getsrand_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmessage_headerX(List<string> rulenames)
		{
			rulenames.Insert(0, "message-headerX");
			State rule = State.NoCloneAlternation(Getmessage_headerX0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmessage_headerZ(List<string> rulenames)
		{
			rulenames.Insert(0, "message-headerZ");
			State rule = State.NoCloneAlternation(Getmessage_headerZ0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmessage_headerW(List<string> rulenames)
		{
			rulenames.Insert(0, "message-headerW");
			State rule = State.NoCloneAlternation(Getmessage_headerW0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSIP_messageX(List<string> rulenames)
		{
			rulenames.Insert(0, "SIP-messageX");
			State rule = State.NoCloneAlternation(GetSIP_messageX0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
	}
}
