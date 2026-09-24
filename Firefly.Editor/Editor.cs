using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Firefly.Editor.Backends;
using Firefly.Editor.Panels;
using Firefly.Engine.Graphics.Primitives;
using Firefly.Engine.Scene;
using Firefly.Engine.Graphics;
using Firefly.Editor.Camera;
using OpenTK.Mathematics;


namespace Firefly.Editor
{
    public class Editor : Engine.Engine
    {
        Scene scene;
        Shader shader;
        EditorCamera _editorCamera;
        private HierarchyPanel _hierarchyPanel;
        private ScenePanel _scenePanel;
        private AssetsPannel _assetsPannel;
        private InspectorPanel _inspectorPanel;
        DirectionLight light;
        public Editor(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {

            _hierarchyPanel = new();
            _scenePanel = new();
            _assetsPannel = new();
            _inspectorPanel = new();


            _editorCamera = new EditorCamera(
                _scenePanel.Framebuffer.Width,
                _scenePanel.Framebuffer.Height
            );
            WindowState = WindowState.Maximized;

            shader = new Shader(
                "./Shaders/shader.vert",
                "./Shaders/shader.frag"
            );

            scene = new();

            light = new DirectionLight();

        }


        protected override void Initialize()
        {






            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            shader.Use();

            ImGui.CreateContext();


            var io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            ImguiImplOpenTK4.Init(this);
            ImguiImplOpenGL3.Init();



            //Temporarily
            Material redMaterial = new Material("Red Material", shader);
            redMaterial.Color = new Vector3(1.0f, 0.1f, 0.1f);
            redMaterial.Metallic = 0.2f;
            redMaterial.Roughness = 0.7f;

            Material blueMaterial = new Material("Blue Material", shader);
            blueMaterial.Color = new Vector3(0.1f, 0.2f, 1.0f);
            blueMaterial.Metallic = 0.4f;
            blueMaterial.Roughness = 0.3f;

            Material goldMaterial = new Material("Gold Material", shader);
            goldMaterial.Color = new Vector3(1.0f, 0.55f, 0.08f);
            goldMaterial.Metallic = 0.9f;
            goldMaterial.Roughness = 0.2f;

            Material greenMaterial = new Material("Green Material", shader);
            greenMaterial.Color = new Vector3(0.1f, 0.8f, 0.25f);
            greenMaterial.Metallic = 0.0f;
            greenMaterial.Roughness = 0.8f;

            Material floorMaterial = new Material("Floor Material", shader);
            floorMaterial.Color = new Vector3(0.18f, 0.18f, 0.2f);
            floorMaterial.Metallic = 0.0f;
            floorMaterial.Roughness = 0.9f;


            SceneObject floor = Plane.Create("Floor", floorMaterial);
            floor.Transform.Position = new Vector3(0, -1, 0);
            floor.Transform.Scale = new Vector3(15, 1, 15);
            scene.Add(floor);


            SceneObject centerSphere = Sphere.Create("Golden Sphere", goldMaterial);
            centerSphere.Transform.Position = new Vector3(0, 0, 0);
            centerSphere.Transform.Scale = new Vector3(1.5f);
            scene.Add(centerSphere);


            SceneObject redCube = Cube.Create("Red Cube", redMaterial);
            redCube.Transform.Position = new Vector3(-3, 0, 0);
            redCube.Transform.Rotation = new Vector3(0, 25, 15);
            scene.Add(redCube);


            SceneObject blueCube = Cube.Create("Blue Cube", blueMaterial);
            blueCube.Transform.Position = new Vector3(3, 0, 0);
            blueCube.Transform.Rotation = new Vector3(15, -30, 0);
            scene.Add(blueCube);


            SceneObject pyramidLeft = Pyramid.Create("Left Pyramid", greenMaterial);
            pyramidLeft.Transform.Position = new Vector3(-5, 0, -3);
            pyramidLeft.Transform.Scale = new Vector3(1.5f, 2.5f, 1.5f);
            scene.Add(pyramidLeft);


            SceneObject pyramidRight = Pyramid.Create("Right Pyramid", redMaterial);
            pyramidRight.Transform.Position = new Vector3(5, 0, -3);
            pyramidRight.Transform.Scale = new Vector3(1.5f, 2.5f, 1.5f);
            scene.Add(pyramidRight);


            SceneObject sphereLeft = Sphere.Create("Blue Sphere", blueMaterial);
            sphereLeft.Transform.Position = new Vector3(-3, 1.5f, -4);
            sphereLeft.Transform.Scale = new Vector3(0.75f);
            scene.Add(sphereLeft);


            SceneObject sphereRight = Sphere.Create("Red Sphere", redMaterial);
            sphereRight.Transform.Position = new Vector3(3, 1.5f, -4);
            sphereRight.Transform.Scale = new Vector3(0.75f);
            scene.Add(sphereRight);


            SceneObject towerBase = Cube.Create("Tower Base", blueMaterial);
            towerBase.Transform.Position = new Vector3(0, 0, -6);
            towerBase.Transform.Scale = new Vector3(2.5f, 0.5f, 2.5f);
            scene.Add(towerBase);

            SceneObject tower = Cube.Create("Tower", goldMaterial);
            tower.Transform.Position = new Vector3(0, 2, -6);
            tower.Transform.Scale = new Vector3(1.0f, 3.0f, 1.0f);
            scene.Add(tower);

            SceneObject towerTop = Pyramid.Create("Tower Top", goldMaterial);
            towerTop.Transform.Position = new Vector3(0, 4.0f, -6);
            towerTop.Transform.Scale = new Vector3(1.5f);
            scene.Add(towerTop);


            GL.Enable(EnableCap.DepthTest);

            foreach (SceneObject obj in scene.Objects)
            {
                Console.WriteLine(obj.Name);
                Console.WriteLine(obj.Transform.Position);
            }


            shader.SetVec3("lightDirection", light.Direction);
            shader.SetVec3("lightColor", light.Color);
            shader.SetFloat("lightIntensity", light.Intensity);
        }



        protected override void Update(float deltaTime)
        {

            ImguiImplOpenTK4.NewFrame();
            ImguiImplOpenGL3.NewFrame();
            ImGui.NewFrame();



            //Temporary
            _editorCamera.Update(
                deltaTime,
                KeyboardState,
                MouseState,
                _scenePanel.IsHovered
            );


        }

        protected override void Render(float deltaTime)
        {

            if (_scenePanel.IsResized)
            {
                _editorCamera.Resize(
                   _scenePanel.Framebuffer.Width,
                   _scenePanel.Framebuffer.Height
               );
            }
            _scenePanel.Framebuffer.Bind();

            GL.ClearColor(0.15f, 0.15f, 0.15f, 1.0f);
            GL.Clear(
                ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit
            );

            foreach (SceneObject obj in scene.Objects)
            {
                obj.Draw(_editorCamera.Camera);
            }
            _scenePanel.Framebuffer.Unbind();

            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );

            RenderDockspace();


            _hierarchyPanel.Render(deltaTime, scene);
            _scenePanel.Render();
            _assetsPannel.Render();
            _inspectorPanel.Render(_hierarchyPanel);


            ImGui.Render();

            GL.Disable(EnableCap.DepthTest);
            ImguiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());
            GL.Enable(EnableCap.DepthTest);


            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }


        private void RenderDockspace()
        {
            ImGuiViewportPtr viewport = ImGui.GetMainViewport();

            ImGui.SetNextWindowPos(viewport.WorkPos);
            ImGui.SetNextWindowSize(viewport.WorkSize);
            ImGui.SetNextWindowViewport(viewport.ID);

            ImGuiWindowFlags windowFlags =
                ImGuiWindowFlags.NoDocking |
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoBringToFrontOnFocus |
                ImGuiWindowFlags.NoNavFocus;

            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);

            ImGui.Begin("DockSpace", windowFlags);

            ImGui.PopStyleVar(2);

            uint dockspaceId = ImGui.GetID("MainDockSpace");

            ImGui.DockSpace(
                dockspaceId,
                System.Numerics.Vector2.Zero,
                ImGuiDockNodeFlags.None
            );

            ImGui.End();
        }
    }
}
