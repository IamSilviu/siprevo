using System;
using System.Collections.Generic;
using Fsm;

namespace DfaCompiler
{
	public interface INfaGenerator
	{
		State GetNfaRoot();
		event EventHandler<MarkRuleEventArgs> MarkRule;
	}
}
