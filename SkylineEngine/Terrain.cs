using System;
using SkylineEngine.Collision;

namespace SkylineEngine
{
    public sealed class Terrain : Component
    {
        private MeshFilter m_meshFilter;
        private Material m_material;
        private int m_width;
        private int m_depth;
        private float[] m_heights;

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
            var shader = Resources.Load<Shader>("res/Shaders/Terrain.shader");

            gameObject.name = "Terrain";

            gameObject.AddComponent<MeshRenderer>();
            m_meshFilter = gameObject.AddComponent<MeshFilter>();
            m_material = gameObject.AddComponent<Material>();

            m_material.skyColor = new Color32(255, 255, 255, 255);
            m_material.shader = shader;

            m_material.textures.Add(Resources.Load<Texture>("res/Textures/SplatMap.jpg"));
            m_material.textures.Add(Resources.Load<Texture>("res/Textures/Grass.jpg"));
            m_material.textures.Add(Resources.Load<Texture>("res/Textures/Grass3.jpg"));
            m_material.textures.Add(Resources.Load<Texture>("res/Textures/Mud.png"));
            m_material.textures.Add(Resources.Load<Texture>("res/Textures/GrassFlowers.png"));

            m_width = 200;
            m_depth = 200;

            

            var mesh = MeshPrimitive.CreateTerrain((uint)m_width, (uint)m_depth, 1);

            

            Load("res/AppData/Terrain.data", ref mesh);
            m_meshFilter.mesh = mesh;

            var collider = gameObject.AddComponent<MeshCollider>();
            var rigidbody = gameObject.AddComponent<Rigidbody>();

            rigidbody.mass = 0;
            rigidbody.Initialize();
            rigidbody.SetFriction(0.1f);
        }

        private bool Load(string filepath, ref Mesh mesh)
        {
            if (mesh == null)
            {
                Debug.Log("Mess is null");
                return false;
            }

            var buffer = System.IO.File.ReadAllBytes(filepath);

            if(buffer == null)
            {
                Debug.Log("Can't read from file " + filepath);
                return false;
            }

            if (buffer.Length == 0)
            {
                Debug.Log("Terrain file could not be loaded because there is no data in the file " + filepath);
                return false;
            }

            int length = buffer.Length;
            int numVertices = length / 4;
            int offset = 0;
            m_heights = new float[numVertices];

            for (int i = 0; i < numVertices; i++)
            {
                float y = BitConverter.ToSingle(buffer, offset);
                mesh.vertices[i].position.y = y;
                m_heights[i] = y;
                offset += 4;
            }

            return true;
        }

        public float SampleHeight(Vector3 position)
        {
            int x = (int)(Mathf.InverseLerp(0, m_width, position.x) * m_width);
            int y = (int)(Mathf.InverseLerp(0, m_depth, position.z * -1.0f) * m_depth);

            int index = (y * m_depth) + x;


            //if(index < m_heights.Length)
            //    return m_heights[index];

            return 0;
        }
    }
}
