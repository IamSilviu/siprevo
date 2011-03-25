using System;

namespace Sip.Message
{
	public enum StatusCodes
	{
		None = -1,

		Trying = 100,
		Ringing = 180,
		CallIsBeingForwarded = 181,
		Queued = 182,
		SessionProgress = 183,

		OK = 200,

		MultipleChoices = 300,
		MovedPermanently = 301,
		MovedTemporarily = 302,
		UseProxy = 305,
		AlternativeService = 380,

		BadRequest = 400,
		Unauthorized = 401,
		PaymentRequired = 402,
		Forbidden = 403,
		NotFound = 404,
		MethodNotAllowed = 405,
		NotAcceptable = 406,
		ProxyAuthenticationRequired = 407,
		RequestTimeout = 408,
		Gone = 410,
		RequestEntityTooLarge = 413,
		RequestURITooLarge = 414,
		UnsupportedMediaType = 415,
		UnsupportedURIScheme = 416,
		BadExtension = 420,
		ExtensionRequired = 421,
		IntervalTooBrief = 423,
		TemporarilyUnavailable = 480,
		CallLegTransactionDoesNotExist = 481,
		LoopDetected = 482,
		TooManyHops = 483,
		AddressIncomplete = 484,
		Ambiguous = 485,
		BusyHere = 486,
		RequestTerminated = 487,
		NotAcceptableHere = 488,
		RequestPending = 491,
		Undecipherable = 493,

		InternalServerError = 500,
		NotImplemented = 501,
		BadGateway = 502,
		ServiceUnavailable = 503,
		ServerTimeOut = 504,
		SipVersionNotSupported = 505,
		MessageTooLarge = 513,

		BusyEverywhere = 600,
		Decline = 603,
		DoesNotExistAnywhere = 604,
	};

	public static class StatusCodesConverter
	{
		public readonly static ByteArrayPart NoReason = new ByteArrayPart(@"Error");

		public readonly static ByteArrayPart Trying = new ByteArrayPart(@"Trying");
		public readonly static ByteArrayPart Ringing = new ByteArrayPart(@"Ringing");
		public readonly static ByteArrayPart CallIsBeingForwarded = new ByteArrayPart(@"Call Is Being Forwarded");
		public readonly static ByteArrayPart Queued = new ByteArrayPart(@"Queued");
		public readonly static ByteArrayPart SessionProgress = new ByteArrayPart(@"Session Progress");
		public readonly static ByteArrayPart OK = new ByteArrayPart(@"OK");
		public readonly static ByteArrayPart MultipleChoices = new ByteArrayPart(@"Multiple Choices");
		public readonly static ByteArrayPart MovedPermanently = new ByteArrayPart(@"Moved Permanently");
		public readonly static ByteArrayPart MovedTemporarily = new ByteArrayPart(@"Moved Temporarily");
		public readonly static ByteArrayPart UseProxy = new ByteArrayPart(@"Use Proxy");
		public readonly static ByteArrayPart AlternativeService = new ByteArrayPart(@"Alternative Service");
		public readonly static ByteArrayPart BadRequest = new ByteArrayPart(@"Bad Request");
		public readonly static ByteArrayPart Unauthorized = new ByteArrayPart(@"Unauthorized");
		public readonly static ByteArrayPart PaymentRequired = new ByteArrayPart(@"Payment Required");
		public readonly static ByteArrayPart Forbidden = new ByteArrayPart(@"Forbidden");
		public readonly static ByteArrayPart NotFound = new ByteArrayPart(@"Not Found");
		public readonly static ByteArrayPart MethodNotAllowed = new ByteArrayPart(@"Method Not Allowed");
		public readonly static ByteArrayPart NotAcceptable = new ByteArrayPart(@"Not Acceptable");
		public readonly static ByteArrayPart ProxyAuthenticationRequired = new ByteArrayPart(@"Proxy Authentication Required");
		public readonly static ByteArrayPart RequestTimeout = new ByteArrayPart(@"Request Timeout");
		public readonly static ByteArrayPart Gone = new ByteArrayPart(@"Gone");
		public readonly static ByteArrayPart RequestEntityTooLarge = new ByteArrayPart(@"Request Entity Too Large");
		public readonly static ByteArrayPart RequestURITooLarge = new ByteArrayPart(@"Request URI Too Large");
		public readonly static ByteArrayPart UnsupportedMediaType = new ByteArrayPart(@"Unsupported Media Type");
		public readonly static ByteArrayPart UnsupportedURIScheme = new ByteArrayPart(@"Unsupported URI Scheme");
		public readonly static ByteArrayPart BadExtension = new ByteArrayPart(@"Bad Extension");
		public readonly static ByteArrayPart ExtensionRequired = new ByteArrayPart(@"Extension Required");
		public readonly static ByteArrayPart IntervalTooBrief = new ByteArrayPart(@"Interval Too Brief");
		public readonly static ByteArrayPart TemporarilyUnavailable = new ByteArrayPart(@"Temporarily Unavailable");
		public readonly static ByteArrayPart CallLegTransactionDoesNotExist = new ByteArrayPart(@"Call Leg Transaction Does Not Exist");
		public readonly static ByteArrayPart LoopDetected = new ByteArrayPart(@"Loop Detected");
		public readonly static ByteArrayPart TooManyHops = new ByteArrayPart(@"Too Many Hops");
		public readonly static ByteArrayPart AddressIncomplete = new ByteArrayPart(@"Address Incomplete");
		public readonly static ByteArrayPart Ambiguous = new ByteArrayPart(@"Ambiguous");
		public readonly static ByteArrayPart BusyHere = new ByteArrayPart(@"Busy Here");
		public readonly static ByteArrayPart RequestTerminated = new ByteArrayPart(@"Request Terminated");
		public readonly static ByteArrayPart NotAcceptableHere = new ByteArrayPart(@"Not AcceptableHere");
		public readonly static ByteArrayPart RequestPending = new ByteArrayPart(@"Request Pending");
		public readonly static ByteArrayPart Undecipherable = new ByteArrayPart(@"Undecipherable");
		public readonly static ByteArrayPart InternalServerError = new ByteArrayPart(@"Internal Server Error");
		public readonly static ByteArrayPart NotImplemented = new ByteArrayPart(@"Not Implemented");
		public readonly static ByteArrayPart BadGateway = new ByteArrayPart(@"Bad Gateway");
		public readonly static ByteArrayPart ServiceUnavailable = new ByteArrayPart(@"Service Unavailable");
		public readonly static ByteArrayPart ServerTimeOut = new ByteArrayPart(@"Server Time Out");
		public readonly static ByteArrayPart SipVersionNotSupported = new ByteArrayPart(@"SIP Version Not Supported");
		public readonly static ByteArrayPart MessageTooLarge = new ByteArrayPart(@"Message Too Large");
		public readonly static ByteArrayPart BusyEverywhere = new ByteArrayPart(@"Busy Everywhere");
		public readonly static ByteArrayPart Decline = new ByteArrayPart(@"Decline");
		public readonly static ByteArrayPart DoesNotExistAnywhere = new ByteArrayPart(@"Does Not Exist Anywhere");

