using Firefly.Engine.Graphics;
using Firefly.Engine.Input;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;


namespace Firefly.Engine
{
    public class Application : GameWindow
    {
        float[] vertices = {
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };

        private int _width = 700, _height = 500;
        Shader shader;
        Camera camera;
        InputManager input = new();
        Mesh mesh;
        
        float deltaTime;

        public Application(string title, int width, int height) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            shader = new Shader("./Shaders/shader.vert", "./Shaders/shader.frag");
            _width = width;
            _height = height;
        }

        ~Application()
        {
        }
        // Movement speed (units per second)
        float moveSpeed = 5f;

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            input.Update(KeyboardState, MouseState);

            if (input.IsKeyPressed(Keys.Escape)) Close();

            // Use the delta time from FrameEventArgs (this is the correct one for updates)
            float dt = (float)args.Time;

            // Build a direction vector
            Vector3 direction = Vector3.Zero;

            // Horizontal movement (WASD)
            if (input.IsKeyDown(Keys.W)) direction.Z -= 1f; // Forward
            if (input.IsKeyDown(Keys.S)) direction.Z += 1f; // Backward
            if (input.IsKeyDown(Keys.A)) direction.X -= 1f; // Left
            if (input.IsKeyDown(Keys.D)) direction.X += 1f; // Right

            // Vertical movement (Space = up, LeftControl = down)
            if (input.IsKeyDown(Keys.Space)) direction.Y += 1f; // Up
            if (input.IsKeyDown(Keys.LeftControl)) direction.Y -= 1f; // Down

            // Normalize so diagonal movement isn't faster
            if (direction != Vector3.Zero)
            {
                direction.Normalize();

                // Optional: sprint with LeftShift
                float speed = moveSpeed;
                if (input.IsKeyPressed(Keys.LeftShift)) speed *= 3f;

                camera.Position += direction * speed * dt;
            }
        }
        Stopwatch stopwatch = new Stopwatch();
        double lastTime = 0;

        Texture2D texture1;
        Texture2D texture2;
        protected override void OnLoad()
        {
            base.OnLoad();

            mesh = new Mesh(vertices);

            shader.PrintInfo();

            texture1 = new Texture2D("./Shaders/Assets/Texture.png");
            texture2 = new Texture2D("./Shaders/Assets/wall.jpg");

            camera = new Camera(_width, _height);



            shader.Use();

            shader.SetInt("texture1", 0);
            shader.SetInt("texture2", 1);

            stopwatch.Start();

            GL.Enable(EnableCap.DepthTest);


            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        }

        private float _rotationAngle = 0.0f;
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            texture1.Use(TextureUnit.Texture0);

            shader.Use();
            camera.Use(shader);
            camera.Update();

            mesh.Bind();
            double currentTime = stopwatch.Elapsed.TotalSeconds;
            deltaTime = (float)(currentTime - lastTime);
            Matrix4 model = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_rotationAngle));






            shader.SetMatrix4("model", model);


            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();

            _rotationAngle += 50.0f * deltaTime;

            lastTime = currentTime;
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            _width = e.Width;
            _height = e.Height;

            camera.Resize(e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            texture1.Dispose();
            texture2.Dispose();
            base.OnUnload();
        }
    }
}
