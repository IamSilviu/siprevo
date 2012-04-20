using System;
using Base.Message;

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

		public void WriteResponse(SipMessageReader request, StatusCodes statusCode)
		{
			WriteResponse(request, statusCode, GenerateTag());
		}

		public void WriteResponse(SipMessageReader request, StatusCodes statusCode, ByteArrayPart localTag)
		{
			WriteStatusLine(statusCode);
			CopyViaToFromCallIdRecordRouteCSeq(request, statusCode, localTag);
			WriteContentLength(0);
			WriteCustomHeaders();
			WriteCRLF();
		}

		public void CopyViaToFromCallIdRecordRouteCSeq(SipMessageReader request, StatusCodes statusCode)
		{
			CopyViaToFromCallIdRecordRouteCSeq(request, statusCode, GenerateTag());
		}

		public void CopyViaToFromCallIdRecordRouteCSeq(SipMessageReader request, StatusCodes statusCode, ByteArrayPart localTag)
		{
			for (int i = 0; i < request.Count.HeaderCount; i++)
			{
				switch (request.Headers[i].HeaderName)
				{
					case HeaderNames.RecordRoute:
					case HeaderNames.Via:
						WriteHeader(request.Headers[i]);
						break;

					case HeaderNames.CallId:
						WriteCallId(request.CallId);
						break;

					case HeaderNames.CSeq:
						CSeq = request.CSeq.Value;
						Method = request.CSeq.Method;
						WriteHeader(request.Headers[i]);
						break;

					case HeaderNames.From:
						Write(C.From__);

						fromAddrspec = new Range(end + request.From.AddrSpec.Value.Begin -
							request.Headers[i].Value.Begin, request.From.AddrSpec.Value.Length);

						if (request.From.Tag.IsValid)
							fromTag = new Range(end + request.From.Tag.Begin -
								request.Headers[i].Value.Begin, request.From.Tag.Length);

						if (request.From.Epid.IsValid)
							fromEpid = new Range(end + request.From.Epid.Begin -
								request.Headers[i].Value.Begin, request.From.Epid.Length);

						Write(request.Headers[i].Value);
						WriteCRLF();
						break;

					case HeaderNames.To:
						Write(C.To__);

						toAddrspec = new Range(end + request.To.AddrSpec.Value.Begin -
							request.Headers[i].Value.Begin, request.To.AddrSpec.Value.Length);

						if (request.To.Tag.IsValid)
							toTag = new Range(end + request.To.Tag.Begin -
								request.Headers[i].Value.Begin, request.To.Tag.Length);

						Write(request.Headers[i].Value);

						if (request.To.Tag.IsInvalid && localTag.IsValid && statusCode != StatusCodes.Trying && request.Method != Methods.Cancelm)
						{
							Write(C._tag_);
							toTag = new Range(end, localTag.Length);
							Write(localTag);
						}

						WriteCRLF();
						break;
				}
			}
		}

		// TODO: should be removed
		//
		//public void WriteResponseHeaders(SipMessageReader request, StatusCodes statusCode)
		//{
		//    WriteResponseHeaders(request, statusCode, GenerateTag());
		//}

		// TODO: should be removed
		//
		//public void WriteResponseHeaders(SipMessageReader request, StatusCodes statusCode, ByteArrayPart totag)
		//{
		//    int viaCount = 0;
		//    for (int i = 0; i < request.Count.HeaderCount; i++)
		//    {
		//        switch (request.Headers[i].HeaderName)
		//        {
		//            case HeaderNames.CallId:
		//            case HeaderNames.From:
		//            case HeaderNames.RecordRoute:
		//            case HeaderNames.CSeq:
		//                WriteHeader(request.Headers[i]);
		//                break;

		//            case HeaderNames.To:
		//                if (request.To.Tag.IsValid || statusCode == StatusCodes.Trying || request.Method == Methods.Cancelm || totag.IsInvalid)
		//                    WriteHeader(request.Headers[i]);
		//                else
		//                    WriteHeaderWithTag(request.Headers[i], totag);
		//                break;

		//            case HeaderNames.Via:
		//                // здесь ошибка!!!
		//                WriteVia(request.Headers[i], request.Via[viaCount++]);
		//                break;
		//        }
		//    }
		//}

		// TODO: should be removed if possible
		//
		//public void WriteHeaders(SipMessageReader reader, ulong exclude)
		//{
		//    if (HeaderNames.Via.IsMasked(exclude) == false)
		//    {
		//        for (int i = 0; i < reader.Count.ViaCount; i++)
		//            if (reader.Via[i].IsRemoved == false)
		//            {
		//                reader.Via[i].CommaAndValue.TrimStartComma();
		//                reader.Via[i].CommaAndValue.TrimSws();
		//                Write(C.Via, C.HCOLON, C.SP, reader.Via[i].CommaAndValue, C.CRLF);
		//            }
		//    }

		//    if (HeaderNames.Route.IsMasked(exclude) == false)
		//    {
		//        for (int i = 0; i < reader.Count.RouteCount; i++)
		//            if (reader.Route[i].IsRemoved == false)
		//            {
		//                reader.Route[i].CommaAndValue.TrimStartComma();
		//                reader.Route[i].CommaAndValue.TrimSws();
		//                Write(C.Route, C.HCOLON, C.SP, reader.Route[i].CommaAndValue, C.CRLF);
		//            }
		//    }

		//    if (HeaderNames.RecordRoute.IsMasked(exclude) == false)
		//    {
		//        for (int i = 0; i < reader.Count.RecordRouteCount; i++)
		//            if (reader.RecordRoute[i].IsRemoved == false)
		//            {
		//                reader.RecordRoute[i].CommaAndValue.TrimStartComma();
		//                reader.RecordRoute[i].CommaAndValue.TrimSws();
		//                Write(C.RecordRoute, C.HCOLON, C.SP, reader.RecordRoute[i].CommaAndValue, C.CRLF);
		//            }
		//    }

		//    for (int i = 0; i < reader.Count.HeaderCount; i++)
		//    {
		//        switch (reader.Headers[i].HeaderName)
		//        {
		//            case HeaderNames.Via:
		//            case HeaderNames.Route:
		//            case HeaderNames.RecordRoute:
		//                break;

		//            default:
		//                if (reader.Headers[i].HeaderName.IsMasked(exclude))
		//                    break;

		//                WriteHeader(reader.Headers[i]);
		//                break;
		//        }
		//    }
		//}
	}
}
