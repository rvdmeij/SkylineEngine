using BulletSharp;

namespace SkylineEngine.Collision
{
    public class Collider : Component
    {
        public CollisionShape shape;
        public Vector3 size;
        public Vector3 center;

        public virtual bool Initialize()
        {
            return false;
        }

        public void Dispose()
        {
            shape.Dispose();
        }
    }
}
