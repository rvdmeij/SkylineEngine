using BulletSharp;

namespace SkylineEngine.Collision
{
    public sealed class SphereCollider : Collider
    {
        private float m_radius;

        public float radius
        {
            get { return m_radius; }
            set { m_radius = value; }
        }

        public override bool Initialize()
        {
            shape = new SphereShape(radius);
            return true;
        }
    }
}
