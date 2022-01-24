using System.Runtime.InteropServices;

namespace SkylineEngine.Utilities.Converters
{
    [StructLayout(LayoutKind.Explicit)]
    public struct DoubleConverter
    {
        [FieldOffset(0)] public double Value;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;
        [FieldOffset(2)] public byte Byte3;
        [FieldOffset(3)] public byte Byte4;
        [FieldOffset(4)] public byte Byte5;
        [FieldOffset(5)] public byte Byte6;
        [FieldOffset(6)] public byte Byte7;
        [FieldOffset(7)] public byte Byte8;

        public DoubleConverter(double value)
        {
            Byte1 = Byte2 = Byte3 = Byte4 = Byte5 = Byte6 = Byte7 = Byte8 = 0;
            Value = value;
        }

        public static implicit operator double(DoubleConverter value)
        {
            return value.Value;
        }

        public static implicit operator DoubleConverter(double value)
        {
            return new DoubleConverter(value);
        }
    }
}
