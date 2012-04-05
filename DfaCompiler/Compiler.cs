using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using Fsm;
using DfaCompiler;

namespace DfaCompiler
{
	public class Compiler
	{
		private Dictionary<string, ActionsDescription> _marks;
		private Dictionary<string, Dictionary<string, ActionsDescription>> _groups;
		private List<string> _suppressWarning;
		private INfaGenerator nfaGenerator;

		public Compiler(INfaGenerator nfaGenerator)
		{
			this.nfaGenerator = nfaGenerator;
			_marks = new Dictionary<string, ActionsDescription>();
			_groups = new Dictionary<string, Dictionary<string, ActionsDescription>>();
			_suppressWarning = new List<string>();
		}

		public void ExecuteCommand(string command, string className, string namespace1, string marksPath, string suppressWarningPath, string allMarksPath)
		{
			switch (command)
			{
				case "update":
					GenerateAllMarksFile(marksPath, allMarksPath);
					break;
				case "compile":
					CreateDfa(className, namespace1, marksPath, suppressWarningPath);
					break;
				case "fake":
					CreateFakeDfa(className, namespace1, marksPath, suppressWarningPath);
					break;
			}
		}

		public void CreateDfa(string className, string namespace1, string marksPath, string suppressWarningPath)
		{
			LoadMarks(marksPath);
			LoadSuppressWarngin(suppressWarningPath);

			var dfa = CompileDfa();

			var generator1 = new Generator(OptimizationMode.NoOptimization);
			generator1.Generate(className, className, namespace1, dfa, true);
			var generator2 = new Generator(OptimizationMode.SingleStatic);
			generator2.Generate(className + "Optimized", className, namespace1, dfa, false);
			var generator3 = new Generator(OptimizationMode.IndexedArray);
			generator3.Generate(className + "Optimized2", className, namespace1, dfa, false);
		}

		public void CreateFakeDfa(string className, string namespace1, string marksPath, string suppressWarningPath)
		{
			LoadMarks(marksPath);
			LoadSuppressWarngin(suppressWarningPath);

			var generator = new Generator(OptimizationMode.NoOptimization);
			generator.ParseDeclaration(_marks);
			generator.Generate(className, className, namespace1, null, true);
		}

		public void GenerateAllMarksFile(string marksPath, string allMarksPath)
		{
			try
			{
				Console.WriteLine("Generating all marks file...");

				LoadMarks(marksPath);

				int loaded = _marks.Count;

				nfaGenerator.MarkRule += MarkRule_GenerateAllMarksHandler;
				nfaGenerator.GetNfaRoot();

				int total = _marks.Count;

				if (loaded < total)
				{
					using (var writer = File.CreateText(allMarksPath))
					{
						foreach (var mark in _marks)
						{
							if (mark.Value.Description != "")
								writer.WriteLine("{0} -> {1}", mark.Key, mark.Value.Description);
							else
								writer.WriteLine(mark.Key);
						}
						writer.Flush();
					}
				}

				nfaGenerator.MarkRule -= MarkRule_GenerateAllMarksHandler;
				_marks.Clear();

				Console.WriteLine("Marks", loaded);
				Console.WriteLine("  Loaded:	{0}", loaded);
				Console.WriteLine("   Total:	{0}", total);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to generate all marks file\r\n{0}", ex.Message);
			}
		}

		private void MarkRule_GenerateAllMarksHandler(object sender, MarkRuleEventArgs e)
		{
			var path = e.GetRulesPath();

			if (_marks.ContainsKey(path) == false)
				_marks.Add(path, ActionsDescription.TryParse("", ""));
		}

		private DfaState CompileDfa()
		{
			nfaGenerator.MarkRule += MarkRuleHandler;

			Console.WriteLine("Creating NFA...");
			var nfa = nfaGenerator.GetNfaRoot();
			nfa.MarkFinal();
			Console.WriteLine("Max NFA state id: {0}", State.MaxId);
			Console.WriteLine("Done.");

			foreach (var pair in _marks)
				if (pair.Value.IsEmpty == false && pair.Value.UseCount == 0)
				{
					if (_suppressWarning.Contains(pair.Key) == false)
					{
						Console.WriteLine("Warning: Rule was not used");
						Console.WriteLine("{0}", pair.Key);
					}
				}

			nfa.Pack(true);

			int count;
			var dfa = nfa.ToDfa3(out count, true);
			Console.WriteLine("DFA Complied States: {0}", count);

			var minCount = dfa.Minimize4(true);
			Console.WriteLine("Minimized DFA States: {0}", minCount);

			nfaGenerator.MarkRule -= MarkRuleHandler;

			return dfa;
		}

