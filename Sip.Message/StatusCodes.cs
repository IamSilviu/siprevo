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
		SIPVersionNotSupported = 505,
		MessageTooLarge = 513,

		BusyEverywhere = 600,
		Decline = 603,
		DoesNotExistAnywhere = 604,
	};
}
