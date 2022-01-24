using System.Runtime.InteropServices;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public static readonly Color black = new Color(0, 0, 0, 255);
        public static readonly Color blue = new Color(0, 0, 255, 255);
        public static readonly Color clear = new Color(0, 0, 0, 0);
        public static readonly Color cyan = new Color(0, 255, 255, 255);
        public static readonly Color gray = new Color(127, 127, 127, 255);
        public static readonly Color green = new Color(0, 255, 0, 255);
        public static readonly Color grey = new Color(127, 127, 127, 255);
        public static readonly Color magenta = new Color(255, 0, 255, 255);
        public static readonly Color red = new Color(255, 0, 0, 255);
        public static readonly Color white = new Color(255, 255, 255, 255);
        public static readonly Color yellow = new Color(255, 235, 4, 255);

        public Color(int r, int g, int b, int a)
        {
            this.r = (r / 255.0f);
            this.g = (g / 255.0f);
            this.b = (b / 255.0f);
            this.a = (a / 255.0f);
        }

        public Color(uint rgba)
        {
            byte R = ((byte)((rgba & -16777216) >> 0x18));
            byte G = (byte)((rgba & 0xff0000) >> 0x10);
            byte B = (byte)((rgba & 0xff00) >> 8);
            byte A = (byte)(rgba & 0xff);

            this.r = (float)(R / 255.0f);
            this.g = (float)(G / 255.0f);
            this.b = (float)(B / 255.0f);
            this.a = (float)(A / 255.0f);
        }

        public float[] ToArray()
        {
            return new float[] { r, g, b, a };
        }
    }
}
