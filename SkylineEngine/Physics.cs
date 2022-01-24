using System.Runtime.InteropServices;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CameraAndMouseProperties
    {
        public int mouseX;
        public int mouseY;
        public int screenWidth;
        public int screenHeight;
        public float aspect;
        public float fieldOfView;
        public float zNear;
        public float zFar;
    }

    public struct TriangleIntersection
    {
        public int index;
        public int triangleIndex1;
        public int triangleIndex2;
        public int triangleIndex3;
        public float lastPos;

        public TriangleIntersection(int index, int triangleIndex1, int triangleIndex2, int triangleIndex3, float lastPosition)
        {
            this.index = index;
            this.triangleIndex1 = triangleIndex1;
            this.triangleIndex2 = triangleIndex2;
            this.triangleIndex3 = triangleIndex3;
            this.lastPos = lastPosition;
        }
    }

    public static class Physics
    {
        [DllImport(SkylineBase.nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void PhysicsCalculateRay(ref CameraAndMouseProperties camAndMouseProperties, ref Vector3 position, ref Vector3 forward, ref Vector3 up, ref Quaternion rotation, ref Vector3 scale, ref Vector3 origin, ref Vector3 direction);

        public static bool Raycast(out RaycastHit hit)
        {
            hit = null;
            
            float mX = Input.GetMousePosition().x;
            float mY = Screen.height - Input.GetMousePosition().y;

            float mouseX = (mX / (float)Screen.width  - 0.5f) * 2.0f;
            float mouseY = (mY / (float)Screen.height  - 0.5f) * 2.0f;

            var lRayStart_NDC = new OpenTK.Mathematics.Vector4(mouseX, mouseY, -1.0f, 1.0f);
            var lRayEnd_NDC   = new OpenTK.Mathematics.Vector4(mouseX, mouseY,  0.0f, 1.0f);

            var invertedViewProjection = Camera.main.GetPerspectiveProjectionMatrix().Inverted() * Camera.main.GetViewMatrix().Inverted();

            OpenTK.Mathematics.Vector4 lRayStart_World = lRayStart_NDC * invertedViewProjection;
            OpenTK.Mathematics.Vector4 lRayEnd_World   = lRayEnd_NDC * invertedViewProjection;

            lRayStart_World /= lRayStart_World.W;
            lRayEnd_World   /= lRayEnd_World.W;

            OpenTK.Mathematics.Vector4 ray = lRayEnd_World - lRayStart_World;

            Vector3 lRayDir_world = new Vector3(ray.X, ray.Y, ray.Z);
	        lRayDir_world = Vector3.Normalize(lRayDir_world);

	        Vector3 out_origin = new Vector3(lRayStart_World.X, lRayStart_World.Y, lRayStart_World.Z);
	        Vector3 out_direction = Vector3.Normalize(lRayDir_world);

            TriangleIntersection intersection = new TriangleIntersection();
            intersection.index = 0;
            intersection.triangleIndex1 = -1;
            intersection.triangleIndex2 = -1;
            intersection.triangleIndex3 = -1;
            intersection.lastPos = float.MaxValue;

            for (int i = 0; i < RenderPipeline.GetMeshRenderers().Count; i++)
            {
                if (RenderPipeline.GetMeshRenderers()[i].gameObject.layer != Layer.IgnoreRaycast)
                {
                    Mesh mesh = RenderPipeline.GetMeshRenderers()[i].meshFilter.mesh;
                    Transform transform = RenderPipeline.GetMeshRenderers()[i].meshFilter.gameObject.transform;

                    var transformation = transform.GetViewMatrix();

                    for (int j = 0; j < mesh.indices.Length / 3; j++)
                    {
                        float currIntersectionPos;

                        var v1 = mesh.vertices[mesh.indices[j * 3]].position;
                        var v2 = mesh.vertices[mesh.indices[j * 3 + 1]].position;
                        var v3 = mesh.vertices[mesh.indices[j * 3 + 2]].position;

                        OpenTK.Mathematics.Vector4 v1t = new OpenTK.Mathematics.Vector4(v1.x, v1.y, v1.z, 1) * transformation;
                        OpenTK.Mathematics.Vector4 v2t = new OpenTK.Mathematics.Vector4(v2.x, v2.y, v2.z, 1) * transformation;
                        OpenTK.Mathematics.Vector4 v3t = new OpenTK.Mathematics.Vector4(v3.x, v3.y, v3.z, 1) * transformation;

                        v1 = new Vector3(v1t.X, v1t.Y, v1t.Z);
                        v2 = new Vector3(v2t.X, v2t.Y, v2t.Z);
                        v3 = new Vector3(v3t.X, v3t.Y, v3t.Z);

                        if (RayIntersectsTriangle(out_origin, out_direction, v1, v2, v3, out currIntersectionPos))
                        {
                            if (currIntersectionPos < intersection.lastPos)
                            {
                                intersection.lastPos = currIntersectionPos;
                                intersection.triangleIndex1 = (int)mesh.indices[j * 3];
                                intersection.triangleIndex2 = (int)mesh.indices[j * 3 + 1];
                                intersection.triangleIndex3 = (int)mesh.indices[j * 3 + 2];
                                intersection.index = i;
                            }
                        }
                    }
                }
            }

            if (intersection.triangleIndex1 >= 0)
            {
                hit = new RaycastHit();
                hit.point = out_origin + (out_direction * intersection.lastPos);
                hit.distance = Vector3.Distance(out_origin, hit.point);
                hit.triangleIndex1 = intersection.triangleIndex1;
                hit.triangleIndex2 = intersection.triangleIndex2;
                hit.triangleIndex3 = intersection.triangleIndex3;
                hit.transform = RenderPipeline.GetMeshRenderers()[intersection.index].gameObject.transform;
                return true;
            }

            return false;
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, float length, out RaycastHit hit)
        {
            hit = null;
            
            TriangleIntersection intersection = new TriangleIntersection();
            intersection.index = 0;
            intersection.triangleIndex1 = -1;
            intersection.triangleIndex2 = -1;
            intersection.triangleIndex3 = -1;
            intersection.lastPos = float.MaxValue;

            for (int i = 0; i < RenderPipeline.GetMeshRenderers().Count; i++)
            {
                if (RenderPipeline.GetMeshRenderers()[i].gameObject.layer != Layer.IgnoreRaycast)
                {
                    Mesh mesh = RenderPipeline.GetMeshRenderers()[i].meshFilter.mesh;
                    Transform transform = RenderPipeline.GetMeshRenderers()[i].meshFilter.gameObject.transform;

                    var transformation = transform.GetViewMatrix();

                    for (int j = 0; j < mesh.indices.Length / 3; j++)
                    {
                        float currIntersectionPos;

                        var v1 = mesh.vertices[mesh.indices[j * 3]].position;
                        var v2 = mesh.vertices[mesh.indices[j * 3 + 1]].position;
                        var v3 = mesh.vertices[mesh.indices[j * 3 + 2]].position;

                        OpenTK.Mathematics.Vector4 v1t = new OpenTK.Mathematics.Vector4(v1.x, v1.y, v1.z, 1) * transformation;
                        OpenTK.Mathematics.Vector4 v2t = new OpenTK.Mathematics.Vector4(v2.x, v2.y, v2.z, 1) * transformation;
                        OpenTK.Mathematics.Vector4 v3t = new OpenTK.Mathematics.Vector4(v3.x, v3.y, v3.z, 1) * transformation;

                        v1 = new Vector3(v1t.X, v1t.Y, v1t.Z);
                        v2 = new Vector3(v2t.X, v2t.Y, v2t.Z);
                        v3 = new Vector3(v3t.X, v3t.Y, v3t.Z);

                        if (RayIntersectsTriangle(origin, direction, v1, v2, v3, out currIntersectionPos))
                        {
                            if (currIntersectionPos < intersection.lastPos)
                            {
                                intersection.lastPos = currIntersectionPos;
                                intersection.triangleIndex1 = (int)mesh.indices[j * 3];
                                intersection.triangleIndex2 = (int)mesh.indices[j * 3 + 1];
                                intersection.triangleIndex3 = (int)mesh.indices[j * 3 + 2];
                                intersection.index = i;
                            }
                        }
                    }
                }
            }

            if (intersection.triangleIndex1 >= 0)
            {
                float totalDistance = Vector3.Distance(origin, origin + (direction * intersection.lastPos));

                if(totalDistance <= length)
                {
                    hit = new RaycastHit();
                    hit.point = origin + (direction * intersection.lastPos);
                    hit.distance = Vector3.Distance(origin, hit.point);
                    hit.triangleIndex1 = intersection.triangleIndex1;
                    hit.triangleIndex2 = intersection.triangleIndex2;
                    hit.triangleIndex3 = intersection.triangleIndex3;
                    hit.transform = RenderPipeline.GetMeshRenderers()[intersection.index].gameObject.transform;
                    return true;
                }
            }

            return false;            
        }

        private static bool LineIntersects(Vector3 l1p1, Vector3 l1p2, Vector3 l2p1, Vector3 l2p2, ref Vector3 hitpoint)
        {
            float x1 = l1p1.x;
            float y1 = l1p1.z;
            float x2 = l1p2.x;
            float y2 = l1p2.z;

            float x3 = l2p1.x;
            float y3 = l2p1.z;
            float x4 = l2p2.x;
            float y4 = l2p2.z;

            float denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (Mathf.Abs(denominator) <= float.Epsilon)
                return false;

            float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denominator;
            float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denominator;

            if (t > 0 && t < 1 && u > 0)
            {
                hitpoint.x = x1 + t * (x2 - x1);
                hitpoint.z = y1 + t * (y2 - y1);
                return true;
            }
            else
                return false;
        }

        private static bool RayIntersectsTriangle(Vector3 origin, Vector3 dir, Vector3 v0, Vector3 v1, Vector3 v2, out float intersection)
        {
            intersection = 0;

            // Triangle edges
            Vector3 e1 = (v1 -v0);
            Vector3 e2 = (v2 -v0);

            const float epsilon = 0.000001f;

            Vector3 P, Q;
            //float i;
            float t;

            // Calculate determinant
            P = Vector3.Cross(dir, e2);
            float det = Vector3.Dot(e1, P);
            // If determinant is (close to) zero, the ray lies in the plane of the triangle or parallel it's plane
            if ((det > -epsilon) && (det < epsilon))
            {
                return false;
            }
            float invDet = 1.0f / det;

            // Distance from first vertex to ray origin
            Vector3 T = origin - v0;

            // Calculate u parameter
            float u = Vector3.Dot(T, P) * invDet;
            // Intersection point lies outside of the triangle
            if ((u < 0.0f) || (u > 1.0f))
            {
                return false;
            }

            //Prepare to test v parameter
            Q = Vector3.Cross(T, e1);

            // Calculate v parameter
            float v = Vector3.Dot(dir, Q) * invDet;
            // Intersection point lies outside of the triangle
            if (v < 0.0f || u + v > 1.0f) 
                return false;

            // Calculate t
            t = Vector3.Dot(e2, Q) * invDet;

            if (t > epsilon)
            {
                // Triangle interesected
                intersection = t;
                return true;
            }

            // No intersection
            return false;
        }

    }
}
