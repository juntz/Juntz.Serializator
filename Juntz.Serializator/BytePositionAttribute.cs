using System;

namespace Juntz.Serializator
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BytePositionAttribute : Attribute
    {
        public BytePositionAttribute(int index, int length)
        {
            Index = index;
            Length = length;
        }

        public int Index { get; }
        public int Length { get; }
    }
}
