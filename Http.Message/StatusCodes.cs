using System;
using System.Text;
using Base.Message;

namespace Http.Message
{
	public enum StatusCodes
	{
		None = -1,

		Continue = 100,
		SwitchingProtocols = 101,
		OK = 200,
		Created = 201,
		Accepted = 202,
		NonAuthoritativeInformation = 203,
		NoContent = 204,
		ResetContent = 205,
		PartialContent = 206,
		MultipleChoices = 300,
		MovedPermanently = 301,
		Found = 302,
		SeeOther = 303,
		NotModified = 304,
		UseProxy = 305,
		TemporaryRedirect = 307,
		BadRequest = 400,
		Unauthorized = 401,
		PaymentRequired = 402,
		Forbidden = 403,
		NotFound = 404,
		MethodNotAllowed = 405,
		NotAcceptable = 406,
		ProxyAuthenticationRequired = 407,
		RequestTimeout = 408,
		Conflict = 409,
		Gone = 410,
		LengthRequired = 411,
		PreconditionFailed = 412,
		RequestEntityTooLarge = 413,
		RequestUriTooLong = 414,
		UnsupportedMediaType = 415,
		RequestedRangeNotSatisfiable = 416,
		ExpectationFailed = 417,
		InternalServerError = 500,
		NotImplemented = 501,
		BadGateway = 502,
		ServiceUnavailable = 503,
		GatewayTimeout = 504,
		HttpVersionNotSupported = 505,
	}

	public static class StatusCodesConverter
	{
		public readonly static byte[] Default = Create(@"Error");

		public readonly static byte[] Continue = Create("Continue");
		public readonly static byte[] SwitchingProtocols = Create("Switching Protocols");
		public readonly static byte[] OK = Create("OK");
		public readonly static byte[] Created = Create("Created");
		public readonly static byte[] Accepted = Create("Accepted");
		public readonly static byte[] NonAuthoritativeInformation = Create("Non-Authoritative Information");
		public readonly static byte[] NoContent = Create("No Content");
		public readonly static byte[] ResetContent = Create("Reset Content");
		public readonly static byte[] PartialContent = Create("Partial Content");
		public readonly static byte[] MultipleChoices = Create("Multiple Choices");
		public readonly static byte[] MovedPermanently = Create("Moved Permanently");
		public readonly static byte[] Found = Create("Found");
		public readonly static byte[] SeeOther = Create("See Other");
		public readonly static byte[] NotModified = Create("Not Modified");
		public readonly static byte[] UseProxy = Create("Use Proxy");
		public readonly static byte[] TemporaryRedirect = Create("Temporary Redirect");
		public readonly static byte[] BadRequest = Create("Bad Request");
		public readonly static byte[] Unauthorized = Create("Unauthorized");
		public readonly static byte[] PaymentRequired = Create("Payment Required");
		public readonly static byte[] Forbidden = Create("Forbidden");
		public readonly static byte[] NotFound = Create("Not Found");
		public readonly static byte[] MethodNotAllowed = Create("Method Not Allowed");
		public readonly static byte[] NotAcceptable = Create("Not Acceptable");
		public readonly static byte[] ProxyAuthenticationRequired = Create("Proxy Authentication Required");
		public readonly static byte[] RequestTimeout = Create("Request Timeout");
		public readonly static byte[] Conflict = Create("Conflict");
		public readonly static byte[] Gone = Create("Gone");
		public readonly static byte[] LengthRequired = Create("Length Required");
		public readonly static byte[] PreconditionFailed = Create("Precondition Failed");
		public readonly static byte[] RequestEntityTooLarge = Create("Request Entity Too Large");
		public readonly static byte[] RequestUriTooLong = Create("Request-URI Too Long");
		public readonly static byte[] UnsupportedMediaType = Create("Unsupported Media Type");
		public readonly static byte[] RequestedRangeNotSatisfiable = Create("Requested Range Not Satisfiable");
		public readonly static byte[] ExpectationFailed = Create("Expectation Failed");
		public readonly static byte[] InternalServerError = Create("Internal Server Error");
		public readonly static byte[] NotImplemented = Create("Not Implemented");
		public readonly static byte[] BadGateway = Create("Bad Gateway");
		public readonly static byte[] ServiceUnavailable = Create("Service Unavailable");
		public readonly static byte[] GatewayTimeout = Create("Gateway Timeout");
		public readonly static byte[] HttpVersionNotSupported = Create("HTTP Version Not Supported");

		public static byte[] GetReason(this StatusCodes statusCode)
		{
			switch (statusCode)
			{
				case StatusCodes.Continue: return Continue;
				case StatusCodes.SwitchingProtocols: return SwitchingProtocols;
				case StatusCodes.OK: return OK;
				case StatusCodes.Created: return Created;
				case StatusCodes.Accepted: return Accepted;
				case StatusCodes.NonAuthoritativeInformation: return NonAuthoritativeInformation;
				case StatusCodes.NoContent: return NoContent;
				case StatusCodes.ResetContent: return ResetContent;
				case StatusCodes.PartialContent: return PartialContent;
				case StatusCodes.MultipleChoices: return MultipleChoices;
				case StatusCodes.MovedPermanently: return MovedPermanently;
				case StatusCodes.Found: return Found;
				case StatusCodes.SeeOther: return SeeOther;
				case StatusCodes.NotModified: return NotModified;
				case StatusCodes.UseProxy: return UseProxy;
				case StatusCodes.TemporaryRedirect: return TemporaryRedirect;
				case StatusCodes.BadRequest: return BadRequest;
				case StatusCodes.Unauthorized: return Unauthorized;
				case StatusCodes.PaymentRequired: return PaymentRequired;
				case StatusCodes.Forbidden: return Forbidden;
				case StatusCodes.NotFound: return NotFound;
				case StatusCodes.MethodNotAllowed: return MethodNotAllowed;
				case StatusCodes.NotAcceptable: return NotAcceptable;
				case StatusCodes.ProxyAuthenticationRequired: return ProxyAuthenticationRequired;
				case StatusCodes.RequestTimeout: return RequestTimeout;
				case StatusCodes.Conflict: return Conflict;
				case StatusCodes.Gone: return Gone;
				case StatusCodes.LengthRequired: return LengthRequired;
				case StatusCodes.PreconditionFailed: return PreconditionFailed;
				case StatusCodes.RequestEntityTooLarge: return RequestEntityTooLarge;
				case StatusCodes.RequestUriTooLong: return RequestUriTooLong;
				case StatusCodes.UnsupportedMediaType: return UnsupportedMediaType;
				case StatusCodes.RequestedRangeNotSatisfiable: return RequestedRangeNotSatisfiable;
				case StatusCodes.ExpectationFailed: return ExpectationFailed;
				case StatusCodes.InternalServerError: return InternalServerError;
				case StatusCodes.NotImplemented: return NotImplemented;
				case StatusCodes.BadGateway: return BadGateway;
				case StatusCodes.ServiceUnavailable: return ServiceUnavailable;
				case StatusCodes.GatewayTimeout: return GatewayTimeout;
				case StatusCodes.HttpVersionNotSupported: return HttpVersionNotSupported;
				default:
					return Default;
			}
		}

		public static byte[] Create(string text)
		{
			return Encoding.UTF8.GetBytes(text);
		}
	}

}
