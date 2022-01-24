using BulletSharp;

namespace SkylineEngine.Collision
{
    public sealed class CapsuleCollider : Collider
    {
        private float m_radius;
        private float m_height;

        public float radius
        {
            get { return m_radius; }
            set { m_radius = value; }
        }

        public float height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public override bool Initialize()
        {
            shape = new CapsuleShape(radius, height);
            return true;
        }
    }
}
