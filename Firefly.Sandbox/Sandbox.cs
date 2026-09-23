using Firefly.Engine.Graphics;
using Firefly.Engine.Input;
using Firefly.Engine.Scene;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace Firefly.Sandbox;

public class Sandbox : Engine.Engine
{
    private Mesh mesh;
    private Shader shader;
    private Camera camera;
    private InputManager input = new();

    private Texture2D texture1;
    private Texture2D texture2;
    private List<SceneObject> cubes = new();

    private float yaw = -90f;
    private float pitch = 0f;
    private float moveSpeed = 5f;

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

    

    public Sandbox(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        CursorState = CursorState.Grabbed;
        camera = new Camera(Size.X, Size.Y);
        WindowState = WindowState.Maximized;
    }

    protected override void Initialize()
    {
        mesh = new Mesh(vertices);

        shader = new Shader(
            "./Shaders/shader.vert",
            "./Shaders/shader.frag"
        );

        texture1 = new Texture2D("./Shaders/Assets/Texture.png");
        texture2 = new Texture2D("./Shaders/Assets/wall.jpg");

        shader.Use();
        shader.SetInt("texture1", 0);
        shader.SetInt("texture2", 1);

        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        // Create 10 cubes
        Vector3[] positions =
        {
        new Vector3( 0f,  0f,  0f),
        new Vector3( 2f,  0f,  0f),
        new Vector3(-2f,  0f,  0f),

        new Vector3( 0f,  2f,  0f),
        new Vector3( 0f, -2f,  0f),

        new Vector3( 0f,  0f, -3f),
        new Vector3( 2f,  1f, -5f),
        new Vector3(-2f,  1f, -5f),

        new Vector3( 4f,  0f, -6f),
        new Vector3(-4f,  0f, -6f)
    };

        foreach (Vector3 position in positions)
        {
            SceneObject cube = new SceneObject(mesh, shader);
            cube.Transform.Position = position;

            cubes.Add(cube);
        }
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
    }

    protected override void Render()
    {
        GL.Clear(
            ClearBufferMask.ColorBufferBit |
            ClearBufferMask.DepthBufferBit
        );

        texture1.Use(TextureUnit.Texture0);

        shader.Use();

        camera.Use(shader);
        camera.Update();

        foreach (SceneObject cube in cubes)
        {
            cube.Draw(camera);
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

