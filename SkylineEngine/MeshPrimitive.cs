using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace SkylineEngine
{
    public static class MeshPrimitive
    {
        private static Vertex[] CreateQuad(Vector3 offset)
        {
            Vertex[] vertices = new Vertex[4];

            Vertex bottomLeft = new Vertex();
            Vertex bottomRight = new Vertex();
            Vertex topleft = new Vertex();
            Vertex topRight = new Vertex();

            topleft.position = new Vector3(0.0f, 0.0f, -1.0f) + offset;
            bottomLeft.position = new Vector3(0.0f, 0.0f, 0.0f) + offset;
            bottomRight.position = new Vector3(1.0f, 0.0f, 0.0f) + offset;
            topRight.position = new Vector3(1.0f, 0.0f, -1.0f) + offset;

            topleft.uv = new Vector2(0.0f, 1.0f);
            bottomLeft.uv = new Vector2(0.0f, 0.0f);
            bottomRight.uv = new Vector2(1.0f, 0.0f);
            topRight.uv = new Vector2(1.0f, 1.0f);

            topleft.normal = new Vector3(0.0f, 1.0f, 0.0f);
            bottomLeft.normal = new Vector3(0.0f, 1.0f, 0.0f);
            bottomRight.normal = new Vector3(0.0f, 1.0f, 0.0f);
            topRight.normal = new Vector3(0.0f, 1.0f, 0.0f);

            vertices[0] = topleft;
            vertices[1] = bottomLeft;
            vertices[2] = topRight;
            vertices[3] = bottomRight;

            return vertices;
        }

        public static Mesh CreateBillboard(Vector3 scale)
        {
            Mesh m = new Mesh();
            m.vertices = new Vertex[4];
            m.indices = new uint[6];

            Vertex bottomLeft = new Vertex();
            Vertex bottomRight = new Vertex();
            Vertex topleft = new Vertex();
            Vertex topRight = new Vertex();

            topleft.position     = new Vector3(0.0f, 1.0f, 0.0f);
            bottomLeft.position  = new Vector3(0.0f, 0.0f, 0.0f);
            bottomRight.position = new Vector3(1.0f, 0.0f, 0.0f);
            topRight.position    = new Vector3(1.0f, 1.0f, 0.0f);

            topleft.uv           = new Vector2(0.0f, 1.0f);
            bottomLeft.uv        = new Vector2(0.0f, 0.0f);
            bottomRight.uv       = new Vector2(1.0f, 0.0f);
            topRight.uv          = new Vector2(1.0f, 1.0f);

            topleft.normal       = new Vector3(0, 1, 0);
            bottomLeft.normal    = new Vector3(0, 1, 0);
            bottomRight.normal   = new Vector3(0, 1, 0);
            topRight.normal      = new Vector3(0, 1, 0);

            m.vertices[0] = topleft;
            m.vertices[1] = bottomLeft;
            m.vertices[2] = topRight;
            m.vertices[3] = bottomRight;

            m.indices[0] = 0;
            m.indices[1] = 1;
            m.indices[2] = 2;

            m.indices[3] = 1;
            m.indices[4] = 3;
            m.indices[5] = 2;

            SetScale(ref m, scale);

            return m;
        }

        public static Mesh CreateTree(Vector3 scale)
        {
            Mesh mesh = CreateBillboard(scale);

            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            var vertices2 = mesh.vertices;
            var indices2 = mesh.indices;

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices.Add(mesh.vertices[i]);
            }

            for (int i = 0; i < mesh.indices.Length; i++)
            {
                indices.Add(mesh.indices[i]);
            }

            Matrix4 rotationMatrix = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0).ToOpenTKVector(), (Mathf.PI / 2.0f));

            for (int i = 0; i < vertices2.Length; i++)
            {
                OpenTK.Mathematics.Vector3 v = vertices2[i].position.ToOpenTKVector();
                v = new Matrix3(rotationMatrix) * v;
                v -= new OpenTK.Mathematics.Vector3(scale.x / 2.0f * -1.0f, 0, scale.y / 2.0f);

                Vertex vertex = vertices2[i];
                vertex.position = new Vector3(v.X, v.Y, v.Z);
                vertices.Add(vertex);
            }
            
            for(int i = 0; i < indices2.Length; i++)
            {
                indices2[i] += (uint)vertices2.Length;
                indices.Add(indices2[i]);
            }

            mesh.vertices = vertices.ToArray();
            mesh.indices = indices.ToArray();

            mesh.RecalculateNormals();

            return mesh;
        }

        public static Mesh CreatePlane2D(Vector3 scale)
        {
            Vertex[] vertices = new Vertex[6];

            vertices[0].position = new Vector3(0.0f, 0.0f, 0.0f);
            vertices[1].position = new Vector3(1.0f, 0.0f, 0.0f);
            vertices[2].position = new Vector3(1.0f, 1.0f, 0.0f);

            vertices[3].position = new Vector3(1.0f, 1.0f, 0.0f);
            vertices[4].position = new Vector3(0.0f, 1.0f, 0.0f);
            vertices[5].position = new Vector3(0.0f, 0.0f, 0.0f);

            vertices[0].uv = new Vector2(0.0f, 0.0f);
            vertices[1].uv = new Vector2(1.0f, 0.0f);
            vertices[2].uv = new Vector2(1.0f, 1.0f);

            vertices[3].uv = new Vector2(1.0f, 1.0f);
            vertices[4].uv = new Vector2(0.0f, 1.0f);
            vertices[5].uv = new Vector2(0.0f, 0.0f);        

            uint[] indices = new uint[] 
            {
                0, 1, 2,
                3, 4, 5
            };

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.indices = indices;
            SetScale(ref mesh, scale);
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Mesh CreateCube(Vector3 scale)
        {
            Mesh m = new Mesh();

            m.vertices = new Vertex[24];

            for(int i = 0; i < 24; i++)
            {
                Vertex v = new Vertex();
                m.vertices[i] = v;
            }   
            
            //Vertices
            m.vertices[0].position  = new Vector3( 0.5f, -0.5f,  0.5f);
            m.vertices[1].position  = new Vector3(-0.5f, -0.5f,  0.5f);
            m.vertices[2].position  = new Vector3( 0.5f,  0.5f,  0.5f);
            m.vertices[3].position  = new Vector3(-0.5f,  0.5f,  0.5f);

            m.vertices[4].position  = new Vector3( 0.5f,  0.5f, -0.5f);
            m.vertices[5].position  = new Vector3(-0.5f,  0.5f, -0.5f);
            m.vertices[6].position  = new Vector3( 0.5f, -0.5f, -0.5f);
            m.vertices[7].position  = new Vector3(-0.5f, -0.5f, -0.5f);

            m.vertices[8].position  = new Vector3( 0.5f,  0.5f,  0.5f);
            m.vertices[9].position  = new Vector3(-0.5f,  0.5f,  0.5f);
            m.vertices[10].position = new Vector3( 0.5f,  0.5f, -0.5f);
            m.vertices[11].position = new Vector3(-0.5f,  0.5f, -0.5f);
            m.vertices[12].position = new Vector3( 0.5f, -0.5f, -0.5f);
            m.vertices[13].position = new Vector3( 0.5f, -0.5f,  0.5f);
            m.vertices[14].position = new Vector3(-0.5f, -0.5f,  0.5f);
            m.vertices[15].position = new Vector3(-0.5f, -0.5f, -0.5f);
            m.vertices[16].position = new Vector3(-0.5f, -0.5f,  0.5f);
            m.vertices[17].position = new Vector3(-0.5f,  0.5f,  0.5f);
            m.vertices[18].position = new Vector3(-0.5f,  0.5f, -0.5f);
            m.vertices[19].position = new Vector3(-0.5f, -0.5f, -0.5f);
            m.vertices[20].position = new Vector3( 0.5f, -0.5f, -0.5f);
            m.vertices[21].position = new Vector3( 0.5f,  0.5f, -0.5f);
            m.vertices[22].position = new Vector3( 0.5f,  0.5f,  0.5f);
            m.vertices[23].position = new Vector3( 0.5f, -0.5f,  0.5f);

            //UVS
            m.vertices[0].uv  = new Vector2(0.0f, 0.0f);
            m.vertices[1].uv  = new Vector2(1.0f, 0.0f);
            m.vertices[2].uv  = new Vector2(0.0f, 1.0f);
            m.vertices[3].uv  = new Vector2(1.0f, 1.0f);
            m.vertices[4].uv  = new Vector2(0.0f, 1.0f);
            m.vertices[5].uv  = new Vector2(1.0f, 1.0f);
            m.vertices[6].uv  = new Vector2(0.0f, 1.0f);
            m.vertices[7].uv  = new Vector2(1.0f, 1.0f);
            m.vertices[8].uv  = new Vector2(0.0f, 0.0f);
            m.vertices[9].uv  = new Vector2(1.0f, 0.0f);
            m.vertices[10].uv = new Vector2(0.0f, 0.0f);
            m.vertices[11].uv = new Vector2(1.0f, 0.0f);
            m.vertices[12].uv = new Vector2(0.0f, 0.0f);
            m.vertices[13].uv = new Vector2(0.0f, 1.0f);
            m.vertices[14].uv = new Vector2(1.0f, 1.0f);
            m.vertices[15].uv = new Vector2(1.0f, 0.0f);
            m.vertices[16].uv = new Vector2(0.0f, 0.0f);
            m.vertices[17].uv = new Vector2(0.0f, 1.0f);
            m.vertices[18].uv = new Vector2(1.0f, 1.0f);
            m.vertices[19].uv = new Vector2(1.0f, 0.0f);
            m.vertices[20].uv = new Vector2(0.0f, 0.0f);
            m.vertices[21].uv = new Vector2(0.0f, 1.0f);
            m.vertices[22].uv = new Vector2(1.0f, 1.0f);
            m.vertices[23].uv = new Vector2(1.0f, 0.0f);

            //Normals
            //m.vertices[0].normal  = new Vector3( 0.0f,  0.0f,  1.0f);
            //m.vertices[1].normal  = new Vector3( 0.0f,  0.0f,  1.0f);
            //m.vertices[2].normal  = new Vector3( 0.0f,  0.0f,  1.0f);
            //m.vertices[3].normal  = new Vector3( 0.0f,  0.0f,  1.0f);
            //m.vertices[4].normal  = new Vector3( 0.0f,  1.0f,  0.0f);
            //m.vertices[5].normal  = new Vector3( 0.0f,  1.0f,  0.0f);
            //m.vertices[6].normal  = new Vector3( 0.0f,  0.0f, -1.0f);
            //m.vertices[7].normal  = new Vector3( 0.0f,  0.0f, -1.0f);
            //m.vertices[8].normal  = new Vector3( 0.0f,  1.0f,  0.0f);
            //m.vertices[9].normal  = new Vector3( 0.0f,  1.0f,  0.0f);
            //m.vertices[10].normal = new Vector3( 0.0f,  0.0f, -1.0f);
            //m.vertices[11].normal = new Vector3( 0.0f,  0.0f, -1.0f);
            //m.vertices[12].normal = new Vector3( 0.0f, -1.0f,  0.0f);
            //m.vertices[13].normal = new Vector3( 0.0f, -1.0f,  0.0f);
            //m.vertices[14].normal = new Vector3( 0.0f, -1.0f,  0.0f);
            //m.vertices[15].normal = new Vector3( 0.0f, -1.0f,  0.0f);
            //m.vertices[16].normal = new Vector3(-1.0f,  0.0f,  0.0f);
            //m.vertices[17].normal = new Vector3(-1.0f,  0.0f,  0.0f);
            //m.vertices[18].normal = new Vector3(-1.0f,  0.0f,  0.0f);
            //m.vertices[19].normal = new Vector3(-1.0f,  0.0f,  0.0f);
            //m.vertices[20].normal = new Vector3( 1.0f,  0.0f,  0.0f);
            //m.vertices[21].normal = new Vector3( 1.0f,  0.0f,  0.0f);
            //m.vertices[22].normal = new Vector3( 1.0f,  0.0f,  0.0f);
            //m.vertices[23].normal = new Vector3( 1.0f,  0.0f,  0.0f);

            //Indices
            m.indices = new uint[]
            {
                0, 2, 3,
                0, 3, 1,

                8, 4, 5,
                8, 5, 9,

                10, 6, 7,
                10, 7, 11,

                12, 13, 14,
                12, 14, 15,

                16, 17, 18,
                16, 18, 19,

                20, 21, 22,
                20, 22, 23
            };  
            
            SetScale(ref m, scale);

            m.RecalculateNormals();
            
            return m;
        }

        public static Mesh CreateSphere(Vector3 scale)
        {
            Mesh m = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            uint sectorCount = 72;
            uint stackCount = 24;
            float radius = 1.0f;

            float x, y, z, xy;                              // vertex position
            float lengthInv = 1.0f / radius;    // vertex normal
            float s, t;                                     // vertex texCoord


            float sectorStep = 2 * Mathf.PI / sectorCount;
            float stackStep = Mathf.PI / stackCount;
            float sectorAngle, stackAngle;

            for(uint i = 0; i <= stackCount; ++i)
            {
                stackAngle = Mathf.PI / 2 - i* stackStep;        // starting from pi/2 to -pi/2
                xy = radius* Mathf.Cos(stackAngle);             // r * cos(u)
                z = radius* Mathf.Sin(stackAngle);              // r * sin(u)

                // add (sectorCount+1) vertices per stack
                // the first and last vertices have same position and normal, but different tex coords
                for(int j = 0; j <= sectorCount; ++j)
                {
                    Vertex v = new Vertex();

                    sectorAngle = j* sectorStep;           // starting from 0 to 2pi

                    // vertex position (x, y, z)
                    x = xy* Mathf.Cos(sectorAngle);             // r * cos(u) * cos(v)
                    y = xy* Mathf.Sin(sectorAngle);             // r * cos(u) * sin(v)          
                    v.position = new Vector3(x, y, z);

                    // vertex tex coord (s, t) range between [0, 1]
                    s = (float) j / sectorCount;
                    t = (float) i / stackCount;          
                    v.uv = new Vector2(s, t);
                    
                    vertices.Add(v);
                }
            }

            m.vertices = vertices.ToArray();

            uint k1, k2;
            for(uint i = 0; i < stackCount; ++i)
            {
                k1 = i* (sectorCount + 1);     // beginning of current stack
                k2 = k1 + sectorCount + 1;      // beginning of next stack

                for(int j = 0; j<sectorCount; ++j, ++k1, ++k2)
                {
                    // 2 triangles per sector excluding first and last stacks
                    // k1 => k2 => k1+1
                    if(i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);
                    }

                    // k1+1 => k2 => k2+1
                    if(i != (stackCount-1))
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }

            m.indices = indices.ToArray();

            SetScale(ref m, scale);
            m.RecalculateNormals();

            return m;
        }

        public static Mesh CreatePlane(Vector3 scale)
        {
            Mesh m = new Mesh();
            m.vertices = new Vertex[4];
            m.indices = new uint[6];

            Vertex bottomLeft = new Vertex();
            Vertex bottomRight = new Vertex();
            Vertex topleft = new Vertex();
            Vertex topRight = new Vertex();

            topleft.position = new Vector3(0.0f, 0.0f, -1.0f) ;
            bottomLeft.position = new Vector3(0.0f, 0.0f, 0.0f);
            bottomRight.position = new Vector3(1.0f, 0.0f, 0.0f);
            topRight.position = new Vector3(1.0f, 0.0f, -1.0f);

            //topleft.position = new Vector3(0.0f, 1.0f, 0.0f) ;
            //bottomLeft.position = new Vector3(0.0f, 0.0f, 0.0f);
            //bottomRight.position = new Vector3(1.0f, 0.0f, 0.0f);
            //topRight.position = new Vector3(1.0f, 1.0f, 0.0f);

            //topleft.position = new Vector3(0.0f, 0.0f, 0.0f);
            //bottomLeft.position = new Vector3(0.0f, -1.0f, 0.0f);
            //bottomRight.position = new Vector3(1.0f, -1.0f, 0.0f);
            //topRight.position = new Vector3(1.0f, 0.0f, 0.0f);

            topleft.uv = new Vector2(0.0f, 1.0f);
            bottomLeft.uv = new Vector2(0.0f, 0.0f);
            bottomRight.uv = new Vector2(1.0f, 0.0f);
            topRight.uv = new Vector2(1.0f, 1.0f);

            topleft.normal = new Vector3(0.0f, 1.0f, 0.0f);
            bottomLeft.normal = new Vector3(0.0f, 1.0f, 0.0f);
            bottomRight.normal = new Vector3(0.0f, 1.0f, 0.0f);
            topRight.normal = new Vector3(0.0f, 1.0f, 0.0f);

            m.indices[0] = 0;
            m.indices[1] = 1;
            m.indices[2] = 2;

            m.indices[3] = 2;
            m.indices[4] = 1;
            m.indices[5] = 3;

            m.vertices[0] = topleft;
            m.vertices[1] = bottomLeft;
            m.vertices[2] = topRight;
            m.vertices[3] = bottomRight;

            SetScale(ref m, scale);
            m.RecalculateNormals();

            return m;
        }

        public static Mesh CreateTerrain(uint width, uint depth)
        {
            width += 1;
            depth += 1;
            uint sizeX = width;
            uint sizeZ = depth;

            Mesh mesh = new Mesh();
            mesh.vertices = new Vertex[((width + 1) * (depth + 1))];
            mesh.indices = new uint[(sizeX * sizeZ * 6)];

            for (int i = 0, z = 0; z <= sizeZ; z++)
            {
                for (int x = 0; x <= sizeX; x++)
                {
                    mesh.vertices[i].position = new Vector3(x, 0, z);

                    float u = Mathf.InverseLerp(0, width, x);
                    float v = Mathf.InverseLerp(0, depth, z);

                    mesh.vertices[i].uv = new Vector2(u, v);
                    i++;
                }
            }

            uint verts = 0;
            uint indices = 0;

            for (int z = 0; z < sizeZ; z++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    mesh.indices[indices + 0] = verts + 0;
                    mesh.indices[indices + 1] = verts + sizeX + 1;
                    mesh.indices[indices + 2] = verts + 1;

                    mesh.indices[indices + 3] = verts + 1;
                    mesh.indices[indices + 4] = verts + sizeX + 1;
                    mesh.indices[indices + 5] = verts + sizeX + 2;

                    verts++;
                    indices += 6;
                }
                verts++;
            }

            SetScale(ref mesh, new Vector3(100, 1, 100));

            mesh.RecalculateNormals();
            return mesh;
        }

        public static Mesh CreateTerrain(uint width, uint height, int LOD, Vector2 size)
        {
            Mesh mesh = new Mesh();

            width += 1;
            height += 1;

            int meshSimplificationIncrement = (LOD == 0) ? 1 : LOD * 2;
            uint verticesPerLine = (uint)((width - 1) / meshSimplificationIncrement + 1);

            float topLeftX = 0;
            float topLeftZ = 0;
            //  unsigned int numVertices = width * height;
            //  unsigned int numIndices = (width-1) * (height-1) * 6;
            //uint numVertices = verticesPerLine * verticesPerLine;
            uint numVertices = verticesPerLine * height;
            uint numIndices = (verticesPerLine - 1) * (verticesPerLine - 1) * 6;
            uint vertexIndex = 0;
            int triangleIndex = 0;

            mesh.vertices = new Vertex[numVertices];
            mesh.indices = new uint[numIndices];

            for (int y = 0; y < height; y += meshSimplificationIncrement)
            {
                for (int x = 0; x < width; x += meshSimplificationIncrement)
                {
                    mesh.vertices[vertexIndex] = new Vertex();
                    mesh.vertices[vertexIndex].position = new Vector3(topLeftX + x, 0.0f, topLeftZ - y);
                    mesh.vertices[vertexIndex].uv = new Vector2((float)x / (width - 1.0f), (float)y / (height - 1.0f));

                    if (x < width - 1 && y < height - 1)
                    {
                        mesh.indices[triangleIndex + 0] = vertexIndex;
                        mesh.indices[triangleIndex + 1] = vertexIndex + verticesPerLine + 1;
                        mesh.indices[triangleIndex + 2] = vertexIndex + verticesPerLine;

                        mesh.indices[triangleIndex + 3] = vertexIndex + verticesPerLine + 1;
                        mesh.indices[triangleIndex + 4] = vertexIndex;
                        mesh.indices[triangleIndex + 5] = vertexIndex + 1;
                        triangleIndex += 6;
                    }
                    vertexIndex++;
                }
            }

            SetScale(ref mesh, new Vector3(size.x, 1, size.y));
            mesh.RecalculateNormals();
            return mesh;
        }

        private static int GetPointIndex(int c, int x) 
        {
            if (c < 0) return 0; // In case of center point
            x = x % ((c + 1) * 6); // Make the point index circular
                                // Explanation: index = number of points in previous circles + central point + x
                                // hence: (0+1+2+...+c)*6+x+1 = ((c/2)*(c+1))*6+x+1 = 3*c*(c+1)+x+1

            return (3 * c * (c + 1) + x + 1);
        }

        public static Mesh CreateCircle(int res, Vector3 scale) 
        {

            float d = 1f / res;

            var vtc = new List<Vector2>();
            vtc.Add(Vector3.zero); // Start with only center point
            var tris = new List<int>();

            // First pass => build vertices
            for (int circ = 0; circ < res; ++circ) {
                float angleStep = (Mathf.PI * 2f) / ((circ + 1) * 6);
                for (int point = 0; point < (circ + 1) * 6; ++point) {
                    vtc.Add(new Vector2(
                        Mathf.Cos(angleStep * point),
                        Mathf.Sin(angleStep * point)) * d * (circ + 1));
                }
            }

            // Second pass => connect vertices into triangles
            for (int circ = 0; circ < res; ++circ) {
                for (int point = 0, other = 0; point < (circ + 1) * 6; ++point) {
                    if (point % (circ + 1) != 0) {
                        // Create 2 triangles
                        tris.Add(GetPointIndex(circ - 1, other + 1));
                        tris.Add(GetPointIndex(circ - 1, other));
                        tris.Add(GetPointIndex(circ, point));
                        tris.Add(GetPointIndex(circ, point));
                        tris.Add(GetPointIndex(circ, point + 1));
                        tris.Add(GetPointIndex(circ - 1, other + 1));
                        ++other;
                    } else {
                        // Create 1 inverse triange
                        tris.Add(GetPointIndex(circ, point));
                        tris.Add(GetPointIndex(circ, point + 1));
                        tris.Add(GetPointIndex(circ - 1, other));
                        // Do not move to the next point in the smaller circle
                    }
                }
            }



            // Create the mesh
            var mesh = new Mesh();
            mesh.vertices = new Vertex[vtc.Count];
            mesh.indices = new uint[tris.Count];

            for(int i = 0; i < vtc.Count; i++)
            {
                mesh.vertices[i] = new Vertex();
                mesh.vertices[i].position = new Vector3(vtc[i].x, 0, vtc[i].y);
            }

            for(int i = 0; i < tris.Count; i++)
            {
                mesh.indices[i] = (uint)tris[i];
            }

            SetScale(ref mesh, scale);

            mesh.RecalculateNormals();

            return mesh;
        }

        public static Mesh CreateCylinder(Vector3 scale)
        {
            const int DEFAULT_RADIAL_SEGMENTS = 24;
            const int DEFAULT_HEIGHT_SEGMENTS = 2;
            const int MIN_RADIAL_SEGMENTS = 3;
            const int MIN_HEIGHT_SEGMENTS = 1;
            const float DEFAULT_RADIUS = 0.5f;
            const float DEFAULT_HEIGHT = 1.0f;
            
            int radialSegments = DEFAULT_RADIAL_SEGMENTS;
            int heightSegments = DEFAULT_HEIGHT_SEGMENTS;            

            int numVertexColumns, numVertexRows;
            float radius = DEFAULT_RADIUS;
            float length = DEFAULT_HEIGHT;

            //create the mesh
            Mesh mesh = new Mesh();            
            
            //sanity check
            if(radialSegments < MIN_RADIAL_SEGMENTS)	
                radialSegments = MIN_RADIAL_SEGMENTS;
            if(heightSegments < MIN_HEIGHT_SEGMENTS)	
                heightSegments = MIN_HEIGHT_SEGMENTS;
            
            //calculate how many vertices we need
            numVertexColumns = radialSegments + 1;	//+1 for welding
            numVertexRows = heightSegments + 1;
            
            //calculate sizes
            int numVertices = numVertexColumns * numVertexRows;
            int numUVs = numVertices;									//always
            int numSideTris = radialSegments * heightSegments * 2;		//for one cap
            int numCapTris = radialSegments - 2;						//fact
            int trisArrayLength = (numSideTris  + numCapTris * 2) * 3;	//3 places in the array for each tri
            
            //optional: log the number of tris
            //Debug.Log ("CustomCylinder has " + trisArrayLength/3 + " tris");
            
            //initialize arrays
            Vector3[] Vertices = new Vector3[ numVertices ];
            Vector2[] UVs = new Vector2[ numUVs ];
            int[] Tris = new int[ trisArrayLength ];

            mesh.vertices = new Vertex[numVertices];
            mesh.indices = new uint[trisArrayLength];
            
            //precalculate increments to improve performance
            float heightStep = length / heightSegments;
            float angleStep = 2 *Mathf.PI / radialSegments;
            float uvStepH = 1.0f/radialSegments;
            float uvStepV = 1.0f/heightSegments;
            
            for(int j=0; j<numVertexRows; j++)
            {			
                for(int i=0; i<numVertexColumns; i++)
                {
                    //calculate angle for that vertex on the unit circle
                    float angle = i * angleStep;
                    
                    //"fold" the sheet around as a cylinder by placing the first and last vertex of each row at the same spot
                    if(i == numVertexColumns - 1)
                    {
                        angle = 0;
                    }
                    
                    //position current vertex
                    Vertices[j*numVertexColumns + i] = new Vector3(radius * Mathf.Cos(angle), j * heightStep, radius * Mathf.Sin(angle));
                    
                    //calculate UVs
                    UVs[j*numVertexColumns + i] = new Vector2( i * uvStepH, j * uvStepV );
                    
                    //create the tris				
                    if(j==0 || i >= numVertexColumns - 1)
                    {
                        //nothing to do on the first and last "floor" on the tris, capping is done below
                        //also nothing to do on the last column of vertices
                        continue;
                    }
                    else 
                    {
                        //create 2 tris below each vertex
                        //6 seems like a magic number. For every vertex we draw 2 tris in this for-loop, therefore we need 2*3=6 indices in the Tris array
                        //offset the base by the number of slots we need for the bottom cap tris. Those will be populated once we draw the cap
                        int baseIndex = numCapTris * 3 + (j-1)*radialSegments*6 + i*6;
                        
                        //1st tri - below and in front
                        Tris[baseIndex + 0] = j*numVertexColumns + i;
                        Tris[baseIndex + 1]	= j*numVertexColumns + i + 1;
                        Tris[baseIndex + 2]	= (j-1)*numVertexColumns + i;
                        
                        //2nd tri - the one it doesn't touch
                        Tris[baseIndex + 3] = (j-1)*numVertexColumns + i;
                        Tris[baseIndex + 4]	= j*numVertexColumns + i + 1;
                        Tris[baseIndex + 5]	= (j-1)*numVertexColumns + i + 1;
                    }
                }
            }
            
            //draw caps
            bool leftSided = true;
            int leftIndex = 0;
            int rightIndex = 0;
            int middleIndex = 0;
            int topCapVertexOffset = numVertices - numVertexColumns;
            for(int i=0; i<numCapTris; i++) 
            {
                int bottomCapBaseIndex = i*3;
                int topCapBaseIndex = (numCapTris + numSideTris) * 3 + i*3;

                if(i==0) 
                {
                    middleIndex = 0;
                    leftIndex = 1;
                    rightIndex = numVertexColumns - 2;
                    leftSided = true;
                }
                else if(leftSided)
                {
                    middleIndex = rightIndex;
                    rightIndex--;
                }
                else 
                {
                    middleIndex = leftIndex;
                    leftIndex++; 
                }

                leftSided = !leftSided;
                
                //assign bottom tris
                Tris[bottomCapBaseIndex + 0] = rightIndex;
                Tris[bottomCapBaseIndex + 1] = middleIndex;
                Tris[bottomCapBaseIndex + 2] = leftIndex;
                
                //assign top tris
                Tris[topCapBaseIndex + 0] = topCapVertexOffset + leftIndex;
                Tris[topCapBaseIndex + 1] = topCapVertexOffset + middleIndex;
                Tris[topCapBaseIndex + 2] = topCapVertexOffset + rightIndex;
            }
            
            for(int i = 0; i < Vertices.Length; i++)
            {
                mesh.vertices[i].position = Vertices[i];
                mesh.vertices[i].uv = UVs[i];
            }

            for(int i = 0; i < Tris.Length; i++)
            {
                mesh.indices[i] = (uint)Tris[i];
            }
            
            SetScale(ref mesh, scale);

            mesh.RecalculateNormals();

            return mesh;            
        }

        public static Mesh CreateCapsule(Vector3 scale)
        {
            float height = 2.0f;
            float radius = 0.5f;   
            int segments = 32;
            int rings = 8;
            float cylinderHeight = height - radius*2;
            int vertexCount = 2*rings*segments + 2;
            int triangleCount = 4*rings*segments;
            float horizontalAngle = 360f/segments;
            float verticalAngle = 90f/rings;

            var vertices = new Vector3[vertexCount];
            var normals = new Vector3[vertexCount];
            var triangles = new int[3*triangleCount];

            Mesh mesh = new Mesh();
            mesh.vertices = new Vertex[vertexCount];
            mesh.indices = new uint[triangles.Length];

            int vi = 2;
            int ti = 0;
            int topCapIndex = 0;
            int bottomCapIndex = 1;

            vertices[topCapIndex] = new Vector3(0, cylinderHeight/2 + radius, 0);
            normals[topCapIndex] = new Vector3(0, 1, 0);
            vertices[bottomCapIndex] = new Vector3(0, -cylinderHeight/2 - radius, 0);
            normals[bottomCapIndex] = new Vector3(0, -1, 0);

            for (int s = 0; s < segments; s++)
            {
                for (int r = 1; r <= rings; r++)
                {
                    // Top cap vertex
                    Vector3 normal = PointOnSphere(1, s*horizontalAngle, 90 - r*verticalAngle);
                    Vector3 vertex = new Vector3(radius*normal.x, radius*normal.y + cylinderHeight/2, radius*normal.z);
                    vertices[vi] = vertex;
                    normals[vi] = normal;
                    vi++;

                    // Bottom cap vertex
                    vertices[vi] = new Vector3(vertex.x, -vertex.y, vertex.z);
                    normals[vi] = new Vector3(normal.x, -normal.y, normal.z);
                    vi++;

                    int top_s1r1 = vi - 2;
                    int top_s1r0 = vi - 4;
                    int bot_s1r1 = vi - 1;
                    int bot_s1r0 = vi - 3;
                    int top_s0r1 = top_s1r1 - 2*rings;
                    int top_s0r0 = top_s1r0 - 2*rings;
                    int bot_s0r1 = bot_s1r1 - 2*rings;
                    int bot_s0r0 = bot_s1r0 - 2*rings;
                    if (s == 0)
                    {
                        top_s0r1 += vertexCount - 2;
                        top_s0r0 += vertexCount - 2;
                        bot_s0r1 += vertexCount - 2;
                        bot_s0r0 += vertexCount - 2;
                    }

                    // Create cap triangles
                    if (r == 1)
                    {
                        triangles[3*ti + 0] = topCapIndex;
                        triangles[3*ti + 1] = top_s0r1;
                        triangles[3*ti + 2] = top_s1r1;
                        ti++;

                        triangles[3*ti + 0] = bottomCapIndex;
                        triangles[3*ti + 1] = bot_s1r1;
                        triangles[3*ti + 2] = bot_s0r1;
                        ti++;
                    }
                    else
                    {
                        triangles[3*ti + 0] = top_s1r0;
                        triangles[3*ti + 1] = top_s0r0;
                        triangles[3*ti + 2] = top_s1r1;
                        ti++;

                        triangles[3*ti + 0] = top_s0r0;
                        triangles[3*ti + 1] = top_s0r1;
                        triangles[3*ti + 2] = top_s1r1;
                        ti++;

                        triangles[3*ti + 0] = bot_s0r1;
                        triangles[3*ti + 1] = bot_s0r0;
                        triangles[3*ti + 2] = bot_s1r1;
                        ti++;

                        triangles[3*ti + 0] = bot_s0r0;
                        triangles[3*ti + 1] = bot_s1r0;
                        triangles[3*ti + 2] = bot_s1r1;
                        ti++;
                    }
                }

                // Create side triangles
                int top_s1 = vi - 2;
                int top_s0 = top_s1 - 2*rings;
                int bot_s1 = vi - 1;
                int bot_s0 = bot_s1 - 2*rings;
                if (s == 0)
                {
                    top_s0 += vertexCount - 2;
                    bot_s0 += vertexCount - 2;
                }

                triangles[3*ti + 0] = top_s0;
                triangles[3*ti + 1] = bot_s1;
                triangles[3*ti + 2] = top_s1;
                ti++;

                triangles[3*ti + 0] = bot_s0;
                triangles[3*ti + 1] = bot_s1;
                triangles[3*ti + 2] = top_s0;
                ti++;
            }

            for(int i = 0; i < vertices.Length; i++)
            {
                mesh.vertices[i].position = vertices[i];
                mesh.vertices[i].normal = normals[i];
            }

            for(int i = 0; i < triangles.Length; i++)
            {
                mesh.indices[i] = (uint)triangles[i];
            }

            SetScale(ref mesh, scale);

            mesh.RecalculateNormals();
            
            return mesh;
        }

        public static Vector3 PointOnSphere(float radius, float horizontalAngle, float verticalAngle)
        {
            return PointOnSpheroid(radius, radius, horizontalAngle, verticalAngle);
        }

        public static Vector3 PointOnSphere(Vector3 center, float radius)
        {
            return center + Random.onUnitSphere * radius;
        }

        public static Vector3 PointOnSpheroid(float radius, float height, float horizontalAngle, float verticalAngle)
        {
            float horizontalRadians = horizontalAngle*Mathf.Deg2Rad;
            float verticalRadians = verticalAngle*Mathf.Deg2Rad;
            float cosVertical = Mathf.Cos(verticalRadians);

            return new Vector3(
                radius*Mathf.Sin(horizontalRadians)*cosVertical,
                height*Mathf.Sin(verticalRadians),
                radius*Mathf.Cos(horizontalRadians)*cosVertical);
        }

        public static void SetScale(ref Mesh mesh, Vector3 scale)
        {
            for(int i = 0; i < mesh.vertices.Length; i++)
            {
                Vertex v = mesh.vertices[i];
                v.position *= scale;
                mesh.vertices[i] = v;
            }
        }
    }
}
