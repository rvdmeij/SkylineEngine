using SkylineEngine.Shaders;

namespace SkylineEngine
{
    public sealed class Water : Component
    {
        private MeshFilter m_meshFilter;
        private Material m_material;
        private int m_width;
        private int m_depth;
        private Vector2 m_size;

        public MeshFilter meshFilter { get { return m_meshFilter; } }
        public Material material { get { return m_material; } }
        public int width { get { return m_width; } }
        public int depth { get { return m_depth; } }

        public Vector2i resolution
        {
            get
            {
                return new Vector2i(m_width,m_depth);
            }
            set
            {
                m_width = value.x;
                m_depth = value.y;
                Reinitialize();
            }
        }

        public Vector2 size
        {
            get
            {
                return m_size;
            }
            set
            {
                m_size = value;
                Reinitialize();
            }
        }

        public Color32 color
        {
            get
            {
                return m_material.diffuseColor;
            }
            set
            {
                m_material.diffuseColor = value;
            }
        }

        public Texture texture
        {
            get
            {
                return m_material.textures[0];
            }
            set
            {
                m_material.textures[0] = value;
            }
        }

        public float opacity
        {
            get
            {
                return m_material.opacity;
            }
            set
            {
                m_material.opacity = value;
            }
        }

        public Vector2 uvScale
        {
            get
            {
                return m_material.uvScale;
            }
            set
            {
                m_material.uvScale = value;
            }
        }

        public override void InitializeComponent()
        {
            if (meshFilter == null && material == null)
            {
                m_size = new Vector2(10, 10);
                Create();
            }
        }

        private void Create()
        {
            var shader = Resources.LoadShader(WaterShader.vertex, WaterShader.fragment, "Water");

            gameObject.name = "Water";

            gameObject.AddComponent<MeshRenderer>();
            m_meshFilter = gameObject.AddComponent<MeshFilter>();
            m_material = gameObject.AddComponent<Material>();

            m_material.shader = shader;
            m_material.alphaBlend = true;

            m_material.textures.Add(Resources.Load<Texture>("Default"));

            m_width = 200;
            m_depth = 200;

            var mesh = MeshPrimitive.CreateTerrain((uint)m_width, (uint)m_depth, 1, m_size);

            m_meshFilter.mesh = mesh;
        }

        private void Reinitialize()
        {
            m_meshFilter.mesh = MeshPrimitive.CreateTerrain((uint)m_width, (uint)m_depth, 0, m_size);            
        }
    }
}
