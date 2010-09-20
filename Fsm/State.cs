using System;
using System.Collections.Generic;

namespace Fsm
{
	public class State
		: MarkImpl
		, IIndexed
	{
		public static readonly byte? Epsilon = null;

		private Map<byte?, State> _transition;

		public State()
		{
			Indexer<State>.Add(this);

			_transition = new Map<byte?, State>();
		}

		public State(byte? c, State s)
			: this()
		{
			_transition.Add(c, s);
		}

		public State(byte? c, State s1, State s2)
			: this()
		{
			_transition.Add(c, s1);
			_transition.Add(c, s2);
		}

		#region IIndexed

		public Int32 Id
		{
			get;
			private set;
		}

		void IIndexed.SetId(int id)
		{
			Id = id;
		}

		#endregion

		public Map<byte?, State> Transition
		{
			get { return _transition; }
		}

		public static int MaxId
		{
			get { return Indexer<State>.MaxId; }
		}

		#region Legacy

		public bool IsBeginRange
		{
			get { return Mark == Marks.BeginRange; }
			set { if(value) Mark = Marks.BeginRange; }
		}

		public bool IsEndRange
		{
			get { return Mark == Marks.EndRange; }
			set { if (value) Mark = Marks.EndRange; }
		}

		public string RangeName
		{
			get { return Name; }
			set { Name = value; }
		}

		public bool IsConst
		{
			get { return Mark == Marks.Const; }
		}

		public string ConstName
		{
			get { if (IsConst) return Name; else return null; }
			set
			{
				if (string.IsNullOrEmpty(value) == false)
				{
					Name = value;
					Mark = Marks.Const;
				}
			}
		}

		public string ConstValue
		{
			get { return Value; }
			set { Value = value; }
		}

		public int ConstPriority
		{
			get { return Priority; }
			set { Priority = value; }
		}

		public bool IsCount
		{
			get { return Mark == Marks.Count; }
		}

		public string CountName
		{
			get { if (IsCount) return Name; else return null; }
			set
			{
				if (string.IsNullOrEmpty(value) == false)
				{
					Name = value;
					Mark = Marks.Count;
				}
			}
		}

		public int CountMax
		{
			get { return Max; }
			set { Max = value; }
		}

		public bool IsFinal
		{
			get { return Mark == Marks.Final; }
			set { if (value) Mark = Marks.Final; }
		}

		#endregion

		public string Tag
		{
			get;
			set;
		}

		public static void MarkRange(ref State begin1, string name)
		{
			begin1 = MarkRange(begin1, name);
		}

		public static State MarkRange(State begin1, string name)
		{
			var begin2 = new State(Epsilon, begin1);
			begin2.IsBeginRange = true;
			begin2.RangeName = name;

			var end2 = new State();
			end2.IsEndRange = true;
			end2.RangeName = name;

			var end1 = begin1.FindEnd();
			end1.Transition.Add(Epsilon, end2);

			return begin2;
		}

		public void MarkConst(string name, string value, int priority)
		{
			var end2 = new State();
			end2.ConstName = name;
			end2.ConstValue = value;
			end2.ConstPriority = priority;

			var end1 = FindEnd();
			end1.Transition.Add(Epsilon, end2);
		}

		public void MarkDecimal(string name)
		{
			MarkEach(name, Marks.Decimal);
		}

		public void MarkHex(string name)
		{
			MarkEach(name, Marks.Hex);
		}

		public void MarkResetIfInvalid(string name)
		{
			MarkEach(name, Marks.ResetRangeIfInvalid);
		}

		public void MarkReset(string name)
		{
			FindEnd().Transition.Add(Epsilon, 
				new State() { Name = name, Mark = Marks.ResetRange, });
		}

		public void MarkContinueRange(string name)
		{
			MarkEach(name, Marks.ContinueRange);
		}

		protected void MarkEach(string name, Marks markType)
		{
			var eclosure = Eclosure();
			var end = FindEnd();

			ForEach((state) =>
			{
				if (eclosure.Contains(state) == false)
				{
					var mark = new State(Epsilon, state)
					{
						Name = name,
						Mark = markType,
					};

					state.Transition.Add(Epsilon, mark);
				}
			});

			end.Transition.Add(Epsilon, new State());
		}

		public void MarkCount(string name, int max, int default1)
		{
			var end2 = new State();
			end2.CountName = name;
			end2.CountMax = max;
			end2.Default = default1;

			var end1 = FindEnd();
			end1.Transition.Add(Epsilon, end2);
		}

		public void MarkBool(string name)
		{
			var end2 = new State();
			end2.Mark = Marks.Bool;
			end2.Name = name;

			var end1 = FindEnd();
			end1.Transition.Add(Epsilon, end2);
		}

		public static State MarkBeginCount(State begin1, string name, int max)
		{
			return new State(Epsilon, begin1)
			{
				Mark = Marks.Count,
				RangeName = name,
				Max = max,
			};
		}

		public void MarkFinal()
		{
			var end2 = new State()
			{
				Mark = Marks.Final,
			};

			var end1 = FindEnd();
			end1.Transition.Add(Epsilon, end2);
		}

		public State GetNextOne(byte? c)
		{
			foreach (var next in _transition.Get(c))
				return next;
			return null;
		}

		public State Clone()
		{
			return Clone(new Dictionary<int, State>());
		}

		public HashSet<State> FindEnds()
		{
			var ends = new HashSet<State>();
			FindEnds(new HashSet<int>(), ends);
			return ends;
		}

		public State FindEnd()
		{
			foreach (var end in FindEnds())
				return end;
			throw new InvalidProgramException();
		}

		public int Count()
		{
			return Count(new HashSet<int>());
		}

		public HashSet<State> Eclosure()
		{
			var eclosure = new HashSet<State>();
			Eclosure(eclosure);
			return eclosure;
		}

		private HashSet<State> _cachedEclosure;

		internal HashSet<State> GetCachedEclosure()
		{
			if (_cachedEclosure == null)
				_cachedEclosure = Eclosure();
			return _cachedEclosure;
		}

		private void Eclosure(HashSet<State> eclosure)
		{
			eclosure.Add(this);

			foreach (var state in Transition.Get(Epsilon))
				if (eclosure.Contains(state) == false)
					state.Eclosure(eclosure);
		}

		/// <summary>
		/// 
		/// </summary>
		public static State Repeat(int repeat1, int repeat2, State state1)
		{
			if (repeat1 == -1 && repeat2 == -1)
				return -state1;

			if (repeat1 == -1)
				repeat1 = 0;

			State result = new State();
			for (int i = 0; i < repeat1; i++)
				result = result + state1;

			if (repeat2 != -1)
			{
				for (int i = repeat1; i < repeat2; i++)
					result = result + !state1;
			}
			else
			{
				result = result + -state1;
			}

			return result;
		}

		/// <summary>
		/// Example: arg, arg, arg
		/// </summary>
		public static State NoCloneRepeatBy(State item, State separator)
		{
			separator.FindEnd().Transition.Add(State.Epsilon, item);

			var itemEnd = item.FindEnd();
			itemEnd.Transition.Add(State.Epsilon, new State());
			itemEnd.Transition.Add(State.Epsilon, separator);

			return item;
		}

		/// <summary>
		/// [...] rule
		/// </summary>
		public static State operator !(State state)
		{
			return NoCloneOption(state.Clone());
		}

		public static State NoCloneOption(State start)
		{
			var end = new State();
			var oldEnd = start.FindEnd();

			oldEnd.Transition.Add(Epsilon, end);
			start.Transition.Add(Epsilon, end);

			return start;
		}

		/// <summary>
		/// +(...) rule
		/// </summary>
		public static State operator +(State state)
		{
			return NoClonePlus(state.Clone());
		}

		public static State NoClonePlus(State state)
		{
			var start = state;

			var end = new State();
			var oldEnd = start.FindEnd();

			oldEnd.Transition.Add(Epsilon, start);
			oldEnd.Transition.Add(Epsilon, end);

			return start;
		}

		/// <summary>
		/// *(...) rule
		/// </summary>
		public static State operator -(State state)
		{
			return NoCloneStar(state.Clone());
		}

		public static State NoCloneStar(State state)
		{
			var oldStart = state;

			var end = new State();
			var start = new State(Epsilon, oldStart, end);
			var oldEnd = start.FindEnd();

			oldEnd.Transition.Add(Epsilon, oldStart);
			oldEnd.Transition.Add(Epsilon, end);

			return start;
		}

		/// <summary>
		/// Concatanation
		/// </summary>
		public static State operator +(State state1, State state2)
		{
			var start = state1.Clone();
			start.FindEnd().Transition.Add(Epsilon, state2.Clone());
			return start;
		}

		public static State operator +(String string1, State state2)
		{
			return Thompson.Create(string1) + state2;
		}

		public static State operator +(State state1, String string2)
		{
			return state1 + Thompson.Create(string2);
		}

		public static State operator +(int byte1, State state2)
		{
			return Thompson.Create(byte1) + state2;
		}

		public static State operator +(State state1, int byte2)
		{
			return state1 + Thompson.Create(byte2);
		}

		public static State NoCloneConcatanation(params State[] sequence)
		{
			for (int i = sequence.Length - 1; i >= 1; i--)
				sequence[i - 1].FindEnd().Transition.Add(Epsilon, sequence[i]);

			return sequence[0];
		}

		/// <summary>
		/// Alternation
		/// </summary>
		public static State operator |(State state1, State state2)
		{
			var start = new State();
			start.Transition.Add(Epsilon, state1.Clone());
			start.Transition.Add(Epsilon, state2.Clone());

			var end = new State();
			foreach (var oldend in start.FindEnds())
				oldend.Transition.Add(Epsilon, end);

			return start;
		}

		public static State operator |(String string1, State state2)
		{
			return Thompson.Create(string1) | state2;
		}

		public static State operator |(State state1, String string2)
		{
			return state1 | Thompson.Create(string2);
		}

		public static State operator |(int byte1, State state2)
		{
			return Thompson.Create(byte1) | state2;
		}

		public static State operator |(State state1, int byte2)
		{
			return state1 | Thompson.Create(byte2);
		}

		public static State NoCloneAlternation(params State[] alternations)
		{
			if (alternations.Length > 1)
			{
				var start = new State();
				var end = new State();

				foreach (var item in alternations)
				{
					start.Transition.Add(Epsilon, item);
					foreach (var oldend in item.FindEnds())
						oldend.Transition.Add(Epsilon, end);
				}

				return start;
			}

			return alternations[0];
		}

		/// <summary>
		/// Create from string
		/// </summary>
		public static implicit operator State(string string1)
		{
			return Thompson.Create(string1);
		}

		/// <summary>
		/// Create from byte[]
		/// </summary>
		public static implicit operator State(byte[] bytes)
		{
			return Thompson.Create(bytes);
		}

		/// <summary>
		/// Create from int (byte)
		/// </summary>
		public static implicit operator State(int byte1)
		{
			return Thompson.Create(byte1);
		}

		private State Clone(Dictionary<int, State> proccessed)
		{
			var copy = new State();
			//copy.IsBeginRange = IsBeginRange;
			//copy.IsEndRange = IsEndRange;
			//copy.RangeName = RangeName;
			//copy.IsFinal = IsFinal;
			//copy.ConstName = ConstName;
			//copy.ConstValue = ConstValue;
			//copy.ConstPriority = ConstPriority;
			//copy.CountName = CountName;
			//copy.CountMax = CountMax;
			copy.CopyIMarkFrom(this);
			copy.Tag = Tag;

			proccessed.Add(Id, copy);

			foreach (var pair in Transition)
			{
				State nextState;
				if (proccessed.TryGetValue(pair.Value.Id, out nextState) == false)
					nextState = pair.Value.Clone(proccessed);
				copy.Transition.Add(pair.Key, nextState);
			}

			return copy;
		}

		public void CopyIMarkFrom(IMark imark)
		{
			CopyFrom(imark);
		}

		private void FindEnds(HashSet<int> proccessed, HashSet<State> ends)
		{
			proccessed.Add(Id);

			bool hasTransition = false;
			foreach (var pair in Transition)
			{
				hasTransition = true;

				if (proccessed.Contains(pair.Value.Id) == false)
					pair.Value.FindEnds(proccessed, ends);
			}


			if (hasTransition == false)
				ends.Add(this);
		}

		private int Count(HashSet<int> proccessed)
		{
			proccessed.Add(Id);

			int count = 1;
			foreach (var pair in Transition)
				if (proccessed.Contains(pair.Value.Id) == false)
					count += pair.Value.Count(proccessed);

			return count;
		}

		private void ForEach(Action<State> action)
		{
			var all = new HashSet<State>();
			ForEach(all);

			foreach (var item in all)
				action(item);
		}

		private void ForEach(HashSet<State> proccessed)
		{
			proccessed.Add(this);

			foreach (var pair in Transition)
				if (proccessed.Contains(pair.Value) == false)
					pair.Value.ForEach(proccessed);
		}
	}
}
