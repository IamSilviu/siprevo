using System;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Reflection;
using Base.Message;

namespace Sip.Message
{
	public enum AuthQops
	{
		None,
		Auth,
		AuthInt,
	}

	#region struct Addrspec {...}

	public partial struct Addrspec
	{
		public bool IsValid
		{
			get { return Hostport.Host.IsValid; }
		}
	}

	#endregion

	#region struct Fromto {...}

	public partial struct Fromto
	{
		public Addrspec AddrSpec
		{
			get
			{
				if (AddrSpec1.Hostport.Host.IsValid)
					return AddrSpec1;
				return AddrSpec2;
			}
		}
	}

	#endregion

	#region struct Hostport {...}

	public partial struct Hostport
	{
		IPAddress ip;

		public IPAddress IP
		{
			get
			{
				if (ip == IPAddress.None)
					ip = Host.ToIPAddress();

				return ip;
			}
			set
			{
				ip = value;
				Host.SetDefaultValue();
			}
		}

		partial void OnSetDefaultValue()
		{
			ip = IPAddress.None;
		}
	}

	#endregion

	#region class SipMessageReader {...}

	public partial class SipMessageReader
	{
		public int GetExpires(int contact)
		{
			if (Contact[contact].Expires >= 0)
				return Contact[contact].Expires;
			if (Expires >= 0)
				return Expires;
			return 3600;
		}

		public static void InitializeAsync(Action<int> callback)
		{
			var reader = new SipMessageReader();

			reader.LoadTables();
			ThreadPool.QueueUserWorkItem((stateInfo) => { callback(reader.CompileParseMethod()); });
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
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Sip.Message.dfa");
		}

