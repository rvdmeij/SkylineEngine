using System.Runtime.InteropServices;
using SkylineEngine.Utilities;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vector3 position;
        public Vector3 color;
        public Vector2 uv;
        public Vector3 normal;

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[12];
            BinaryConverter.GetBytes(position.x, bytes, 0);
            BinaryConverter.GetBytes(position.y, bytes, 4);
            BinaryConverter.GetBytes(position.z, bytes, 8);
            BinaryConverter.GetBytes(color.x, bytes, 12);
            BinaryConverter.GetBytes(color.y, bytes, 16);
            BinaryConverter.GetBytes(color.z, bytes, 20);
            BinaryConverter.GetBytes(uv.x, bytes, 24);
            BinaryConverter.GetBytes(uv.y, bytes, 28);
            BinaryConverter.GetBytes(normal.x, bytes, 32);
            BinaryConverter.GetBytes(normal.y, bytes, 36);
            BinaryConverter.GetBytes(normal.z, bytes, 40);
            return bytes;
        }
    }
}
