using System;
namespace SkylineEngine
{
    public abstract class Renderer : Component
    {
        public abstract bool Initialize();
        public abstract void Render();
    }
}
