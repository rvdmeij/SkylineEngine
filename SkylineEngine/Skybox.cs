using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace SkylineEngine
{
    public class SkyboxFaces
    {
        public string left;
        public string right;
        public string top;
        public string bottom;
        public string front;
        public string back;

        public SkyboxFaces(string left, string right, string top, string bottom, string front, string back)
        {
            this.right = right;
            this.left = left;
            this.top = top;
            this.bottom = bottom;
            this.front = front;
            this.back = back;
        }
    }

    public class Skybox
    {
        private uint m_texture;
        Vector3[] verticesBuffer;
        Shader shader;
        uint VAO;
        uint VBO;

        public Skybox()
        {
            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));

            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));

            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));

            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));

            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));

            vertices.Add(new Vector3(-1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, 1.0f));

            verticesBuffer = vertices.ToArray();
        }

        public void SetShader(Shader shader)
        {
            this.shader = shader;
            this.shader.Bind();
        }

        /// <summary>
        /// Load the skybox with the specified front, back, top, bottom, left and right textures
        /// </summary>
        /// <param name="front">Path to front texture.</param>
        /// <param name="back">Path to back texture.</param>
        /// <param name="top">Path to top texture.</param>
        /// <param name="bottom">Path to bottom texture.</param>
        /// <param name="left">Path to left texture.</param>
        /// <param name="right">Path to right texture.</param>
        public void Load(string front, string back, string top, string bottom, string left, string right)
        {
            List<string> faces = new List<string>()
            {
                right,
                left,
                top,
                bottom,
                front,
                back
            };

            Load(faces);
        }

        /// <summary>
        /// Load the skybox with the specified faces.
        /// </summary>
        /// <param name="faces">Path to skyboxtextures in this order: right, left, top, bottom, front, back.</param>
        public void Load(List<string> faces)
        {
            GL.GenTextures(1, out m_texture);
            
            
            GL.BindTexture(TextureTarget.TextureCubeMap, m_texture);
            

            int width;
            int height;
            //int channels;

            for (int i = 0; i < faces.Count; i++)
            {
                byte[] imageData = Image.Load(faces[i], out width, out height);

                if (imageData != null)
                {
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, imageData);
                }
                else
                {
                    Debug.Log("Cubemap texture failed to load: " + faces[i]);
                }
            }

            int textureMagFilterMode = (int)TextureMagFilter.Linear;
            int textureMinFilterMode = (int)TextureMinFilter.Linear;
            int textureWrapMode = (int)TextureWrapMode.ClampToEdge;

            GL.TexParameterI(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, ref textureMinFilterMode);
            GL.TexParameterI(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, ref textureMagFilterMode);
            GL.TexParameterI(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, ref textureWrapMode);
            GL.TexParameterI(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, ref textureWrapMode);
            GL.TexParameterI(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, ref textureWrapMode);

        }

        public void Bind()
        {
            int size = verticesBuffer.Length * Marshal.SizeOf(typeof(Vector3));

            GL.GenVertexArrays(1, out VAO);            
            GL.GenBuffers(1, out VBO);            
            GL.BindVertexArray(VAO);            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);            
            GL.BufferData(BufferTarget.ArrayBuffer, size, verticesBuffer, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public void Draw()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.BindVertexArray(VAO);            
            GL.ActiveTexture(TextureUnit.Texture0);            
            GL.BindTexture(TextureTarget.TextureCubeMap, m_texture);            
            GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, 0, 36);            
            GL.BindVertexArray(0);            
            GL.UseProgram(0);            
            GL.ActiveTexture(TextureUnit.Texture0);            
            GL.BindTexture(TextureTarget.Texture2D, 0);            
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);        
            GL.Enable(EnableCap.DepthTest);            
        }
    }

    public class ProceduralSkybox
    {
        private Vector3[] verticesBuffer;
        private Shader shader;
        private uint VAO;
        private uint VBO;

        public ProceduralSkybox()
        {
            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));

            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));

            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));

            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));

            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, -1.0f));

            vertices.Add(new Vector3(-1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, -1.0f));
            vertices.Add(new Vector3(-1.0f, -1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, -1.0f, 1.0f));

            verticesBuffer = vertices.ToArray();
        }

        public void SetShader(Shader shader)
        {
            this.shader = shader;
            this.shader.Bind();
        }

        public void Bind()
        {
            int size = verticesBuffer.Length * Marshal.SizeOf(typeof(Vector3));

            GL.GenVertexArrays(1, out VAO);            
            GL.GenBuffers(1, out VBO);            
            GL.BindVertexArray(VAO);            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);            
            GL.BufferData(BufferTarget.ArrayBuffer, size, verticesBuffer, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public void Draw()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.BindVertexArray(VAO);
            GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, 0, 36);            
            GL.BindVertexArray(0);            
            GL.UseProgram(0);                     
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);             
        }
    }
}
