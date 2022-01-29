using System;
using SkylineEngine.Collision;
using SkylineEngine.Shaders;

namespace SkylineEngine
{
    public sealed class Terrain : Component
    {
        private MeshFilter m_meshFilter;
        private Material m_material;
        private MeshCollider m_collider;
        private Rigidbody m_rigidbody;
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

        public Texture splatMap
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

        public Texture texture1
        {
            get
            {
                return m_material.textures[1];
            }
            set
            {
                m_material.textures[1] = value;
            }
        }

        public Texture texture2
        {
            get
            {
                return m_material.textures[2];
            }
            set
            {
                m_material.textures[2] = value;
            }
        }

        public Texture texture3
        {
            get
            {
                return m_material.textures[3];
            }
            set
            {
                m_material.textures[3] = value;
            }
        }

        public Texture texture4
        {
            get
            {
                return m_material.textures[4];
            }
            set
            {
                m_material.textures[4] = value;
            }
        }

        public Color32 ambientColor
        {
            get
            {
                return m_material.ambientColor;
            }
            set
            {
                material.ambientColor = value;
            }
        }

        public Color32 diffuseColor
        {
            get
            {
                return m_material.diffuseColor;
            }
            set
            {
                material.diffuseColor = value;
            }
        }

        public Color32 specularColor
        {
            get
            {
                return m_material.specularColor;
            }
            set
            {
                material.specularColor = value;
            }
        }

        public Color32 skyColor
        {
            get
            {
                return m_material.skyColor;
            }
            set
            {
                material.skyColor = value;
            }
        }

        public float specularStrength
        {
            get
            {
                return m_material.specularStrength;
            }
            set
            {
                material.specularStrength = value;
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
            var shader = Resources.LoadShader(TerrainShader.vertex, TerrainShader.fragment, "Terrain");

            gameObject.name = "Terrain";

            gameObject.AddComponent<MeshRenderer>();
            m_meshFilter = gameObject.AddComponent<MeshFilter>();
            m_material = gameObject.AddComponent<Material>();

            m_material.skyColor = new Color32(255, 255, 255, 255);
            m_material.shader = shader;

            m_material.textures.Add(Resources.Load<Texture>("Default"));
            m_material.textures.Add(Resources.Load<Texture>("Default"));
            m_material.textures.Add(Resources.Load<Texture>("Default"));
            m_material.textures.Add(Resources.Load<Texture>("Default"));
            m_material.textures.Add(Resources.Load<Texture>("Default"));

            m_width = 200;
            m_depth = 200;

            var mesh = MeshPrimitive.CreateTerrain((uint)m_width, (uint)m_depth, 0, m_size);

            m_meshFilter.mesh = mesh;

            m_collider = gameObject.AddComponent<MeshCollider>();
            m_rigidbody = gameObject.AddComponent<Rigidbody>();

            m_rigidbody.mass = 0;
            m_rigidbody.SetFriction(0.1f);
        }

        public int GetNumVertices()
        {
            return m_meshFilter.mesh.vertices.Length;
        }

        public void SetHeight(int index, float height)
        {
            if(index >= m_meshFilter.mesh.vertices.Length)
                return;

            m_meshFilter.mesh.vertices[index].position.y = height;
        }

        public void Update()
        {
            m_meshFilter.mesh.RecalculateNormals();
            m_meshFilter.mesh.Update();
            m_collider.mesh = m_meshFilter.mesh;
            PhysicsPipeline.PopData(m_rigidbody);
            PhysicsPipeline.PushData(m_rigidbody);
        }

        private void Reinitialize()
        {
            m_meshFilter.mesh = MeshPrimitive.CreateTerrain((uint)m_width, (uint)m_depth, 0, m_size);
            //m_meshFilter.mesh.Update();
            m_collider.mesh = m_meshFilter.mesh;
            PhysicsPipeline.PopData(m_rigidbody);
            PhysicsPipeline.PushData(m_rigidbody);
        }
    }
}
