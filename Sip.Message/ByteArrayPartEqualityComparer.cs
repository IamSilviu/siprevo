using System;
using System.Collections.Generic;

namespace Sip.Message
{
	class ByteArrayPartEqualityComparer
		: EqualityComparer<ByteArrayPart>
	{
		private static readonly ByteArrayPartEqualityComparer defaultComparer =
			new ByteArrayPartEqualityComparer();

		public new static EqualityComparer<ByteArrayPart> Default
		{
			get { return defaultComparer; }
		}

		public override bool Equals(ByteArrayPart x, ByteArrayPart y)
		{
			return x.IsEqualValue(y);
		}

		public override int GetHashCode(ByteArrayPart x)
		{
			return x.Bytes.GetHashCode() ^ x.Begin ^ x.End;
		}
	}
}
