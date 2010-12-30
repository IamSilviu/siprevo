using System;

namespace Sip.Message
{
	public class HeaderMasks
	{
		public const ulong Extension = 0x0000000000000001;
		public const ulong ContentType = 0x0000000000000002;
		public const ulong ContentEncoding = 0x0000000000000004;
		public const ulong From = 0x0000000000000008;
		public const ulong CallId = 0x0000000000000010;
		public const ulong Supported = 0x0000000000000020;
		public const ulong ContentLength = 0x0000000000000040;
		public const ulong Contact = 0x0000000000000080;
		public const ulong Event = 0x0000000000000100;
		public const ulong Subject = 0x0000000000000200;
		public const ulong To = 0x0000000000000400;
		public const ulong AllowEvents = 0x0000000000000800;
		public const ulong Via = 0x0000000000001000;
		public const ulong CSeq = 0x0000000000002000;
		public const ulong Date = 0x0000000000004000;
		public const ulong Allow = 0x0000000000008000;
		public const ulong Route = 0x0000000000010000;
		public const ulong Accept = 0x0000000000020000;
		public const ulong Server = 0x0000000000040000;
		public const ulong Require = 0x0000000000080000;
		public const ulong Warning = 0x0000000000100000;
		public const ulong Priority = 0x0000000000200000;
		public const ulong ReplyTo = 0x0000000000400000;
		public const ulong CallInfo = 0x0000000000800000;
		public const ulong Timestamp = 0x0000000001000000;
		public const ulong AlertInfo = 0x0000000002000000;
		public const ulong ErrorInfo = 0x0000000004000000;
		public const ulong UserAgent = 0x0000000008000000;
		public const ulong InReplyTo = 0x0000000010000000;
		public const ulong MinExpires = 0x0000000020000000;
		public const ulong RetryAfter = 0x0000000040000000;
		public const ulong Unsupported = 0x0000000080000000;
		public const ulong MaxForwards = 0x0000000100000000;
		public const ulong MimeVersion = 0x0000000200000000;
		public const ulong Organization = 0x0000000400000000;
		public const ulong RecordRoute = 0x0000000800000000;
		public const ulong Authorization = 0x0000001000000000;
		public const ulong ProxyRequire = 0x0000002000000000;
		public const ulong AcceptEncoding = 0x0000004000000000;
		public const ulong AcceptLanguage = 0x0000008000000000;
		public const ulong ContentLanguage = 0x0000010000000000;
		public const ulong WwwAuthenticate = 0x0000020000000000;
		public const ulong ProxyAuthenticate = 0x0000040000000000;
		public const ulong SubscriptionState = 0x0000080000000000;
		public const ulong AuthenticationInfo = 0x0000100000000000;
		public const ulong ContentDisposition = 0x0000200000000000;
		public const ulong ProxyAuthorization = 0x0000400000000000;
		public const ulong ProxyAuthenticationInfo = 0x0000800000000000;
	}

	static class HeaderMasksHelper
	{
		public static bool IsMasked(this HeaderNames name, ulong mask)
		{
			return (mask & name.ToMask()) > 0;
		}

		public static ulong ToMask(this HeaderNames name)
		{
			switch (name)
			{
				case HeaderNames.Extension: return HeaderMasks.Extension;
				case HeaderNames.ContentType: return HeaderMasks.ContentType;
				case HeaderNames.ContentEncoding: return HeaderMasks.ContentEncoding;
				case HeaderNames.From: return HeaderMasks.From;
				case HeaderNames.CallId: return HeaderMasks.CallId;
				case HeaderNames.Supported: return HeaderMasks.Supported;
				case HeaderNames.ContentLength: return HeaderMasks.ContentLength;
				case HeaderNames.Contact: return HeaderMasks.Contact;
				case HeaderNames.Event: return HeaderMasks.Event;
				case HeaderNames.Subject: return HeaderMasks.Subject;
				case HeaderNames.To: return HeaderMasks.To;
				case HeaderNames.AllowEvents: return HeaderMasks.AllowEvents;
				case HeaderNames.Via: return HeaderMasks.Via;
				case HeaderNames.CSeq: return HeaderMasks.CSeq;
				case HeaderNames.Date: return HeaderMasks.Date;
				case HeaderNames.Allow: return HeaderMasks.Allow;
				case HeaderNames.Route: return HeaderMasks.Route;
				case HeaderNames.Accept: return HeaderMasks.Accept;
				case HeaderNames.Server: return HeaderMasks.Server;
				case HeaderNames.Require: return HeaderMasks.Require;
				case HeaderNames.Warning: return HeaderMasks.Warning;
				case HeaderNames.Priority: return HeaderMasks.Priority;
				case HeaderNames.ReplyTo: return HeaderMasks.ReplyTo;
				case HeaderNames.CallInfo: return HeaderMasks.CallInfo;
				case HeaderNames.Timestamp: return HeaderMasks.Timestamp;
				case HeaderNames.AlertInfo: return HeaderMasks.AlertInfo;
				case HeaderNames.ErrorInfo: return HeaderMasks.ErrorInfo;
				case HeaderNames.UserAgent: return HeaderMasks.UserAgent;
				case HeaderNames.InReplyTo: return HeaderMasks.InReplyTo;
				case HeaderNames.MinExpires: return HeaderMasks.MinExpires;
				case HeaderNames.RetryAfter: return HeaderMasks.RetryAfter;
				case HeaderNames.Unsupported: return HeaderMasks.Unsupported;
				case HeaderNames.MaxForwards: return HeaderMasks.MaxForwards;
				case HeaderNames.MimeVersion: return HeaderMasks.MimeVersion;
				case HeaderNames.Organization: return HeaderMasks.Organization;
				case HeaderNames.RecordRoute: return HeaderMasks.RecordRoute;
				case HeaderNames.Authorization: return HeaderMasks.Authorization;
				case HeaderNames.ProxyRequire: return HeaderMasks.ProxyRequire;
				case HeaderNames.AcceptEncoding: return HeaderMasks.AcceptEncoding;
				case HeaderNames.AcceptLanguage: return HeaderMasks.AcceptLanguage;
				case HeaderNames.ContentLanguage: return HeaderMasks.ContentLanguage;
				case HeaderNames.WwwAuthenticate: return HeaderMasks.WwwAuthenticate;
				case HeaderNames.ProxyAuthenticate: return HeaderMasks.ProxyAuthenticate;
				case HeaderNames.SubscriptionState: return HeaderMasks.SubscriptionState;
				case HeaderNames.AuthenticationInfo: return HeaderMasks.AuthenticationInfo;
				case HeaderNames.ContentDisposition: return HeaderMasks.ContentDisposition;
				case HeaderNames.ProxyAuthorization: return HeaderMasks.ProxyAuthorization;
				case HeaderNames.ProxyAuthenticationInfo: return HeaderMasks.ProxyAuthenticationInfo;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
