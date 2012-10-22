using System;
using System.Collections.Generic;
using System.Text;
using Irony.Parsing;
using Fsm;

namespace BnfToDfa
{
	partial class Builder
	{
		private readonly ParseTree tree;
		private readonly Dictionary<string, AlternationExpression> rules;
		private Func<State, RulePath, State> markAction;

		public Builder(ParseTree tree)
		{
			this.tree = tree;

			this.rules = new Dictionary<string, AlternationExpression>();
		}

		public State CreateNfa(string rootName, Func<State, RulePath, State> markAction)
		{
			try
			{
				this.markAction = markAction;

				if (rules.ContainsKey(rootName) == false)
					throw new BuilderException(@"@Builder: root rule not found");

				return rules[rootName].GetNfa(new RulePath(rootName));
			}
			finally
			{
				this.markAction = null;
			}
		}

		protected State OnMarkRule(State start, RulePath path)
		{
			if (markAction != null)
				return markAction(start, path);
			return start;
		}

		public void BuildExpressions()
		{
			foreach (var child in tree.Root.ChildNodes)
				BuildRuleExpressions(child);
		}

		private void BuildRuleExpressions(ParseTreeNode node)
		{
			var ruleName = node.FindTokenAndGetText();
			var rule = rules.ContainsKey(ruleName) ? rules[ruleName] : rules[ruleName] = new AlternationExpression();

			ParseTreeNode elements = null;
			foreach (var child in node.ChildNodes)
				if (child.Term.Name == "elements")
					elements = child;

			rule.Add(
				BuildAlternationExpression(elements.ChildNodes[0]));
		}

		private IExpression BuildExpression(ParseTreeNode node)
		{
			return BuildAlternationExpression(node);
		}

		private IExpression BuildAlternationExpression(ParseTreeNode node)
		{
			int count = node.ChildNodes.Count;

			if (count <= 0)
			{
				throw new BuilderException(node, @"Invalid alternation expression, no child nodes");
			}
			else if (count == 1)
			{
				return BuildSubtractionExpression(node.ChildNodes[0]);
			}
			else
			{
				var alternation = new AlternationExpression();

				foreach (var child in node.ChildNodes)
					alternation.Add(BuildSubtractionExpression(child));

				return alternation;
			}
		}

		private IExpression BuildSubtractionExpression(ParseTreeNode node)
		{
			int count = node.ChildNodes.Count;

			if (count == 1)
			{
				return BuildConcatanationExpression(node.ChildNodes[0]);
			}
			else if (count == 2)
			{
				return new SubtractionExpression(
					BuildConcatanationExpression(node.ChildNodes[0]),
					BuildConcatanationExpression(node.ChildNodes[1]));
			}
			else
			{
				throw new Exception(@"Invalid substraction expression, too many args");
			}
		}

		private IExpression BuildConcatanationExpression(ParseTreeNode node)
		{
			int count = node.ChildNodes.Count;

			if (count <= 0)
			{
				throw new Exception(@"Invalid concatanation rule, no child nodes");
			}
			else if (count == 1)
			{
				return BuildRepeationExpression(node.ChildNodes[0]);
			}
			else
			{
				var concatanation = new ConcatanationExpression();

				foreach (var child in node.ChildNodes)
					concatanation.Add(BuildRepeationExpression(child));

				return concatanation;
			}
		}

		private IExpression BuildRepeationExpression(ParseTreeNode node)
		{
			if (node.ChildNodes[0].ChildNodes.Count == 0)
			{
				return BuildElementExpression(node.ChildNodes[1]);
			}
			else
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

				switch (repeatChar)
				{
					case "*":
						return new RepeationExpression(repeat1, repeat2, BuildElementExpression(node.ChildNodes[1]));

					case "#":
						if (repeat2 != -1)
							throw new NotImplementedException();
						return new RepeationByExpression(repeat1, repeat2, BuildElementExpression(node.ChildNodes[1]), BuildRuleLinkExpression("COMMA"));

					default:
						throw new NotImplementedException();
				}
			}
		}

		private IExpression BuildElementExpression(ParseTreeNode node)
		{
			switch (node.ChildNodes[0].Term.Name)
			{
				case "rulename":
					return BuildRuleLinkExpression(node.ChildNodes[0].FindTokenAndGetText());
				case "group":
					return BuildExpression(node.ChildNodes[0].ChildNodes[1]);
				case "option":
					return new OptionExpression(BuildExpression(node.ChildNodes[0].ChildNodes[1]));
				case "numval":
					return BuildNumvalExpression(node.ChildNodes[0]);
				case "charval":
					return BuildCharvalExpression(node.ChildNodes[0]);
				case "func":
					return BuildFuncCallExpression(node.ChildNodes[0]);
				default:
					throw new InvalidProgramException();
			}
		}

		private IExpression BuildRuleLinkExpression(string name)
		{
			return new RuleLinkExpression(this, name, rules);
		}

