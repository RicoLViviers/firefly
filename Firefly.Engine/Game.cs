using Firefly.Engine.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace Firefly.Engine
{
    public class Game : GameWindow
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

        /*float[] vertices = {
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };*/

        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        Shader shader;
        public int VBO, VAO, EBO;
        private float _width, _height;

        // NEW: camera/movement state as fields
        private Vector3 cameraPos = new Vector3(0.0f, 0.0f, 3.0f);
        private Vector3 cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 cameraUp = Vector3.UnitY;
        private float speed = 1.5f;
        float deltaTime;

        public Game(string title, int width, int height)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            shader = new Shader("./Shaders/shader.vert", "./Shaders/shader.frag");
            _width = width;
            _height = height;
        }

        ~Game()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VBO);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            KeyboardState input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyPressed(Keys.W))
            {
                cameraPos += cameraFront * speed * deltaTime; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                cameraPos -= cameraFront * speed * deltaTime; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                cameraPos -= Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * speed * deltaTime; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                cameraPos += Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * speed * deltaTime; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                cameraPos += cameraUp * speed * deltaTime; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                cameraPos -= cameraUp * speed * deltaTime; //Down
            }
        }
        Stopwatch stopwatch = new Stopwatch();
        double lastTime = 0;
        
        Texture2D texture1;
        Texture2D texture2;
        protected override void OnLoad()
        {
            base.OnLoad();

            shader.PrintInfo();

            texture1 = new Texture2D("./Shaders/Assets/Texture.png");
            texture2 = new Texture2D("./Shaders/Assets/wall.jpg");


            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);


            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

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
            
            GL.BindVertexArray(VAO);
            double currentTime = stopwatch.Elapsed.TotalSeconds;
            deltaTime = (float)(currentTime - lastTime);
            Matrix4 model = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_rotationAngle));



            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)(_width / _height), 0.1f, 100.0f);


            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(cameraPos - cameraTarget);
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(cameraUp, cameraDirection));
            Vector3 realCameraUp = Vector3.Cross(cameraDirection, cameraRight);
            Matrix4 view = Matrix4.LookAt(cameraPos, cameraPos + cameraFront, cameraUp);



            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);


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
        }

        protected override void OnUnload()
        {
            texture1.Dispose();
            texture2.Dispose();
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(EBO);
            base.OnUnload();
        }

    }
}