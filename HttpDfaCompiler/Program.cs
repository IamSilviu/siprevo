using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using DfaCompiler;
using Fsm;

namespace HttpDfaCompiler
{
	class Program
	{
		static int Main(string[] args)
		{
			var compiler = new Compiler(new HttpNfaGenerator());

			var command = "compile";//"update";

			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\";

			compiler.ExecuteCommand(
				command,
				"HttpMessageReader",
				"Http.Message",
				path + "http.mark.txt",
				path + "suppress.warning.txt",
				path + "http.all-marks.txt");

			return 0;
		}

		class HttpNfaGenerator
			: GeneratedXbnf
			, INfaGenerator
		{
			State INfaGenerator.GetNfaRoot()
			{
				return base.Getreq(new List<string>());
			}

			event EventHandler<MarkRuleEventArgs> INfaGenerator.MarkRule
			{
				add { base.MarkRule += value; }
				remove { base.MarkRule -= value; }
			}
		}
	}
}
