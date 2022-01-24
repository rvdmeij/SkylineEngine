using System;
namespace SkylineEngine
{
    public struct BoundingBox
    {
        public Vector3 min;
        public Vector3 max;
        public Vector3 center;
        public Vector3 size;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
            this.center = (min + max) / 2.0f;
            this.size = max - min;
        }

        public BoundingBox(Vector3[] points)
        {
            Vector3 min = points[0];
            Vector3 max = points[0];
            
            for (int i = 1; i < points.Length; i++)
            {
                Vector3.Min(ref min, ref points[i], out min);
                Vector3.Max(ref max, ref points[i], out max);
            }
            
            this.min = min;
            this.max = max;
            this.center = (min + max) / 2.0f;
            this.size = max - min;            
        }

        public BoundingBox(ref Mesh mesh)
        {
            Vector3 min = mesh.vertices[0].position;
            Vector3 max = mesh.vertices[0].position;
            
            for (int i = 1; i < mesh.vertices.Length; i++)
            {
                Vector3.Min(ref min, ref mesh.vertices[i].position, out min);
                Vector3.Max(ref max, ref mesh.vertices[i].position, out max);
            }
            
            this.min = min;
            this.max = max;
            this.center = (min + max) / 2.0f;
            this.size = max - min;
        }
    }
}
