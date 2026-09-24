using Firefly.Engine.Graphics;
using Firefly.Engine.Graphics.Primitives;
using Firefly.Engine.Input;
using Firefly.Engine.Scene;
using Firefly.Engine.Physics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;



namespace Firefly.Sandbox
{
    public class Sandbox : Engine.Engine
    {
        private readonly Shader shader;
        private readonly Camera camera;
        private readonly InputManager input = new();

        private readonly Texture2D texture1;
        private readonly Texture2D texture2;
        private readonly List<SceneObject> objects = [];
        private readonly RigidBody rigidBody = new();

        private float yaw = -90f;
        private float pitch = 0f;
        private readonly float moveSpeed = 5f;
        SceneObject sphere;


        public Sandbox(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            camera = new Camera(Size.X, Size.Y);

            shader = new Shader(
                "./Shaders/shader.vert",
                "./Shaders/shader.frag"
            );

            texture1 = new Texture2D("./Shaders/Assets/Texture.png");
            texture2 = new Texture2D("./Shaders/Assets/wall.jpg");

            CursorState = CursorState.Grabbed;
            WindowState = WindowState.Maximized;
        }

        protected override void Initialize()
        {




            shader.Use();
            shader.SetInt("texture1", 0);
            shader.SetInt("texture2", 1);

            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            // Create 10 cubes
            Vector3[] positions =
            [
                new( 0f,  0f,  0f),
            new( 2f,  0f,  0f),
            new(-2f,  0f,  0f),

            new( 0f,  2f,  0f),
            new( 0f, -2f,  0f),

            new( 0f,  0f, -3f),
            new( 2f,  1f, -5f),
            new(-2f,  1f, -5f),

            new( 4f,  0f, -6f),
            new(-4f,  0f, -6f)
            ];

            foreach (Vector3 position in positions)
            {
                SceneObject cube = Cube.Create(shader);

                cube.Transform.Position = position;
                objects.Add(cube);
            }

            SceneObject floor = Plane.Create(shader);

            floor.Transform.Position = new Vector3(0, -1, 0);
            floor.Transform.Scale = new Vector3(10, 1, 10);

            objects.Add(floor);

            SceneObject pyramid = Pyramid.Create(shader);
            pyramid.Transform.Position = new Vector3(2, 0, -3);

            objects.Add(pyramid);


            sphere = Sphere.Create(shader);
            sphere.Transform.Position = new(-1, 1, -2);
            objects.Add(sphere);
        }

        protected override void Update(float deltaTime)
        {
            input.Update(KeyboardState, MouseState);

            if (input.IsKeyPressed(Keys.Escape))
                Close();

            // Mouse look

            Vector2 mouseDelta = input.MouseDelta;

            yaw += mouseDelta.X * 0.15f;
            pitch -= mouseDelta.Y * 0.15f;

            pitch = MathHelper.Clamp(pitch, -89f, 89f);

            Vector3 front;

            front.X =
                MathF.Cos(MathHelper.DegreesToRadians(yaw)) *
                MathF.Cos(MathHelper.DegreesToRadians(pitch));

            front.Y =
                MathF.Sin(MathHelper.DegreesToRadians(pitch));

            front.Z =
                MathF.Sin(MathHelper.DegreesToRadians(yaw)) *
                MathF.Cos(MathHelper.DegreesToRadians(pitch));

            camera.Front = Vector3.Normalize(front);

            // Movement

            Vector3 direction = Vector3.Zero;

            if (input.IsKeyDown(Keys.W))
                direction += camera.Front;

            if (input.IsKeyDown(Keys.S))
                direction -= camera.Front;

            if (input.IsKeyDown(Keys.A))
                direction -= camera.Right;

            if (input.IsKeyDown(Keys.D))
                direction += camera.Right;

            if (input.IsKeyDown(Keys.Space))
                direction.Y += 1f;

            if (input.IsKeyDown(Keys.LeftControl))
                direction.Y -= 1f;

            if (direction != Vector3.Zero)
            {
                direction.Normalize();

                float speed = moveSpeed;

                if (input.IsKeyDown(Keys.LeftShift))
                    speed *= 3f;

                camera.Position += direction * speed * deltaTime;
            }


            rigidBody.Update(deltaTime);

            sphere.Transform.Position += rigidBody.Velocity * deltaTime;
        }

        protected override void Render(float deltaTime)
        {
            GL.Clear(
                ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit
            );

            texture1.Use(TextureUnit.Texture0);

            shader.Use();

            camera.Use(shader);
            camera.Update();

            foreach (SceneObject obj in objects)
            {
                obj.Draw(camera);
            }

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
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
