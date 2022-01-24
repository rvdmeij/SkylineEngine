using System.Runtime.InteropServices;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Color32
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color32(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color32(int r, int g, int b, int a)
        {
            r = (int)Mathf.Clamp(r, 0, 255);
            g = (int)Mathf.Clamp(g, 0, 255);
            b = (int)Mathf.Clamp(b, 0, 255);
            a = (int)Mathf.Clamp(a, 0, 255);

            this.r = (float)(r / 255.0f);
            this.g = (float)(g / 255.0f);
            this.b = (float)(b / 255.0f);
            this.a = (float)(a / 255.0f);
        }

        public Color32(uint rgba)
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

        public static Color32 Lerp(Color32 colorA, Color32 colorB, float t)
        {
            float r = Mathf.Lerp(colorA.r, colorB.r, t);
            float g = Mathf.Lerp(colorA.g, colorB.g, t);
            float b = Mathf.Lerp(colorA.b, colorB.b, t);
            float a = Mathf.Lerp(colorA.a, colorB.a, t);
            return new Color32(r, g, b, a);
        }

        public float[] ToArray()
        {
            return new float[] { r, g, b, a };
        }
    }
}