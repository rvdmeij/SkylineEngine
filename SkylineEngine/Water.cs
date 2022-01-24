using System;

namespace SkylineEngine
{
    public sealed class Water : Component
    {
        private MeshFilter m_meshFilter;
        private Material m_material;
        private int m_width;
        private int m_depth;

        public MeshFilter meshFilter { get { return m_meshFilter; } }
        public Material material { get { return m_material; } }
        public int width { get { return m_width; } }
        public int depth { get { return m_depth; } }

        public override void InitializeComponent()
        {
            if (meshFilter == null && material == null)
            {
                Create();
            }
        }

        private void Create()
        {
            var shader = Resources.Load<Shader>("res/Shaders/WaterTest.shader");

            gameObject.name = "Water";

            gameObject.AddComponent<MeshRenderer>();
            m_meshFilter = gameObject.AddComponent<MeshFilter>();
            m_material = gameObject.AddComponent<Material>();

            m_material.skyColor = new Color32(255, 255, 255, 255);
            m_material.shader = shader;
            m_material.alphaBlend = true;

            m_material.textures.Add(Resources.Load<Texture>("res/Textures/Water6.png"));

            m_width = 200;
            m_depth = 200;

            var mesh = MeshPrimitive.CreateTerrain((uint)m_width, (uint)m_depth, 1);

            m_meshFilter.mesh = mesh;
        }
    }
}
