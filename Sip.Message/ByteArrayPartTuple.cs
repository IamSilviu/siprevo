//using System;

//namespace Sip.Message.Tuple
//{
//    public struct ByteArrayPartTuple
//        : IEquatable<ByteArrayPartTuple>
//    {
//        public ByteArrayPart Part0;
//        public ByteArrayPart Part1;
//        public ByteArrayPart Part2;
//        public ByteArrayPart Part3;
//        public ByteArrayPart Part4;

//        public const int PartsCount = 5;

//        public int Length
//        {
//            get { return Part0.Length + Part1.Length + Part2.Length + Part3.Length + Part4.Length; }
//        }

//        public ByteArrayPart GetPart(int index)
//        {
//            switch (index)
//            {
//                case 0: return Part0;
//                case 1: return Part1;
//                case 2: return Part2;
//                case 3: return Part3;
//                case 4: return Part4;
//                default:
//                    throw new ArgumentOutOfRangeException(@"index");
//            }
//        }

//        public bool Equals(ByteArrayPartTuple y)
//        {
//            int length = Length;

//            if (length != y.Length)
//                return false;

//            int index, indexY, offset, offsetY;
//            ByteArrayPart part, partY;

//            index = indexY = -1;
//            offset = offsetY = 0;
//            part = partY = new ByteArrayPart();

//            for (int i = 0; i < length; i++)
//            {
//                while (part.Length <= i - offset)
//                {
//                    if (++index < PartsCount)
//                    {
//                        offset += part.Length;
//                        part = GetPart(index);
//                    }
//                    else
//                        return false;
//                }

//                while (partY.Length <= i - offsetY)
//                {
//                    if (++indexY < PartsCount)
//                    {
//                        offsetY += partY.Length;
//                        partY = y.GetPart(index);
//                    }
//                    else
//                        return false;
//                }

//                if (part.Bytes[part.Begin + i - offset] != partY.Bytes[partY.Begin + i - offsetY])
//                    return false;
//            }

//            return true;
//        }

//        public override bool Equals(Object obj)
//        {
//            return obj is ByteArrayPartTuple && Equals((ByteArrayPartTuple)obj);
//        }

//        public override int GetHashCode()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
