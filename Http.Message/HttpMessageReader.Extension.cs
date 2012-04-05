using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Http.Message
{
	public partial class HttpMessageReader
	{
		partial void OnAfterParse()
		{
			if (IsFinal)
			{
				CorrectCounts();
			}
		}

		private void CorrectCounts()
		{
			Count.UpgradeCount++;
		}
	}
}