		private void MarkRuleHandler(object sender, MarkRuleEventArgs e)
		{
			var path = e.GetRulesPath();

			ActionsDescription mark;
			if (_marks.TryGetValue(path, out mark) && mark.IsEmpty == false)
			{
				mark.UseCount++;

				foreach (var action in mark.Actions)
				{
					switch (action.Mark)
					{
						case Marks.Custom:
							e.Start = State.MarkCustom(e.Start, action.Args[0], action.Args[1], action.Args[2], action.Args[3]);
							break;
						case Marks.Const:
							e.Start.MarkConst(action.Args[0], action.Args[1], int.Parse(action.Args[2]));
							break;
						case Marks.Range:
							e.Start = State.MarkRange(e.Start, action.Args[0], int.Parse(action.Args[1]), int.Parse(action.Args[2]));
							break;
						case Marks.BeginRange:
							e.Start = State.MarkBeginRange(e.Start, action.Args[0], action.Args[1] == "AtBegin", int.Parse(action.Args[2]));
							break;
						case Marks.EndRange:
						case Marks.EndRangeIfInvalid:
							e.Start = State.MarkEndRange(e.Start, action.Mark, action.Args[0], action.Args[1] == "AtBegin", int.Parse(action.Args[2]));
							break;
						case Marks.ContinueRange:
							e.Start.MarkContinueRange(action.Args[0]);
							break;
						case Marks.Decimal:
							e.Start.MarkDecimal(action.Args[0]);
							break;
						case Marks.Hex:
							e.Start.MarkHex(action.Args[0]);
							break;
						case Marks.Count:
							e.Start.MarkCount(action.Args[0], int.Parse(action.Args[1]), int.Parse(action.Args[2]));
							break;
						case Marks.Bool:
						case Marks.BoolEx:
						case Marks.BoolExNot:
							e.Start.MarkBool(action.Mark, action.Args[0]);
							break;
						case Marks.ResetRange:
							e.Start.MarkReset(action.Args[0]);
							break;
						case Marks.ResetRangeIfInvalid:
							e.Start.MarkResetIfInvalid(action.Args[0]);
							break;
						//case Marks.Error:
						//    e.Start = State.MarkAlternativesAsError(e.Start);
						//    break;
						default:
							throw new Exception();
					}
				}
			}
		}

		private void LoadMarks(string path)
		{
			_marks.Clear();

			string[] strings1 = new string[0];
			if (File.Exists(path))
				strings1 = File.ReadAllLines(path);
			else
				Console.WriteLine("Marks file not found: {0}", path);

			foreach (var mark in strings1)
			{
				if (mark.StartsWith("//") == false && string.IsNullOrEmpty(mark.Trim()) == false)
				{
					if (mark.StartsWith("."))
					{
						int arrow = mark.IndexOf("->", 1);

						if (arrow < 0)
						{
							Console.WriteLine("Error: Failed to parse group {0}", mark);
							throw new Exception();
						}

						int point = mark.IndexOf(".", 1, arrow);

						if (point < 0)
						{
							point = arrow;
							while (mark[point - 1] == ' ')
								point--;
						}

						var groupName = mark.Substring(1, point - 1);
						if (_groups.ContainsKey(groupName) == false)
							_groups.Add(groupName, new Dictionary<string, ActionsDescription>());

						AddMark(_groups[groupName], mark.Substring(point));
					}
					else
					{
						AddMark(_marks, mark);
					}
				}
			}
		}

		private void AddMark(Dictionary<string, ActionsDescription> marks, string mark)
		{
			string markpath, description;
			int x = mark.IndexOf("->");
			if (x < 0)
			{
				markpath = mark;
				description = "";
			}
			else
			{
				markpath = mark.Substring(0, x).Trim();
				description = mark.Substring(x + 2).Trim();
			}

			if (marks.ContainsKey(markpath))
			{
				Console.WriteLine("Warning: Duplicated mark {0}", markpath);
			}
			else
			{
				var xmark = ActionsDescription.TryParse(description, markpath);
				if (xmark == null)
				{
					throw new Exception(string.Format("Error: Failed to parse mark {0}", markpath));
				}
				else
				{
					if (xmark.Actions == null || xmark.Actions[0].Mark != Marks.Group)
						marks.Add(markpath, xmark);
					else
					{
						try
						{
							var group = _groups[xmark.Actions[0].Args[1]];

							foreach (var item in group)
								marks.Add(markpath + item.Key, item.Value.Clone("?", xmark.Actions[0].Args[0]));
						}
						catch (KeyNotFoundException)
						{
							Console.WriteLine("Group not defined {0}", xmark.Actions[0].Args[0]);
							throw new Exception();
						}
					}
				}
			}
		}

		private void LoadSuppressWarngin(string path)
		{
			if (File.Exists(path))
			{
				var lines = File.ReadAllLines(path);

				foreach (var line in lines)
					if (string.IsNullOrEmpty(line) == false && line.Trim().StartsWith(@"//") == false)
						_suppressWarning.Add(line.Trim());
			}
		}
	}
}
