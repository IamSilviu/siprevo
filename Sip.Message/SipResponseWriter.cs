using System;

namespace Sip.Message
{
	public class SipResponseWriter
		: SipMessageWriter
	{
		public int StatusCode;

		public void Write(byte[] bytes)
		{
			_writer.SetArray(bytes);

			_writer.Write(H.SipVersion, H.SP, StatusCode, H.SP, H.GetMethod(Method), H.CLRF);
			WriteHeaders();

			_writer.SetArray(null);
		}
	}
}
