using System;
using SkylineEngine.Utilities.Converters;

namespace SkylineEngine.Utilities
{
    public enum StringEncoding
    {
        Unicode,
        UTF8
    }

    public static unsafe class BinaryConverter
    {
        private static System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
        private static System.Text.UnicodeEncoding unicode = new System.Text.UnicodeEncoding();
                
        public static void GetBytes(long value, byte[] buffer, int offset)
        {
            Int64Converter converter = value;
            buffer[offset] = converter.Byte1;
            buffer[offset + 1] = converter.Byte2;
            buffer[offset + 2] = converter.Byte3;
            buffer[offset + 3] = converter.Byte4;
            buffer[offset + 4] = converter.Byte5;
            buffer[offset + 5] = converter.Byte6;
            buffer[offset + 6] = converter.Byte7;
            buffer[offset + 7] = converter.Byte8;
        }

        public static void GetBytes(ulong value, byte[] buffer, int offset)
        {
            UInt64Converter converter = value;
            buffer[offset] = converter.Byte1;
            buffer[offset + 1] = converter.Byte2;
            buffer[offset + 2] = converter.Byte3;
            buffer[offset + 3] = converter.Byte4;
            buffer[offset + 4] = converter.Byte5;
            buffer[offset + 5] = converter.Byte6;
            buffer[offset + 6] = converter.Byte7;
            buffer[offset + 7] = converter.Byte8;
        }

        public static void GetBytes(uint value, byte[] buffer, int offset)
        {
            UInt32Converter converter = value;
            buffer[offset] = converter.Byte1;
            buffer[offset + 1] = converter.Byte2;
            buffer[offset + 2] = converter.Byte3;
            buffer[offset + 3] = converter.Byte4;
        }

        public static void GetBytes(short value, byte[] buffer, int offset)
        {
            Int16Converter converter = value;
            buffer[offset] = converter.Byte1;
            buffer[offset + 1] = converter.Byte2;
        }

        public static void GetBytes(ushort value, byte[] buffer, int offset)
        {
            UInt16Converter converter = value;
            buffer[offset] = converter.Byte1;
            buffer[offset + 1] = converter.Byte2;
        }

        public static void GetBytes(double value, byte[] buffer, int offset)
        {
            DoubleConverter converter = value;
            buffer[offset] = converter.Byte1;
            buffer[offset + 1] = converter.Byte2;
            buffer[offset + 2] = converter.Byte3;
            buffer[offset + 3] = converter.Byte4;
            buffer[offset + 4] = converter.Byte5;
            buffer[offset + 5] = converter.Byte6;
            buffer[offset + 6] = converter.Byte7;
            buffer[offset + 7] = converter.Byte8;
        }

        public static void GetBytes(float value, byte[] buffer, int offset)
        {
            SingleConverter converter = value;
            buffer[offset] = converter.Byte1;
            buffer[offset + 1] = converter.Byte2;
            buffer[offset + 2] = converter.Byte3;
            buffer[offset + 3] = converter.Byte4;
        }

        public static int GetBytes(string v, byte[] buffer, int offset, StringEncoding encoding = StringEncoding.UTF8)
        {
            byte[] bytes = null;

            switch (encoding)
            {
                case StringEncoding.Unicode:
                    bytes = utf8.GetBytes(v);
                    break;
                case StringEncoding.UTF8:
                    bytes = unicode.GetBytes(v);
                    break;
                default:
                    bytes = utf8.GetBytes(v);
                    break;
            }

            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
            return bytes.Length;
        }

        public static string ToString(byte[] buffer, int offset, int length, StringEncoding encoding = StringEncoding.UTF8)
        {
            if(encoding == StringEncoding.UTF8)
                return utf8.GetString(buffer, offset, length);
            else if(encoding == StringEncoding.Unicode)
                return unicode.GetString(buffer, offset, length);
            else
                return utf8.GetString(buffer, offset, length);
        }

        public static bool IsLittleEndian()
        {
            return BitConverter.IsLittleEndian;
        }

        public static void ToBigEndian(byte[] buffer)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
        }

        public static void ToLittleEndian(byte[] buffer)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
        }

        public static void SwapEndian(byte[] buffer)
        {
            Array.Reverse(buffer);
        }        
    }
}