using OpenTK.Graphics.OpenGL;

namespace SkylineEngine
{
    public enum TextureCoordinate
    {
        S = TextureParameterName.TextureWrapS,
        T = TextureParameterName.TextureWrapT,
        R = TextureParameterName.TextureWrapR
    }

    public class Texture : Resource
    {
        private uint m_texture;
        private int m_width;
        private int m_height;
        private string m_name;
        private byte[] m_imageData;

        public int textureId { get { return (int)m_texture; } }
        public int width { get { return m_width; } }
        public int height { get { return m_height; } }
        public string name { get { return m_name; } }
        public byte[] imageData { get { return m_imageData; } }

        public static readonly float MaxAniso;
        public const GetPName MAX_TEXTURE_MAX_ANISOTROPY = (GetPName)0x84FF;

        public int GetId()
        {
            return (int)m_texture;
        }

        static Texture()
        {
            //MaxAniso = GL.GetFloat(MAX_TEXTURE_MAX_ANISOTROPY);
        }

        public Texture(int texID, int width, int height)
        {
            this.m_texture = (uint)texID;
            this.m_width = width;
            this.m_height = height;
        }

        public Texture(int width, int height, string name)
        {
            this.m_texture = 0;
            this.m_width = width;
            this.m_height = height;
            this.m_name = name;
            GL.GenTextures(1, out m_texture);
        }

        public Texture(int width, int height, byte[] data = null, string name = "texture")
        {
            this.m_width = width;
            this.m_height = height;
            m_texture = 0;

            if (data == null)
            {
                int size = sizeof(uint);
                m_imageData = new byte[width * height * size];
                int index = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        m_imageData[index + 0] = 255;
                        m_imageData[index + 1] = 255;
                        m_imageData[index + 2] = 255;
                        m_imageData[index + 3] = 255;
                        index += 4;
                    }
                }
            }
            else
            {
                m_imageData = new byte[data.Length];
                System.Buffer.BlockCopy(data, 0, m_imageData, 0, data.Length);
            }

            int textureWrapMode = (int)TextureWrapMode.Repeat;

            GL.GenTextures(1, out m_texture);
            GL.BindTexture(TextureTarget.Texture2D, m_texture);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ref textureWrapMode);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ref textureWrapMode);

            int textureMagFilterMode = (int)TextureMagFilter.Linear;
            int textureMinFilterMode = (int)TextureMinFilter.LinearMipmapLinear;

            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref textureMagFilterMode);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref textureMinFilterMode);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_width, m_height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, m_imageData);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            m_name = "texture";
        }

        public Texture(string filePath)
        {
            m_texture = 0;

            if (!System.IO.File.Exists(filePath))
                throw new System.Exception("File does not exist: " + filePath);

            m_imageData = Image.Load(filePath, out m_width, out m_height);

            int textureWrapMode = (int)TextureWrapMode.Repeat;

            GL.GenTextures(1, out m_texture);

            GL.BindTexture(TextureTarget.Texture2D, m_texture);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ref textureWrapMode);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ref textureWrapMode);            

            int textureMagFilterMode = (int)TextureMagFilter.Linear;
            int textureMinFilterMode = (int)TextureMinFilter.LinearMipmapLinear;

            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref textureMagFilterMode);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref textureMinFilterMode);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_width, m_height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, m_imageData);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            m_name = filePath;

        }

        public void Dispose()
        {
            GL.DeleteTextures(1, ref m_texture);
        }

        public void Bind(int unit)
        {
            Assert(unit <= 31);

            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            GL.BindTexture(TextureTarget.Texture2D, m_texture);
        }

        public void SetMinFilter(TextureMinFilter filter)
        {
            GL.BindTexture(TextureTarget.Texture2D, m_texture);
            int f = (int)filter;
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref f);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void SetMagFilter(TextureMagFilter filter)
        {
            GL.BindTexture(TextureTarget.Texture2D, m_texture);
            int f = (int)filter;
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref f);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void SetAnisotropy(float level)
        {
            // const TextureParameterName TEXTURE_MAX_ANISOTROPY = (TextureParameterName)0x84FE;

            // GL.BindTexture(TextureTarget.Texture2D, m_texture);
            // GL.TexParameterI(TextureTarget.Texture2D, TEXTURE_MAX_ANISOTROPY, Mathf.Clamp(level, 1, MaxAniso));
            // GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void SetWrap(TextureCoordinate coord, TextureWrapMode mode)
        {
            GL.BindTexture(TextureTarget.Texture2D, m_texture);
            int m = (int)mode;
            GL.TexParameterI(TextureTarget.Texture2D, (TextureParameterName)coord, ref m);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private bool Assert(bool expression)
        {
            if (!expression)
                throw new System.Exception("Texture unit is outside of allowed range");
            return expression;
        }
    }
}
