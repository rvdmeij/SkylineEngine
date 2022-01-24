using System;
namespace SkylineEngine
{
    public sealed class MeshFilter : Component
    {
        private Mesh m_mesh;

        public Mesh mesh 
        {
            get { return m_mesh; }
            set 
            {
                m_mesh = value;
                Initialize();
            }
        }

        private void Initialize()
        {
            if (gameObject.GetComponent<MeshRenderer>() != null)
            {
                RenderPipeline.PushData<MeshRenderer>(this.gameObject);
            }
        }
    }
}
