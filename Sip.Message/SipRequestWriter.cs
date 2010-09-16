using System;

namespace Sip.Message
{
	public class SipRequestWriter
		: SipMessageWriter
	{
		public IByteArrayPart RequestUri { get; set; }

		public void Write(byte[] bytes)
		{
			_writer.SetArray(bytes);

			_writer.Write(H.GetMethod(Method), H.SP, RequestUri, H.SP, H.SipVersion, H.CLRF);
			WriteHeaders();

			_writer.SetArray(null);
		}
	}
}