		private IExpression BuildNumvalExpression(ParseTreeNode node)
		{
			if (node.ChildNodes[1].FirstChild.Term.Name == "hexval")
			{
				var node2 = node.ChildNodes[1].FirstChild;
				var hex1 = Convert.ToByte("0" + node2.FirstChild.FindTokenAndGetText(), 16);

				//if (node2.ChildNodes.Count == 1)
				//{
				//    return new NumvalExpression(hex1);
				//}
				//else
				// ?????????????????????????????????????????????????????????????

				{
					var operation = node2.ChildNodes[1].FindTokenAndGetText();

					if (operation == "-")
					{
						return new NumvalExpression(hex1,
							Convert.ToByte("0x" + node2.ChildNodes[1].LastChild.FindTokenAndGetText(), 16));
					}
					else if (operation == ".")
					{
						var expression = new NumvalExpression();

						var hexvalpointstart = node2.ChildNodes[1].FirstChild;

						expression.Add(hex1);
						foreach (var hexval1 in hexvalpointstart.ChildNodes)
							expression.Add(Convert.ToByte("0x" + hexval1.LastChild.FindTokenAndGetText(), 16));

						return expression;
					}
					else
					{
						return new NumvalExpression(hex1);
					}
					//else
					//{
					//    throw new SemanticErrorException(node, @"Unknow operation in Numval expression", operation);
					//}
				}
			}

			throw new InvalidProgramException("Dec and Binary const not implemented");
		}

		private IExpression BuildCharvalExpression(ParseTreeNode node)
		{
			var charval = node.FindTokenAndGetText();//.Replace("\\", "\\\\");
			charval = charval.Substring(1, charval.Length - 2);
			return new CharvalExpression(this, charval);
		}

		private IExpression BuildFuncCallExpression(ParseTreeNode node)
		{
			var name = node.ChildNodes[1].FindTokenAndGetText();

			var expression = new FuncCallExpression(name);

			foreach (var argument in node.ChildNodes[3].ChildNodes)
			{
				expression.AddArgument(
					BuildExpression(argument.ChildNodes[0]));
			}

			return expression;
		}

		//public string CreateNfaRules(ParseTreeNode node)
		//{
		//    string result = "";
		//    foreach (var child in node.ChildNodes)
		//        result += CreateNfaRulePart(child);
		//    foreach (var child in node.ChildNodes)
		//        result += CreateNfaRule(child);
		//    return result;
		//}

		//public string CreateNfaRule(ParseTreeNode node)
		//{
		//    string ruleName = node.FindTokenAndGetText();

		//    if (_partNumber.ContainsKey(ruleName))
		//    {
		//        string parts = "";
		//        int number = _partNumber[ruleName];
		//        for (int i = 0; i <= number; i++)
		//        {
		//            if (parts != "")
		//                parts += ", ";
		//            parts += "Get" + ruleName.Replace('-', '_') + i + "(rulenames)";
		//        }

		//        _partNumber.Remove(ruleName);

		//        return "\t\tpublic State Get" + ruleName.Replace('-', '_') + "(List<string> rulenames)\r\n" +
		//            "\t\t{\r\n" +
		//            "\t\t\trulenames.Insert(0, \"" + node.FindTokenAndGetText() + "\");\r\n" +
		//            "\t\t\tState rule = State.NoCloneAlternation(" + parts + ");\r\n" +
		//            "\t\t\trule = OnMarkRule(rule, rulenames);\r\n" +
		//            "\t\t\trulenames.RemoveAt(0);\r\n" +
		//            "\t\t\treturn rule;\r\n" +
		//            "\t\t}\r\n";
		//    }

		//    return "";
		//}

		//public string CreateNfaRulePart(ParseTreeNode node)
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
		//        "\t\t\tState rule = " + CreateAlternation(elements.ChildNodes[0], true) + ";\r\n" +
		//        "\t\t\treturn rule;\r\n" +
		//        "\t\t}\r\n";
		//}

		//public string CreateAlternation(ParseTreeNode node, bool change)
		//{
		//    int count = 0;
		//    string result = "";
		//    foreach (var child in node.ChildNodes)
		//    {
		//        if (result != "")
		//            result += ",";
		//        //result += CreateConcatanation(child, change);
		//        result += CreateSubstraction(child, change);
		//        count++;
		//    }

		//    if (count > 1)
		//        result = "State.NoCloneAlternation(" + result + ")";

		//    return result;
		//}

		//public string CreateSubstraction(ParseTreeNode node, bool change)
		//{
		//    List<string> items = new List<string>();
		//    foreach (var child in node.ChildNodes)
		//        items.Add(CreateConcatanation(child, change));

		//    if (items.Count == 1)
		//        return items[0];

		//    if (items.Count == 2)
		//        return "State.Substract(" + items[0] + "," + items[1] + ")";

		//    throw new NotImplementedException();
		//    //return CreateConcatanation(node.FirstChild, change);
		//}

		//public string CreateConcatanation(ParseTreeNode node, bool change)
		//{
		//    List<string> items = new List<string>();

		//    foreach (var child in node.ChildNodes)
		//        items.Add(CreateRepeation(child));

		//    string result = "";
		//    foreach (var item in items)
		//    {
		//        if (result != "")
		//            result += ",";
		//        result += item;
		//    }

