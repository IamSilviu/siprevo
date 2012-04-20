using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;

namespace Http.Message
{
	public partial class HttpMessageReader
	{
		public static void InitializeAsync(Action<int> callback)
		{
			var reader = new HttpMessageReader();

			reader.LoadTables();
			ThreadPool.QueueUserWorkItem((stateInfo) =>
				{
					if (callback != null)
						callback(reader.CompileParseMethod());
				});
		}

		public int CompileParseMethod()
		{
			int start = Environment.TickCount;

			SetDefaultValue();
			Parse(new byte[] { 0 }, 0, 1);

			return Environment.TickCount - start;
		}

		public void LoadTables()
		{
			LoadTables(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Http.Message.dfa");
		}

		partial void OnAfterParse()
		{
			if (IsFinal)
			{
				CorrectCounts();
			}
		}

		private void CorrectCounts()
		{
			Count.Upgrade++;
			Count.SecWebSocketProtocol++;
			Count.SecWebSocketExtensions++;
		}

		public bool HasContentLength
		{
			get { return ContentLength != int.MinValue; }
		}

		public bool HasUpgarde(Upgrades upgrade)
		{
			for (int i = 0; i < Count.Upgrade; i++)
				if (Upgrade[i] == upgrade)
					return true;
			return false;
		}

		#region struct HostStruct {...}

		public partial struct HostStruct
		{
			public new string ToString()
			{
				return Host.ToString() + ":" + Port.ToString();
			}
		}

		#endregion
	}
}
