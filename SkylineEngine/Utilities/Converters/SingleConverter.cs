using System.Runtime.InteropServices;

namespace SkylineEngine.Utilities.Converters
{
    [StructLayout(LayoutKind.Explicit)]
    public struct SingleConverter
    {
        [FieldOffset(0)] public float Value;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;
        [FieldOffset(2)] public byte Byte3;
        [FieldOffset(3)] public byte Byte4;

        public SingleConverter(float value)
        {
            Byte1 = Byte2 = Byte3 = Byte4 = 0;
            Value = value;
        }

        public static implicit operator float(SingleConverter value)
        {
            return value.Value;
        }

        public static implicit operator SingleConverter(float value)
        {
            return new SingleConverter(value);
        }
    }
}
