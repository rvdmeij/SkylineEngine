using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SkylineEngine
{
    public sealed class ParticleSystem : Renderer
    {
        public class Particle
        {
            public struct Properties
            {
                public Vector3 position;
                public Quaternion rotation;
                public Vector3 scale;
                public Vector3 velocity;
                public Vector3 velocityVariation;
                public Color32 colorBegin;
                public Color32 colorEnd;
                public float sizeBegin;
                public float sizeEnd;
                public float sizeVariation;
                public float lifeTime;
            }

            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
            public Vector3 velocity;
            public Color32 colorBegin;
            public Color32 colorEnd;
            public float sizeBegin;
            public float sizeEnd;
            public float lifeTime;
            public float lifeRemaining;
            public bool active;

            public Properties properties;
            private Matrix4 parentMatrix;

            public Particle()
            {
                this.properties = new Properties();
                this.position = Vector3.zero;
                this.rotation = Quaternion.identity;
                this.scale = Vector3.one;
                this.velocity = Vector3.zero;
                this.colorBegin = new Color32(255, 0, 0, 255);
                this.colorEnd = new Color32(255, 255, 255, 255);
                this.sizeBegin = 1.0f;
                this.sizeEnd = 0.1f;
                this.lifeTime = 2.0f;
                this.lifeRemaining = 2.0f;
                this.active = false;
                parentMatrix = Matrix4.Identity;
            }

            public Matrix4 GetViewMatrix()
            {
                Matrix4 translationMatrix = Matrix4.CreateTranslation(position.ToOpenTKVector());
                Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(rotation.ToOpenTKQuaternion());
                Matrix4 scaleMatrix = Matrix4.CreateScale(scale.ToOpenTKVector());
                return scaleMatrix * rotationMatrix * translationMatrix * parentMatrix;
            }

            public void OnParentTransformChanged(ref Matrix4 parent)
            {
                parentMatrix = parent.ClearRotation();
            }
        }

        private Particle[] m_particles;
        private Material m_material;
        private int m_numParticles = 1000;
        private int m_PoolIndex = 999;
        private int m_vao = 0;
        private int m_vbo = 0;
        private int m_ebo = 0;
        private Mesh mesh;

        public Material material
        {
            get { return m_material; }
        }

        public override void InitializeComponent()
        {
            RenderPipeline.PushData<ParticleSystem>(this.gameObject);

            transform.onTransformChanged += Transform_OnTransformChanged;
        }

        void Transform_OnTransformChanged(ref Matrix4 m)
        {
            if (m_particles == null)
                return;

            for (int i = 0; i < m_particles.Length; i++)
            {
                m_particles[i].OnParentTransformChanged(ref m);
            }
        }


        public override bool Initialize()
        {
            if (m_vao > 0)
                return false;

            m_particles = new Particle[m_numParticles];

            for (int i = 0; i < m_particles.Length; i++)
            {
                m_particles[i] = new Particle();
            }

            mesh = MeshPrimitive.CreatePlane2D(new Vector3(1, 1, 1));

            GL.CreateVertexArrays(1, out m_vao);
            GL.BindVertexArray(m_vao);

            GL.CreateBuffers(1, out m_vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.vertices.Length * Marshal.SizeOf(typeof(Vertex)), mesh.vertices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "position"));

            //Color
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "color"));

            //Texcoord
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "uv"));

            //Normal
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "normal"));

            GL.CreateBuffers(1, out m_ebo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.indices.Length * sizeof(uint), mesh.indices, BufferUsageHint.StaticDraw);

            m_material = gameObject.AddComponent<Material>();
            m_material.shader = Resources.Load<Shader>("res/Shaders/Particle.shader");
            m_material.textures.Add(Resources.Load<Texture>("res/Textures/Grass.jpg"));
            material.Initialize();

            Debug.Log("Texture ID " + m_material.textures[0].textureId);

            return true;
        }

        public override void Render()
        {
            if(!enabled)
                return;

            if(!gameObject.activeSelf)
                return;                

            Update();

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            m_material.textures[0].Bind(0);
            m_material.shader.Bind();
            m_material.UpdateUniforms();

            Matrix4 view = Camera.main.GetViewMatrix();
            Matrix4 proj = Camera.main.GetPerspectiveProjectionMatrix();

            Matrix4 lookAt = Matrix4.LookAt(transform.position.ToOpenTKVector(), Camera.main.transform.position.ToOpenTKVector(), Vector3.up.ToOpenTKVector());

            m_material.shader.SetMat4("u_View", view);
            m_material.shader.SetMat4("u_Projection", proj);
            m_material.shader.SetMat4("u_LookAt", lookAt);

            for (int i = 0; i < m_particles.Length; i++)
            {
                if (!m_particles[i].active)
                    continue;

                // Fade away particles
                float life = m_particles[i].lifeRemaining / m_particles[i].lifeTime;

                Color32 color = Color32.Lerp(m_particles[i].colorEnd, m_particles[i].colorBegin, life);
                color.a = color.a * life;

                float size = Mathf.Lerp(m_particles[i].sizeEnd, m_particles[i].sizeBegin, life);

                m_particles[i].scale = new Vector3(size, size, 0);

                Matrix4 transformation = m_particles[i].GetViewMatrix();

                m_material.shader.SetMat4("u_Model", transformation);
                m_material.shader.SetInt("u_Texture0", (int)m_material.textures[0].textureId);
                m_material.shader.SetFloat4("u_Color", color);

                GL.BindVertexArray(m_vao);
                GL.DrawElements(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
                GL.BindVertexArray(0);
            }

            GL.BindVertexArray(0);

            GL.UseProgram(0);

            GL.ActiveTexture(TextureUnit.Texture0);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public override void Render(Material material)
        {

        }

        private void Update()
        {
            for (int i = 0; i < m_particles.Length; i++)
            {
                if (!m_particles[i].active)
                    continue;

                if (m_particles[i].lifeRemaining <= 0.0f)
                {
                    m_particles[i].active = false;
                    m_particles[i].position = transform.position;
                    continue;
                }

                m_particles[i].lifeRemaining -= Time.deltaTime;
                m_particles[i].position += m_particles[i].velocity * Time.deltaTime;
                m_particles[i].rotation = Quaternion.Euler(new Vector3(0.01f * Time.time, 0, 0));
            }
        }

        public void Emit(Particle.Properties particleProps)
        {
            Particle particle = m_particles[m_PoolIndex];

            //if (particle.active)
            //    return;

            particle.active = true;
            particle.position = particleProps.position;
            particle.rotation.eulerAngles = new Vector3(1, 1, 1) * Random.Range(0.0f, 1.0f) * 2.0f * Mathf.PI;

            // Velocity
            particle.velocity = particleProps.velocity;
            particle.velocity.x += particleProps.velocityVariation.x * (Random.Range(0.0f, 1.0f) - 0.5f);
            particle.velocity.y += particleProps.velocityVariation.y * (Random.Range(0.0f, 1.0f) - 0.5f);
            particle.velocity.z += particleProps.velocityVariation.z * (Random.Range(0.0f, 1.0f) - 0.5f);

            // Color
            particle.colorBegin = particleProps.colorBegin;
            particle.colorEnd = particleProps.colorEnd;

            particle.lifeTime = particleProps.lifeTime;
            particle.lifeRemaining = particleProps.lifeTime;
            particle.sizeBegin = particleProps.sizeBegin + particleProps.sizeVariation * (Random.Range(0.0f, 1.0f) - 0.5f);
            particle.sizeEnd = particleProps.sizeEnd;

            m_PoolIndex--;
            if (m_PoolIndex < 0)
                m_PoolIndex = m_particles.Length - 1;
        }
    }
}
