using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using XbnfGrammar1;
using Irony.Parsing;

namespace XbnfParser
{
	class Program
	{
		static int Main(string[] args)
		{
			try
			{
				bool mode2 = (((args.Length >= 3) ? args[2] : "") == "mode2");

				Console.WriteLine("Create grammar");
				var grammar = new XbnfGrammar(mode2 ? XbnfGrammar.Mode.HttpCompatible : XbnfGrammar.Mode.Strict);

				Console.WriteLine("Create parser");
				var parser = new Parser(grammar);

				Console.WriteLine("Read XBNF from {0}", args[0]);
				var xbnf = File.ReadAllText(args[0]);

				Console.WriteLine("Optimize");
				var oprimized = Optimize(xbnf);

				Console.WriteLine("Parse");
				var tree = parser.Parse(oprimized, "<source>");

				Console.WriteLine("Convert to C#");
				var csharp = grammar.RunSample(tree);

				Console.WriteLine("Write C# to {0}", args[1]);
				File.WriteAllText(args[1], AddHeaderFooter(csharp));
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
				return -1;
			}
			return 0;
		}

		static string Optimize(string xbnf)
		{
			var repeatBy = new Regex(@"(?<item>[A-Za-z0-9\-_]+)\s+\*\((?<separator>[A-Za-z0-9\-_]+)\s+\k<item>\)");

			return repeatBy.Replace(xbnf, "{State.NoCloneRepeatBy, ${item}, ${separator}}");
		}

		static string AddHeaderFooter(string source)
		{
			return
				"using System;\r\n" +
				"using System.Collections.Generic;\r\n" +
				"using Fsm;\r\n" +
				"\r\n" +
				"namespace DfaCompiler\r\n" +
				"{\r\n" +
				//"	class MarkRuleEventArgs: EventArgs\r\n" +
				//"	{\r\n" +
				//"		public MarkRuleEventArgs(State start, List<string> rulenames)\r\n" +
				//"		{\r\n" +
				//"			Start = start;\r\n" +
				//"			Rulenames = rulenames;\r\n" +
				//"		}\r\n" +
				//"		public State Start { get; set; }\r\n" +
				//"		public List<string> Rulenames { get; private set; }\r\n" +
				//"		public string GetRulesPath(string separator)\r\n" +
				//"		{\r\n" +
				//"			string result = \"\";\r\n" +
				//"			for (int i = Rulenames.Count - 1; i >= 0; i--)\r\n" +
				//"			{\r\n" +
				//"				if (result != \"\")\r\n" +
				//"					result += separator;\r\n" +
				//"				result += Rulenames[i];\r\n" +
				//"			}\r\n" +
				//"			return result;\r\n" +
				//"		}\r\n" +
				//"		public string GetRulesPath()\r\n" +
				//"		{\r\n" +
				//"			return GetRulesPath(\".\");\r\n" +
				//"		}\r\n" +
				//"	}\r\n" +
				//"	class ChangeRuleEventArgs: EventArgs\r\n" +
				//"	{\r\n" +
				//"		public ChangeRuleEventArgs(State[] states, List<string> rulenames)\r\n" +
				//"		{\r\n" +
				//"			States = states;\r\n" +
				//"			Rulenames = rulenames;\r\n" +
				//"		}\r\n" +
				//"		public State[] States { get; set; }\r\n" +
				//"		public List<string> Rulenames { get; private set; }\r\n" +
				//"	}\r\n" +
				"	class GeneratedXbnf\r\n" +
				"	{\r\n" +
				"		public event EventHandler<MarkRuleEventArgs> MarkRule;\r\n" +
				"		public event EventHandler<ChangeRuleEventArgs> ChangeConcatanation;\r\n" +
				"		private State OnMarkRule(State start, List<string> rulenames)\r\n" +
				"		{\r\n" +
				"			if (MarkRule != null)\r\n" +
				"			{\r\n" +
				"				var args = new MarkRuleEventArgs(start, rulenames);\r\n" +
				"				MarkRule(this, args);\r\n" +
				"				return args.Start;\r\n" +
				"			}\r\n" +
				"			return start;\r\n" +
				"		}\r\n" +
				"		private State[] OnChangeConcatanation(List<string> rulenames, params State[] states)\r\n" +
				"		{\r\n" +
				"			if (ChangeConcatanation != null)\r\n" +
				"			{\r\n" +
				"				var args = new ChangeRuleEventArgs(states, rulenames);\r\n" +
				"				ChangeConcatanation(this, args);\r\n" +
				"				return args.States;\r\n" +
				"			}\r\n" +
				"			return states;\r\n" +
				"		}\r\n" +
				"		private State FromString(string charval, List<string> rulenames)\r\n" +
				"		{\r\n" +
				"			rulenames.Insert(0, charval);\r\n" +
				"			State rule = (State)charval;\r\n" +
				"			rule = OnMarkRule(rule, rulenames);\r\n" +
				"			rulenames.RemoveAt(0);\r\n" +
				"			return rule;\r\n" +
				"		}\r\n" +
				source +
				"	}\r\n" +
				"}\r\n";
		}
	}
}
