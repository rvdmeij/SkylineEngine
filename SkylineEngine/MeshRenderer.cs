using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace SkylineEngine
{
    public sealed class MeshRenderer : Renderer
    {
        public bool doubleSided;
        public FrontFaceDirection frontFaceDirection = FrontFaceDirection.Ccw;

        private MeshFilter m_meshFilter;
        private List<Material> m_materials;

        public MeshFilter meshFilter { get { return m_meshFilter; } }

        public bool meshAvailable
        {
            get
            {
                if (m_meshFilter == null)
                    return false;

                if(m_meshFilter.mesh.indices == null)
                {
                    if (m_meshFilter.mesh.GetVAO() > 0 && m_meshFilter.mesh.GetVBO() > 0)
                        return true;
                    return false;
                }
                else
                {
                    if(m_meshFilter.mesh.indices.Length > 0)
                    {
                        if (m_meshFilter.mesh.GetEBO() > 0 && m_meshFilter.mesh.GetVAO() > 0 && m_meshFilter.mesh.GetVBO() > 0)
                            return true;
                        return false;
                    }
                    else
                    {
                        if (m_meshFilter.mesh.GetVAO() > 0 && m_meshFilter.mesh.GetVBO() > 0)
                            return true;
                        return false;
                    }
                }
            }
        }

        public override bool Initialize()
        {
            if (m_meshFilter == null)
            {
                m_meshFilter = gameObject.GetComponent<MeshFilter>();

                if (m_meshFilter != null)
                {
                    if (meshFilter.mesh == null)
                        return false;

                    m_meshFilter.mesh.Initialize();
                }
                else
                    return false;
            }

            m_materials = new List<Material>(gameObject.GetComponents<Material>());

            if (m_materials == null)
                return false;

            if (m_materials.Count == 0)
                return false;

            for (int i = 0; i < m_materials.Count; i++)
            {
                m_materials[i].Initialize();
            }

            return true;
        }

        public override void Render()
        {
            if (!enabled)
                return;

            if(!gameObject.activeSelf)
                return;

            if (!meshAvailable)
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
                    if (!doubleSided)
                    {
                        GL.Enable(EnableCap.CullFace);
                        GL.CullFace(CullFaceMode.Back);
                    }
                    else
                    {
                        GL.Disable(EnableCap.CullFace);
                    }
                }

                if (m_materials[i].wireframe)
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                }
                else
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }

                if (m_materials[i].textures != null)
                {
                    int unit = 0;
                    for (int j = 0; j < m_materials[i].textures.Count; j++)
                    {
                        m_materials[i].textures[j].Bind(unit);
                        unit++;
                    }
                }

                m_materials[i].shader.Bind();
                m_materials[i].UpdateUniforms();

                GL.FrontFace(frontFaceDirection);

                //Bind VAO
                GL.BindVertexArray(meshFilter.mesh.GetVAO());

                //RENDER
                if (m_meshFilter.mesh.indices.Length == 0)
                {
                    GL.DrawArrays(m_materials[i].mode, 0, m_meshFilter.mesh.vertices.Length);
                }
                else
                {
                    GL.DrawElements(m_materials[i].mode, m_meshFilter.mesh.indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
                    GL.BindVertexArray(0);
                }

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
