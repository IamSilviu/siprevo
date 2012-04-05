using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace Fsm
{
	//public class Dfa
	//{
	//    private DfaState _start;
	//    private DfaState _current;
	//    private byte[] _bytes;
	//    private UTF8Encoding _utf;
	//    private Dictionary<string, int> _begins;
	//    private Dictionary<string, int> _ends;
	//    private Dictionary<string, string> _consts;
	//    private Dictionary<string, int> _counts;
	//    private Dictionary<string, int> _decimals;
	//    private Dictionary<string, string> _chars;
	//    private int _compliedCount;

	//    public Dfa(State nfa, bool minimize, bool showProgress)
	//    {
	//        _utf = new UTF8Encoding();

	//        var compileStart = DateTime.Now;
	//        _start = nfa.ToDfa2(out _compliedCount, showProgress);
	//        CompileTime = DateTime.Now - compileStart;

	//        if (minimize)
	//        {
	//            var minimizeStart = DateTime.Now;
	//            MinimizedCount = _start.Minimize2(showProgress);
	//            MinimizeTime = DateTime.Now - minimizeStart;
	//        }
	//    }

	//    public Dfa(DfaState dfa)
	//    {
	//        _utf = new UTF8Encoding();
	//        _start = dfa;

	//        CompileTime = new TimeSpan();
	//        MinimizeTime = new TimeSpan();
	//    }

	//    public TimeSpan CompileTime
	//    {
	//        get;
	//        set;
	//    }

	//    public TimeSpan MinimizeTime
	//    {
	//        get;
	//        set;
	//    }

	//    public int CompliedCount
	//    {
	//        get { return _compliedCount; }
	//    }

	//    public int MinimizedCount
	//    {
	//        get;
	//        private set;
	//    }

	//    public DfaState Start
	//    {
	//        get { return _start; }
	//    }

	//    public bool PassedAll
	//    {
	//        get;
	//        private set;
	//    }

	//    public int PassedBytes
	//    {
	//        get;
	//        private set;
	//    }

	//    public bool IsFinal
	//    {
	//        get
	//        {
	//            if (_current != null)
	//                return _current.IsFinal;
	//            return false;
	//        }
	//    }

	//    public void ProccessString(string test)
	//    {
	//        _current = _start;
	//        _begins = new Dictionary<string, int>();
	//        _ends = new Dictionary<string, int>();
	//        _consts = new Dictionary<string, string>();
	//        _counts = new Dictionary<string, int>();
	//        _decimals = new Dictionary<string, int>();
	//        _chars = new Dictionary<string, string>();

	//        _bytes = _utf.GetBytes(test);

	//        int i = 0;
	//        while (_current != null && i < _bytes.Length)
	//        {
	//            AnalizeState(i, (i > 0) ? _bytes[i - 1] : (byte)0);
	//            _current = _current.GetTransited(_bytes[i++]);

	//            if (_current != null && _current.IsFinal)
	//                break;
	//        }
	//        AnalizeState(i, _bytes[i - 1]);

	//        PassedBytes = i;
	//        PassedAll = (i == _bytes.Length);
	//    }

	//    private void AnalizeState(int i, byte ch)
	//    {
	//        if (_current != null)
	//        {
	//            foreach (var nfa1 in _current.NfaStates)
	//                if (nfa1.IsCount)
	//                    _counts[nfa1.CountName] = GetCount(nfa1.CountName) + 1;

	//            foreach (var nfa1 in _current.NfaStates)
	//                if (nfa1.IsBeginRange)
	//                {
	//                    var name = SubstituteCounts(nfa1.RangeName);
	//                    if (_begins.ContainsKey(name) == false)
	//                        _begins[name] = i;
	//                }

	//            foreach (var nfa1 in _current.NfaStates)
	//                if (nfa1.IsEndRange)
	//                    _ends[SubstituteCounts(nfa1.RangeName)] = i;

	//            if (_current.Consts.Count > 0)
	//                foreach (var pair in _current.ConstNameValues)
	//                    _consts[SubstituteCounts(pair.Key)] = pair.Value;

	//            if (_current.HasMarks)
	//            {
	//                foreach (var decimal1 in _current.Decimals)
	//                {
	//                    var decimalName = SubstituteCounts(decimal1.Name);
	//                    if (_decimals.ContainsKey(decimalName))
	//                    {
	//                        _decimals[decimalName] = _decimals[decimalName] * 10 + ch - 48;
	//                    }
	//                    else
	//                        _decimals.Add(decimalName, ch - 48);
	//                }

	//                foreach (var char1 in _current.Chars)
	//                {
	//                    var ch1 = _utf.GetChars(new byte[] { ch })[0].ToString();
	//                    var charName = SubstituteCounts(char1.Name);
	//                    if (_chars.ContainsKey(charName))
	//                    {
	//                        _chars[charName] = _chars[charName] + ch1;
	//                    }
	//                    else
	//                        _chars.Add(charName, ch1);
	//                }
	//            }
	//        }
	//    }

	//    private int GetCount(string name)
	//    {
	//        int count = 0;
	//        _counts.TryGetValue(name, out count);
	//        return count;
	//    }

	//    private string SubstituteCounts(string varname)
	//    {
	//        var regex = new Regex(@"\[([a-zA-Z0-1]+)\]");
	//        var matches = regex.Matches(varname);

	//        foreach (Match match in matches)
	//            varname = varname.Replace(match.Value, "[" + GetCount(match.Groups[1].Value).ToString() + "]");

	//        return varname;
	//    }

	//    public void WriteToFile(string fileName)
	//    {
	//        XmlWriterSettings settings = new XmlWriterSettings();
	//        settings.Indent = true;
	//        settings.IndentChars = ("\t");

	//        using (XmlWriter writer = XmlWriter.Create(fileName, settings))
	//        {
	//            writer.WriteStartElement(@"dfa");

	//            writer.WriteStartElement(@"info");
	//            writer.WriteElementString("compliedStates", CompliedCount.ToString());
	//            writer.WriteElementString("minimizedStates", MinimizedCount.ToString());
	//            writer.WriteElementString("compileTime", CompileTime.ToString());
	//            writer.WriteElementString("minimizeTime", MinimizeTime.ToString());
	//            writer.WriteEndElement();

	//            writer.WriteStartElement(@"states");

	//            _start.ForEach((state) =>
	//                {
	//                    writer.WriteStartElement(@"state");

	//                    writer.WriteStartElement(@"action");

	//                    writer.WriteElementString("final", state.IsFinal.ToString());

	//                    //var begins = new HashSet<string>();
	//                    //var ends = new HashSet<string>();
	//                    //var counts = new HashSet<string>();
	//                    //var consts = new Dictionary<string, int>();

	//                    //foreach (var nfaState in state.NfaStates)
	//                    //{
	//                    //    if (nfaState.IsBeginRange)
	//                    //        begins.Add(nfaState.RangeName);
	//                    //    if (nfaState.IsEndRange)
	//                    //        ends.Add(nfaState.RangeName);
	//                    //    if (nfaState.IsCount)
	//                    //        counts.Add(nfaState.CountName);
	//                    //    if(nfaState.IsConst)
	//                    //        if(consts.ContainsKey(nfaState.ConstName))
	//                    //            if(consts[nfaState.ConstName]>nfaState.ConstPriority)
	//                    //                consts[]
	//                    //}

	//                    writer.WriteEndElement();

	//                    writer.WriteEndElement();
	//                });

	//            writer.WriteEndElement();

	//            writer.WriteEndElement();
	//            writer.Flush();
	//        }
	//    }

	//    public void PrintResults()
	//    {
	//        Console.WriteLine("DFA Complied States: {0}", CompliedCount);
	//        Console.WriteLine("Minimized DFA States: {0}", MinimizedCount);
	//        Console.WriteLine("Compile Time: {0}", CompileTime);
	//        Console.WriteLine("Minimize Time: {0}", MinimizeTime);
	//        Console.WriteLine("Passed {0} bytes{1}", PassedBytes, PassedAll ? " (ALL)" : "");
	//        Console.WriteLine("Final: {0}", IsFinal);

	//        Console.WriteLine();
	//        Console.WriteLine("Ranges:");
	//        foreach (var pair in _begins)
	//        {
	//            int end;
	//            if (_ends.TryGetValue(pair.Key, out end))
	//            {
	//                Console.WriteLine("\t{0} -> {1}", pair.Key,
	//                    _utf.GetString(_bytes, pair.Value, end - pair.Value));
	//            }
	//            else
	//            {
	//                Console.WriteLine("Error: Found start only for [{0}]", pair.Key);
	//            }
	//        }

	//        Console.WriteLine();
	//        Console.WriteLine("Consts:");
	//        foreach (var pair in _consts)
	//            Console.WriteLine("\t{0} -> {1}", pair.Key, pair.Value);

	//        Console.WriteLine();
	//        Console.WriteLine("Counts:");
	//        foreach (var pair in _counts)
	//            Console.WriteLine("\t{0} -> {1}", pair.Key, pair.Value);

	//        Console.WriteLine();
	//        Console.WriteLine("Decimals:");
	//        foreach (var pair in _decimals)
	//            Console.WriteLine("\t{0} -> {1}", pair.Key, pair.Value);

	//        Console.WriteLine();
	//        Console.WriteLine("Chars:");
	//        foreach (var pair in _chars)
	//            Console.WriteLine("\t{0} -> {1}", pair.Key, pair.Value);

	//        Console.WriteLine();
	//    }
	//}
}
