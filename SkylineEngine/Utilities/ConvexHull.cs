using System.Runtime.InteropServices;

namespace SkylineEngine.Utilities
{
    internal static unsafe class ConvexHull
    {
        [DllImport(SkylineBase.nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ConvexHullCreate(float* verticesIn, int numVerticesIn, float* verticesOut, int* numVerticesOut, int* indicesOut, int* numIndicesOut, int vertexStride);
        
        public static void Create(Mesh mesh)
        {
            int numVerticesIn = mesh.vertices.Length;
            float[] verticesOut = new float[numVerticesIn];
            int[] indicesOut = new int[mesh.indices.Length];

            fixed(float* verticesInPtr = &mesh.vertices[0].position.x)
            {
                fixed(float* verticesOutPtr = &verticesOut[0])
                {
                    fixed(int* indicesOutPtr = &indicesOut[0])
                    {
                        int numVerticesOut = 0;
                        int numIndicesOut = 0;
                        int vertexStride = Marshal.SizeOf<Vertex>();
                        ConvexHullCreate(verticesInPtr, numVerticesIn, verticesOutPtr, &numVerticesOut, indicesOutPtr, &numIndicesOut, vertexStride);

                        Debug.Log("Created " + numVerticesOut + " vertices and " + numIndicesOut + " indices");
                    }
                }
            }
        }
    }
}