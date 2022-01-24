using System;
using System.Runtime.InteropServices;

namespace SkylineEngine.Utilities.Converters
{
    [StructLayout(LayoutKind.Explicit)]
    public struct UInt32Converter
    {
        [FieldOffset(0)] public uint Value;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;
        [FieldOffset(2)] public byte Byte3;
        [FieldOffset(3)] public byte Byte4;

        public UInt32Converter(uint value)
        {
            Byte1 = Byte2 = Byte3 = Byte4 = 0;
            Value = value;
        }

        public static implicit operator UInt32(UInt32Converter value)
        {
            return value.Value;
        }

        public static implicit operator UInt32Converter(uint value)
        {
            return new UInt32Converter(value);
        }
    }
}
