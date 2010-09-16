using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using Server.Memory;

namespace Sip.Message
{
	public partial struct Fromto
	{
		public Addrspec AddrSpec
		{
			get
			{
				if (AddrSpec1.Hostport.Host.IsEmpty == false)
					return AddrSpec1;
				return AddrSpec2;
			}
		}
	}

	public partial class SipMessageReader
	: IDefaultValue
	{
		public partial struct ContactStruct
		{
			public Addrspec AddrSpec
			{
				get
				{
					if (AddrSpec1.Hostport.Host.IsEmpty == false)
						return AddrSpec1;
					return AddrSpec2;
				}
			}
		}
	}

	public partial struct Hostport
	{
		IPAddress ip;

		public IPAddress Ip
		{
			get
			{
				if (ip == IPAddress.None)
					IPAddress.TryParse(Encoding.UTF8.GetString(Host.Items, Host.Offset, Host.Length), out ip);

				return ip;
			}
			set
			{
				ip = value;
				Host.SetDefaultValue();
			}
		}

		partial void SetDefaultValueEx()
		{
			ip = IPAddress.None;
		}
	}
}