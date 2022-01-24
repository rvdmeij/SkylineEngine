using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using SkylineEngine.Utilities;

namespace SkylineEngine
{
    public sealed class Mesh
    {
        public Vertex[] vertices;
        public uint[] indices;
        public BoundingBox bounds;

        private uint VAO;
        private uint VBO;
        private uint EBO;

        public Mesh()
        {
            VAO = 0;
            VBO = 0;
            EBO = 0;
        }

        ~Mesh()
        {
            //Dispose();
        }

        public void Dispose()
        {
            if (this.VAO > 0 || this.VBO > 0 || this.EBO > 0)
            {
                GL.DeleteVertexArrays(1, ref this.VAO);
                GL.DeleteBuffers(1, ref this.VBO);

                if (indices?.Length > 0)
                {
                    GL.DeleteBuffers(1, ref this.EBO);
                }
            }
        }

        private byte[] ToBytes()
        {
            int dataLength = vertices.Length * Marshal.SizeOf(typeof(Vertex));
            int size = Marshal.SizeOf(typeof(Vertex));
            byte[] bytes = new byte[dataLength];

            int offset = 0;
            for(int i = 0; i < vertices.Length; i++)
            {
                BinaryConverter.GetBytes(vertices[i].position.x, bytes, offset + 0);
                BinaryConverter.GetBytes(vertices[i].position.y, bytes, offset + 4);
                BinaryConverter.GetBytes(vertices[i].position.z, bytes, offset + 8);
                BinaryConverter.GetBytes(vertices[i].color.x, bytes, offset + 12);
                BinaryConverter.GetBytes(vertices[i].color.y, bytes, offset + 16);
                BinaryConverter.GetBytes(vertices[i].color.z, bytes, offset + 20);
                BinaryConverter.GetBytes(vertices[i].uv.x, bytes, offset + 24);
                BinaryConverter.GetBytes(vertices[i].uv.y, bytes, offset + 28);
                BinaryConverter.GetBytes(vertices[i].normal.x, bytes, offset + 32);
                BinaryConverter.GetBytes(vertices[i].normal.y, bytes, offset + 36);
                BinaryConverter.GetBytes(vertices[i].normal.z, bytes, offset + 40);
                offset += size                ;
            }

            return bytes;
        }

        public void Initialize(bool instanced = false)
        {
            Dispose();

            //Create VAO
            GL.CreateVertexArrays(1, out this.VAO);
            GL.BindVertexArray(this.VAO);

            //GEN VBO AND BIND AND SEND DATA
            GL.GenBuffers(1, out this.VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Marshal.SizeOf(typeof(Vertex)), vertices, BufferUsageHint.StaticDraw);

            //GEN EBO AND BIND AND SEND DATA
            if (this.indices?.Length > 0)
            {
                GL.GenBuffers(1, out this.EBO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            }

            //SET VERTEXATTRIBPOINTERS AND ENABLE (INPUT ASSEMBLY)
            //Position
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "position"));

            //Color
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "color"));

            //Texcoord
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "uv"));

            //Normal
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "normal"));

            if (instanced)
            {
                //Offset
                GL.EnableVertexAttribArray(4);
                GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "offset"));
                GL.VertexAttribDivisor(4, 1);
            }

            //BIND VAO 0
            GL.BindVertexArray(0);
        }

        public void Update()
        {
            if (this.indices?.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), indices, BufferUsageHint.DynamicDraw);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Marshal.SizeOf(typeof(Vertex)), vertices, BufferUsageHint.DynamicDraw);
        }

        public void RecalculateNormals()
        {
            int triangleCount = indices.Length / 3;

            for (int i = 0; i < triangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = (int)indices[normalTriangleIndex];
                int vertexIndexB = (int)indices[normalTriangleIndex + 1];
                int vertexIndexC = (int)indices[normalTriangleIndex + 2];
                Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);

                Vertex v1 = vertices[vertexIndexA];
                Vertex v2 = vertices[vertexIndexB];
                Vertex v3 = vertices[vertexIndexC];

                v1.normal += triangleNormal;
                v2.normal += triangleNormal;
                v3.normal += triangleNormal;

                v1.normal = Vector3.Normalize(v1.normal);
                v2.normal = Vector3.Normalize(v2.normal);
                v3.normal = Vector3.Normalize(v3.normal);

                vertices[vertexIndexA] = v1;
                vertices[vertexIndexB] = v2;
                vertices[vertexIndexC] = v3;
            }
        }

        public Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
        {
            Vector3 pA = vertices[indexA].position;
            Vector3 pB = vertices[indexB].position;
            Vector3 pC = vertices[indexC].position;

            Vector3 sideAB = pB - pA;
            Vector3 sideAC = pC - pA;
            return Vector3.Normalize(Vector3.Cross(sideAB, sideAC));
        }

        public uint GetVAO()
        {
            return VAO;
        }

        public uint GetVBO()
        {
            return VBO;
        }

        public uint GetEBO()
        {
            return EBO;
        }
    }
}
