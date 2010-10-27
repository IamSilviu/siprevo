using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Fsm;

namespace SipDfaCompiler
{
	partial class Generator
	{
		private void GenerateParseMethod(DfaState dfa, int errorState)
		{
			_main.WriteLine("#region int Parse(..)");

			_main.WriteLine("public int Parse(byte[] bytes, int offset, int length)");
			_main.WriteLine("{");

			if (dfa != null)
			{
				_main.WriteLine("int i = offset;");

				GenerateSwitch(dfa, errorState, SwitchMode.JumpOnly);
				_main.WriteLine("i++;");

				_main.WriteLine("int end = offset + length;");
				_main.WriteLine("for( ; i < end; i++)");
				_main.WriteLine("{");

				GenerateSwitch(dfa, errorState, SwitchMode.ActionJump);

				_main.WriteLine("}");

				GenerateSwitch(dfa, errorState, SwitchMode.ActionOnly);

				_main.WriteLine("exit1: ;");
				_main.WriteLine("return i - offset;");
			}
			else
				_main.WriteLine("return 0;");

			_main.WriteLine("}");

			_main.WriteLine("#endregion");
		}

		enum SwitchMode
		{
			JumpOnly,
			ActionOnly,
			ActionJump
		}

		private void GenerateSwitch(DfaState dfa, int errorState, SwitchMode mode)
		{
			_main.WriteLine("switch(state)");
			_main.WriteLine("{");

			dfa.ForEachNR((state) =>
			{
				_main.WriteLine("case State{0}:", state.Index);

				foreach (var nfa1 in state.AllMarks)
				{
					if (nfa1.Mark == Marks.ResetRange)
					{
						var name = GetVarname(nfa1.Name, "");
						_main.WriteLine("{0}.SetDefaultValue();", name);
					//	_main.WriteLine("{0}.Begin = -1;", name);
					//	_main.WriteLine("{0}.End = -1;", name);
					}

					if (nfa1.Mark == Marks.ResetRangeIfInvalid)
					{
						var name = GetVarname(nfa1.Name, "");
						_main.WriteLine("if({0}.End <0) {0}.Begin = -1;", name);
					}
				}

				foreach (var nfa1 in state.NfaStates)
				{
					if (nfa1.Mark == Marks.Count)
						_main.WriteLine("{0}++;", GetVarname(nfa1.Name, "Count."));
				}

				foreach (var mark in state.AllMarks)
				{
					if (mark.Mark == Marks.ContinueRange)
					{
						var ifv = GetCountComparation(RemoveExtraInfo(mark.Name));
						if (ifv != "")
							ifv += " && ";
						_main.WriteLine("if({1}{0}.End == i-1) {0}.End = i;", GetVarname(mark.Name, ""), ifv);
					}
				}

				foreach (var nfa1 in state.NfaStates)
				{
					if (nfa1.Mark == Marks.BeginRange)
					{
						var extra = GetCountComparation(RemoveExtraInfo(nfa1.Name));
						if (extra != "")
							extra += " && ";
						_main.WriteLine("if ({1}{0} < 0) {0} = i;", GetVarname(nfa1.Name, "") + ".Begin", extra);
					}

					if (nfa1.Mark == Marks.EndRange)
					{
						var ifv = GetCountComparation(RemoveExtraInfo(nfa1.Name));
						if (ifv != "")
							_main.Write("if({0}) ", ifv);
						_main.WriteLine("{0} = i;", GetVarname(nfa1.Name, "") + ".End");
					}

					//if (nfa1.Mark == Marks.ContinueRange)
					//{
					//    var ifv = GetCountComparation(RemoveExtraInfo(nfa1.Name));
					//    if (ifv != "")
					//        ifv += " && ";
					//    _main.WriteLine("if({1}{0}.End == i-1) {0}.End = i;", GetVarname(nfa1.Name, ""), ifv);
					//}

					if (nfa1.Mark == Marks.Bool)
						_main.WriteLine("{0} = true;", GetVarname(nfa1.Name, ""));

					if (nfa1.Mark == Marks.Final)
						_main.WriteLine("Final = true;");
				}

				if (mode == SwitchMode.ActionJump || mode == SwitchMode.ActionOnly)
				{
					if (state.HasMarks)
					{
						foreach (var decimal1 in state.Decimals)
							_main.WriteLine("{0} = ({0} << 1) * 5 + bytes[i - 1] - 48;", GetVarname(decimal1.Name, ""));

						foreach (var hex1 in state.Hexes)
							_main.WriteLine("{0} = ({0} << 4) + GetHexDigit(bytes[i - 1]);", GetVarname(hex1.Name, ""));
					}
				}

				if (state.Consts.Count > 0)
				{
					foreach (var pair in state.ConstNameValues)
					{
						var ifv = GetCountComparation(RemoveExtraInfo(pair.Key));

						if (ifv != "")
							_main.Write("if(" + ifv + ") ");

						_main.WriteLine("{0} = {1}s.{2};",
							AddCountPrefix(RemoveExtraInfo(pair.Key)), 
							RemoveBrackets(VariableInfo.GetShortName(pair.Key)), 
							pair.Value);
					}
				}

				if (state.IsFinal)
				{
					_main.WriteLine("goto exit1;");
				}
				else
				{
					if (mode == SwitchMode.ActionJump || mode == SwitchMode.JumpOnly)
						_main.WriteLine("state = table{0}[bytes[i]];", state.Index);
					_main.WriteLine("break;");
				}
			});

			_main.WriteLine("case State{0}:", errorState);
			_main.WriteLine("Error = true;");
			_main.WriteLine("goto exit1;");

			_main.WriteLine("}");
		}

