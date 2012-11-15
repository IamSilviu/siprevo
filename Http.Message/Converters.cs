using System;
using System.Net;
using System.Text;
using Base.Message;

namespace Http.Message
{
	public static class Converters
	{
		private static readonly byte[][] methods;

		static Converters()
		{
			methods = InitializeMethods();

			Verify(methods, typeof(Methods));
		}

		private static byte[][] InitializeMethods()
		{
			var values = Enum.GetValues(typeof(Methods));
			var methods = new byte[values.Length][];

			for (int i = 0; i < values.Length; i++)
			{
				var value = (Methods)values.GetValue(i);

				if (value == Methods.None || value == Methods.Extension)
					methods[i] = new byte[0];
				else
					methods[i] = Encoding.UTF8.GetBytes(value.ToString().ToUpper());
			}

			return methods;
		}

		public static byte[] ToUtf8Bytes(this Methods transport)
		{
			return methods[(int)transport];
		}

		private static void Verify(byte[][] values, Type type)
		{
			int length = Enum.GetValues(type).Length;

			for (int i = 0; i < length; i++)
				if (values[i] == null)
					throw new InvalidProgramException(
						string.Format(@"Converter value {0} not defined for type {1}", i, type.Name));
		}
	}
}
