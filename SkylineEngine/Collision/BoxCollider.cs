using BulletSharp;

namespace SkylineEngine.Collision
{
    public sealed class BoxCollider : Collider
    {
        public override bool Initialize()
        {
            shape = new BoxShape(size.x / 2, size.y / 2, size.z / 2);
            return true;
        }
    }
}
