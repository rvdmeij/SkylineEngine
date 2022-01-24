using System;
using System.Runtime.InteropServices;
using BulletSharp;
using System.IO;

namespace SkylineEngine.Collision
{
    public sealed class MeshCollider : Collider
    {
        private TriangleIndexVertexArray indexVertexArrays;

        private Mesh m_mesh;

        public Mesh mesh
        {
            get { return m_mesh; }
            set 
            { 
                m_mesh = value;
                if(m_mesh != null)
                {
                    CreateMeshData(m_mesh);
                }
            }

        }

        public override bool Initialize()
        {
            if (CreateMeshData())
            {
                shape = new BvhTriangleMeshShape(indexVertexArrays, false);
                return true;
            }
            else if(mesh != null)
            {
                shape = new BvhTriangleMeshShape(indexVertexArrays, false);
                return true;
            }
            return false;
        }

        private bool CreateMeshData(Mesh mesh)
        {
            int totalVerts = mesh.vertices.Length;
            int totalTriangles = mesh.indices.Length / 3;
            int triangleIndexStride = 3 * sizeof(int);

            var indexMesh = new IndexedMesh();
            indexMesh.Allocate(totalTriangles, totalVerts, triangleIndexStride, 12);
            indexMesh.NumTriangles = totalTriangles;
            indexMesh.NumVertices = totalVerts;
            indexMesh.TriangleIndexStride = 3 * sizeof(int);
            indexMesh.VertexStride = 12;

            int[] indices = new int[mesh.indices.Length];
            Buffer.BlockCopy(mesh.indices, 0, indices, 0, mesh.indices.Length * sizeof(int));

            //mesh.SetData(indices, vertices);            

            using (var indicesStream = indexMesh.GetTriangleStream())
            {
                var ind = new BinaryWriter(indicesStream);
                
                for(int i = 0; i < indices.Length; i++)
                    ind.Write(indices[i]);

                ind.Dispose();
            }

            using (var vertexStream = indexMesh.GetVertexStream())
            {
                var verts = new BinaryWriter(vertexStream);
                for(int i = 0; i < mesh.vertices.Length; i++)
                {
                    verts.Write(mesh.vertices[i].position.x);
                    verts.Write(mesh.vertices[i].position.y);
                    verts.Write(mesh.vertices[i].position.z);
                }
                verts.Dispose();
            }


            indexVertexArrays = new TriangleIndexVertexArray();
            indexVertexArrays.AddIndexedMesh(indexMesh);

            return true;            
        }

        private bool CreateMeshData()
        {
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();

            if(filter == null)
            {
                Debug.Log("Can't create MeshCollider because there is no MeshFilter on this game object");
                return false;
            }

            if(filter.mesh == null)
            {
                Debug.Log("Can't create MeshCollider because the MeshFilter on this game object has no mesh");
                return false;
            }

            int totalVerts = filter.mesh.vertices.Length;
            int totalTriangles = filter.mesh.indices.Length / 3;
            int triangleIndexStride = 3 * sizeof(int);

            var mesh = new IndexedMesh();
            mesh.Allocate(totalTriangles, totalVerts, triangleIndexStride, 12);
            mesh.NumTriangles = totalTriangles;
            mesh.NumVertices = totalVerts;
            mesh.TriangleIndexStride = 3 * sizeof(int);
            mesh.VertexStride = 12;

            int[] indices = new int[filter.mesh.indices.Length];
            Buffer.BlockCopy(filter.mesh.indices, 0, indices, 0, filter.mesh.indices.Length * sizeof(int));

            //mesh.SetData(indices, vertices);            

            using (var indicesStream = mesh.GetTriangleStream())
            {
                var ind = new BinaryWriter(indicesStream);
                
                for(int i = 0; i < indices.Length; i++)
                    ind.Write(indices[i]);

                ind.Dispose();
            }

            using (var vertexStream = mesh.GetVertexStream())
            {
                var verts = new BinaryWriter(vertexStream);
                for(int i = 0; i < filter.mesh.vertices.Length; i++)
                {
                    verts.Write(filter.mesh.vertices[i].position.x);
                    verts.Write(filter.mesh.vertices[i].position.y);
                    verts.Write(filter.mesh.vertices[i].position.z);
                }
                verts.Dispose();
            }


            indexVertexArrays = new TriangleIndexVertexArray();
            indexVertexArrays.AddIndexedMesh(mesh);

            return true;
        }
    }
}