		public static ByteArrayPart GetReason(this StatusCodes statusCode)
		{
			switch (statusCode)
			{
				case StatusCodes.Trying:
					return Trying;
				case StatusCodes.Ringing:
					return Ringing;
				case StatusCodes.CallIsBeingForwarded:
					return CallIsBeingForwarded;
				case StatusCodes.Queued:
					return Queued;
				case StatusCodes.SessionProgress:
					return SessionProgress;
				case StatusCodes.OK:
					return OK;
				case StatusCodes.MultipleChoices:
					return MultipleChoices;
				case StatusCodes.MovedPermanently:
					return MovedPermanently;
				case StatusCodes.MovedTemporarily:
					return MovedTemporarily;
				case StatusCodes.UseProxy:
					return UseProxy;
				case StatusCodes.AlternativeService:
					return AlternativeService;
				case StatusCodes.BadRequest:
					return BadRequest;
				case StatusCodes.Unauthorized:
					return Unauthorized;
				case StatusCodes.PaymentRequired:
					return PaymentRequired;
				case StatusCodes.Forbidden:
					return Forbidden;
				case StatusCodes.NotFound:
					return NotFound;
				case StatusCodes.MethodNotAllowed:
					return MethodNotAllowed;
				case StatusCodes.NotAcceptable:
					return NotAcceptable;
				case StatusCodes.ProxyAuthenticationRequired:
					return ProxyAuthenticationRequired;
				case StatusCodes.RequestTimeout:
					return RequestTimeout;
				case StatusCodes.Gone:
					return Gone;
				case StatusCodes.RequestEntityTooLarge:
					return RequestEntityTooLarge;
				case StatusCodes.RequestURITooLarge:
					return RequestURITooLarge;
				case StatusCodes.UnsupportedMediaType:
					return UnsupportedMediaType;
				case StatusCodes.UnsupportedURIScheme:
					return UnsupportedURIScheme;
				case StatusCodes.BadExtension:
					return BadExtension;
				case StatusCodes.ExtensionRequired:
					return ExtensionRequired;
				case StatusCodes.IntervalTooBrief:
					return IntervalTooBrief;
				case StatusCodes.TemporarilyUnavailable:
					return TemporarilyUnavailable;
				case StatusCodes.CallLegTransactionDoesNotExist:
					return CallLegTransactionDoesNotExist;
				case StatusCodes.LoopDetected:
					return LoopDetected;
				case StatusCodes.TooManyHops:
					return TooManyHops;
				case StatusCodes.AddressIncomplete:
					return AddressIncomplete;
				case StatusCodes.Ambiguous:
					return Ambiguous;
				case StatusCodes.BusyHere:
					return BusyHere;
				case StatusCodes.RequestTerminated:
					return RequestTerminated;
				case StatusCodes.NotAcceptableHere:
					return NotAcceptableHere;
				case StatusCodes.RequestPending:
					return RequestPending;
				case StatusCodes.Undecipherable:
					return Undecipherable;
				case StatusCodes.InternalServerError:
					return InternalServerError;
				case StatusCodes.NotImplemented:
					return NotImplemented;
				case StatusCodes.BadGateway:
					return BadGateway;
				case StatusCodes.ServiceUnavailable:
					return ServiceUnavailable;
				case StatusCodes.ServerTimeOut:
					return ServerTimeOut;
				case StatusCodes.SipVersionNotSupported:
					return SipVersionNotSupported;
				case StatusCodes.MessageTooLarge:
					return MessageTooLarge;
				case StatusCodes.BusyEverywhere:
					return BusyEverywhere;
				case StatusCodes.Decline:
					return Decline;
				case StatusCodes.DoesNotExistAnywhere:
					return DoesNotExistAnywhere;

				case StatusCodes.None:
					throw new ArgumentOutOfRangeException(@"statusCode: " + statusCode.ToString());
				default:
					return NoReason;
			}
		}
	}
}
