using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using XbnfGrammar1;
using Irony.Parsing;
using Fsm;

namespace BnfToDfa
{
	class Program
	{
		static int Main(string[] args)
		{
			try
			{
				//api.xbnf.txt api.mark.txt API

				var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\";

				bool mode2 = (((args.Length >= 4) ? args[3] : "") == "mode2");
				var rootRule = args[2];

				Console.WriteLine("Load grammar");
				var grammar = new XbnfGrammar(mode2 ? XbnfGrammar.Mode.HttpCompatible : XbnfGrammar.Mode.Strict);

				Console.WriteLine("Create parser");
				var parser = new Parser(grammar);

				Console.WriteLine("Read XBNF from {0}", args[0]);
				var xbnf = File.ReadAllText(args[0]);

				Console.WriteLine("Optimize");
				var oprimized = Optimize(xbnf);

				Console.WriteLine("Parse");
				var tree = parser.Parse(oprimized, "<source>");
				if (tree == null)
					throw new Exception(@"Failed to parse");

				Console.WriteLine("Build expressions");
				var builder = new Builder(tree);
				builder.BuildExpressions();

				Console.WriteLine("Load marks");
				var marker = new Marker();
				if (args.Length >= 2)
					marker.LoadMarks(path + args[1]);
				//if (args.Length >= 3)
				//    marker.LoadSuppressWarngin(path + args[2]);

				Console.WriteLine("Build NFA");
				var nfa = builder.CreateNfa(rootRule, marker.MarkRuleHandler);
				nfa.MarkFinal();
				Console.WriteLine("Max NFA state id: {0}", Fsm.State.MaxId);

				Console.WriteLine("Check unused rules");
				foreach (var unused in marker.GetUnusedRules())
					Console.WriteLine("UNUSED: {0}", unused);

				Console.WriteLine("Pack NFA");
				PackNfa.Pack(nfa, true);

				int count;
				var dfa = nfa.ToDfa3(out count, true);
				Console.WriteLine("DFA Complied States: {0}", count);

				var minCount = dfa.Minimize(true);
				Console.WriteLine("Minimized DFA States: {0}", minCount);

				Console.WriteLine("Write DFA");
				Writer.Write(dfa, "dfa.xml");

				//Console.WriteLine("Convert to C#");
				//var csharp = grammar.RunSample(tree);

				//Console.WriteLine("Write C# to {0}", args[1]);
				//File.WriteAllText(args[1], AddHeaderFooter(csharp));
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

			return repeatBy.Replace(xbnf, "{RepeatBy, ${item}, ${separator}}");
		}
	}
}
