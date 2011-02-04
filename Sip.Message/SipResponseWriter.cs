using System;

namespace Sip.Message
{
	public class SipResponseWriter
		: SipMessageWriter
	{
		public SipResponseWriter()
		{
		}

		public SipResponseWriter(int size)
			: base(size)
		{
		}

		public void WriteResponse(SipMessageReader request, StatusCodes statusCode)
		{
			WriteResponse(request, statusCode, statusCode.GetReason());
		}

		public void WriteStatusLineToTop(StatusCodes statusCode)
		{
			WriteStatusLineToTop(statusCode, statusCode.GetReason());
		}

		public void WriteStatusLineToTop(StatusCodes statusCode, ByteArrayPart reason)
		{
			WriteToTop(C.CRLF);
			WriteToTop(reason, 100);
			WriteToTop(C.SP);
			WriteToTop((uint)statusCode);
			WriteToTop(C.SP);
			WriteToTop(C.SIP_2_0);
		}

		public void WriteResponse(SipMessageReader request, StatusCodes statusCode, ByteArrayPart reasonPhrase)
		{
			WriteStatusLine(statusCode, reasonPhrase);

			int viaCount = 0;
			for (int i = 0; i < request.Count.HeaderCount; i++)
			{
				switch (request.Headers[i].HeaderName)
				{
					case HeaderNames.CallId:
					case HeaderNames.From:
					case HeaderNames.RecordRoute:
					case HeaderNames.CSeq:
						WriteHeader(request.Headers[i]);
						break;

					case HeaderNames.To:
						if (request.To.Tag.IsValid || statusCode == StatusCodes.Trying || request.Method == Methods.Cancelm)
							WriteHeader(request.Headers[i]);
						else
							WriteHeaderWithTag(request.Headers[i], GenerateTag());
						break;

					case HeaderNames.Via:
						// здесь ошибка!!!
						WriteVia(request.Headers[i], request.Via[viaCount++]);
						break;
				}
			}

			WriteContentLength(0);
			//WriteCRLF();
		}

		public void WriteResponseHeaders(SipMessageReader request, StatusCodes statusCode)
		{
			WriteResponseHeaders(request, statusCode, GenerateTag());
		}

		public void WriteResponseHeaders(SipMessageReader request, StatusCodes statusCode, ByteArrayPart totag)
		{
			int viaCount = 0;
			for (int i = 0; i < request.Count.HeaderCount; i++)
			{
				switch (request.Headers[i].HeaderName)
				{
					case HeaderNames.CallId:
					case HeaderNames.From:
					case HeaderNames.RecordRoute:
					case HeaderNames.CSeq:
						WriteHeader(request.Headers[i]);
						break;

					case HeaderNames.To:
						if (request.To.Tag.IsValid || statusCode == StatusCodes.Trying || request.Method == Methods.Cancelm)
							WriteHeader(request.Headers[i]);
						else
							WriteHeaderWithTag(request.Headers[i], totag);
						break;

					case HeaderNames.Via:
						// здесь ошибка!!!
						WriteVia(request.Headers[i], request.Via[viaCount++]);
						break;
				}
			}
		}

		public void WriteHeaders(SipMessageReader reader, ulong exclude)
		{
			if (HeaderNames.Via.IsMasked(exclude) == false)
			{		for (int i = 0; i < reader.Count.ViaCount; i++)
				if (reader.Via[i].IsRemoved == false)
				{
					reader.Via[i].CommaAndValue.TrimStartComma();
					reader.Via[i].CommaAndValue.TrimSws();
					Write(C.Via, C.HCOLON, C.SP, reader.Via[i].CommaAndValue, C.CRLF);
				}
			}

			if (HeaderNames.Route.IsMasked(exclude) == false)
			{
				for (int i = 0; i < reader.Count.RouteCount; i++)
					if (reader.Route[i].IsRemoved == false)
					{
						reader.Route[i].CommaAndValue.TrimStartComma();
						reader.Route[i].CommaAndValue.TrimSws();
						Write(C.Route, C.HCOLON, C.SP, reader.Route[i].CommaAndValue, C.CRLF);
					}
			}

			if (HeaderNames.RecordRoute.IsMasked(exclude) == false)
			{
				for (int i = 0; i < reader.Count.RecordRouteCount; i++)
					if (reader.RecordRoute[i].IsRemoved == false)
					{
						reader.RecordRoute[i].CommaAndValue.TrimStartComma();
						reader.RecordRoute[i].CommaAndValue.TrimSws();
						Write(C.RecordRoute, C.HCOLON, C.SP, reader.RecordRoute[i].CommaAndValue, C.CRLF);
					}
			}

			for (int i = 0; i < reader.Count.HeaderCount; i++)
			{
				switch (reader.Headers[i].HeaderName)
				{
					case HeaderNames.Via:
					case HeaderNames.Route:
					case HeaderNames.RecordRoute:
						break;

					default:
						if (reader.Headers[i].HeaderName.IsMasked(exclude))
							break;

						WriteHeader(reader.Headers[i]);
						break;
				}
			}
		}

		public void WriteSipEtag(int value)
		{
			Write(C.SIP_ETag__);
			WriteAsHex8(value);
			Write(C.CRLF);
		}
	}
}
