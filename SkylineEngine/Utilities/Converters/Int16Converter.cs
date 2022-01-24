using System;
using System.Runtime.InteropServices;

namespace SkylineEngine.Utilities.Converters
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Int16Converter
    {
        [FieldOffset(0)] public Int16 Value;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;

        public Int16Converter(Int16 value)
        {
            Byte1 = Byte2 = 0;
            Value = value;
        }

        public static implicit operator Int16(Int16Converter value)
        {
            return value.Value;
        }

        public static implicit operator Int16Converter(Int16 value)
        {
            return new Int16Converter(value);
        }
    }
}
