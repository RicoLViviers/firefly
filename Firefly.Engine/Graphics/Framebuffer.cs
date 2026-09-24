using OpenTK.Graphics.OpenGL4;

namespace Firefly.Engine.Graphics
{
    public class Framebuffer : IDisposable
    {
        public int Handle;
        public int ColorTexture;
        public int DepthStencil;

        public int Width;
        public int Height;

        public Framebuffer(int width, int height)
        {
            Width = width;
            Height = height;

            Handle = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);

            // 1. Generate the object names/handles first
            ColorTexture = GL.GenTexture();
            DepthStencil = GL.GenRenderbuffer();

            // 2. NEW POSITION: Allocate memory for the textures/buffers immediately
            UpdateAttachments();

            // 3. Configure texture filtering parameters
            GL.BindTexture(TextureTarget.Texture2D, ColorTexture);
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear
            );

            // 4. Attach the allocated objects to your framebuffer
            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D,
                ColorTexture,
                0
            );

            GL.FramebufferRenderbuffer(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.DepthStencilAttachment,
                RenderbufferTarget.Renderbuffer,
                DepthStencil
            );

            // Verify everything bound correctly
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer)
                != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Framebuffer is incomplete!");
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }


        private void UpdateAttachments()
        {
            // Resize / allocate color texture
            GL.BindTexture(TextureTarget.Texture2D, ColorTexture);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba8,
                Width,
                Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                IntPtr.Zero
            );

            GL.BindRenderbuffer(
                RenderbufferTarget.Renderbuffer,
                DepthStencil
            );

            GL.RenderbufferStorage(
                RenderbufferTarget.Renderbuffer,
                RenderbufferStorage.Depth24Stencil8,
                Width,
                Height
            );
        }

        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0)
                return;

            if (width == Width && height == Height)
                return;

            Width = width;
            Height = height;

            UpdateAttachments();
        }

        public void Bind()
        {
            GL.BindFramebuffer(
                FramebufferTarget.Framebuffer,
                Handle
            );

            GL.Viewport(0, 0, Width, Height);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(
                FramebufferTarget.Framebuffer,
                0
            );
        }

        public void Dispose()
        {
            if (Handle == 0)
                return;

            GL.DeleteFramebuffer(Handle);
            GL.DeleteTexture(ColorTexture);
            GL.DeleteRenderbuffer(DepthStencil);

            Handle = 0;
            ColorTexture = 0;
            DepthStencil = 0;

            GC.SuppressFinalize(this);
        }
    }
}