		//    if (items.Count > 1)
		//    {
		//        if (change)
		//            result = "State.NoCloneConcatanation(OnChangeConcatanation(rulenames," + result + "))";
		//        else
		//            result = "State.NoCloneConcatanation(" + result + ")";
		//    }

		//    return result;
		//}

		//public string CreateRepeation(ParseTreeNode node)
		//{
		//    if (node.ChildNodes[0].ChildNodes.Count == 0)
		//        return CreateElement(node.ChildNodes[1]);

		//    return CreateRepeat(node);
		//}

		//public string CreateRepeat(ParseTreeNode node)
		//{
		//    string repeatChar = "*";
		//    int repeat1 = -1, repeat2 = -1;
		//    var repeat = node.ChildNodes[0].ChildNodes[0];

		//    if (repeat.ChildNodes.Count == 1)
		//    {
		//        repeat2 = repeat1 = int.Parse(repeat.ChildNodes[0].FindTokenAndGetText());
		//    }
		//    else if (repeat.ChildNodes.Count >= 3)
		//    {
		//        if (repeat.ChildNodes[0].ChildNodes.Count != 0)
		//            repeat1 = int.Parse(repeat.ChildNodes[0].FindTokenAndGetText());

		//        repeatChar = repeat.ChildNodes[1].FindTokenAndGetText();

		//        if (repeat.ChildNodes[2].ChildNodes.Count != 0)
		//            repeat2 = int.Parse(repeat.ChildNodes[2].FindTokenAndGetText());
		//    }

		//    string result = null;
		//    if (repeatChar == "*")
		//    {
		//        result = "State.Repeat(" + repeat1 + "," + repeat2 + "," + CreateElement(node.ChildNodes[1]) + ")";
		//    }
		//    else if (repeatChar == "#")
		//    {
		//        if (repeat2 != -1)
		//            throw new NotImplementedException();

		//        result = "State.NoCloneRepeatBy(" + CreateElement(node.ChildNodes[1]) + ", GetCOMMA(rulenames))";

		//        if (repeat1 <= 0)
		//            result = "State.NoCloneOption(" + result + ")";

		//        if (repeat1 > 1)
		//            result = "State.Repeat(" + repeat1 + "," + repeat2 + "," + result + ")";
		//    }
		//    else
		//        throw new NotImplementedException();

		//    return result;
		//}

		//public string CreateElement(ParseTreeNode node)
		//{
		//    switch (node.ChildNodes[0].Term.Name)
		//    {
		//        case "rulename":
		//            return "Get" + node.ChildNodes[0].FindTokenAndGetText().Replace('-', '_') + "(rulenames)";
		//        case "group":
		//            return "(" + CreateAlternation(node.ChildNodes[0].ChildNodes[1], false) + ")";
		//        case "option":
		//            return "State.NoCloneOption(" + CreateAlternation(node.ChildNodes[0].ChildNodes[1], false) + ")";
		//        case "numval":
		//            return CreateNumval(node.ChildNodes[0]);
		//        case "charval":
		//            return CreateCharval(node.ChildNodes[0]);
		//        case "func":
		//            return CreateFunc(node.ChildNodes[0]);
		//    }

		//    throw new InvalidProgramException();
		//}

		//public string CreateNumval(ParseTreeNode node)
		//{
		//    if (node.ChildNodes[1].FirstChild.Term.Name == "hexval")
		//    {
		//        var hexval = node.ChildNodes[1].FirstChild;

		//        var hexdig1 = "0" + hexval.FirstChild.FindTokenAndGetText();
		//        if (hexval.ChildNodes.Count == 1)
		//            return hexdig1;

		//        var op = hexval.ChildNodes[1].FindTokenAndGetText();

		//        if (op == "-")
		//            return "(" + hexdig1 + ".To(0x" +
		//                hexval.ChildNodes[1].LastChild.FindTokenAndGetText() + "))";

		//        if (op == ".")
		//        {
		//            var hexvalpointstart = hexval.ChildNodes[1].FirstChild;

		//            string arr = hexdig1 + ",";
		//            foreach (var hexval1 in hexvalpointstart.ChildNodes)
		//                arr += "0x" + hexval1.LastChild.FindTokenAndGetText() + ",";

		//            return "(State)(new byte[] {" + arr + "})";
		//        }

		//        return hexdig1;
		//    }

		//    throw new InvalidProgramException("Dec and Binary const not implemented");
		//}

		//public string CreateCharval(ParseTreeNode node)
		//{
		//    return "FromString(" + node.FindTokenAndGetText().Replace("\\", "\\\\") + ",rulenames)";
		//}

		//public string CreateFunc(ParseTreeNode node)
		//{
		//    string args = "";

		//    foreach (var funcarg in node.ChildNodes[3].ChildNodes)
		//    {
		//        if (args != "")
		//            args += ",";
		//        args += CreateAlternation(funcarg.ChildNodes[0], false);
		//    }

		//    return node.ChildNodes[1].FindTokenAndGetText() + "(" + args + ")";
		//}

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
