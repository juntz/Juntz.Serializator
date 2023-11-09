using System;

namespace Juntz.Serializator
{
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Struct)
    ]
    public class ByteLayoutAttribute : Attribute
    {
        public ByteLayoutAttribute(int length)
        {
            Length = length;
        }

        public int Length { get; }
    }
}