		private void GenerateTables(DfaState dfa, int errorState)
		{
			if (dfa != null)
			{
				_main.WriteLine("#region States Tables");

				dfa.ForEachNR((state) =>
					{
						_main.WriteLine("private static int[] table{0};", state.Index);

						int next;
						DfaState nextState;
						for (int i = 0; i <= byte.MaxValue; i++)
						{
							next = errorState;
							nextState = state.Transition[i];
							if (nextState != null)
								next = nextState.Index;
							_table.Write((Int32)next);
						}
					});

				_main.WriteLine("#endregion");
			}
			else
				_main.WriteLine("// NO TABLES");
		}

		private void GenerateLoadFunction3(int count, bool empty)
		{
			_main.WriteLine("#region void LoadTables(..)");

			_main.WriteLine("public void LoadTables(string path)");
			_main.WriteLine("{");

			if (empty == false)
			{
				_main.WriteLine("const int maxItems = byte.MaxValue + 1;");
				_main.WriteLine("const int maxBytes = sizeof(Int32) * maxItems;");

				_main.WriteLine("using (var reader = new DeflateStream(File.OpenRead(path), CompressionMode.Decompress))");
				_main.WriteLine("{");
				_main.WriteLine("byte[] buffer = new byte[maxBytes];");

				for (int i = 0; i < count; i++)
				{
					_main.WriteLine("table{0} = new int[maxItems];", i);
					_main.WriteLine("reader.Read(buffer, 0, buffer.Length);");
					_main.WriteLine("Buffer.BlockCopy(buffer, 0, table{0}, 0, maxBytes);", i);
				}

				_main.WriteLine("}");
			}

			_main.WriteLine("}");

			_main.WriteLine("#endregion");
		}

		//private void GenerateLoadFunction(int count, bool empty)
		//{
		//    _main.WriteLine("public void LoadTables(string path)");
		//    _main.WriteLine("{");

		//    if (empty == false)
		//    {
		//        _main.WriteLine("const int maxItems = byte.MaxValue + 1;");
		//        _main.WriteLine("const int maxBytes = sizeof(Int32) * maxItems;");

		//        _main.WriteLine("using (var reader = new DeflateStream(File.OpenRead(path), CompressionMode.Decompress))");
		//        _main.WriteLine("{");
		//        _main.WriteLine("byte[] buffer = new byte[maxBytes];");
		//        _main.WriteLine("Int32[] intBuffer = new Int32[maxItems];");

