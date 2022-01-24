using System.Runtime.InteropServices;

namespace DearImGui
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ImVec2
    {
        public float x;
        public float y;

        public ImVec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
