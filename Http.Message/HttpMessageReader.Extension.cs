using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using Base.Message;

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
			Count.Upgrade++;
			Count.SecWebSocketProtocol++;
			Count.SecWebSocketExtensions++;
			Count.Cookie++;
			Count.AuthorizationCount++;
			Count.IfMatches++;
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

		public int FindCookie(byte[] name)
		{
			for (int i = 0; i < Count.Cookie; i++)
				if (Cookie[i].Name.Equals(name))
					return i;
			return -1;
		}

		public bool TryGetCookieUInt(byte[] name, out uint value)
		{
			int i = FindCookie(name);

			value = 0;
			if (i >= 0)
			{
				for (int j = Cookie[i].Value.Begin; j < Cookie[i].Value.End; j++)
				{
					byte b = Cookie[i].Value.Bytes[j];
					if (b < 0x30 && b > 0x39)
						return false;

					b -= 0x30;

					unchecked
					{
						value *= 10;
						value += b;
					}
				}

				return true;
			}

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

		#region struct CookieStruct {...}

		public partial struct CookieStruct
		{
			public new string ToString()
			{
				return Name.ToString() + "=" + Value.ToString();
			}
		}

		#endregion
	}
}
