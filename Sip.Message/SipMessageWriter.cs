using System;
using Server.Memory;

namespace Sip.Message
{
	public class test
	{
		public test()
		{
			var x = Memory<SipResponseWriter>.New();
			Memory<SipResponseWriter>.Free(ref x);
		}
	}

	public abstract partial class SipMessageWriter
		: IDefaultValue
	{
		protected ByteArrayWriter _writer;

		public SipMessageWriter()
		{
			_writer = new ByteArrayWriter();

			From = new FromHeader(_writer);
			To = new ToHeader(_writer);
		}

		public Methods Method;
		public FromHeader From { get; private set; }
		public ToHeader To { get; private set; }
		public IByteArrayPart CallId { get; set; }
		public int CSeq { get; set; }
		public int MaxForwards { get; set; }

		protected void WriteHeaders()
		{
			To.Write();
			From.Write();

			_writer.Write(H.CallId, H.HCOLON, H.SP, CallId, H.CLRF);
			_writer.Write(H.CSeq, H.HCOLON, H.SP, CSeq, H.SP, H.GetMethod(Method), H.CLRF);

			if (MaxForwards >= 0)
				_writer.Write(H.MaxForwards, H.HCOLON, H.SP, MaxForwards, H.CLRF);
		}

		#region class Header {...}

		public abstract class Header
		{
			protected ByteArrayWriter _writer;

			public Header(ByteArrayWriter writer)
			{
			}

			protected void WriteHeaderBegin(IByteArrayPart name)
			{
				_writer.Write(name, H.HCOLON, H.SP);
			}
		}

		#endregion

		#region class FromHeader {...}

		public class FromHeader
			: Header
			, IDefaultValue
		{
			public FromHeader(ByteArrayWriter writer)
				:base(writer)
			{
			}

			public IByteArrayPart Name { get; set; }
			public IByteArrayPart AddrSpec { get; set; }

			public void Write()
			{
				WriteHeaderBegin(H.From);
				_writer.WriteIf(H.DQUOTE, Name, H.DQUOTE, H.SP);
				_writer.Write(H.LAQUOT, AddrSpec, H.RAQUOT);
				_writer.Write(H.CLRF);
			}

			public void SetDefaultValue()
			{
				Name = null;
				AddrSpec = null;
			}
		}

		#endregion

		#region class ToHeader {...}

		public class ToHeader
			: Header
			, IDefaultValue
		{
			public ToHeader(ByteArrayWriter writer)
				: base(writer)
			{
			}

			public IByteArrayPart Name { get; set; }
			public IByteArrayPart AddrSpec { get; set; }

			public void Write()
			{
				WriteHeaderBegin(H.To);
				_writer.WriteIf(H.DQUOTE, Name, H.DQUOTE, H.SP);
				_writer.Write(H.LAQUOT, AddrSpec, H.RAQUOT);
				_writer.Write(H.CLRF);
			}

			public void SetDefaultValue()
			{
				Name = null;
				AddrSpec = null;
			}
		}

		#endregion

		#region IDefaultValue

		public void SetDefaultValue()
		{
			From.SetDefaultValue();
			To.SetDefaultValue();
			CallId.SetDefaultValue();
			CSeq = -1;
			MaxForwards = -1;
		}

		#endregion
	}
}
