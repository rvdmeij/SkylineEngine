using OpenTK.Graphics.OpenGL;

namespace SkylineEngine
{
    public class WaterFrameBuffer
    {
        protected static int REFLECTION_WIDTH = 320;
        private static int REFLECTION_HEIGHT = 180;

        protected static int REFRACTION_WIDTH = 1280;
        private static int REFRACTION_HEIGHT = 720;

        private int reflectionFrameBuffer;
        private int reflectionTexture;
        private int reflectionDepthBuffer;

        private int refractionFrameBuffer;
        private int refractionTexture;
        private int refractionDepthTexture;

        public int ReflectionTexture
        {
            get { return reflectionTexture; }
        }
        
        public int RefractionTexture
        {
            get { return refractionTexture; }
        }
        
        public int RefractionDepthTexture
        {
            get { return refractionDepthTexture; }
        }

        public WaterFrameBuffer()
        {
            InitializeReflectionFrameBuffer();
		    initialiseRefractionFrameBuffer();    
        }

        public void Dispose()
        {
            GL.DeleteFramebuffers(1, ref reflectionFrameBuffer);
            GL.DeleteTextures(1, ref reflectionTexture);
            GL.DeleteRenderbuffers(1, ref reflectionDepthBuffer);
            GL.DeleteFramebuffers(1, ref refractionFrameBuffer);
            GL.DeleteTextures(1, ref refractionTexture);
            GL.DeleteTextures(1, ref refractionDepthTexture);
        }

        //call before rendering to this FBO
        public void BindReflectionFrameBuffer() 
        {
            BindFrameBuffer(reflectionFrameBuffer, REFLECTION_WIDTH, REFLECTION_HEIGHT);
        }
        
        //call before rendering to this FBO
        public void BindRefractionFrameBuffer() 
        {
            BindFrameBuffer(refractionFrameBuffer, REFRACTION_WIDTH, REFRACTION_HEIGHT);
        }
        
        //call to switch to default frame buffer
        public void UnbindCurrentFrameBuffer() 
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, Screen.width, Screen.height);
        }

        private void InitializeReflectionFrameBuffer() 
        {
            reflectionFrameBuffer = CreateFrameBuffer();
            reflectionTexture = CreateTextureAttachment(REFLECTION_WIDTH,REFLECTION_HEIGHT);
            reflectionDepthBuffer = CreateDepthBufferAttachment(REFLECTION_WIDTH,REFLECTION_HEIGHT);
            UnbindCurrentFrameBuffer();
        }
        
        private void initialiseRefractionFrameBuffer() 
        {
            refractionFrameBuffer = CreateFrameBuffer();
            refractionTexture = CreateTextureAttachment(REFRACTION_WIDTH,REFRACTION_HEIGHT);
            refractionDepthTexture = CreateDepthTextureAttachment(REFRACTION_WIDTH,REFRACTION_HEIGHT);
            UnbindCurrentFrameBuffer();
        }
        
        private void BindFrameBuffer(int frameBuffer, int width, int height)
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);//To make sure the texture isn't bound
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.Viewport(0, 0, width, height);
        }

        private int CreateFrameBuffer() 
        {
            GL.GenFramebuffers(1, out int frameBuffer);
            //generate name for frame buffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            //create the framebuffer
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            //indicate that we will always render to color attachment 0
            return frameBuffer;
        }

        private int CreateTextureAttachment( int width, int height) 
        {
            int textureMagFilterMode = (int)TextureMagFilter.Linear;

            GL.GenTextures(1, out int texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, (System.IntPtr)null);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref textureMagFilterMode);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref textureMagFilterMode);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, texture, 0);
            return texture;
        }
        
        private int CreateDepthTextureAttachment(int width, int height)
        {
            int textureMagFilterMode = (int)TextureMagFilter.Linear;

            GL.GenTextures(1, out int texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, (System.IntPtr)null);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref textureMagFilterMode);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref textureMagFilterMode);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, texture, 0);
            return texture;
        }

        private int CreateDepthBufferAttachment(int width, int height) 
        {
            GL.GenRenderbuffers(1, out int depthBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
            return depthBuffer;
        }
    }
}