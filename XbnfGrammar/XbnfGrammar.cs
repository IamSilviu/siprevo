using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace XbnfGrammar1
{
	public class XbnfGrammar
		: Grammar
	{
		public enum Mode
		{
			Strict,
			HttpCompatible,
		}

		private Dictionary<string, int> _partNumber;
		private Mode mode;

		public XbnfGrammar()
			: this(Mode.HttpCompatible)
		{
		}

		public XbnfGrammar(Mode mode)
		{
			this.mode = mode;

			var alternationChar = (mode == Mode.Strict) ? "/" : "|";

			var rulename = new IdentifierTerminal("rulename", "-_", null);
			var funcname = new IdentifierTerminal("funcname", ".", null);
			var newrulename = new IdentifierTerminal("newrulename", "-_", null);
			var comment = new CommentTerminal("comment", "/*", "*/");
			var bindig1 = new NumberLiteral("bindig", NumberOptions.Binary | NumberOptions.IntOnly);
			var bindig2 = new NumberLiteral("bindig", NumberOptions.Binary | NumberOptions.IntOnly);
			var hexdig1 = new NumberLiteral("hexdig", NumberOptions.Hex | NumberOptions.IntOnly);
			var hexdig2 = new NumberLiteral("hexdig", NumberOptions.Hex | NumberOptions.IntOnly);
			var decdig1 = new NumberLiteral("decvalue", NumberOptions.IntOnly);
			var decdig2 = new NumberLiteral("decvalue", NumberOptions.IntOnly);
			var charval = new StringLiteral("charval", "\"", StringOptions.NoEscapes);
			var repeat1 = new NumberLiteral("repeat1", NumberOptions.IntOnly);
			var repeat2 = new NumberLiteral("repeat2", NumberOptions.IntOnly);
			var minus = ToTerm("-", "minus");
			var point = ToTerm(".", "point");

			bindig1.AddPrefix("b", NumberOptions.None);
			hexdig1.AddPrefix("x", NumberOptions.None);
			decdig1.AddPrefix("d", NumberOptions.None);

			base.NonGrammarTerminals.Add(comment);


			// NON TERMINALS
			var numval = new NonTerminal("numval");

			var hexval = new NonTerminal("hexval");
			var hexvalp = new NonTerminal("hexvalpoint");
			var hexvalps = new NonTerminal("hexvalpointstar");

			var binval = new NonTerminal("binval");
			var binvalp = new NonTerminal("binvalpoint");
			var binvalps = new NonTerminal("binvalpointstar");

			var decval = new NonTerminal("decval");
			var decvalp = new NonTerminal("decvalpoint");
			var decvalps = new NonTerminal("decvalpointstar");

			var rule = new NonTerminal("rule");
			var rulelist = new NonTerminal("rulelist");
			var alternation = new NonTerminal("alternation");
			var concatenation = new NonTerminal("concatenation");
			var substraction = new NonTerminal("substraction");
			var repetition = new NonTerminal("repetition");
			var repeat = new NonTerminal("repeat");
			var element = new NonTerminal("element");
			var elements = new NonTerminal("elements");
			var group = new NonTerminal("group");
			var option = new NonTerminal("option");
			var func = new NonTerminal("func");
			var funcarg = new NonTerminal("funcarg");
			var funcargs = new NonTerminal("funcargs");



			// RULES
			hexval.Rule = hexdig1 + (hexvalps | (minus + hexdig2) | Empty);
			hexvalp.Rule = point + hexdig2;
			hexvalps.Rule = MakePlusRule(hexvalps, hexvalp);

			binval.Rule = bindig1 + (binvalps | (minus + bindig2) | Empty);
			binvalp.Rule = point + bindig2;
			binvalps.Rule = MakePlusRule(binvalps, binvalp);

			decval.Rule = decdig1 + (decvalps | (minus + decdig2) | Empty);
			decvalp.Rule = point + decdig2;
			decvalps.Rule = MakePlusRule(decvalps, decvalp);

			numval.Rule = ToTerm("%") + (binval | hexval | decval);

			BnfExpression rp = ToTerm("*");
			if (mode == Mode.HttpCompatible)
				rp = rp | "#";

			repeat.Rule = ((repeat1) | ((repeat1 | Empty) + rp + (repeat2 | Empty)));
			group.Rule = ToTerm("(") + alternation + ")";
			option.Rule = ToTerm("[") + alternation + "]";

			funcarg.Rule = alternation;
			funcargs.Rule = MakePlusRule(funcargs, ToTerm(","), funcarg);
			func.Rule = ToTerm("{") + funcname + "," + funcargs + "}";

			//alternation.Rule = MakePlusRule(alternation, ToTerm(alternationChar), concatenation);
			//concatenation.Rule = MakePlusRule(concatenation, repetition);
			alternation.Rule = MakePlusRule(alternation, ToTerm(alternationChar), substraction);
			substraction.Rule = MakePlusRule(substraction, ToTerm("&!"), concatenation);
			//concatenation + ((ToTerm("--") + concatenation) | Empty);
			concatenation.Rule = MakePlusRule(concatenation, repetition);

			repetition.Rule = (Empty | repeat) + element;
			element.Rule = rulename | group | option | numval | charval | func;

			elements.Rule = alternation;
			rule.Rule = newrulename + (ToTerm("=") | ToTerm("=" + alternationChar)) + elements + ";";
			rulelist.Rule = MakeStarRule(rulelist, rule);

			base.Root = rulelist;
			base.LanguageFlags |= LanguageFlags.CanRunSample;
		}

		public override string RunSample(ParseTree parseTree)
		{
			//CreateDefinedRulesList(parseTree.Root, defined);
			//CreateUsedRulesList(parseTree.Root, used);
			//return "Not-Defined:\r\n" + ListToString(used.Except(defined)) 
			//    + "\r\nNot-Used:\r\n" + ListToString(defined.Except(used));

			_partNumber = new Dictionary<string, int>();

			return CreateNfaRules(parseTree.Root);
		}

		public string CreateNfaRules(ParseTreeNode node)
		{
			string result = "";
			foreach (var child in node.ChildNodes)
				result += CreateNfaRulePart(child);
			foreach (var child in node.ChildNodes)
				result += CreateNfaRule(child);
			return result;
		}

		public string CreateNfaRule(ParseTreeNode node)
		{
			string ruleName = node.FindTokenAndGetText();

			if (_partNumber.ContainsKey(ruleName))
			{
				string parts = "";
				int number = _partNumber[ruleName];
				for (int i = 0; i <= number; i++)
				{
					if (parts != "")
						parts += ", ";
					parts += "Get" + ruleName.Replace('-', '_') + i + "(rulenames)";
				}

				_partNumber.Remove(ruleName);

				return "\t\tpublic State Get" + ruleName.Replace('-', '_') + "(List<string> rulenames)\r\n" +
					"\t\t{\r\n" +
					"\t\t\trulenames.Insert(0, \"" + node.FindTokenAndGetText() + "\");\r\n" +
					"\t\t\tState rule = State.NoCloneAlternation(" + parts + ");\r\n" +
					"\t\t\trule = OnMarkRule(rule, rulenames);\r\n" +
					"\t\t\trulenames.RemoveAt(0);\r\n" +
					"\t\t\treturn rule;\r\n" +
					"\t\t}\r\n";
			}

			return "";
		}

		public string CreateNfaRulePart(ParseTreeNode node)
		{
			ParseTreeNode elements = null;
			foreach (var child in node.ChildNodes)
				if (child.Term.Name == "elements")
					elements = child;

			var nodeName = node.FindTokenAndGetText();
			int number = 0;
			if (_partNumber.ContainsKey(nodeName))
				number = _partNumber[nodeName] + 1;
			_partNumber[nodeName] = number;

			return "\t\tpublic State Get" + node.FindTokenAndGetText().Replace('-', '_') + number + "(List<string> rulenames)\r\n" +
				"\t\t{\r\n" +
				"\t\t\tState rule = " + CreateAlternation(elements.ChildNodes[0], true) + ";\r\n" +
				"\t\t\treturn rule;\r\n" +
				"\t\t}\r\n";
		}

		//public string CreateNfaRule(ParseTreeNode node)
		//{
		//    ParseTreeNode elements = null;
		//    foreach (var child in node.ChildNodes)
		//        if (child.Term.Name == "elements")
		//            elements = child;

		//    var nodeName = node.FindTokenAndGetText();
		//    int number = 0;
		//    if (_partNumber.ContainsKey(nodeName))
		//        number = _partNumber[nodeName] + 1;
		//    _partNumber[nodeName] = number;

		//    return "\t\tpublic State Get" + node.FindTokenAndGetText().Replace('-', '_') + number + "(List<string> rulenames)\r\n" +
		//        "\t\t{\r\n" +
		//        "\t\t\trulenames.Insert(0, \"" + node.FindTokenAndGetText() + "\");\r\n" +
		//        "\t\t\tState rule = " + CreateAlternation(elements.ChildNodes[0]) + ";\r\n" +
		//        "\t\t\trule = OnMarkRule(rule, rulenames);\r\n" +
		//        "\t\t\trulenames.RemoveAt(0);\r\n" +
		//        "\t\t\treturn rule;\r\n" +
		//        "\t\t}\r\n";
		//}

		public string CreateAlternation(ParseTreeNode node, bool change)
		{
			int count = 0;
			string result = "";
			foreach (var child in node.ChildNodes)
			{
				if (result != "")
					result += ",";
				//result += CreateConcatanation(child, change);
				result += CreateSubstraction(child, change);
				count++;
			}

			if (count > 1)
				result = "State.NoCloneAlternation(" + result + ")";

			return result;
		}

		public class Optimization
		{
			public string Path = "";
			public List<string> Rulenames = new List<string>();
		}

		public string CreateSubstraction(ParseTreeNode node, bool change)
		{
			List<string> items = new List<string>();
			foreach (var child in node.ChildNodes)
				items.Add(CreateConcatanation(child, change));

			if (items.Count == 1)
				return items[0];

			if (items.Count == 2)
				return "State.Substract(" + items[0] + "," + items[1] + ")";

			throw new NotImplementedException();
			//return CreateConcatanation(node.FirstChild, change);
		}

		public string CreateConcatanation(ParseTreeNode node, bool change)
		{
			List<string> items = new List<string>();

			foreach (var child in node.ChildNodes)
				items.Add(CreateRepeation(child));

			string result = "";
			foreach (var item in items)
			{
				if (result != "")
					result += ",";
				result += item;
			}

			if (items.Count > 1)
			{
				if (change)
					result = "State.NoCloneConcatanation(OnChangeConcatanation(rulenames," + result + "))";
				else
					result = "State.NoCloneConcatanation(" + result + ")";
			}

			return result;
		}

		public string CreateRepeation(ParseTreeNode node)
		{
			if (node.ChildNodes[0].ChildNodes.Count == 0)
				return CreateElement(node.ChildNodes[1]);

			return CreateRepeat(node);
		}

		public string CreateRepeat(ParseTreeNode node)
		{
			string repeatChar = "*";
			int repeat1 = -1, repeat2 = -1;
			var repeat = node.ChildNodes[0].ChildNodes[0];

			if (repeat.ChildNodes.Count == 1)
			{
				repeat2 = repeat1 = int.Parse(repeat.ChildNodes[0].FindTokenAndGetText());
			}
			else if (repeat.ChildNodes.Count >= 3)
			{
				if (repeat.ChildNodes[0].ChildNodes.Count != 0)
					repeat1 = int.Parse(repeat.ChildNodes[0].FindTokenAndGetText());

				repeatChar = repeat.ChildNodes[1].FindTokenAndGetText();

				if (repeat.ChildNodes[2].ChildNodes.Count != 0)
					repeat2 = int.Parse(repeat.ChildNodes[2].FindTokenAndGetText());
			}

			string result = null;
			if (repeatChar == "*")
			{
				result = "State.Repeat(" + repeat1 + "," + repeat2 + "," + CreateElement(node.ChildNodes[1]) + ")";
			}
			else if (repeatChar == "#")
			{
				if (repeat2 != -1)
					throw new NotImplementedException();

				result = "State.NoCloneRepeatBy(" + CreateElement(node.ChildNodes[1]) + ", GetCOMMA(rulenames))";

				if (repeat1 <= 0)
					result = "State.NoCloneOption(" + result + ")";

				if (repeat1 > 1)
					result = "State.Repeat(" + repeat1 + "," + repeat2 + "," + result + ")";
			}
			else
				throw new NotImplementedException();

			return result;
		}

		public string CreateElement(ParseTreeNode node)
		{
			switch (node.ChildNodes[0].Term.Name)
			{
				case "rulename":
					return "Get" + node.ChildNodes[0].FindTokenAndGetText().Replace('-', '_') + "(rulenames)";
				case "group":
					return "(" + CreateAlternation(node.ChildNodes[0].ChildNodes[1], false) + ")";
				case "option":
					return "State.NoCloneOption(" + CreateAlternation(node.ChildNodes[0].ChildNodes[1], false) + ")";
				case "numval":
					return CreateNumval(node.ChildNodes[0]);
				case "charval":
					return CreateCharval(node.ChildNodes[0]);
				case "func":
					return CreateFunc(node.ChildNodes[0]);
			}

			throw new InvalidProgramException();
		}

		public string CreateNumval(ParseTreeNode node)
		{
			if (node.ChildNodes[1].FirstChild.Term.Name == "hexval")
			{
				var hexval = node.ChildNodes[1].FirstChild;

				var hexdig1 = "0" + hexval.FirstChild.FindTokenAndGetText();
				if (hexval.ChildNodes.Count == 1)
					return hexdig1;

				var op = hexval.ChildNodes[1].FindTokenAndGetText();

				if (op == "-")
					return "(" + hexdig1 + ".To(0x" +
						hexval.ChildNodes[1].LastChild.FindTokenAndGetText() + "))";

				if (op == ".")
				{
					var hexvalpointstart = hexval.ChildNodes[1].FirstChild;

					string arr = hexdig1 + ",";
					foreach (var hexval1 in hexvalpointstart.ChildNodes)
						arr += "0x" + hexval1.LastChild.FindTokenAndGetText() + ",";

					return "(State)(new byte[] {" + arr + "})";
				}

				return hexdig1;
			}

			throw new InvalidProgramException("Dec and Binary const not implemented");
		}

		public string CreateCharval(ParseTreeNode node)
		{
			return "FromString(" + node.FindTokenAndGetText().Replace("\\", "\\\\") + ",rulenames)";
		}

		public string CreateFunc(ParseTreeNode node)
		{
			string args = "";

			foreach (var funcarg in node.ChildNodes[3].ChildNodes)
			{
				if (args != "")
					args += ",";
				args += CreateAlternation(funcarg.ChildNodes[0], false);
			}

			return node.ChildNodes[1].FindTokenAndGetText() + "(" + args + ")";
		}

		//-----------------------------------------

		public void CreateDefinedRulesList(ParseTreeNode node, List<string> rulenames)
		{
			foreach (var child in node.ChildNodes)
				CreateDefinedRulesList(child, rulenames);

			if (node.Term.Name == "newrulename")
				rulenames.Add(node.FindTokenAndGetText());
		}

		public void CreateUsedRulesList(ParseTreeNode node, List<string> rulenames)
		{
			foreach (var child in node.ChildNodes)
				CreateUsedRulesList(child, rulenames);

			if (node.Term.Name == "rulename")
			{
				var rulename = node.FindTokenAndGetText();

				if (rulenames.Find((fromList) => { return fromList == rulename; }) == null)
					rulenames.Add(node.FindTokenAndGetText());
			}
		}

		private string ListToString(IEnumerable<string> list)
		{
			int count = 0;
			string result = "------------\r\n";
			foreach (var rulename in list)
			{
				result += rulename + "\r\n";
				count++;
			}

			result += "------------\r\n";
			result += string.Format("Count: {0}\r\n", count);

			return result;
		}

		public string ValidateXbnf(ParseTreeNode node)
		{
			foreach (var child in node.ChildNodes)
				ValidateXbnf(child);

			return "Ok";
		}
	}
}
