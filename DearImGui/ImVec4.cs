using System.Runtime.InteropServices;

namespace DearImGui
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ImVec4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public ImVec4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public ImVec4(float value)
        {
            this.x = value;
            this.y = value;
            this.z = value;
            this.w = 1.0f;
        }

        public ImVec4(uint rgba)
        {
            byte R = ((byte)((rgba & -16777216) >> 0x18));
            byte G = (byte)((rgba & 0xff0000) >> 0x10);
            byte B = (byte)((rgba & 0xff00) >> 8);
            byte A = (byte)(rgba & 0xff);

            this.x = (float)(R / 255.0);
            this.y = (float)(G / 255.0);
            this.z = (float)(B / 255.0);
            this.w = (float)(A / 255.0);
        }
    }
}
