using System;
using System.Collections.Generic;
using Fsm;

namespace DfaCompiler
{
	class GeneratedXbnf
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
		public State GetOCTET0(List<string> rulenames)
		{
			State rule = (0x00.To(0x0FF));
			return rule;
		}
		public State GetCHAR0(List<string> rulenames)
		{
			State rule = (0x00.To(0x7F));
			return rule;
		}
		public State GetUPALPHA0(List<string> rulenames)
		{
			State rule = (0x41.To(0x5A));
			return rule;
		}
		public State GetLOALPHA0(List<string> rulenames)
		{
			State rule = (0x61.To(0x7A));
			return rule;
		}
		public State GetALPHA0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetUPALPHA(rulenames),GetLOALPHA(rulenames));
			return rule;
		}
		public State GetDIGIT0(List<string> rulenames)
		{
			State rule = (0x30.To(0x39));
			return rule;
		}
		public State GetCTL0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((0x00.To(0x1F)),0x7F);
			return rule;
		}
		public State GetCR0(List<string> rulenames)
		{
			State rule = 0x0D;
			return rule;
		}
		public State GetLF0(List<string> rulenames)
		{
			State rule = 0x0A;
			return rule;
		}
		public State GetSP0(List<string> rulenames)
		{
			State rule = 0x20;
			return rule;
		}
		public State GetHT0(List<string> rulenames)
		{
			State rule = 0x09;
			return rule;
		}
		public State GetDQUOTE0(List<string> rulenames)
		{
			State rule = 0x22;
			return rule;
		}
		public State GetCRLF0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetCR(rulenames),GetLF(rulenames)));
			return rule;
		}
		public State GetLWS0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.NoCloneOption(GetCRLF(rulenames)),State.Repeat(1,-1,(State.NoCloneAlternation(GetSP(rulenames),GetHT(rulenames))))));
			return rule;
		}
		public State GetTEXT0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((0x20.To(0x7E)),(0x80.To(0x0FF)),GetLWS(rulenames));
			return rule;
		}
		public State GetHEX0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("A",rulenames),FromString("B",rulenames),FromString("C",rulenames),FromString("D",rulenames),FromString("E",rulenames),FromString("F",rulenames),FromString("a",rulenames),FromString("b",rulenames),FromString("c",rulenames),FromString("d",rulenames),FromString("e",rulenames),FromString("f",rulenames),GetDIGIT(rulenames));
			return rule;
		}
		public State Gettoken0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,(State.NoCloneAlternation(Getalphanum(rulenames),FromString("-",rulenames),FromString(".",rulenames),FromString("!",rulenames),FromString("%",rulenames),FromString("*",rulenames),FromString("_",rulenames),FromString("+",rulenames),FromString("`",rulenames),FromString("'",rulenames),FromString("~",rulenames))));
			return rule;
		}
		public State Getseparators0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("(",rulenames),FromString(")",rulenames),FromString("<",rulenames),FromString(">",rulenames),FromString("@",rulenames),FromString(",",rulenames),FromString(";",rulenames),FromString(":",rulenames),FromString("\\",rulenames),GetDQUOTE(rulenames),FromString("/",rulenames),FromString("[",rulenames),FromString("]",rulenames),FromString("?",rulenames),FromString("=",rulenames),FromString("{",rulenames),FromString("}",rulenames),GetSP(rulenames),GetHT(rulenames));
			return rule;
		}
		public State Getcomment0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("(",rulenames),State.Repeat(-1,-1,(Getctext(rulenames))),FromString(")",rulenames)));
			return rule;
		}
		public State Getctext0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((0x21.To(0x27)),(0x2A.To(0x5B)),(0x5D.To(0x7E)),GetUTF8_NONASCII(rulenames),GetLWS(rulenames));
			return rule;
		}
		public State Getquoted_string0(List<string> rulenames)
		{
			State rule = (State.NoCloneConcatanation(GetDQUOTE(rulenames),State.Repeat(-1,-1,(State.NoCloneAlternation(Getqdtext(rulenames),Getquoted_pair(rulenames)))),GetDQUOTE(rulenames)));
			return rule;
		}
		public State Getqdtext0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetLWS(rulenames),0x21,(0x23.To(0x5B)),(0x5D.To(0x7E)),GetUTF8_NONASCII(rulenames));
			return rule;
		}
		public State Getquoted_pair0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("\\",rulenames),GetCHAR(rulenames)));
			return rule;
		}
		public State GetHTTP_Version0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("HTTP",rulenames),FromString("/",rulenames),State.Repeat(1,-1,GetDIGIT(rulenames)),FromString(".",rulenames),State.Repeat(1,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State GetURI0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(GetabsoluteURI(rulenames),GetrelativeURI(rulenames))),State.NoCloneOption(State.NoCloneConcatanation(FromString("#",rulenames),Getfragment(rulenames)))));
			return rule;
		}
		public State GetabsoluteURI0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getscheme(rulenames),FromString(":",rulenames),State.Repeat(-1,-1,(State.NoCloneAlternation(Getuchar(rulenames),Getreserved(rulenames))))));
			return rule;
		}
		public State GetrelativeURI0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getnet_path(rulenames),Getabs_path(rulenames),Getrel_path(rulenames));
			return rule;
		}
		public State Getnet_path0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("//",rulenames),Getnet_loc(rulenames),State.NoCloneOption(Getabs_path(rulenames))));
			return rule;
		}
		public State Getabs_path0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("/",rulenames),Getrel_path(rulenames)));
			return rule;
		}
		public State Getrel_path0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.NoCloneOption(Getpath(rulenames)),State.NoCloneOption(State.NoCloneConcatanation(FromString(";",rulenames),Getparams(rulenames))),State.NoCloneOption(State.NoCloneConcatanation(FromString("?",rulenames),Getquery(rulenames)))));
			return rule;
		}
		public State Getpath0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getfsegment(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString("/",rulenames),Getsegment(rulenames))))));
			return rule;
		}
		public State Getfsegment0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,Getpchar(rulenames));
			return rule;
		}
		public State Getsegment0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,Getpchar(rulenames));
			return rule;
		}
		public State Getparams0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getparam(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),Getparam(rulenames))))));
			return rule;
		}
		public State Getparam0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(Getpchar(rulenames),FromString("/",rulenames))));
			return rule;
		}
		public State Getscheme0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,(State.NoCloneAlternation(GetALPHA(rulenames),GetDIGIT(rulenames),FromString("+",rulenames),FromString("-",rulenames),FromString(".",rulenames))));
			return rule;
		}
		public State Getnet_loc0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(Getpchar(rulenames),FromString(";",rulenames),FromString("?",rulenames))));
			return rule;
		}
		public State Getquery0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(Getuchar(rulenames),Getreserved(rulenames))));
			return rule;
		}
		public State Getfragment0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(Getuchar(rulenames),Getreserved(rulenames))));
			return rule;
		}
		public State Getpchar0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getuchar(rulenames),FromString(":",rulenames),FromString("@",rulenames),FromString("&",rulenames),FromString("=",rulenames),FromString("+",rulenames));
			return rule;
		}
		public State Getuchar0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getunreserved(rulenames),Getescape(rulenames));
			return rule;
		}
		public State Getunreserved0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetALPHA(rulenames),GetDIGIT(rulenames),Getsafe(rulenames),Getextra(rulenames),Getnational(rulenames));
			return rule;
		}
		public State Getescape0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("%",rulenames),GetHEX(rulenames),GetHEX(rulenames)));
			return rule;
		}
		public State Getreserved0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString(";",rulenames),FromString("/",rulenames),FromString("?",rulenames),FromString(":",rulenames),FromString("@",rulenames),FromString("&",rulenames),FromString("=",rulenames),FromString("+",rulenames));
			return rule;
		}
		public State Getextra0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("!",rulenames),FromString("*",rulenames),FromString("'",rulenames),FromString("(",rulenames),FromString(")",rulenames),FromString(",",rulenames));
			return rule;
		}
		public State Getsafe0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("$",rulenames),FromString("-",rulenames),FromString("_",rulenames),FromString(".",rulenames));
			return rule;
		}
		public State Getunsafe0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetCTL(rulenames),GetSP(rulenames),GetDQUOTE(rulenames),FromString("#",rulenames),FromString("%",rulenames),FromString("<",rulenames),FromString(">",rulenames));
			return rule;
		}
		public State Getnational0(List<string> rulenames)
		{
			State rule = (0x80.To(0x0FF));
			return rule;
		}
		public State Gethttp_URL0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("http:",rulenames),FromString("//",rulenames),Gethost(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(":",rulenames),Getport(rulenames))),State.NoCloneOption(State.NoCloneConcatanation(Getabs_path(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("?",rulenames),Getquery(rulenames)))))));
			return rule;
		}
		public State Getport0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State GetHTTP_date0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getrfc1123_date(rulenames),Getrfc850_date(rulenames),Getasctime_date(rulenames));
			return rule;
		}
		public State Getrfc1123_date0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getwkday(rulenames),FromString(",",rulenames),GetSP(rulenames),Getdate1(rulenames),GetSP(rulenames),Gettime(rulenames),GetSP(rulenames),FromString("GMT",rulenames)));
			return rule;
		}
		public State Getrfc850_date0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getweekday(rulenames),FromString(",",rulenames),GetSP(rulenames),Getdate2(rulenames),GetSP(rulenames),Gettime(rulenames),GetSP(rulenames),FromString("GMT",rulenames)));
			return rule;
		}
		public State Getasctime_date0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getwkday(rulenames),GetSP(rulenames),Getdate3(rulenames),GetSP(rulenames),Gettime(rulenames),GetSP(rulenames),State.Repeat(4,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State Getdate10(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(2,-1,GetDIGIT(rulenames)),GetSP(rulenames),Getmonth(rulenames),GetSP(rulenames),State.Repeat(4,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State Getdate20(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(2,-1,GetDIGIT(rulenames)),FromString("-",rulenames),Getmonth(rulenames),FromString("-",rulenames),State.Repeat(2,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State Getdate30(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getmonth(rulenames),GetSP(rulenames),(State.NoCloneAlternation(State.Repeat(2,-1,GetDIGIT(rulenames)),(State.NoCloneConcatanation(GetSP(rulenames),State.Repeat(1,-1,GetDIGIT(rulenames))))))));
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
		public State Getweekday0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("Monday",rulenames),FromString("Tuesday",rulenames),FromString("Wednesday",rulenames),FromString("Thursday",rulenames),FromString("Friday",rulenames),FromString("Saturday",rulenames),FromString("Sunday",rulenames));
			return rule;
		}
		public State Getmonth0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("Jan",rulenames),FromString("Feb",rulenames),FromString("Mar",rulenames),FromString("Apr",rulenames),FromString("May",rulenames),FromString("Jun",rulenames),FromString("Jul",rulenames),FromString("Aug",rulenames),FromString("Sep",rulenames),FromString("Oct",rulenames),FromString("Nov",rulenames),FromString("Dec",rulenames));
			return rule;
		}
		public State Getdelta_seconds0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Getcharset0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getcontent_coding0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Gettransfer_coding0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("chunked",rulenames),Gettransfer_extension(rulenames));
			return rule;
		}
		public State Gettransfer_extension0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),Getparameter(rulenames))))));
			return rule;
		}
		public State Getparameter0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getattribute(rulenames),FromString("=",rulenames),Getvalue(rulenames)));
			return rule;
		}
		public State Getattribute0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getvalue0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames));
			return rule;
		}
		public State GetChunked_Body0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(-1,-1,Getchunk(rulenames)),Getlast_chunk(rulenames),Gettrailer(rulenames),GetCRLF(rulenames)));
			return rule;
		}
		public State Getchunk0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getchunk_size(rulenames),State.NoCloneOption(Getchunk_extension(rulenames)),GetCRLF(rulenames),Getchunk_data(rulenames),GetCRLF(rulenames)));
			return rule;
		}
		public State Getchunk_size0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetHEX(rulenames));
			return rule;
		}
		public State Getlast_chunk0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(1,-1,(FromString("0",rulenames))),State.NoCloneOption(Getchunk_extension(rulenames)),GetCRLF(rulenames)));
			return rule;
		}
		public State Getchunk_extension0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),Getchunk_ext_name(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),Getchunk_ext_val(rulenames))))));
			return rule;
		}
		public State Getchunk_ext_name0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getchunk_ext_val0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames));
			return rule;
		}
		public State Getchunk_data0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getchunk_size(rulenames),(GetOCTET(rulenames))));
			return rule;
		}
		public State Gettrailer0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneConcatanation(Getentity_header(rulenames),GetCRLF(rulenames))));
			return rule;
		}
		public State Getmedia_type0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettype(rulenames),FromString("/",rulenames),Getsubtype(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),Getparameter(rulenames))))));
			return rule;
		}
		public State Gettype0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getsubtype0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getproduct0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("/",rulenames),Getproduct_version(rulenames)))));
			return rule;
		}
		public State Getproduct_version0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getqvalue0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(FromString("0",rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(".",rulenames),State.Repeat(0,3,GetDIGIT(rulenames)))))),(State.NoCloneConcatanation(FromString("1",rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(".",rulenames),State.Repeat(0,3,(FromString("0",rulenames))))))));
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
		public State Getentity_tag0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.NoCloneOption(Getweak(rulenames)),Getopaque_tag(rulenames)));
			return rule;
		}
		public State Getweak0(List<string> rulenames)
		{
			State rule = FromString("W/",rulenames);
			return rule;
		}
		public State Getopaque_tag0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getrange_unit0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getbytes_unit(rulenames),Getother_range_unit(rulenames));
			return rule;
		}
		public State Getbytes_unit0(List<string> rulenames)
		{
			State rule = FromString("bytes",rulenames);
			return rule;
		}
		public State Getother_range_unit0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetHTTP_message0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetRequest(rulenames),GetResponse(rulenames));
			return rule;
		}
		public State Getgeneric_message0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getstart_line(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(Getmessage_header(rulenames),GetCRLF(rulenames)))),GetCRLF(rulenames),State.NoCloneOption(Getmessage_body(rulenames))));
			return rule;
		}
		public State Getstart_line0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetRequest_Line(rulenames),GetStatus_Line(rulenames));
			return rule;
		}
		public State Getmessage_header0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getfield_name(rulenames),GetHCOLON(rulenames),State.NoCloneOption(Getfield_value(rulenames))));
			return rule;
		}
		public State Getfield_name0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getfield_value0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation(Getfield_content(rulenames),GetLWS(rulenames))));
			return rule;
		}
		public State Getfield_content0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,GetTEXT(rulenames));
			return rule;
		}
		public State Getmessage_body0(List<string> rulenames)
		{
			State rule = Getentity_body(rulenames);
			return rule;
		}
		public State Getgeneral_header0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetCache_Control(rulenames),GetConnection(rulenames),GetDate(rulenames),GetPragma(rulenames),GetTrailer(rulenames),GetTransfer_Encoding(rulenames),GetUpgrade(rulenames),GetVia(rulenames),GetWarning(rulenames));
			return rule;
		}
		public State GetRequest0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetRequest_Line(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation((State.NoCloneAlternation(Getgeneral_header(rulenames),Getrequest_header(rulenames),Getentity_header(rulenames))),GetCRLF(rulenames)))),GetCRLF(rulenames),State.NoCloneOption(Getmessage_body(rulenames))));
			return rule;
		}
		public State GetRequest_Line0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetMethod(rulenames),GetSP(rulenames),GetRequest_URI(rulenames),GetSP(rulenames),GetHTTP_Version(rulenames),GetCRLF(rulenames)));
			return rule;
		}
		public State GetMethod0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("OPTIONS",rulenames),FromString("GET",rulenames),FromString("HEAD",rulenames),FromString("POST",rulenames),FromString("PUT",rulenames),FromString("DELETE",rulenames),FromString("TRACE",rulenames),FromString("CONNECT",rulenames),Getextension_method(rulenames));
			return rule;
		}
		public State Getextension_method0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetRequest_URI0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("*",rulenames),GetabsoluteURI(rulenames),Getabs_path(rulenames));
			return rule;
		}
		public State Getrequest_header0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetAccept(rulenames),GetAccept_Charset(rulenames),GetAccept_Encoding(rulenames),GetAccept_Language(rulenames),GetAuthorization(rulenames),GetExpect(rulenames),GetFrom(rulenames),GetHost(rulenames),GetIf_Match(rulenames),GetIf_Modified_Since(rulenames),GetIf_None_Match(rulenames),GetIf_Range(rulenames),GetIf_Unmodified_Since(rulenames),GetMax_Forwards(rulenames),GetProxy_Authorization(rulenames),GetRange(rulenames),GetReferer(rulenames),GetTE(rulenames),GetUser_Agent(rulenames));
			return rule;
		}
		public State GetResponse0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetStatus_Line(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation((State.NoCloneAlternation(Getgeneral_header(rulenames),Getresponse_header(rulenames),Getentity_header(rulenames))),GetCRLF(rulenames)))),GetCRLF(rulenames),State.NoCloneOption(Getmessage_body(rulenames))));
			return rule;
		}
		public State GetStatus_Line0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetHTTP_Version(rulenames),GetSP(rulenames),GetStatus_Code(rulenames),GetSP(rulenames),GetReason_Phrase(rulenames),GetCRLF(rulenames)));
			return rule;
		}
		public State GetStatus_Code0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("100",rulenames),FromString("101",rulenames),FromString("200",rulenames),FromString("201",rulenames),FromString("202",rulenames),FromString("203",rulenames),FromString("204",rulenames),FromString("205",rulenames),FromString("206",rulenames),FromString("300",rulenames),FromString("301",rulenames),FromString("302",rulenames),FromString("303",rulenames),FromString("304",rulenames),FromString("305",rulenames),FromString("307",rulenames),FromString("400",rulenames),FromString("401",rulenames),FromString("402",rulenames),FromString("403",rulenames),FromString("404",rulenames),FromString("405",rulenames),FromString("406",rulenames),FromString("407",rulenames),FromString("408",rulenames),FromString("409",rulenames),FromString("410",rulenames),FromString("411",rulenames),FromString("412",rulenames),FromString("413",rulenames),FromString("414",rulenames),FromString("415",rulenames),FromString("416",rulenames),FromString("417",rulenames),FromString("500",rulenames),FromString("501",rulenames),FromString("502",rulenames),FromString("503",rulenames),FromString("504",rulenames),FromString("505",rulenames),Getextension_code(rulenames));
			return rule;
		}
		public State Getextension_code0(List<string> rulenames)
		{
			State rule = State.Repeat(3,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State GetReason_Phrase0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,GetTEXT(rulenames));
			return rule;
		}
		public State Getresponse_header0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetAccept_Ranges(rulenames),GetAge(rulenames),GetETag(rulenames),GetLocation(rulenames),GetProxy_Authenticate(rulenames),GetRetry_After(rulenames),GetServer(rulenames),GetVary(rulenames),GetWWW_Authenticate(rulenames));
			return rule;
		}
		public State Getentity_header0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetAllow(rulenames),GetContent_Encoding(rulenames),GetContent_Language(rulenames),GetContent_Length(rulenames),GetContent_Location(rulenames),GetContent_MD5(rulenames),GetContent_Range(rulenames),GetContent_Type(rulenames),GetExpires(rulenames),GetLast_Modified(rulenames));
			return rule;
		}
		public State Getextension_header0(List<string> rulenames)
		{
			State rule = Getmessage_header(rulenames);
			return rule;
		}
		public State Getentity_body0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,GetOCTET(rulenames));
			return rule;
		}
		public State GetAccept0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Accept",rulenames),GetHCOLON(rulenames),State.NoCloneOption(State.NoCloneRepeatBy((State.NoCloneConcatanation(Getmedia_range(rulenames),State.NoCloneOption(Getaccept_params(rulenames)))), GetCOMMA(rulenames)))));
			return rule;
		}
		public State Getmedia_range0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.NoCloneAlternation(FromString("*/*",rulenames),(State.NoCloneConcatanation(Gettype(rulenames),FromString("/",rulenames),FromString("*",rulenames))),(State.NoCloneConcatanation(Gettype(rulenames),FromString("/",rulenames),Getsubtype(rulenames))))),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),Getparameter(rulenames))))));
			return rule;
		}
		public State Getaccept_params0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString(";",rulenames),FromString("q",rulenames),FromString("=",rulenames),Getqvalue(rulenames),State.Repeat(-1,-1,(Getaccept_extension(rulenames)))));
			return rule;
		}
		public State Getaccept_extension0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString(";",rulenames),Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),(State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames)))))));
			return rule;
		}
		public State GetAccept_Charset0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Accept-Charset",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy((State.NoCloneConcatanation((State.NoCloneAlternation(Getcharset(rulenames),FromString("*",rulenames))),State.NoCloneOption(State.NoCloneConcatanation(FromString(";",rulenames),FromString("q",rulenames),FromString("=",rulenames),Getqvalue(rulenames))))), GetCOMMA(rulenames))));
			return rule;
		}
		public State GetAccept_Encoding0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Accept-Encoding",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy((State.NoCloneConcatanation(Getcodings(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(";",rulenames),FromString("q",rulenames),FromString("=",rulenames),Getqvalue(rulenames))))), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getcodings0(List<string> rulenames)
		{
			State rule = (State.NoCloneAlternation(Getcontent_coding(rulenames),FromString("*",rulenames)));
			return rule;
		}
		public State GetAccept_Language0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Accept-Language",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy((State.NoCloneConcatanation(Getlanguage_range(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(";",rulenames),FromString("q",rulenames),FromString("=",rulenames),Getqvalue(rulenames))))), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getlanguage_range0(List<string> rulenames)
		{
			State rule = (State.NoCloneAlternation((State.NoCloneConcatanation(State.Repeat(1,8,GetALPHA(rulenames)),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString("-",rulenames),State.Repeat(1,8,GetALPHA(rulenames))))))),FromString("*",rulenames)));
			return rule;
		}
		public State GetAccept_Ranges0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Accept-Ranges",rulenames),GetHCOLON(rulenames),Getacceptable_ranges(rulenames)));
			return rule;
		}
		public State Getacceptable_ranges0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(State.NoCloneRepeatBy(Getrange_unit(rulenames), GetCOMMA(rulenames)),FromString("none",rulenames));
			return rule;
		}
		public State GetAge0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Age",rulenames),GetHCOLON(rulenames),Getage_value(rulenames)));
			return rule;
		}
		public State Getage_value0(List<string> rulenames)
		{
			State rule = Getdelta_seconds(rulenames);
			return rule;
		}
		public State GetAllow0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Allow",rulenames),GetHCOLON(rulenames),State.NoCloneOption(State.NoCloneRepeatBy(GetMethod(rulenames), GetCOMMA(rulenames)))));
			return rule;
		}
		public State GetAuthorization0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Authorization",rulenames),GetHCOLON(rulenames),Getcredentials(rulenames)));
			return rule;
		}
		public State GetCache_Control0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Cache-Control",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getcache_directive(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getcache_directive0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getcache_request_directive(rulenames),Getcache_response_directive(rulenames));
			return rule;
		}
		public State Getcache_request_directive0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("no-cache",rulenames),FromString("no-store",rulenames),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("max-age",rulenames),FromString("=",rulenames),Getdelta_seconds(rulenames))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("max-stale",rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),Getdelta_seconds(rulenames))))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("min-fresh",rulenames),FromString("=",rulenames),Getdelta_seconds(rulenames))),FromString("no-transform",rulenames),FromString("only-if-cached",rulenames),Getcache_extension(rulenames));
			return rule;
		}
		public State Getcache_response_directive0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("public",rulenames),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("private",rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),GetDQUOTE(rulenames),State.NoCloneRepeatBy(Getfield_name(rulenames), GetCOMMA(rulenames)),GetDQUOTE(rulenames))))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("no-cache",rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),GetDQUOTE(rulenames),State.NoCloneRepeatBy(Getfield_name(rulenames), GetCOMMA(rulenames)),GetDQUOTE(rulenames))))),FromString("no-store",rulenames),FromString("no-transform",rulenames),FromString("must-revalidate",rulenames),FromString("proxy-revalidate",rulenames),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("max-age",rulenames),FromString("=",rulenames),Getdelta_seconds(rulenames))),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("s-maxage",rulenames),FromString("=",rulenames),Getdelta_seconds(rulenames))),Getcache_extension(rulenames));
			return rule;
		}
		public State Getcache_extension0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),(State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames)))))));
			return rule;
		}
		public State GetConnection0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Connection",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy((Getconnection_token(rulenames)), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getconnection_token0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetContent_Encoding0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-Encoding",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getcontent_coding(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State GetContent_Language0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-Language",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getlanguage_tag(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State GetContent_Length0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-Length",rulenames),GetHCOLON(rulenames),State.Repeat(1,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State GetContent_Location0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-Location",rulenames),GetHCOLON(rulenames),(State.NoCloneAlternation(GetabsoluteURI(rulenames),GetrelativeURI(rulenames)))));
			return rule;
		}
		public State GetContent_MD50(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-MD5",rulenames),GetHCOLON(rulenames),Getmd5_digest(rulenames)));
			return rule;
		}
		public State Getmd5_digest0(List<string> rulenames)
		{
			State rule = Getbase64_value_non_empty(rulenames);
			return rule;
		}
		public State GetContent_Range0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-Range",rulenames),GetHCOLON(rulenames),Getcontent_range_spec(rulenames)));
			return rule;
		}
		public State Getcontent_range_spec0(List<string> rulenames)
		{
			State rule = Getbyte_content_range_spec(rulenames);
			return rule;
		}
		public State Getbyte_content_range_spec0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getbytes_unit(rulenames),GetSP(rulenames),Getbyte_range_resp_spec(rulenames),FromString("/",rulenames),(State.NoCloneAlternation(Getinstance_length(rulenames),FromString("*",rulenames)))));
			return rule;
		}
		public State Getbyte_range_resp_spec0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(Getfirst_byte_pos(rulenames),FromString("-",rulenames),Getlast_byte_pos(rulenames))),FromString("*",rulenames));
			return rule;
		}
		public State Getinstance_length0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State GetContent_Type0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Content-Type",rulenames),GetHCOLON(rulenames),Getmedia_type(rulenames)));
			return rule;
		}
		public State GetDate0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Date",rulenames),GetHCOLON(rulenames),GetHTTP_date(rulenames)));
			return rule;
		}
		public State GetETag0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("ETag",rulenames),GetHCOLON(rulenames),Getentity_tag(rulenames)));
			return rule;
		}
		public State GetExpect0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Expect",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getexpectation(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getexpectation0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("100-continue",rulenames),Getexpectation_extension(rulenames));
			return rule;
		}
		public State Getexpectation_extension0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),(State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames))),State.Repeat(-1,-1,Getexpect_params(rulenames))))));
			return rule;
		}
		public State Getexpect_params0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString(";",rulenames),Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),(State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames)))))));
			return rule;
		}
		public State GetExpires0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Expires",rulenames),GetHCOLON(rulenames),GetHTTP_date(rulenames)));
			return rule;
		}
		public State GetFrom0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("From",rulenames),GetHCOLON(rulenames),Getmailbox(rulenames)));
			return rule;
		}
		public State GetHost0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Host",rulenames),GetHCOLON(rulenames),Gethost(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(":",rulenames),Getport(rulenames)))));
			return rule;
		}
		public State GetIf_Match0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("If-Match",rulenames),GetHCOLON(rulenames),(State.NoCloneAlternation(FromString("*",rulenames),State.NoCloneRepeatBy(Getentity_tag(rulenames), GetCOMMA(rulenames))))));
			return rule;
		}
		public State GetIf_Modified_Since0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("If-Modified-Since",rulenames),GetHCOLON(rulenames),GetHTTP_date(rulenames)));
			return rule;
		}
		public State GetIf_None_Match0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("If-None-Match",rulenames),GetHCOLON(rulenames),(State.NoCloneAlternation(FromString("*",rulenames),State.NoCloneRepeatBy(Getentity_tag(rulenames), GetCOMMA(rulenames))))));
			return rule;
		}
		public State GetIf_Range0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("If-Range",rulenames),GetHCOLON(rulenames),(State.NoCloneAlternation(Getentity_tag(rulenames),GetHTTP_date(rulenames)))));
			return rule;
		}
		public State GetIf_Unmodified_Since0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("If-Unmodified-Since",rulenames),GetHCOLON(rulenames),GetHTTP_date(rulenames)));
			return rule;
		}
		public State GetLast_Modified0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Last-Modified",rulenames),GetHCOLON(rulenames),GetHTTP_date(rulenames)));
			return rule;
		}
		public State GetLocation0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Location",rulenames),GetHCOLON(rulenames),GetabsoluteURI(rulenames)));
			return rule;
		}
		public State GetMax_Forwards0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Max-Forwards",rulenames),GetHCOLON(rulenames),State.Repeat(1,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State GetPragma0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Pragma",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getpragma_directive(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getpragma_directive0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("no-cache",rulenames),Getextension_pragma(rulenames));
			return rule;
		}
		public State Getextension_pragma0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),(State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames)))))));
			return rule;
		}
		public State GetProxy_Authenticate0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Proxy-Authenticate",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getchallenge(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State GetProxy_Authorization0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Proxy-Authorization",rulenames),GetHCOLON(rulenames),Getcredentials(rulenames)));
			return rule;
		}
		public State Getranges_specifier0(List<string> rulenames)
		{
			State rule = Getbyte_ranges_specifier(rulenames);
			return rule;
		}
		public State Getbyte_ranges_specifier0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getbytes_unit(rulenames),FromString("=",rulenames),Getbyte_range_set(rulenames)));
			return rule;
		}
		public State Getbyte_range_set0(List<string> rulenames)
		{
			State rule = State.NoCloneRepeatBy((State.NoCloneAlternation(Getbyte_range_spec(rulenames),Getsuffix_byte_range_spec(rulenames))), GetCOMMA(rulenames));
			return rule;
		}
		public State Getbyte_range_spec0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getfirst_byte_pos(rulenames),FromString("-",rulenames),State.NoCloneOption(Getlast_byte_pos(rulenames))));
			return rule;
		}
		public State Getfirst_byte_pos0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Getlast_byte_pos0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Getsuffix_byte_range_spec0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("-",rulenames),Getsuffix_length(rulenames)));
			return rule;
		}
		public State Getsuffix_length0(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State GetRange0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Range",rulenames),GetHCOLON(rulenames),Getranges_specifier(rulenames)));
			return rule;
		}
		public State GetReferer0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Referer",rulenames),GetHCOLON(rulenames),(State.NoCloneAlternation(GetabsoluteURI(rulenames),GetrelativeURI(rulenames)))));
			return rule;
		}
		public State GetRetry_After0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Retry-After",rulenames),GetHCOLON(rulenames),(State.NoCloneAlternation(GetHTTP_date(rulenames),Getdelta_seconds(rulenames)))));
			return rule;
		}
		public State GetServer0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Server",rulenames),GetHCOLON(rulenames),State.Repeat(1,-1,(State.NoCloneAlternation(Getproduct(rulenames),Getcomment(rulenames))))));
			return rule;
		}
		public State GetTE0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("TE",rulenames),GetHCOLON(rulenames),State.NoCloneOption(State.NoCloneRepeatBy((Gett_codings(rulenames)), GetCOMMA(rulenames)))));
			return rule;
		}
		public State Gett_codings0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("trailers",rulenames),(State.NoCloneConcatanation(Gettransfer_extension(rulenames),State.NoCloneOption(Getaccept_params(rulenames)))));
			return rule;
		}
		public State GetTrailer0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Trailer",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getfield_name(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State GetTransfer_Encoding0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Transfer-Encoding",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Gettransfer_coding(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State GetUpgrade0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Upgrade",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy((State.NoCloneAlternation(Getproduct(rulenames),State.NoCloneConcatanation(FromString("websocket",rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("/",rulenames),Getproduct_version(rulenames)))))), GetCOMMA(rulenames))));
			return rule;
		}
		public State GetUser_Agent0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("User-Agent",rulenames),GetHCOLON(rulenames),State.Repeat(1,-1,(State.NoCloneAlternation(Getproduct(rulenames),Getcomment(rulenames))))));
			return rule;
		}
		public State GetVary0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Vary",rulenames),GetHCOLON(rulenames),(State.NoCloneAlternation(FromString("*",rulenames),State.NoCloneRepeatBy(Getfield_name(rulenames), GetCOMMA(rulenames))))));
			return rule;
		}
		public State GetVia0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Via",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy((State.NoCloneConcatanation(Getreceived_protocol(rulenames),Getreceived_by(rulenames),State.NoCloneOption(Getcomment(rulenames)))), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getreceived_protocol0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.NoCloneOption(State.NoCloneConcatanation(Getprotocol_name(rulenames),FromString("/",rulenames))),Getprotocol_version(rulenames)));
			return rule;
		}
		public State Getprotocol_name0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getprotocol_version0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getreceived_by0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(Gethost(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(":",rulenames),Getport(rulenames))))),Getpseudonym(rulenames));
			return rule;
		}
		public State Getpseudonym0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State GetWarning0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Warning",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getwarning_value(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getwarning_value0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getwarn_code(rulenames),GetSP(rulenames),Getwarn_agent(rulenames),GetSP(rulenames),Getwarn_text(rulenames),State.NoCloneOption(State.NoCloneConcatanation(GetSP(rulenames),Getwarn_date(rulenames)))));
			return rule;
		}
		public State Getwarn_code0(List<string> rulenames)
		{
			State rule = State.Repeat(3,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Getwarn_agent0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(Gethost(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString(":",rulenames),Getport(rulenames))))),Getpseudonym(rulenames));
			return rule;
		}
		public State Getwarn_text0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getwarn_date0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetDQUOTE(rulenames),GetHTTP_date(rulenames),GetDQUOTE(rulenames)));
			return rule;
		}
		public State GetWWW_Authenticate0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("WWW-Authenticate",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getchallenge(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getauth_scheme0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getauth_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),FromString("=",rulenames),(State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames)))));
			return rule;
		}
		public State Getrealm0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("realm",rulenames),FromString("=",rulenames),Getrealm_value(rulenames)));
			return rule;
		}
		public State Getrealm_value0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getcredentials0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getauth_scheme(rulenames),State.NoCloneOption(State.NoCloneRepeatBy(Getauth_param(rulenames), GetCOMMA(rulenames)))));
			return rule;
		}
		public State Getchallenge0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Basic",rulenames),Getrealm(rulenames)));
			return rule;
		}
		public State Getcredentials1(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Basic",rulenames),Getbasic_credentials(rulenames)));
			return rule;
		}
		public State Getbasic_credentials0(List<string> rulenames)
		{
			State rule = Getbase64_user_pass(rulenames);
			return rule;
		}
		public State Getbase64_user_pass0(List<string> rulenames)
		{
			State rule = Getbase64_value_non_empty(rulenames);
			return rule;
		}
		public State Getuser_pass0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getuserid(rulenames),FromString(":",rulenames),Getpassword(rulenames)));
			return rule;
		}
		public State Getuserid0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,(State.NoCloneAlternation((0x20.To(0x39)),(0x3B.To(0x7E)),(0x80.To(0x0FF)),GetLWS(rulenames))));
			return rule;
		}
		public State Getpassword0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,GetTEXT(rulenames));
			return rule;
		}
		public State Getchallenge1(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Digest",rulenames),Getdigest_challenge(rulenames)));
			return rule;
		}
		public State Getdigest_challenge0(List<string> rulenames)
		{
			State rule = State.NoCloneRepeatBy((State.NoCloneAlternation(Getrealm(rulenames),State.NoCloneOption(Getdomain(rulenames)),Getnonce(rulenames),State.NoCloneOption(Getopaque(rulenames)),State.NoCloneOption(Getstale(rulenames)),State.NoCloneOption(Getalgorithm(rulenames)),State.NoCloneOption(Getqop_options(rulenames)),State.NoCloneOption(Getauth_param(rulenames)))), GetCOMMA(rulenames));
			return rule;
		}
		public State Getdomain0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("domain",rulenames),FromString("=",rulenames),GetDQUOTE(rulenames),GetURI(rulenames),(State.NoCloneConcatanation(State.Repeat(1,-1,GetSP(rulenames)),GetURI(rulenames))),GetDQUOTE(rulenames)));
			return rule;
		}
		public State GetURI1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetabsoluteURI(rulenames),Getabs_path(rulenames));
			return rule;
		}
		public State Getnonce0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("nonce",rulenames),FromString("=",rulenames),Getnonce_value(rulenames)));
			return rule;
		}
		public State Getnonce_value0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getopaque0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("opaque",rulenames),FromString("=",rulenames),Getquoted_string(rulenames)));
			return rule;
		}
		public State Getstale0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("stale",rulenames),FromString("=",rulenames),(State.NoCloneAlternation(FromString("true",rulenames),FromString("false",rulenames)))));
			return rule;
		}
		public State Getalgorithm0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("algorithm",rulenames),FromString("=",rulenames),(State.NoCloneAlternation(FromString("MD5",rulenames),FromString("MD5-sess",rulenames),Gettoken(rulenames)))));
			return rule;
		}
		public State Getqop_options0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("qop",rulenames),FromString("=",rulenames),GetDQUOTE(rulenames),State.NoCloneRepeatBy(Getqop_value(rulenames), GetCOMMA(rulenames)),GetDQUOTE(rulenames)));
			return rule;
		}
		public State Getqop_value0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("auth",rulenames),FromString("auth-int",rulenames),Gettoken(rulenames));
			return rule;
		}
		public State Getcredentials2(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Digest",rulenames),Getdigest_response(rulenames)));
			return rule;
		}
		public State Getdigest_response0(List<string> rulenames)
		{
			State rule = State.NoCloneRepeatBy((State.NoCloneAlternation(Getusername(rulenames),Getrealm(rulenames),Getnonce(rulenames),Getdigest_uri(rulenames),Getresponse(rulenames),State.NoCloneOption(Getalgorithm(rulenames)),State.NoCloneOption(Getcnonce(rulenames)),State.NoCloneOption(Getopaque(rulenames)),State.NoCloneOption(Getmessage_qop(rulenames)),State.NoCloneOption(Getnonce_count(rulenames)),State.NoCloneOption(Getauth_param(rulenames)))), GetCOMMA(rulenames));
			return rule;
		}
		public State Getusername0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("username",rulenames),FromString("=",rulenames),Getusername_value(rulenames)));
			return rule;
		}
		public State Getusername_value0(List<string> rulenames)
		{
			State rule = Getquoted_string(rulenames);
			return rule;
		}
		public State Getdigest_uri0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("uri",rulenames),FromString("=",rulenames),Getdigest_uri_value(rulenames)));
			return rule;
		}
		public State Getdigest_uri_value0(List<string> rulenames)
		{
			State rule = GetRequest_URI(rulenames);
			return rule;
		}
		public State Getmessage_qop0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("qop",rulenames),FromString("=",rulenames),Getqop_value(rulenames)));
			return rule;
		}
		public State Getcnonce0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("cnonce",rulenames),FromString("=",rulenames),Getcnonce_value(rulenames)));
			return rule;
		}
		public State Getcnonce_value0(List<string> rulenames)
		{
			State rule = Getnonce_value(rulenames);
			return rule;
		}
		public State Getnonce_count0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("nc",rulenames),FromString("=",rulenames),Getnc_value(rulenames)));
			return rule;
		}
		public State Getnc_value0(List<string> rulenames)
		{
			State rule = State.Repeat(8,-1,GetLHEX(rulenames));
			return rule;
		}
		public State Getresponse0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("response",rulenames),FromString("=",rulenames),Getrequest_digest(rulenames)));
			return rule;
		}
		public State Getrequest_digest0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetDQUOTE(rulenames),State.Repeat(32,-1,GetLHEX(rulenames)),GetDQUOTE(rulenames)));
			return rule;
		}
		public State GetLHEX0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("0",rulenames),FromString("1",rulenames),FromString("2",rulenames),FromString("3",rulenames),FromString("4",rulenames),FromString("5",rulenames),FromString("6",rulenames),FromString("7",rulenames),FromString("8",rulenames),FromString("9",rulenames),FromString("a",rulenames),FromString("b",rulenames),FromString("c",rulenames),FromString("d",rulenames),FromString("e",rulenames),FromString("f",rulenames));
			return rule;
		}
		public State GetAuthenticationInfo0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Authentication-Info",rulenames),GetHCOLON(rulenames),Getauth_info(rulenames)));
			return rule;
		}
		public State Getauth_info0(List<string> rulenames)
		{
			State rule = State.NoCloneRepeatBy((State.NoCloneAlternation(Getnextnonce(rulenames),State.NoCloneOption(Getmessage_qop(rulenames)),State.NoCloneOption(Getresponse_auth(rulenames)),State.NoCloneOption(Getcnonce(rulenames)),State.NoCloneOption(Getnonce_count(rulenames)))), GetCOMMA(rulenames));
			return rule;
		}
		public State Getnextnonce0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("nextnonce",rulenames),FromString("=",rulenames),Getnonce_value(rulenames)));
			return rule;
		}
		public State Getresponse_auth0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("rspauth",rulenames),FromString("=",rulenames),Getresponse_digest(rulenames)));
			return rule;
		}
		public State Getresponse_digest0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetDQUOTE(rulenames),State.Repeat(-1,-1,GetLHEX(rulenames)),GetDQUOTE(rulenames)));
			return rule;
		}
		public State Getset_cookie_header0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Set-Cookie:",rulenames),GetSP(rulenames),Getset_cookie_string(rulenames)));
			return rule;
		}
		public State Getset_cookie_string0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getcookie_pair(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),GetSP(rulenames),Getcookie_av(rulenames))))));
			return rule;
		}
		public State Getcookie_pair0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getcookie_name(rulenames),FromString("=",rulenames),Getcookie_value(rulenames)));
			return rule;
		}
		public State Getcookie_name0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getcookie_value0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(State.Repeat(-1,-1,Getcookie_octet(rulenames)),(State.NoCloneConcatanation(GetDQUOTE(rulenames),State.Repeat(-1,-1,Getcookie_octet(rulenames)),GetDQUOTE(rulenames))));
			return rule;
		}
		public State Getcookie_octet0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(0x21,(0x23.To(0x2B)),(0x2D.To(0x3A)),(0x3C.To(0x5B)),(0x5D.To(0x7E)));
			return rule;
		}
		public State Getcookie_av0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getexpires_av(rulenames),Getmax_age_av(rulenames),Getdomain_av(rulenames),Getpath_av(rulenames),Getsecure_av(rulenames),Gethttponly_av(rulenames),Getextension_av(rulenames));
			return rule;
		}
		public State Getexpires_av0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Expires=",rulenames),Getsane_cookie_date(rulenames)));
			return rule;
		}
		public State Getsane_cookie_date0(List<string> rulenames)
		{
			State rule = Getrfc1123_date(rulenames);
			return rule;
		}
		public State Getmax_age_av0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Max-Age=",rulenames),Getnon_zero_digit(rulenames),State.Repeat(-1,-1,GetDIGIT(rulenames))));
			return rule;
		}
		public State Getnon_zero_digit0(List<string> rulenames)
		{
			State rule = (0x31.To(0x39));
			return rule;
		}
		public State Getdomain_av0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Domain=",rulenames),Getdomain_value(rulenames)));
			return rule;
		}
		public State Getdomain_value0(List<string> rulenames)
		{
			State rule = State.Substract(Gettoken(rulenames),FromString(";",rulenames));
			return rule;
		}
		public State Getpath_av0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Path=",rulenames),Getpath_value(rulenames)));
			return rule;
		}
		public State Getpath_value0(List<string> rulenames)
		{
			State rule = Getexcept_CTL_semi(rulenames);
			return rule;
		}
		public State Getsecure_av0(List<string> rulenames)
		{
			State rule = FromString("Secure",rulenames);
			return rule;
		}
		public State Gethttponly_av0(List<string> rulenames)
		{
			State rule = FromString("HttpOnly",rulenames);
			return rule;
		}
		public State Getextension_av0(List<string> rulenames)
		{
			State rule = Getexcept_CTL_semi(rulenames);
			return rule;
		}
		public State Getexcept_CTL_semi0(List<string> rulenames)
		{
			State rule = State.Substract(((0x00.To(0x0FF))),(State.NoCloneAlternation(GetCTL(rulenames),FromString(";",rulenames))));
			return rule;
		}
		public State Getcookie_header0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Cookie",rulenames),GetHCOLON(rulenames),Getcookie_string(rulenames)));
			return rule;
		}
		public State Getcookie_string0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getcookie_pair(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),GetSP(rulenames),Getcookie_pair(rulenames))))));
			return rule;
		}
		public State GetSec_WebSocket_Key0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Sec-WebSocket-Key",rulenames),GetHCOLON(rulenames),Getbase64_value_non_empty(rulenames)));
			return rule;
		}
		public State GetSec_WebSocket_Extensions0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Sec-WebSocket-Extensions",rulenames),GetHCOLON(rulenames),Getextension_list(rulenames)));
			return rule;
		}
		public State GetSec_WebSocket_Protocol_Client0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Sec-WebSocket-Protocol",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Gettoken(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State GetSec_WebSocket_Version_Client0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Sec-WebSocket-Version",rulenames),GetHCOLON(rulenames),Getversion(rulenames)));
			return rule;
		}
		public State Getbase64_value_non_empty0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(State.Repeat(1,-1,Getbase64_data(rulenames)),State.NoCloneOption(Getbase64_padding(rulenames)))),Getbase64_padding(rulenames));
			return rule;
		}
		public State Getbase64_data0(List<string> rulenames)
		{
			State rule = State.Repeat(4,4,Getbase64_character(rulenames));
			return rule;
		}
		public State Getbase64_padding0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation((State.NoCloneConcatanation(State.Repeat(2,2,Getbase64_character(rulenames)),FromString("==",rulenames))),(State.NoCloneConcatanation(State.Repeat(3,3,Getbase64_character(rulenames)),FromString("=",rulenames))));
			return rule;
		}
		public State Getbase64_character0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetALPHA(rulenames),GetDIGIT(rulenames),FromString("+",rulenames),FromString("/",rulenames));
			return rule;
		}
		public State Getextension_list0(List<string> rulenames)
		{
			State rule = State.NoCloneRepeatBy(Getextension(rulenames), GetCOMMA(rulenames));
			return rule;
		}
		public State Getextension0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Getextension_token(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation(FromString(";",rulenames),Getextension_param(rulenames))))));
			return rule;
		}
		public State Getextension_token0(List<string> rulenames)
		{
			State rule = Getregistered_token(rulenames);
			return rule;
		}
		public State Getregistered_token0(List<string> rulenames)
		{
			State rule = Gettoken(rulenames);
			return rule;
		}
		public State Getextension_param0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,Gettoken(rulenames),State.NoCloneOption(State.NoCloneConcatanation(FromString("=",rulenames),(State.NoCloneAlternation(Gettoken(rulenames),Getquoted_string(rulenames)))))));
			return rule;
		}
		public State GetNZDIGIT0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("1",rulenames),FromString("2",rulenames),FromString("3",rulenames),FromString("4",rulenames),FromString("5",rulenames),FromString("6",rulenames),FromString("7",rulenames),FromString("8",rulenames),FromString("9",rulenames));
			return rule;
		}
		public State Getversion0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetDIGIT(rulenames),(State.NoCloneConcatanation(GetNZDIGIT(rulenames),GetDIGIT(rulenames))),(State.NoCloneConcatanation(FromString("1",rulenames),GetDIGIT(rulenames),GetDIGIT(rulenames))),(State.NoCloneConcatanation(FromString("2",rulenames),GetDIGIT(rulenames),GetDIGIT(rulenames))));
			return rule;
		}
		public State GetSec_WebSocket_Accept0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Sec-WebSocket-Accept",rulenames),GetHCOLON(rulenames),Getbase64_value_non_empty(rulenames)));
			return rule;
		}
		public State GetSec_WebSocket_Protocol_Server0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Sec-WebSocket-Protocol",rulenames),GetHCOLON(rulenames),Gettoken(rulenames)));
			return rule;
		}
		public State GetSec_WebSocket_Version_Server0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("Sec-WebSocket-Version",rulenames),GetHCOLON(rulenames),State.NoCloneRepeatBy(Getversion(rulenames), GetCOMMA(rulenames))));
			return rule;
		}
		public State Getrequest_header1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetSec_WebSocket_Key(rulenames),GetSec_WebSocket_Extensions(rulenames),GetSec_WebSocket_Protocol_Client(rulenames),GetSec_WebSocket_Version_Client(rulenames));
			return rule;
		}
		public State Getmailbox0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(Getaddr_spec(rulenames),State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.NoCloneOption(Getphrase(rulenames)),Getroute_addr(rulenames))));
			return rule;
		}
		public State Getaddr_spec0(List<string> rulenames)
		{
			State rule = GetabsoluteURI(rulenames);
			return rule;
		}
		public State Getroute_addr0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,FromString("<",rulenames),State.NoCloneOption(Getroute(rulenames)),Getaddr_spec(rulenames),FromString(">",rulenames)));
			return rule;
		}
		public State Getroute0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.NoCloneRepeatBy((State.NoCloneConcatanation(FromString("@",rulenames),Gethostname(rulenames))), GetCOMMA(rulenames)),FromString(":",rulenames)));
			return rule;
		}
		public State Getphrase0(List<string> rulenames)
		{
			State rule = State.Repeat(-1,-1,GetTEXT(rulenames));
			return rule;
		}
		public State Getalphanum0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetALPHA(rulenames),GetDIGIT(rulenames));
			return rule;
		}
		public State GetDIGIT1(List<string> rulenames)
		{
			State rule = (0x30.To(0x39));
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
		public State GetLHEX1(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(GetDIGIT(rulenames),(0x61.To(0x66)));
			return rule;
		}
		public State GetSWS0(List<string> rulenames)
		{
			State rule = State.NoCloneOption(GetLWS(rulenames));
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
		public State GetHCOLON0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,State.Repeat(-1,-1,(State.NoCloneAlternation(GetSP(rulenames),GetHTAB(rulenames)))),FromString(":",rulenames),GetSWS(rulenames)));
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
		public State Getport1(List<string> rulenames)
		{
			State rule = State.Repeat(1,-1,GetDIGIT(rulenames));
			return rule;
		}
		public State Getreq0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,GetRequest_Line(rulenames),State.Repeat(-1,-1,(State.NoCloneConcatanation((State.NoCloneAlternation(Getgeneral_header(rulenames),Getrequest_header(rulenames),Getentity_header(rulenames),Getcookie_header(rulenames),Getextension_header_marked_only(rulenames))),GetCRLF(rulenames)))),GetCRLF(rulenames)));
			return rule;
		}
		public State Getextension_header_marked_only0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.Substract(Gettoken(rulenames),(State.NoCloneAlternation(FromString("Host",rulenames),FromString("Cookie",rulenames),FromString("Content-Type",rulenames),FromString("Content-Length",rulenames),FromString("Upgrade",rulenames),FromString("Referer",rulenames),FromString("Sec-WebSocket-Key",rulenames),FromString("Sec-WebSocket-Extensions",rulenames),FromString("Sec-WebSocket-Protocol",rulenames),FromString("Sec-WebSocket-Version",rulenames))))),GetHCOLON(rulenames),State.NoCloneOption(Getfield_value(rulenames))));
			return rule;
		}
		public State Getextension_header_for_request0(List<string> rulenames)
		{
			State rule = State.NoCloneConcatanation(OnChangeConcatanation(rulenames,(State.Substract(Gettoken(rulenames),(State.NoCloneAlternation(Getgeneral_header_names(rulenames),Getrequest_header_names(rulenames),Getentity_header_names(rulenames))))),GetHCOLON(rulenames),State.NoCloneOption(Getfield_value(rulenames))));
			return rule;
		}
		public State Getgeneral_header_names0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("Cache-Control",rulenames),FromString("Connection",rulenames),FromString("Date",rulenames),FromString("Pragma",rulenames),FromString("Trailer",rulenames),FromString("Transfer-Encoding",rulenames),FromString("Upgrade",rulenames),FromString("Via",rulenames),FromString("Warning",rulenames));
			return rule;
		}
		public State Getrequest_header_names0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("Accept",rulenames),FromString("Accept-Charset",rulenames),FromString("Accept-Encoding",rulenames),FromString("Accept-Language",rulenames),FromString("Authorization",rulenames),FromString("Expect",rulenames),FromString("From",rulenames),FromString("Host",rulenames),FromString("If-Match",rulenames),FromString("If-Modified-Since",rulenames),FromString("If-None-Match",rulenames),FromString("If-Range",rulenames),FromString("If-Unmodified-Since",rulenames),FromString("Max-Forwards",rulenames),FromString("Proxy-Authorization",rulenames),FromString("Range",rulenames),FromString("Referer",rulenames),FromString("TE",rulenames),FromString("User-Agent",rulenames));
			return rule;
		}
		public State Getentity_header_names0(List<string> rulenames)
		{
			State rule = State.NoCloneAlternation(FromString("Allow",rulenames),FromString("Content-Encoding",rulenames),FromString("Content-Language",rulenames),FromString("Content-Length",rulenames),FromString("Content-Location",rulenames),FromString("Content-MD5",rulenames),FromString("Content-Range",rulenames),FromString("Content-Type",rulenames),FromString("Expires",rulenames),FromString("Last-Modified",rulenames));
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
		public State GetCHAR(List<string> rulenames)
		{
			rulenames.Insert(0, "CHAR");
			State rule = State.NoCloneAlternation(GetCHAR0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetUPALPHA(List<string> rulenames)
		{
			rulenames.Insert(0, "UPALPHA");
			State rule = State.NoCloneAlternation(GetUPALPHA0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLOALPHA(List<string> rulenames)
		{
			rulenames.Insert(0, "LOALPHA");
			State rule = State.NoCloneAlternation(GetLOALPHA0(rulenames));
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
		public State GetDIGIT(List<string> rulenames)
		{
			rulenames.Insert(0, "DIGIT");
			State rule = State.NoCloneAlternation(GetDIGIT0(rulenames), GetDIGIT1(rulenames));
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
		public State GetCR(List<string> rulenames)
		{
			rulenames.Insert(0, "CR");
			State rule = State.NoCloneAlternation(GetCR0(rulenames));
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
		public State GetSP(List<string> rulenames)
		{
			rulenames.Insert(0, "SP");
			State rule = State.NoCloneAlternation(GetSP0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetHT(List<string> rulenames)
		{
			rulenames.Insert(0, "HT");
			State rule = State.NoCloneAlternation(GetHT0(rulenames));
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
		public State GetCRLF(List<string> rulenames)
		{
			rulenames.Insert(0, "CRLF");
			State rule = State.NoCloneAlternation(GetCRLF0(rulenames));
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
		public State GetTEXT(List<string> rulenames)
		{
			rulenames.Insert(0, "TEXT");
			State rule = State.NoCloneAlternation(GetTEXT0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetHEX(List<string> rulenames)
		{
			rulenames.Insert(0, "HEX");
			State rule = State.NoCloneAlternation(GetHEX0(rulenames));
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
		public State Getquoted_pair(List<string> rulenames)
		{
			rulenames.Insert(0, "quoted-pair");
			State rule = State.NoCloneAlternation(Getquoted_pair0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetHTTP_Version(List<string> rulenames)
		{
			rulenames.Insert(0, "HTTP-Version");
			State rule = State.NoCloneAlternation(GetHTTP_Version0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetURI(List<string> rulenames)
		{
			rulenames.Insert(0, "URI");
			State rule = State.NoCloneAlternation(GetURI0(rulenames), GetURI1(rulenames));
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
		public State GetrelativeURI(List<string> rulenames)
		{
			rulenames.Insert(0, "relativeURI");
			State rule = State.NoCloneAlternation(GetrelativeURI0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnet_path(List<string> rulenames)
		{
			rulenames.Insert(0, "net_path");
			State rule = State.NoCloneAlternation(Getnet_path0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getabs_path(List<string> rulenames)
		{
			rulenames.Insert(0, "abs_path");
			State rule = State.NoCloneAlternation(Getabs_path0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrel_path(List<string> rulenames)
		{
			rulenames.Insert(0, "rel_path");
			State rule = State.NoCloneAlternation(Getrel_path0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpath(List<string> rulenames)
		{
			rulenames.Insert(0, "path");
			State rule = State.NoCloneAlternation(Getpath0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getfsegment(List<string> rulenames)
		{
			rulenames.Insert(0, "fsegment");
			State rule = State.NoCloneAlternation(Getfsegment0(rulenames));
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
		public State Getparams(List<string> rulenames)
		{
			rulenames.Insert(0, "params");
			State rule = State.NoCloneAlternation(Getparams0(rulenames));
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
		public State Getscheme(List<string> rulenames)
		{
			rulenames.Insert(0, "scheme");
			State rule = State.NoCloneAlternation(Getscheme0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnet_loc(List<string> rulenames)
		{
			rulenames.Insert(0, "net_loc");
			State rule = State.NoCloneAlternation(Getnet_loc0(rulenames));
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
		public State Getfragment(List<string> rulenames)
		{
			rulenames.Insert(0, "fragment");
			State rule = State.NoCloneAlternation(Getfragment0(rulenames));
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
		public State Getuchar(List<string> rulenames)
		{
			rulenames.Insert(0, "uchar");
			State rule = State.NoCloneAlternation(Getuchar0(rulenames));
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
		public State Getescape(List<string> rulenames)
		{
			rulenames.Insert(0, "escape");
			State rule = State.NoCloneAlternation(Getescape0(rulenames));
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
		public State Getextra(List<string> rulenames)
		{
			rulenames.Insert(0, "extra");
			State rule = State.NoCloneAlternation(Getextra0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsafe(List<string> rulenames)
		{
			rulenames.Insert(0, "safe");
			State rule = State.NoCloneAlternation(Getsafe0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getunsafe(List<string> rulenames)
		{
			rulenames.Insert(0, "unsafe");
			State rule = State.NoCloneAlternation(Getunsafe0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnational(List<string> rulenames)
		{
			rulenames.Insert(0, "national");
			State rule = State.NoCloneAlternation(Getnational0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethttp_URL(List<string> rulenames)
		{
			rulenames.Insert(0, "http_URL");
			State rule = State.NoCloneAlternation(Gethttp_URL0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getport(List<string> rulenames)
		{
			rulenames.Insert(0, "port");
			State rule = State.NoCloneAlternation(Getport0(rulenames), Getport1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetHTTP_date(List<string> rulenames)
		{
			rulenames.Insert(0, "HTTP-date");
			State rule = State.NoCloneAlternation(GetHTTP_date0(rulenames));
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
		public State Getrfc850_date(List<string> rulenames)
		{
			rulenames.Insert(0, "rfc850-date");
			State rule = State.NoCloneAlternation(Getrfc850_date0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getasctime_date(List<string> rulenames)
		{
			rulenames.Insert(0, "asctime-date");
			State rule = State.NoCloneAlternation(Getasctime_date0(rulenames));
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
		public State Getdate2(List<string> rulenames)
		{
			rulenames.Insert(0, "date2");
			State rule = State.NoCloneAlternation(Getdate20(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdate3(List<string> rulenames)
		{
			rulenames.Insert(0, "date3");
			State rule = State.NoCloneAlternation(Getdate30(rulenames));
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
		public State Getweekday(List<string> rulenames)
		{
			rulenames.Insert(0, "weekday");
			State rule = State.NoCloneAlternation(Getweekday0(rulenames));
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
		public State Getdelta_seconds(List<string> rulenames)
		{
			rulenames.Insert(0, "delta-seconds");
			State rule = State.NoCloneAlternation(Getdelta_seconds0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcharset(List<string> rulenames)
		{
			rulenames.Insert(0, "charset");
			State rule = State.NoCloneAlternation(Getcharset0(rulenames));
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
		public State Gettransfer_coding(List<string> rulenames)
		{
			rulenames.Insert(0, "transfer-coding");
			State rule = State.NoCloneAlternation(Gettransfer_coding0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettransfer_extension(List<string> rulenames)
		{
			rulenames.Insert(0, "transfer-extension");
			State rule = State.NoCloneAlternation(Gettransfer_extension0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getparameter(List<string> rulenames)
		{
			rulenames.Insert(0, "parameter");
			State rule = State.NoCloneAlternation(Getparameter0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getattribute(List<string> rulenames)
		{
			rulenames.Insert(0, "attribute");
			State rule = State.NoCloneAlternation(Getattribute0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getvalue(List<string> rulenames)
		{
			rulenames.Insert(0, "value");
			State rule = State.NoCloneAlternation(Getvalue0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetChunked_Body(List<string> rulenames)
		{
			rulenames.Insert(0, "Chunked-Body");
			State rule = State.NoCloneAlternation(GetChunked_Body0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getchunk(List<string> rulenames)
		{
			rulenames.Insert(0, "chunk");
			State rule = State.NoCloneAlternation(Getchunk0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getchunk_size(List<string> rulenames)
		{
			rulenames.Insert(0, "chunk-size");
			State rule = State.NoCloneAlternation(Getchunk_size0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getlast_chunk(List<string> rulenames)
		{
			rulenames.Insert(0, "last-chunk");
			State rule = State.NoCloneAlternation(Getlast_chunk0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getchunk_extension(List<string> rulenames)
		{
			rulenames.Insert(0, "chunk-extension");
			State rule = State.NoCloneAlternation(Getchunk_extension0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getchunk_ext_name(List<string> rulenames)
		{
			rulenames.Insert(0, "chunk-ext-name");
			State rule = State.NoCloneAlternation(Getchunk_ext_name0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getchunk_ext_val(List<string> rulenames)
		{
			rulenames.Insert(0, "chunk-ext-val");
			State rule = State.NoCloneAlternation(Getchunk_ext_val0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getchunk_data(List<string> rulenames)
		{
			rulenames.Insert(0, "chunk-data");
			State rule = State.NoCloneAlternation(Getchunk_data0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gettrailer(List<string> rulenames)
		{
			rulenames.Insert(0, "trailer");
			State rule = State.NoCloneAlternation(Gettrailer0(rulenames));
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
		public State Gettype(List<string> rulenames)
		{
			rulenames.Insert(0, "type");
			State rule = State.NoCloneAlternation(Gettype0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsubtype(List<string> rulenames)
		{
			rulenames.Insert(0, "subtype");
			State rule = State.NoCloneAlternation(Getsubtype0(rulenames));
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
		public State Getqvalue(List<string> rulenames)
		{
			rulenames.Insert(0, "qvalue");
			State rule = State.NoCloneAlternation(Getqvalue0(rulenames));
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
		public State Getentity_tag(List<string> rulenames)
		{
			rulenames.Insert(0, "entity-tag");
			State rule = State.NoCloneAlternation(Getentity_tag0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getweak(List<string> rulenames)
		{
			rulenames.Insert(0, "weak");
			State rule = State.NoCloneAlternation(Getweak0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getopaque_tag(List<string> rulenames)
		{
			rulenames.Insert(0, "opaque-tag");
			State rule = State.NoCloneAlternation(Getopaque_tag0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrange_unit(List<string> rulenames)
		{
			rulenames.Insert(0, "range-unit");
			State rule = State.NoCloneAlternation(Getrange_unit0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbytes_unit(List<string> rulenames)
		{
			rulenames.Insert(0, "bytes-unit");
			State rule = State.NoCloneAlternation(Getbytes_unit0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getother_range_unit(List<string> rulenames)
		{
			rulenames.Insert(0, "other-range-unit");
			State rule = State.NoCloneAlternation(Getother_range_unit0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetHTTP_message(List<string> rulenames)
		{
			rulenames.Insert(0, "HTTP-message");
			State rule = State.NoCloneAlternation(GetHTTP_message0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getgeneric_message(List<string> rulenames)
		{
			rulenames.Insert(0, "generic-message");
			State rule = State.NoCloneAlternation(Getgeneric_message0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getstart_line(List<string> rulenames)
		{
			rulenames.Insert(0, "start-line");
			State rule = State.NoCloneAlternation(Getstart_line0(rulenames));
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
		public State Getfield_name(List<string> rulenames)
		{
			rulenames.Insert(0, "field-name");
			State rule = State.NoCloneAlternation(Getfield_name0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getfield_value(List<string> rulenames)
		{
			rulenames.Insert(0, "field-value");
			State rule = State.NoCloneAlternation(Getfield_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getfield_content(List<string> rulenames)
		{
			rulenames.Insert(0, "field-content");
			State rule = State.NoCloneAlternation(Getfield_content0(rulenames));
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
		public State Getgeneral_header(List<string> rulenames)
		{
			rulenames.Insert(0, "general-header");
			State rule = State.NoCloneAlternation(Getgeneral_header0(rulenames));
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
			State rule = State.NoCloneAlternation(Getextension_method0(rulenames));
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
		public State Getrequest_header(List<string> rulenames)
		{
			rulenames.Insert(0, "request-header");
			State rule = State.NoCloneAlternation(Getrequest_header0(rulenames), Getrequest_header1(rulenames));
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
		public State Getresponse_header(List<string> rulenames)
		{
			rulenames.Insert(0, "response-header");
			State rule = State.NoCloneAlternation(Getresponse_header0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getentity_header(List<string> rulenames)
		{
			rulenames.Insert(0, "entity-header");
			State rule = State.NoCloneAlternation(Getentity_header0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_header(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-header");
			State rule = State.NoCloneAlternation(Getextension_header0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getentity_body(List<string> rulenames)
		{
			rulenames.Insert(0, "entity-body");
			State rule = State.NoCloneAlternation(Getentity_body0(rulenames));
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
		public State Getmedia_range(List<string> rulenames)
		{
			rulenames.Insert(0, "media-range");
			State rule = State.NoCloneAlternation(Getmedia_range0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getaccept_params(List<string> rulenames)
		{
			rulenames.Insert(0, "accept-params");
			State rule = State.NoCloneAlternation(Getaccept_params0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getaccept_extension(List<string> rulenames)
		{
			rulenames.Insert(0, "accept-extension");
			State rule = State.NoCloneAlternation(Getaccept_extension0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAccept_Charset(List<string> rulenames)
		{
			rulenames.Insert(0, "Accept-Charset");
			State rule = State.NoCloneAlternation(GetAccept_Charset0(rulenames));
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
		public State Getcodings(List<string> rulenames)
		{
			rulenames.Insert(0, "codings");
			State rule = State.NoCloneAlternation(Getcodings0(rulenames));
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
		public State Getlanguage_range(List<string> rulenames)
		{
			rulenames.Insert(0, "language-range");
			State rule = State.NoCloneAlternation(Getlanguage_range0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAccept_Ranges(List<string> rulenames)
		{
			rulenames.Insert(0, "Accept-Ranges");
			State rule = State.NoCloneAlternation(GetAccept_Ranges0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getacceptable_ranges(List<string> rulenames)
		{
			rulenames.Insert(0, "acceptable-ranges");
			State rule = State.NoCloneAlternation(Getacceptable_ranges0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAge(List<string> rulenames)
		{
			rulenames.Insert(0, "Age");
			State rule = State.NoCloneAlternation(GetAge0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getage_value(List<string> rulenames)
		{
			rulenames.Insert(0, "age-value");
			State rule = State.NoCloneAlternation(Getage_value0(rulenames));
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
		public State GetCache_Control(List<string> rulenames)
		{
			rulenames.Insert(0, "Cache-Control");
			State rule = State.NoCloneAlternation(GetCache_Control0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcache_directive(List<string> rulenames)
		{
			rulenames.Insert(0, "cache-directive");
			State rule = State.NoCloneAlternation(Getcache_directive0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcache_request_directive(List<string> rulenames)
		{
			rulenames.Insert(0, "cache-request-directive");
			State rule = State.NoCloneAlternation(Getcache_request_directive0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcache_response_directive(List<string> rulenames)
		{
			rulenames.Insert(0, "cache-response-directive");
			State rule = State.NoCloneAlternation(Getcache_response_directive0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcache_extension(List<string> rulenames)
		{
			rulenames.Insert(0, "cache-extension");
			State rule = State.NoCloneAlternation(Getcache_extension0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetConnection(List<string> rulenames)
		{
			rulenames.Insert(0, "Connection");
			State rule = State.NoCloneAlternation(GetConnection0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getconnection_token(List<string> rulenames)
		{
			rulenames.Insert(0, "connection-token");
			State rule = State.NoCloneAlternation(Getconnection_token0(rulenames));
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
		public State GetContent_Length(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-Length");
			State rule = State.NoCloneAlternation(GetContent_Length0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContent_Location(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-Location");
			State rule = State.NoCloneAlternation(GetContent_Location0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContent_MD5(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-MD5");
			State rule = State.NoCloneAlternation(GetContent_MD50(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmd5_digest(List<string> rulenames)
		{
			rulenames.Insert(0, "md5-digest");
			State rule = State.NoCloneAlternation(Getmd5_digest0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetContent_Range(List<string> rulenames)
		{
			rulenames.Insert(0, "Content-Range");
			State rule = State.NoCloneAlternation(GetContent_Range0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcontent_range_spec(List<string> rulenames)
		{
			rulenames.Insert(0, "content-range-spec");
			State rule = State.NoCloneAlternation(Getcontent_range_spec0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbyte_content_range_spec(List<string> rulenames)
		{
			rulenames.Insert(0, "byte-content-range-spec");
			State rule = State.NoCloneAlternation(Getbyte_content_range_spec0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbyte_range_resp_spec(List<string> rulenames)
		{
			rulenames.Insert(0, "byte-range-resp-spec");
			State rule = State.NoCloneAlternation(Getbyte_range_resp_spec0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getinstance_length(List<string> rulenames)
		{
			rulenames.Insert(0, "instance-length");
			State rule = State.NoCloneAlternation(Getinstance_length0(rulenames));
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
		public State GetDate(List<string> rulenames)
		{
			rulenames.Insert(0, "Date");
			State rule = State.NoCloneAlternation(GetDate0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetETag(List<string> rulenames)
		{
			rulenames.Insert(0, "ETag");
			State rule = State.NoCloneAlternation(GetETag0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetExpect(List<string> rulenames)
		{
			rulenames.Insert(0, "Expect");
			State rule = State.NoCloneAlternation(GetExpect0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getexpectation(List<string> rulenames)
		{
			rulenames.Insert(0, "expectation");
			State rule = State.NoCloneAlternation(Getexpectation0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getexpectation_extension(List<string> rulenames)
		{
			rulenames.Insert(0, "expectation-extension");
			State rule = State.NoCloneAlternation(Getexpectation_extension0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getexpect_params(List<string> rulenames)
		{
			rulenames.Insert(0, "expect-params");
			State rule = State.NoCloneAlternation(Getexpect_params0(rulenames));
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
		public State GetHost(List<string> rulenames)
		{
			rulenames.Insert(0, "Host");
			State rule = State.NoCloneAlternation(GetHost0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIf_Match(List<string> rulenames)
		{
			rulenames.Insert(0, "If-Match");
			State rule = State.NoCloneAlternation(GetIf_Match0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIf_Modified_Since(List<string> rulenames)
		{
			rulenames.Insert(0, "If-Modified-Since");
			State rule = State.NoCloneAlternation(GetIf_Modified_Since0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIf_None_Match(List<string> rulenames)
		{
			rulenames.Insert(0, "If-None-Match");
			State rule = State.NoCloneAlternation(GetIf_None_Match0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIf_Range(List<string> rulenames)
		{
			rulenames.Insert(0, "If-Range");
			State rule = State.NoCloneAlternation(GetIf_Range0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetIf_Unmodified_Since(List<string> rulenames)
		{
			rulenames.Insert(0, "If-Unmodified-Since");
			State rule = State.NoCloneAlternation(GetIf_Unmodified_Since0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLast_Modified(List<string> rulenames)
		{
			rulenames.Insert(0, "Last-Modified");
			State rule = State.NoCloneAlternation(GetLast_Modified0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetLocation(List<string> rulenames)
		{
			rulenames.Insert(0, "Location");
			State rule = State.NoCloneAlternation(GetLocation0(rulenames));
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
		public State GetPragma(List<string> rulenames)
		{
			rulenames.Insert(0, "Pragma");
			State rule = State.NoCloneAlternation(GetPragma0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpragma_directive(List<string> rulenames)
		{
			rulenames.Insert(0, "pragma-directive");
			State rule = State.NoCloneAlternation(Getpragma_directive0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_pragma(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-pragma");
			State rule = State.NoCloneAlternation(Getextension_pragma0(rulenames));
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
		public State GetProxy_Authorization(List<string> rulenames)
		{
			rulenames.Insert(0, "Proxy-Authorization");
			State rule = State.NoCloneAlternation(GetProxy_Authorization0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getranges_specifier(List<string> rulenames)
		{
			rulenames.Insert(0, "ranges-specifier");
			State rule = State.NoCloneAlternation(Getranges_specifier0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbyte_ranges_specifier(List<string> rulenames)
		{
			rulenames.Insert(0, "byte-ranges-specifier");
			State rule = State.NoCloneAlternation(Getbyte_ranges_specifier0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbyte_range_set(List<string> rulenames)
		{
			rulenames.Insert(0, "byte-range-set");
			State rule = State.NoCloneAlternation(Getbyte_range_set0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbyte_range_spec(List<string> rulenames)
		{
			rulenames.Insert(0, "byte-range-spec");
			State rule = State.NoCloneAlternation(Getbyte_range_spec0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getfirst_byte_pos(List<string> rulenames)
		{
			rulenames.Insert(0, "first-byte-pos");
			State rule = State.NoCloneAlternation(Getfirst_byte_pos0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getlast_byte_pos(List<string> rulenames)
		{
			rulenames.Insert(0, "last-byte-pos");
			State rule = State.NoCloneAlternation(Getlast_byte_pos0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsuffix_byte_range_spec(List<string> rulenames)
		{
			rulenames.Insert(0, "suffix-byte-range-spec");
			State rule = State.NoCloneAlternation(Getsuffix_byte_range_spec0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsuffix_length(List<string> rulenames)
		{
			rulenames.Insert(0, "suffix-length");
			State rule = State.NoCloneAlternation(Getsuffix_length0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetRange(List<string> rulenames)
		{
			rulenames.Insert(0, "Range");
			State rule = State.NoCloneAlternation(GetRange0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetReferer(List<string> rulenames)
		{
			rulenames.Insert(0, "Referer");
			State rule = State.NoCloneAlternation(GetReferer0(rulenames));
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
		public State GetServer(List<string> rulenames)
		{
			rulenames.Insert(0, "Server");
			State rule = State.NoCloneAlternation(GetServer0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetTE(List<string> rulenames)
		{
			rulenames.Insert(0, "TE");
			State rule = State.NoCloneAlternation(GetTE0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gett_codings(List<string> rulenames)
		{
			rulenames.Insert(0, "t-codings");
			State rule = State.NoCloneAlternation(Gett_codings0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetTrailer(List<string> rulenames)
		{
			rulenames.Insert(0, "Trailer");
			State rule = State.NoCloneAlternation(GetTrailer0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetTransfer_Encoding(List<string> rulenames)
		{
			rulenames.Insert(0, "Transfer-Encoding");
			State rule = State.NoCloneAlternation(GetTransfer_Encoding0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetUpgrade(List<string> rulenames)
		{
			rulenames.Insert(0, "Upgrade");
			State rule = State.NoCloneAlternation(GetUpgrade0(rulenames));
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
		public State GetVary(List<string> rulenames)
		{
			rulenames.Insert(0, "Vary");
			State rule = State.NoCloneAlternation(GetVary0(rulenames));
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
		public State Getreceived_protocol(List<string> rulenames)
		{
			rulenames.Insert(0, "received-protocol");
			State rule = State.NoCloneAlternation(Getreceived_protocol0(rulenames));
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
		public State Getreceived_by(List<string> rulenames)
		{
			rulenames.Insert(0, "received-by");
			State rule = State.NoCloneAlternation(Getreceived_by0(rulenames));
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
		public State Getwarn_date(List<string> rulenames)
		{
			rulenames.Insert(0, "warn-date");
			State rule = State.NoCloneAlternation(Getwarn_date0(rulenames));
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
		public State Getauth_scheme(List<string> rulenames)
		{
			rulenames.Insert(0, "auth-scheme");
			State rule = State.NoCloneAlternation(Getauth_scheme0(rulenames));
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
		public State Getcredentials(List<string> rulenames)
		{
			rulenames.Insert(0, "credentials");
			State rule = State.NoCloneAlternation(Getcredentials0(rulenames), Getcredentials1(rulenames), Getcredentials2(rulenames));
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
		public State Getbasic_credentials(List<string> rulenames)
		{
			rulenames.Insert(0, "basic-credentials");
			State rule = State.NoCloneAlternation(Getbasic_credentials0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbase64_user_pass(List<string> rulenames)
		{
			rulenames.Insert(0, "base64-user-pass");
			State rule = State.NoCloneAlternation(Getbase64_user_pass0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getuser_pass(List<string> rulenames)
		{
			rulenames.Insert(0, "user-pass");
			State rule = State.NoCloneAlternation(Getuser_pass0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getuserid(List<string> rulenames)
		{
			rulenames.Insert(0, "userid");
			State rule = State.NoCloneAlternation(Getuserid0(rulenames));
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
		public State Getdigest_challenge(List<string> rulenames)
		{
			rulenames.Insert(0, "digest-challenge");
			State rule = State.NoCloneAlternation(Getdigest_challenge0(rulenames));
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
		public State Getdigest_response(List<string> rulenames)
		{
			rulenames.Insert(0, "digest-response");
			State rule = State.NoCloneAlternation(Getdigest_response0(rulenames));
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
		public State Getresponse(List<string> rulenames)
		{
			rulenames.Insert(0, "response");
			State rule = State.NoCloneAlternation(Getresponse0(rulenames));
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
		public State GetLHEX(List<string> rulenames)
		{
			rulenames.Insert(0, "LHEX");
			State rule = State.NoCloneAlternation(GetLHEX0(rulenames), GetLHEX1(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetAuthenticationInfo(List<string> rulenames)
		{
			rulenames.Insert(0, "AuthenticationInfo");
			State rule = State.NoCloneAlternation(GetAuthenticationInfo0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getauth_info(List<string> rulenames)
		{
			rulenames.Insert(0, "auth-info");
			State rule = State.NoCloneAlternation(Getauth_info0(rulenames));
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
		public State Getset_cookie_header(List<string> rulenames)
		{
			rulenames.Insert(0, "set-cookie-header");
			State rule = State.NoCloneAlternation(Getset_cookie_header0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getset_cookie_string(List<string> rulenames)
		{
			rulenames.Insert(0, "set-cookie-string");
			State rule = State.NoCloneAlternation(Getset_cookie_string0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcookie_pair(List<string> rulenames)
		{
			rulenames.Insert(0, "cookie-pair");
			State rule = State.NoCloneAlternation(Getcookie_pair0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcookie_name(List<string> rulenames)
		{
			rulenames.Insert(0, "cookie-name");
			State rule = State.NoCloneAlternation(Getcookie_name0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcookie_value(List<string> rulenames)
		{
			rulenames.Insert(0, "cookie-value");
			State rule = State.NoCloneAlternation(Getcookie_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcookie_octet(List<string> rulenames)
		{
			rulenames.Insert(0, "cookie-octet");
			State rule = State.NoCloneAlternation(Getcookie_octet0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcookie_av(List<string> rulenames)
		{
			rulenames.Insert(0, "cookie-av");
			State rule = State.NoCloneAlternation(Getcookie_av0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getexpires_av(List<string> rulenames)
		{
			rulenames.Insert(0, "expires-av");
			State rule = State.NoCloneAlternation(Getexpires_av0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsane_cookie_date(List<string> rulenames)
		{
			rulenames.Insert(0, "sane-cookie-date");
			State rule = State.NoCloneAlternation(Getsane_cookie_date0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmax_age_av(List<string> rulenames)
		{
			rulenames.Insert(0, "max-age-av");
			State rule = State.NoCloneAlternation(Getmax_age_av0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getnon_zero_digit(List<string> rulenames)
		{
			rulenames.Insert(0, "non-zero-digit");
			State rule = State.NoCloneAlternation(Getnon_zero_digit0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdomain_av(List<string> rulenames)
		{
			rulenames.Insert(0, "domain-av");
			State rule = State.NoCloneAlternation(Getdomain_av0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getdomain_value(List<string> rulenames)
		{
			rulenames.Insert(0, "domain-value");
			State rule = State.NoCloneAlternation(Getdomain_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpath_av(List<string> rulenames)
		{
			rulenames.Insert(0, "path-av");
			State rule = State.NoCloneAlternation(Getpath_av0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getpath_value(List<string> rulenames)
		{
			rulenames.Insert(0, "path-value");
			State rule = State.NoCloneAlternation(Getpath_value0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getsecure_av(List<string> rulenames)
		{
			rulenames.Insert(0, "secure-av");
			State rule = State.NoCloneAlternation(Getsecure_av0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Gethttponly_av(List<string> rulenames)
		{
			rulenames.Insert(0, "httponly-av");
			State rule = State.NoCloneAlternation(Gethttponly_av0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_av(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-av");
			State rule = State.NoCloneAlternation(Getextension_av0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getexcept_CTL_semi(List<string> rulenames)
		{
			rulenames.Insert(0, "except-CTL-semi");
			State rule = State.NoCloneAlternation(Getexcept_CTL_semi0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcookie_header(List<string> rulenames)
		{
			rulenames.Insert(0, "cookie-header");
			State rule = State.NoCloneAlternation(Getcookie_header0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getcookie_string(List<string> rulenames)
		{
			rulenames.Insert(0, "cookie-string");
			State rule = State.NoCloneAlternation(Getcookie_string0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSec_WebSocket_Key(List<string> rulenames)
		{
			rulenames.Insert(0, "Sec-WebSocket-Key");
			State rule = State.NoCloneAlternation(GetSec_WebSocket_Key0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSec_WebSocket_Extensions(List<string> rulenames)
		{
			rulenames.Insert(0, "Sec-WebSocket-Extensions");
			State rule = State.NoCloneAlternation(GetSec_WebSocket_Extensions0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSec_WebSocket_Protocol_Client(List<string> rulenames)
		{
			rulenames.Insert(0, "Sec-WebSocket-Protocol-Client");
			State rule = State.NoCloneAlternation(GetSec_WebSocket_Protocol_Client0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSec_WebSocket_Version_Client(List<string> rulenames)
		{
			rulenames.Insert(0, "Sec-WebSocket-Version-Client");
			State rule = State.NoCloneAlternation(GetSec_WebSocket_Version_Client0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbase64_value_non_empty(List<string> rulenames)
		{
			rulenames.Insert(0, "base64-value-non-empty");
			State rule = State.NoCloneAlternation(Getbase64_value_non_empty0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbase64_data(List<string> rulenames)
		{
			rulenames.Insert(0, "base64-data");
			State rule = State.NoCloneAlternation(Getbase64_data0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbase64_padding(List<string> rulenames)
		{
			rulenames.Insert(0, "base64-padding");
			State rule = State.NoCloneAlternation(Getbase64_padding0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getbase64_character(List<string> rulenames)
		{
			rulenames.Insert(0, "base64-character");
			State rule = State.NoCloneAlternation(Getbase64_character0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_list(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-list");
			State rule = State.NoCloneAlternation(Getextension_list0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension(List<string> rulenames)
		{
			rulenames.Insert(0, "extension");
			State rule = State.NoCloneAlternation(Getextension0(rulenames));
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
		public State Getregistered_token(List<string> rulenames)
		{
			rulenames.Insert(0, "registered-token");
			State rule = State.NoCloneAlternation(Getregistered_token0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_param(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-param");
			State rule = State.NoCloneAlternation(Getextension_param0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetNZDIGIT(List<string> rulenames)
		{
			rulenames.Insert(0, "NZDIGIT");
			State rule = State.NoCloneAlternation(GetNZDIGIT0(rulenames));
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
		public State GetSec_WebSocket_Accept(List<string> rulenames)
		{
			rulenames.Insert(0, "Sec-WebSocket-Accept");
			State rule = State.NoCloneAlternation(GetSec_WebSocket_Accept0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSec_WebSocket_Protocol_Server(List<string> rulenames)
		{
			rulenames.Insert(0, "Sec-WebSocket-Protocol-Server");
			State rule = State.NoCloneAlternation(GetSec_WebSocket_Protocol_Server0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State GetSec_WebSocket_Version_Server(List<string> rulenames)
		{
			rulenames.Insert(0, "Sec-WebSocket-Version-Server");
			State rule = State.NoCloneAlternation(GetSec_WebSocket_Version_Server0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getmailbox(List<string> rulenames)
		{
			rulenames.Insert(0, "mailbox");
			State rule = State.NoCloneAlternation(Getmailbox0(rulenames));
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
		public State Getroute_addr(List<string> rulenames)
		{
			rulenames.Insert(0, "route-addr");
			State rule = State.NoCloneAlternation(Getroute_addr0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getroute(List<string> rulenames)
		{
			rulenames.Insert(0, "route");
			State rule = State.NoCloneAlternation(Getroute0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getphrase(List<string> rulenames)
		{
			rulenames.Insert(0, "phrase");
			State rule = State.NoCloneAlternation(Getphrase0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
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
		public State GetSWS(List<string> rulenames)
		{
			rulenames.Insert(0, "SWS");
			State rule = State.NoCloneAlternation(GetSWS0(rulenames));
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
		public State GetHCOLON(List<string> rulenames)
		{
			rulenames.Insert(0, "HCOLON");
			State rule = State.NoCloneAlternation(GetHCOLON0(rulenames));
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
		public State Getreq(List<string> rulenames)
		{
			rulenames.Insert(0, "req");
			State rule = State.NoCloneAlternation(Getreq0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_header_marked_only(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-header-marked-only");
			State rule = State.NoCloneAlternation(Getextension_header_marked_only0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getextension_header_for_request(List<string> rulenames)
		{
			rulenames.Insert(0, "extension-header-for-request");
			State rule = State.NoCloneAlternation(Getextension_header_for_request0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getgeneral_header_names(List<string> rulenames)
		{
			rulenames.Insert(0, "general-header-names");
			State rule = State.NoCloneAlternation(Getgeneral_header_names0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getrequest_header_names(List<string> rulenames)
		{
			rulenames.Insert(0, "request-header-names");
			State rule = State.NoCloneAlternation(Getrequest_header_names0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
		public State Getentity_header_names(List<string> rulenames)
		{
			rulenames.Insert(0, "entity-header-names");
			State rule = State.NoCloneAlternation(Getentity_header_names0(rulenames));
			rule = OnMarkRule(rule, rulenames);
			rulenames.RemoveAt(0);
			return rule;
		}
	}
}
