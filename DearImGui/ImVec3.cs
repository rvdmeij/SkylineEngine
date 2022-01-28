using System.Runtime.InteropServices;

namespace DearImGui
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ImVec3
    {
        public float x;
        public float y;
        public float z;

        public ImVec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}