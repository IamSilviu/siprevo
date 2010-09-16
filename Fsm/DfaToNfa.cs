using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fsm
{
	public static class DfaToNfa
	{
		public static State ToNfa(this DfaState dfaStart)
		{
			int total = 0;
			dfaStart.ForEach((state) => { state.Index = total++; });

			var states = new State[total];
			for (int i = 0; i < states.Length; i++)
				states[i] = new State();

			dfaStart.ForEach((dfaState) =>
				{
					var start = states[dfaState.Index];
					var end = start;

					if (dfaState.AllMarks.Count > 0)
					{
						end = new State();
						foreach (var mark in dfaState.AllMarks)
						{
							var state = new State();
							state.CopyIMarkFrom(mark);
							state.Transition.Add(State.Epsilon, end);
							start.Transition.Add(State.Epsilon, state);
						}
					}

					for (int i = 0; i < dfaState.Transition.Length; i++)
						if (dfaState.Transition[i] != null)
							end.Transition.Add((byte)i, states[dfaState.Transition[i].Index]);
				});

			return states[dfaStart.Index];
		}
	}
}
