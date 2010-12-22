using System;
using System.Text;
using System.Net;
using Server.Memory;
using System.Collections.Generic;

namespace Sip.Message
{
	public interface IRemovableParams
	{
		bool IsRemoved
		{
			get;
			set;
		}

		ByteArrayPart Param
		{
			get;
		}
	}

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

		public void PostParsing()
		{
			AddrSpec1 = AddrSpec;
		}
	}

	public partial struct Addrspec
	{
		IPAddress _MaddrIP;
		ByteArrayPart _MsReceivedCid;

		public IPAddress xMaddrIP
		{
			get
			{
				return _MaddrIP == IPAddress.None ? Maddr.ToIpAddress() : _MaddrIP;
			}
			set
			{
				_MaddrIP = value;
			}
		}

		public ByteArrayPart xMsReceivedCid
		{
			get
			{
				return _MsReceivedCid.IsValid == false ? MsReceivedCid : _MsReceivedCid;
			}
			set
			{
				_MsReceivedCid = value;
			}
		}

		public bool IsMsReceivedCidPresent
		{
			get
			{
				return xMsReceivedCid.IsValid;
			}
		}

		partial void SetDefaultValueEx()
		{
			xMaddrIP = IPAddress.None;
			_MsReceivedCid.SetDefaultValue();
		}
	}

	public partial struct Hostport
	{
		IPAddress ip;
		int _Port;
		IPAddress _HostIP;

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

		public bool HasPort
		{
			get { return Port != int.MinValue; }
		}

		public int xPort
		{
			get
			{
				return _Port == int.MinValue ? Port : _Port;
			}
			set
			{
				_Port = value;
			}
		}

		public IPAddress xHostIP
		{
			get
			{
				return _HostIP == IPAddress.None ? Host.ToIpAddress() : _HostIP;
			}
			set
			{
				_HostIP = value;
			}
		}

		partial void SetDefaultValueEx()
		{
			ip = IPAddress.None;
			xPort = int.MinValue;
			xHostIP = IPAddress.None;
		}
	}

	public partial struct Header : IRemovableParams
	{
		#region IRemovableParams

		public bool IsRemoved
		{
			get;
			set;
		}

		public ByteArrayPart Param
		{
			get
			{
				return new ByteArrayPart()
				{
					Bytes = Name.Bytes,
					Begin = Name.Begin,
					End = Value.End + 2
				};
			}
		}

		#endregion

		partial void SetDefaultValueEx()
		{
			IsRemoved = false;
		}
	}

	public partial class SipMessageReader : IDefaultValue
	{
		public partial struct ContactStruct
		{
			public bool _RemoveProxy;

			public Addrspec AddrSpec
			{
				get
				{
					if (AddrSpec1.Hostport.Host.IsEmpty == false)
						return AddrSpec1;
					return AddrSpec2;
				}
			}

			public bool HasProxyReplace
			{
				get { return ProxyReplace.IsValid; }
			}

			public void PostParsing()
			{
				AddrSpec1 = AddrSpec;
			}

			public void RemoveProxy()
			{
				_RemoveProxy = true;
			}

			partial void SetDefaultValueEx()
			{
				_RemoveProxy = false;
			}
		}

		public partial struct ViaStruct : IRemovableParams
		{
			#region IRemovableParams

			public bool IsRemoved
			{
				get;
				set;
			}

			public ByteArrayPart Param
			{
				get
				{
					return CommaAndValue;
				}
			}

			#endregion

			public IPAddress ReceivedIP
			{
				get;
				set;
			}

			public int MsReceivedPort
			{
				get;
				set;
			}

			public ByteArrayPart MsReceivedCid
			{
				get;
				set;
			}

			partial void SetDefaultValueEx()
			{
				IsRemoved = false;
				ReceivedIP = IPAddress.None;
				MsReceivedPort = int.MinValue;
				MsReceivedCid.SetDefaultValue();
			}
		}

		public partial struct CSeqStruct
		{
			Methods _Method;

			public Methods xMethod
			{
				get
				{
					return _Method == Methods.None ? Method : _Method;
				}
				set
				{
					_Method = value;
				}
			}

			partial void SetDefaultValueEx()
			{
				_Method = Methods.None;
			}
		}

		public partial struct RouteStruct : IRemovableParams
		{
			#region IRemovableParams

			public bool IsRemoved
			{
				get;
				set;
			}

			public ByteArrayPart Param
			{
				get
				{
					return CommaAndValue;
				}
			}

			#endregion

			partial void SetDefaultValueEx()
			{
				IsRemoved = false;
			}
		}

		public partial struct RecordRouteStruct : IRemovableParams
		{
			#region IRemovableParams

			public bool IsRemoved
			{
				get;
				set;
			}

			public ByteArrayPart Param
			{
				get
				{
					return CommaAndValue;
				}
			}

			#endregion

			partial void SetDefaultValueEx()
			{
				IsRemoved = false;
			}
		}
	}

	#region class SipMessageReader {...}

	public partial class SipMessageReader : IDefaultValue
	{
		public int CompileParseMethod()
		{
			int start = Environment.TickCount;
			
			SetDefaultValue();
			Parse(new byte[] { 0 }, 0, 1);

			return Environment.TickCount - start;
		}

		public bool IsRequest
		{
			get { return StatusCode.Value == int.MinValue; }
		}

		public bool IsResponse
		{
			get { return StatusCode.Value != int.MinValue; }
		}

		public bool HasContentLength
		{
			get { return ContentLength != int.MinValue; }
		}

		public bool IsInvite
		{
			get { return Method == Methods.Invitem; }
		}

		public bool IsAck
		{
			get { return Method == Methods.Ackm; }
		}

		public bool IsCancel
		{
			get { return Method == Methods.Cancelm; }
		}

		public bool HasContact
		{
			get { return Contact[0].Value.IsValid; }
		}

		public bool HasContentType
		{
			get { return ContentType.Type.IsValid; }
		}

		public void CorrectCounts()
		{
			Count.ContactCount++;
			Count.RequireCount++;
			Count.ProxyRequireCount++;
			Count.ViaCount++;
			Count.RouteCount++;
			Count.RecordRouteCount++;
			Count.SupportedCount++;
		}

		public int CountHeaders(HeaderNames name)
		{
			int count = 0;

			for (int i = 0; i < Count.HeaderCount; i++)
				if (Headers[i].HeaderName == name)
					count++;

			return count;
		}

		public int FindHeaderIndex(HeaderNames name, int skipCount)
		{
			for (int i = 0; i < Count.HeaderCount; i++)
			{
				if (Headers[i].HeaderName == name)
				{
					if (skipCount == 0)
						return i;
					skipCount--;
				}
			}

			return -1;
		}

		public IEnumerable<int> FindHeaderIndexes(HeaderNames name)
		{
			for (int i = 0; i < Count.HeaderCount; i++)
				if (Headers[i].HeaderName == name)
					yield return i;
		}

		public Header FindHeader(HeaderNames name)
		{
			for (int i = 0; i < Count.HeaderCount; i++)
				if (Headers[i].HeaderName == name)
					return Headers[i];

			throw new Exception(string.Format(@"Header {0} not found", name.ToString()));
		}
	}

	#endregion

	#region struct SipMessageReader.StatusCodeStruct {...}

	public partial class SipMessageReader : IDefaultValue
	{
		public partial struct StatusCodeStruct
		{
			private StatusCodes code;

			public bool IsInformational()
			{
				return (Value >= 100) && (Value <= 199);
			}

			public bool IsSuccessful()
			{
				return (Value >= 200) && (Value <= 299);
			}

			public bool IsGlobalFailure()
			{
				return (Value >= 600) && (Value <= 699);
			}

			public bool IsClientError()
			{
				return (Value >= 400) && (Value <= 499);
			}

			public StatusCodes Code
			{
				get
				{
					if (code == StatusCodes.None && Value >= 0)
					{
						if (Enum.IsDefined(typeof(StatusCodes), Value))
							code = (StatusCodes)Value;
						else
						{
							int baseCode = Value - Value % 100;
							if (Enum.IsDefined(typeof(StatusCodes), baseCode))
								code = (StatusCodes)baseCode;
						}
					}

					return code;
				}
				set
				{
					code = value;
					Value = (int)code;
				}
			}

			partial void SetDefaultValueEx()
			{
				code = StatusCodes.None;
			}

			public int CompareClassTo(StatusCodes code)
			{
				return CompareClassTo((int)code);
			}

			public int CompareClassTo(int value)
			{
				int value1 = Value / 100;
				int value2 = value / 100;

				if (value1 < value2)
					return -1;

				return (value1 == value2) ? 0 : 1;
			}

			public new string ToString()
			{
				return Value.ToString();
			}

			public static implicit operator StatusCodes(StatusCodeStruct statusCodeStruct)
			{
				return statusCodeStruct.Code;
			}
		}
	}

	#endregion

	/// <summary>
	/// this class should be removed
	/// </summary>
	public static class SipMessageReaderExtension
	{
		public static IPAddress IP(this ByteArrayPart part)
		{
			IPAddress ip;

			if (part.IsValid == true)
			{
				if (IPAddress.TryParse(part.ToString(), out ip) == false)
				{
					ip = IPAddress.None;
				}
			}
			else
			{
				ip = IPAddress.None;
			}

			return ip;
		}
	}
}