		partial void OnAfterParse()
		{
			//	From.SetValidAddrspec();
			//	To.SetValidAddrspec();

			if (IsFinal)
			{
				CorrectCounts();

				if (Method == Methods.None)
					Method = CSeq.Method;
			}
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

		private void CorrectCounts()
		{
			Count.ContactCount++;
			Count.RequireCount++;
			Count.ProxyRequireCount++;
			Count.ViaCount++;
			Count.RouteCount++;
			Count.RecordRouteCount++;
			Count.SupportedCount++;
			Count.AuthorizationCount++;
			Count.ProxyAuthorizationCount++;
			Count.WwwAuthenticateCount++;
			Count.ProxyAuthenticateCount++;
		}

		public int CountHeaders(HeaderNames name)
		{
			int count = 0;

			for (int i = 0; i < Count.HeaderCount; i++)
				if (Headers[i].HeaderName == name)
					count++;

			return count;
		}

		public HeaderNames ValidateMandatoryHeaders()
		{
			if (CallId.IsInvalid)
				return HeaderNames.CallId;

			if (CSeq.Method == Methods.None)
				return HeaderNames.CSeq;

			if (From.AddrSpec.Value.IsInvalid)
				return HeaderNames.From;

			if (To.AddrSpec.Value.IsInvalid)
				return HeaderNames.To;

			if (Via[0].CommaAndValue.IsInvalid)
				return HeaderNames.Via;

			return HeaderNames.None;
		}

		public HeaderNames ValidateHeadersDuplication()
		{
			ulong masks = 0;
			ulong validate = HeaderMasks.CallId | HeaderMasks.CSeq | HeaderMasks.From |
				HeaderMasks.To | HeaderMasks.ContentLength | HeaderMasks.MaxForwards;

			for (int i = 0; i < Count.HeaderCount; i++)
			{
				ulong mask = Headers[i].HeaderName.ToMask();

				if ((mask & validate) > 0)
				{
					if ((mask & masks) > 0)
						return Headers[i].HeaderName;

					masks |= mask;
				}
			}

			return HeaderNames.None;
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

		public bool IsExpiresValid(int minValue)
		{
			return Expires < 0 && Expires > minValue;
		}

		public bool IsExpiresTooBrief(int minValue)
		{
			return Expires > 0 && Expires <= minValue;
		}

		public int GetExpires(int defValue, int maxValue)
		{
			return (Expires < 0) ? defValue : ((Expires < maxValue) ? Expires : maxValue);
		}

		public Challenge? GetAnyChallenge()
		{
			if (Count.WwwAuthenticateCount > 0)
				return WwwAuthenticate[0];
			else if (Count.ProxyAuthenticateCount > 0)
				return ProxyAuthenticate[0];
			return null;
		}

		public bool TryGetCredentialsByRealm(AuthSchemes scheme, ByteArrayPart realm, out Credentials credentials)
		{
			for (int i = 0; i < Count.AuthorizationCount; i++)
			{
				if (Authorization[i].AuthScheme == scheme && Authorization[i].Realm.Equals(realm))
				{
					credentials = Authorization[i];
					return true;
				}
			}

			for (int i = 0; i < Count.ProxyAuthorizationCount; i++)
			{
				if (ProxyAuthorization[i].AuthScheme == scheme && ProxyAuthorization[i].Realm.Equals(realm))
				{
					credentials = ProxyAuthorization[i];
					return true;
				}
			}

			credentials = new Credentials();
			return false;
		}

		public Credentials GetCredentialsByRealm(AuthSchemes scheme, ByteArrayPart realm)
		{
			Credentials credentials;
			TryGetCredentialsByRealm(scheme, realm, out credentials);

			return credentials;
		}

		public bool TryGetCredentialsByTargetname(AuthSchemes scheme, ByteArrayPart targetname, out Credentials credentials, out bool proxy)
		{
			int length = targetname.Length + ((scheme == AuthSchemes.Kerberos) ? 4 : 0);

			for (int i = 0; i < Count.AuthorizationCount; i++)
			{
				if (Authorization[i].AuthScheme == scheme)
					if (Authorization[i].Targetname.Length == length && Authorization[i].Targetname.EndWith(targetname))
					{
						credentials = Authorization[i];
						proxy = false;
						return true;
					}
			}

			for (int i = 0; i < Count.ProxyAuthorizationCount; i++)
			{
				if (ProxyAuthorization[i].AuthScheme == scheme)
					if (ProxyAuthorization[i].Targetname.Length == length && ProxyAuthorization[i].Targetname.EndWith(targetname))
					{
						credentials = ProxyAuthorization[i];
						proxy = true;
						return true;
					}
			}

			credentials = new Credentials();
			proxy = false;
			return false;
		}

		public Credentials GetCredentialsByTargetname(AuthSchemes scheme, ByteArrayPart targetname, out bool proxy)
		{
			Credentials credentials;
			TryGetCredentialsByTargetname(scheme, targetname, out credentials, out proxy);

			return credentials;
		}
	}

	#endregion

	#region struct SipMessageReader.ContactStruct {...}

	public partial class SipMessageReader
	{
		public partial struct ContactStruct
		{
			public Addrspec AddrSpec
			{
				get
				{
					if (AddrSpec1.Hostport.Host.IsValid)
						return AddrSpec1;
					return AddrSpec2;
				}
			}

			public bool HasProxyReplace
			{
				get { return ProxyReplace.IsValid; }
			}
		}
	}

	#endregion

	#region struct SipMessageReader.StatusCodeStruct {...}

	public partial class SipMessageReader
	{
		public partial struct StatusCodeStruct
		{
			private StatusCodes code;

			public bool IsInformational
			{
				get { return (Value >= 100) && (Value <= 199); }
			}

			public bool Is1xx
			{
				get { return (Value >= 100) && (Value <= 199); }
			}

			public bool IsSuccessful
			{
				get { return (Value >= 200) && (Value <= 299); }
			}

			public bool Is2xx
			{
				get { return (Value >= 200) && (Value <= 299); }
			}

			public bool IsGlobalFailure
			{
				get { return (Value >= 600) && (Value <= 699); }
			}

			public bool IsClientError
			{
				get { return (Value >= 400) && (Value <= 499); }
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

			partial void OnSetDefaultValue()
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

	#region struct SipMessageReader.UserAgentStruct {...}

	public enum UserAgents
	{
		None,
		XLite,
		Nch,
		Unknown,
	}

	public partial class SipMessageReader
	{
		public partial struct UserAgentStruct
		{
			private static readonly byte[] xlite = Encoding.UTF8.GetBytes("X-Lite");
			private static readonly byte[] nch = Encoding.UTF8.GetBytes("NCH Software");

			private UserAgents userAgent;

			public UserAgents Defined
			{
				get
				{
					if (userAgent == UserAgents.None)
					{
						if (Product.StartsWith(xlite))
							userAgent = UserAgents.XLite;
						else if (Product.StartsWith(nch))
							userAgent = UserAgents.Nch;
						else
							userAgent = UserAgents.Unknown;
					}

					return userAgent;
				}
			}

			public bool IsXLite
			{
				get { return Defined == UserAgents.XLite; }
			}

			public bool IsNch
			{
				get { return Defined == UserAgents.Nch; }
			}

			partial void OnSetDefaultValue()
			{
				userAgent = UserAgents.None;
			}
		}
	}

	#endregion
}