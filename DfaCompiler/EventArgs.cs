using System;
using System.Collections.Generic;
using Fsm;

namespace DfaCompiler
{
	public class MarkRuleEventArgs : EventArgs
	{
		public MarkRuleEventArgs(State start, List<string> rulenames)
		{
			Start = start;
			Rulenames = rulenames;
		}
		public State Start { get; set; }
		public List<string> Rulenames { get; private set; }
		public string GetRulesPath(string separator)
		{
			string result = "";
			for (int i = Rulenames.Count - 1; i >= 0; i--)
			{
				if (result != "")
					result += separator;
				result += Rulenames[i];
			}
			return result;
		}
		public string GetRulesPath()
		{
			return GetRulesPath(".");
		}
	}

	public class ChangeRuleEventArgs : EventArgs
	{
		public ChangeRuleEventArgs(State[] states, List<string> rulenames)
		{
			States = states;
			Rulenames = rulenames;
		}
		public State[] States { get; set; }
		public List<string> Rulenames { get; private set; }
	}
}
