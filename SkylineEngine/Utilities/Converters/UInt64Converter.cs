using System;
using System.Runtime.InteropServices;

namespace SkylineEngine.Utilities.Converters
{
    [StructLayout(LayoutKind.Explicit)]
    public struct UInt64Converter
    {
        [FieldOffset(0)] public UInt64 Value;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;
        [FieldOffset(2)] public byte Byte3;
        [FieldOffset(3)] public byte Byte4;
        [FieldOffset(4)] public byte Byte5;
        [FieldOffset(5)] public byte Byte6;
        [FieldOffset(6)] public byte Byte7;
        [FieldOffset(7)] public byte Byte8;

        public UInt64Converter(UInt64 value)
        {
            Byte1 = Byte2 = Byte3 = Byte4 = Byte5 = Byte6 = Byte7 = Byte8 = 0;
            Value = value;
        }

        public static implicit operator UInt64(UInt64Converter value)
        {
            return value.Value;
        }

        public static implicit operator UInt64Converter(UInt64 value)
        {
            return new UInt64Converter(value);
        }
    }
}
