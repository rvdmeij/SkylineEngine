using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SkylineEngine.Shaders;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LineVertex
    {
        public Vector3 position;
        public Color32 color;

        public LineVertex(Vector3 position, Color32 color)
        {
            this.position = position;
            this.color = color;
        }
    }

    public sealed class LineRenderer : Renderer
    {
        public LineVertex[] m_lines = null;
        private List<Material> m_materials = new List<Material>();

        private uint VAO = 0;
        private uint VBO = 0;

        public override void InitializeComponent()
        {
            var material = gameObject.AddComponent<Material>();
            material.shader = Resources.LoadShader(LineShader.vertex, LineShader.fragment, "Lines");
        }

        public void AddLines(List<LineVertex> lines)
        {
            m_lines = lines.ToArray();
            RenderPipeline.PushData<LineRenderer>(this.gameObject);
        }

        public override bool Initialize()
        {
            var materials = new List<Material>(gameObject.GetComponents<Material>());
            m_materials = materials;

            for (int i = 0; i < m_materials.Count; i++)
            {
                m_materials[i].Initialize();
            }

            //Create VAO
            GL.CreateVertexArrays(1, out this.VAO);
            GL.BindVertexArray(this.VAO);

            //GEN VBO AND BIND AND SEND DATA
            GL.GenBuffers(1, out this.VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);

            GL.BufferData(BufferTarget.ArrayBuffer, m_lines.Length * Marshal.SizeOf(typeof(LineVertex)), m_lines, BufferUsageHint.StaticDraw);

            //SET VERTEXATTRIBPOINTERS AND ENABLE (INPUT ASSEMBLY)
            //Position
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(LineVertex)), Marshal.OffsetOf(typeof(LineVertex), "position"));

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(LineVertex)), Marshal.OffsetOf(typeof(LineVertex), "color"));

            //BIND VAO 0
            GL.BindVertexArray(0);
            return true;
        }

        public void Update()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, m_lines.Length * Marshal.SizeOf(typeof(LineVertex)), m_lines, BufferUsageHint.DynamicDraw);
        }

        public override void Render()
        {
            if (!enabled)
                return;

            if(!gameObject.activeSelf)
                return;

            Matrix4 model = transform.GetViewMatrix();
            Matrix4 view = Camera.main.GetViewMatrix();
            Matrix4 proj = Camera.main.GetPerspectiveProjectionMatrix();

            for (int i = 0; i < m_materials.Count; i++)
            {
                m_materials[i].model = model;
                m_materials[i].view = view;
                m_materials[i].projection = proj;

                GL.Enable(EnableCap.DepthTest);

                if (m_materials[i].alphaBlend)
                {
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    GL.Disable(EnableCap.CullFace);
                }
                else
                {
                    GL.Disable(EnableCap.Blend);
                }

                m_materials[i].shader.Bind();
                m_materials[i].UpdateUniforms();

                //Bind VAO
                GL.BindVertexArray(this.VAO);

                //RENDER
                GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Lines, 0, m_lines.Length);
                GL.BindVertexArray(0);
                GL.UseProgram(0);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Disable(EnableCap.DepthTest);
            }
        }

        public override void Render(Material material)
        {

        }
    }
}
