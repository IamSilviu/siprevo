using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Fsm;
using DfaCompiler;

namespace DfaToCSharp
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 3)
			{
				WriteUsage();
			}
			else
			{
				var dfaXml = args[0];
				var nameSpace = args[1];
				var className = args[2];

				Console.WriteLine("Load DFA");
				var dfa = Read(dfaXml);

				Console.WriteLine("Generate C# Source Code");
				var generator1 = new Generator(OptimizationMode.NoOptimization);
				generator1.Generate(className, className, nameSpace, dfa.Start, true);
				var generator2 = new Generator(OptimizationMode.SingleStatic);
				generator2.Generate(className + "Optimized", className, nameSpace, dfa.Start, false);
				var generator3 = new Generator(OptimizationMode.IndexedArray);
				generator3.Generate(className + "Optimized2", className, nameSpace, dfa.Start, false);
			}
		}

		private static Dfa Read(string filename)
		{
			XmlSerializer xs = new XmlSerializer(typeof(Dfa));

			using (Stream s = File.OpenRead(filename))
				return (Dfa)xs.Deserialize(s);
		}

		private static void WriteUsage()
		{
			Console.WriteLine("Usage: DfaToCSharp.exe dfa_file_name.xml c#_namespace c#_class_name");
		}
	}
}
