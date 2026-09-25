using Firefly.Engine.Graphics;
using Firefly.Engine.Graphics.Primitives;
using Firefly.Engine.Input;
using Firefly.Engine.Scene;
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

        private readonly List<SceneObject> objects = [];

        private readonly float moveSpeed = 5f;

        private SceneObject core = null!;
        private SceneObject coreRingA = null!;
        private SceneObject coreRingB = null!;
        DirectionLight light;

        private float worldTime = 0f;

        public Sandbox(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            camera = new Camera(Size.X, Size.Y);

            shader = new Shader(
                "./Shaders/shader.vert",
                "./Shaders/shader.frag"
            );

            CursorState = CursorState.Grabbed;
            WindowState = WindowState.Maximized;
            light = new DirectionLight();
        }

        protected override void Initialize()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.015f, 0.02f, 0.035f, 1.0f);

            Material groundMaterial = CreateMaterial(
                "Ground",
                new Vector3(0.08f, 0.09f, 0.11f),
                0.2f,
                0.85f
            );

            Material metalMaterial = CreateMaterial(
                "Dark Metal",
                new Vector3(0.15f, 0.17f, 0.20f),
                0.85f,
                0.25f
            );

            Material lightMetalMaterial = CreateMaterial(
                "Light Metal",
                new Vector3(0.32f, 0.35f, 0.40f),
                0.7f,
                0.3f
            );

            Material blueMaterial = CreateMaterial(
                "Energy Blue",
                new Vector3(0.05f, 0.35f, 1.0f),
                0.25f,
                0.15f
            );

            Material cyanMaterial = CreateMaterial(
                "Energy Cyan",
                new Vector3(0.05f, 0.9f, 1.0f),
                0.1f,
                0.1f
            );

            Material redMaterial = CreateMaterial(
                "Warning Red",
                new Vector3(0.9f, 0.04f, 0.03f),
                0.35f,
                0.3f
            );

            Material goldMaterial = CreateMaterial(
                "Gold",
                new Vector3(0.8f, 0.45f, 0.08f),
                0.9f,
                0.18f
            );

            CreateGround(groundMaterial);
            CreateCentralPlatform(metalMaterial, lightMetalMaterial);
            CreateEnergyCore(blueMaterial, cyanMaterial, goldMaterial);
            CreatePillars(metalMaterial, blueMaterial);
            CreateOuterWalls(metalMaterial, lightMetalMaterial);
            CreateCornerTowers(metalMaterial, redMaterial);
            CreateCrates(lightMetalMaterial, metalMaterial);
            CreateMonuments(goldMaterial, metalMaterial);

            camera.Position = new Vector3(0f, 4f, 14f);
            camera.Yaw = -90f;
            camera.Pitch = -12f;
            camera.Update();


            shader.SetVec3("lightDirection", light.Direction);
            shader.SetVec3("lightColor", light.Color);
            shader.SetFloat("lightIntensity", light.Intensity);

        }

        private Material CreateMaterial(
            string name,
            Vector3 color,
            float metallic,
            float roughness)
        {
            Material material = new Material(name, shader);

            material.Color = color;
            material.Metallic = metallic;
            material.Roughness = roughness;

            return material;
        }

        private void CreateGround(Material material)
        {
            SceneObject ground = Plane.Create("Arena Ground", material);

            ground.Transform.Position = new Vector3(0f, -1f, 0f);
            ground.Transform.Scale = new Vector3(30f, 1f, 30f);

            objects.Add(ground);
        }

        private void CreateCentralPlatform(
            Material metal,
            Material lightMetal)
        {
            SceneObject basePlatform = Cube.Create("Core Platform Base", metal);

            basePlatform.Transform.Position = new Vector3(0f, -0.65f, 0f);
            basePlatform.Transform.Scale = new Vector3(7f, 0.5f, 7f);

            objects.Add(basePlatform);

            SceneObject upperPlatform = Cube.Create("Core Platform", lightMetal);

            upperPlatform.Transform.Position = new Vector3(0f, -0.25f, 0f);
            upperPlatform.Transform.Scale = new Vector3(5.5f, 0.3f, 5.5f);

            objects.Add(upperPlatform);

            CreatePlatformStep(
                new Vector3(0f, -0.6f, 4.2f),
                new Vector3(3f, 0.25f, 1.4f),
                metal
            );

            CreatePlatformStep(
                new Vector3(0f, -0.85f, 5.4f),
                new Vector3(4f, 0.25f, 1.2f),
                metal
            );
        }

        private void CreatePlatformStep(
            Vector3 position,
            Vector3 scale,
            Material material)
        {
            SceneObject step = Cube.Create("Platform Step", material);

            step.Transform.Position = position;
            step.Transform.Scale = scale;

            objects.Add(step);
        }

        private void CreateEnergyCore(
            Material blue,
            Material cyan,
            Material gold)
        {
            SceneObject pedestal = Cube.Create("Core Pedestal", gold);

            pedestal.Transform.Position = new Vector3(0f, 0.3f, 0f);
            pedestal.Transform.Scale = new Vector3(1.6f, 0.6f, 1.6f);

            objects.Add(pedestal);

            SceneObject pedestalTop = Pyramid.Create("Core Mount", gold);

            pedestalTop.Transform.Position = new Vector3(0f, 1.15f, 0f);
            pedestalTop.Transform.Scale = new Vector3(1.4f, 1f, 1.4f);

            objects.Add(pedestalTop);

            core = Sphere.Create("Energy Core", cyan);

            core.Transform.Position = new Vector3(0f, 2.8f, 0f);
            core.Transform.Scale = new Vector3(0.9f);

            objects.Add(core);

            coreRingA = Cube.Create("Core Ring A", blue);

            coreRingA.Transform.Position = new Vector3(0f, 2.8f, 0f);
            coreRingA.Transform.Scale = new Vector3(2.2f, 0.08f, 0.08f);

            objects.Add(coreRingA);

            coreRingB = Cube.Create("Core Ring B", blue);

            coreRingB.Transform.Position = new Vector3(0f, 2.8f, 0f);
            coreRingB.Transform.Scale = new Vector3(0.08f, 0.08f, 2.2f);

            objects.Add(coreRingB);
        }

        private void CreatePillars(
            Material metal,
            Material energy)
        {
            Vector3[] positions =
            [
                new Vector3(5f, 1.5f, 5f),
                new Vector3(-5f, 1.5f, 5f),
                new Vector3(5f, 1.5f, -5f),
                new Vector3(-5f, 1.5f, -5f)
            ];

            foreach (Vector3 position in positions)
            {
                SceneObject pillar = Cube.Create("Energy Pillar", metal);

                pillar.Transform.Position = position;
                pillar.Transform.Scale = new Vector3(0.8f, 5f, 0.8f);

                objects.Add(pillar);

                SceneObject strip = Cube.Create("Energy Strip", energy);

                strip.Transform.Position = position + new Vector3(0f, 0.4f, 0.42f);
                strip.Transform.Scale = new Vector3(0.3f, 3.2f, 0.08f);

                objects.Add(strip);

                SceneObject cap = Pyramid.Create("Pillar Cap", metal);

                cap.Transform.Position = position + new Vector3(0f, 3f, 0f);
                cap.Transform.Scale = new Vector3(1.2f, 1.3f, 1.2f);

                objects.Add(cap);
            }
        }

        private void CreateOuterWalls(
            Material metal,
            Material trim)
        {
            for (int x = -12; x <= 12; x += 4)
            {
                CreateWallSection(
                    new Vector3(x, 0.5f, -12f),
                    new Vector3(3.6f, 3f, 0.5f),
                    metal,
                    trim
                );

                CreateWallSection(
                    new Vector3(x, 0.5f, 12f),
                    new Vector3(3.6f, 3f, 0.5f),
                    metal,
                    trim
                );
            }

            for (int z = -8; z <= 8; z += 4)
            {
                CreateWallSection(
                    new Vector3(-12f, 0.5f, z),
                    new Vector3(0.5f, 3f, 3.6f),
                    metal,
                    trim
                );

                CreateWallSection(
                    new Vector3(12f, 0.5f, z),
                    new Vector3(0.5f, 3f, 3.6f),
                    metal,
                    trim
                );
            }
        }

        private void CreateWallSection(
            Vector3 position,
            Vector3 scale,
            Material wallMaterial,
            Material trimMaterial)
        {
            SceneObject wall = Cube.Create("Arena Wall", wallMaterial);

            wall.Transform.Position = position;
            wall.Transform.Scale = scale;

            objects.Add(wall);

            SceneObject trim = Cube.Create("Wall Trim", trimMaterial);

            trim.Transform.Position = position + new Vector3(0f, 1.7f, 0f);

            if (scale.X > scale.Z)
                trim.Transform.Scale = new Vector3(scale.X, 0.15f, scale.Z + new Vector3(0f, 0f, 0.1f).Z);
            else
                trim.Transform.Scale = new Vector3(scale.X + 0.1f, 0.15f, scale.Z);

            objects.Add(trim);
        }

        private void CreateCornerTowers(
            Material metal,
            Material warning)
        {
            Vector3[] positions =
            [
                new Vector3(10f, 2f, 10f),
                new Vector3(-10f, 2f, 10f),
                new Vector3(10f, 2f, -10f),
                new Vector3(-10f, 2f, -10f)
            ];

            foreach (Vector3 position in positions)
            {
                SceneObject tower = Cube.Create("Defense Tower", metal);

                tower.Transform.Position = position;
                tower.Transform.Scale = new Vector3(2f, 6f, 2f);

                objects.Add(tower);

                SceneObject beacon = Sphere.Create("Warning Beacon", warning);

                beacon.Transform.Position = position + new Vector3(0f, 3.7f, 0f);
                beacon.Transform.Scale = new Vector3(0.45f);

                objects.Add(beacon);
            }
        }

        private void CreateCrates(
            Material lightMetal,
            Material darkMetal)
        {
            CreateCrate(
                new Vector3(7f, -0.2f, 2f),
                new Vector3(1.3f),
                lightMetal
            );

            CreateCrate(
                new Vector3(8.3f, -0.45f, 2.2f),
                new Vector3(0.8f),
                darkMetal
            );

            CreateCrate(
                new Vector3(-7f, -0.2f, -3f),
                new Vector3(1.3f),
                lightMetal
            );

            CreateCrate(
                new Vector3(-8f, -0.45f, -2.6f),
                new Vector3(0.8f),
                darkMetal
            );

            CreateCrate(
                new Vector3(-6.7f, 0.8f, -3f),
                new Vector3(0.8f),
                lightMetal
            );
        }

        private void CreateCrate(
            Vector3 position,
            Vector3 scale,
            Material material)
        {
            SceneObject crate = Cube.Create("Cargo Crate", material);

            crate.Transform.Position = position;
            crate.Transform.Scale = scale;

            objects.Add(crate);
        }

        private void CreateMonuments(
            Material gold,
            Material metal)
        {
            SceneObject monumentLeft = Pyramid.Create("Left Monument", gold);

            monumentLeft.Transform.Position = new Vector3(-7f, 1f, 6f);
            monumentLeft.Transform.Scale = new Vector3(2f, 4f, 2f);

            objects.Add(monumentLeft);

            SceneObject monumentRight = Pyramid.Create("Right Monument", gold);

            monumentRight.Transform.Position = new Vector3(7f, 1f, 6f);
            monumentRight.Transform.Scale = new Vector3(2f, 4f, 2f);

            objects.Add(monumentRight);

            SceneObject bridge = Cube.Create("Monument Bridge", metal);

            bridge.Transform.Position = new Vector3(0f, 3.3f, 6f);
            bridge.Transform.Scale = new Vector3(12f, 0.5f, 0.8f);

            objects.Add(bridge);
        }

        protected override void Update(float deltaTime)
        {
            input.Update(KeyboardState, MouseState);

            if (input.IsKeyPressed(Keys.Escape))
                Close();

            Vector2 mouseDelta = input.MouseDelta;

            camera.Yaw += mouseDelta.X * 0.04f;
            camera.Pitch -= mouseDelta.Y * 0.04f;

            camera.Pitch = MathHelper.Clamp(camera.Pitch, -89f, 89f);

            camera.Update();

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
                direction += Vector3.UnitY;

            if (input.IsKeyDown(Keys.LeftControl))
                direction -= Vector3.UnitY;

            if (direction != Vector3.Zero)
            {
                direction.Normalize();

                float speed = moveSpeed;

                if (input.IsKeyDown(Keys.LeftShift))
                    speed *= 3f;

                camera.Position += direction * speed * deltaTime;
            }

            worldTime += deltaTime;

            float hover = MathF.Sin(worldTime * 2f) * 0.2f;

            core.Transform.Position = new Vector3(
                0f,
                2.8f + hover,
                0f
            );

            coreRingA.Transform.Position = core.Transform.Position;
            coreRingB.Transform.Position = core.Transform.Position;

            coreRingA.Transform.Rotation = new Vector3(
                0f,
                worldTime * 45f,
                worldTime * 25f
            );

            coreRingB.Transform.Rotation = new Vector3(
                worldTime * -30f,
                worldTime * 55f,
                0f
            );

            float pulse = 0.85f + MathF.Sin(worldTime * 3f) * 0.1f;

            core.Transform.Scale = new Vector3(pulse);

            camera.Update();
        }

        protected override void Render(float deltaTime)
        {
            GL.Clear(
                ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit
            );

            shader.Use();

            camera.Use(shader);

            foreach (SceneObject obj in objects)
            {
                obj.Draw(camera);
            }

            SwapBuffers();
        }

        protected override void OnFramebufferResize(
            FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            camera.Resize(e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            shader.Dispose();

            base.OnUnload();
        }
    }
}