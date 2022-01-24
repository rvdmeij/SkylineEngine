using System;
using System.Runtime.InteropServices;

namespace SkylineEngine.Utilities.Converters
{
    [StructLayout(LayoutKind.Explicit)]
    public struct UInt16Converter
    {
        [FieldOffset(0)] public UInt16 Value;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;

        public UInt16Converter(UInt16 value)
        {
            Byte1 = Byte2 = 0;
            Value = value;
        }

        public static implicit operator UInt16(UInt16Converter value)
        {
            return value.Value;
        }

        public static implicit operator UInt16Converter(UInt16 value)
        {
            return new UInt16Converter(value);
        }
    }
}
