namespace SkylineEngine
{
    public abstract class Renderer : Component
    {
        public bool skipReflectionPass;
        public bool skipShadowPass;
        public bool wireFrame;

        public abstract bool Initialize();
        public abstract void Render();
        public abstract void Render(Material material);
    }
}