		//        for (int i = 0; i < count; i++)
		//        {
		//            _main.WriteLine("reader.Read(buffer, 0, buffer.Length);");
		//            _main.WriteLine("Buffer.BlockCopy(buffer, 0, intBuffer, 0, maxBytes);", i);

		//            _main.WriteLine("table{0} = new States[maxItems];", i);
		//            _main.WriteLine("for (int i = 0; i < maxItems; i++)");
		//            _main.WriteLine("table{0}[i] = (States)intBuffer[i];", i);
		//        }

		//        _main.WriteLine("}");
	
		//        //_main.WriteLine("using (var reader = new DeflateStream(File.OpenRead(path), CompressionMode.Decompress))");
		//        //_main.WriteLine("{");
		//        //_main.WriteLine("byte[] buffer = new byte[sizeof(Int32)];");

		//        //for (int i = 0; i < count; i++)
		//        //{
		//        //    _main.WriteLine("table{0} = new States[byte.MaxValue + 1];", i);
		//        //    _main.WriteLine("for (int i = 0; i <= byte.MaxValue; i++)");
		//        //    _main.WriteLine("{");
		//        //    _main.WriteLine("reader.Read(buffer, 0, buffer.Length);");
		//        //    _main.WriteLine("table{0}[i] = (States)BitConverter.ToInt32(buffer, 0);", i);
		//        //    _main.WriteLine("}");
		//        //}
		//        //_main.WriteLine("}");
		//    }

		//    _main.WriteLine("}");
		//}

		private void GenerateGetHexDigitFunction()
		{
			_main.WriteLine("private int GetHexDigit(byte ch)");
			_main.WriteLine("{");
			_main.WriteLine("switch (ch)");
			_main.WriteLine("{");
			_main.WriteLine("case (byte)'0':");
			_main.WriteLine("return 0;");
			_main.WriteLine("case (byte)'1':");
			_main.WriteLine("return 1;");
			_main.WriteLine("case (byte)'2':");
			_main.WriteLine("return 2;");
			_main.WriteLine("case (byte)'3':");
			_main.WriteLine("return 3;");
			_main.WriteLine("case (byte)'4':");
			_main.WriteLine("return 4;");
			_main.WriteLine("case (byte)'5':");
			_main.WriteLine("return 5;");
			_main.WriteLine("case (byte)'6':");
			_main.WriteLine("return 6;");
			_main.WriteLine("case (byte)'7':");
			_main.WriteLine("return 7;");
			_main.WriteLine("case (byte)'8':");
			_main.WriteLine("return 8;");
			_main.WriteLine("case (byte)'9':");
			_main.WriteLine("return 9;");
			_main.WriteLine("}");

			_main.WriteLine("switch (ch)");
			_main.WriteLine("{");
			_main.WriteLine("case (byte)'a':");
			_main.WriteLine("return 10;");
			_main.WriteLine("case (byte)'b':");
			_main.WriteLine("return 11;");
			_main.WriteLine("case (byte)'c':");
			_main.WriteLine("return 12;");
			_main.WriteLine("case (byte)'d':");
			_main.WriteLine("return 13;");
			_main.WriteLine("case (byte)'e':");
			_main.WriteLine("return 14;");
			_main.WriteLine("case (byte)'f':");
			_main.WriteLine("return 15;");
			_main.WriteLine("}");

			_main.WriteLine("switch (ch)");
			_main.WriteLine("{");
			_main.WriteLine("case (byte)'A':");
			_main.WriteLine("return 10;");
			_main.WriteLine("case (byte)'B':");
			_main.WriteLine("return 11;");
			_main.WriteLine("case (byte)'C':");
			_main.WriteLine("return 12;");
			_main.WriteLine("case (byte)'D':");
			_main.WriteLine("return 13;");
			_main.WriteLine("case (byte)'E':");
			_main.WriteLine("return 14;");
			_main.WriteLine("case (byte)'F':");
			_main.WriteLine("return 15;");
			_main.WriteLine("}");

			_main.WriteLine("throw new ArgumentOutOfRangeException(string.Format(\"GetHexDigit: {0} is not hex digit\", ch));");
			_main.WriteLine("}");
		}
	}
}