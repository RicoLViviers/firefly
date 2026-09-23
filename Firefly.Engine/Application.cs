using Firefly.Engine.Graphics;
using Firefly.Engine.Input;
using Firefly.Engine.Scene;
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
            camera = new Camera(_width, _height);
            CursorState = CursorState.Grabbed;
            WindowState = WindowState.Maximized;
        }

        ~Application()
        {
        }
        float moveSpeed = 5f;
        private Vector2 lastMousePosition;
        private float yaw = -90f;
        private float pitch = 0f;

        private float mouseSensitivity = 0.15f;
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            input.Update(KeyboardState, MouseState);

            if (input.IsKeyPressed(Keys.Escape)) Close();

            float dt = (float)args.Time;

            // =========================
            // Mouse Look
            // =========================

            Vector2 mouseDelta = input.MouseDelta;

            float mouseSensitivity = 0.15f;

            yaw += mouseDelta.X * mouseSensitivity;
            pitch -= mouseDelta.Y * mouseSensitivity;

            pitch = MathHelper.Clamp(pitch, -89f, 89f);

            Vector3 front;

            front.X = MathF.Cos(MathHelper.DegreesToRadians(yaw)) *
                      MathF.Cos(MathHelper.DegreesToRadians(pitch));

            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));

            front.Z = MathF.Sin(MathHelper.DegreesToRadians(yaw)) *
                      MathF.Cos(MathHelper.DegreesToRadians(pitch));

            camera.Front = Vector3.Normalize(front);

            Vector3 direction = Vector3.Zero;

            if (input.IsKeyDown(Keys.W)) direction += camera.Front;
            if (input.IsKeyDown(Keys.S)) direction -= camera.Front; 
            if (input.IsKeyDown(Keys.A)) direction -= camera.Right; 
            if (input.IsKeyDown(Keys.D)) direction += camera.Right;

            if (input.IsKeyDown(Keys.Space)) direction.Y += 1f;
            if (input.IsKeyDown(Keys.LeftControl)) direction.Y -= 1f; 

            if (direction != Vector3.Zero)
            {
                direction.Normalize();

                float speed = moveSpeed;

                if (input.IsKeyDown(Keys.LeftShift))
                    speed *= 3f;

                camera.Position += direction * speed * dt;
            }
        }
        Stopwatch stopwatch = new Stopwatch();
        double lastTime = 0;

        Texture2D texture1;
        Texture2D texture2;
        SceneObject cube;
        protected override void OnLoad()
        {
            base.OnLoad();

            mesh = new Mesh(vertices);
            cube = new SceneObject(mesh, shader);

            shader.PrintInfo();

            texture1 = new Texture2D("./Shaders/Assets/Texture.png");
            texture2 = new Texture2D("./Shaders/Assets/wall.jpg");

            



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

            cube.Draw(camera);


            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
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
