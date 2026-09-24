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


namespace Firefly.Editor
{
    public class Editor : Engine.Engine
    {
        Scene scene;
        Shader shader;
        EditorCamera _editorCamera;
        private readonly Texture2D texture1;
        private HierarchyPanel _hierarchyPanel;
        private ScenePanel _scenePanel;
        public Editor(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {

            _hierarchyPanel = new();
            _scenePanel = new();


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

            texture1 = new Texture2D("./Shaders/Assets/Texture.png");
        }



        protected override void Initialize()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            shader.Use();
            shader.SetInt("texture1", 0);
            shader.SetInt("texture2", 1);

            ImGui.CreateContext();


            var io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            ImguiImplOpenTK4.Init(this);
            ImguiImplOpenGL3.Init();



            //Temporarily
            SceneObject cube = Cube.Create(shader);
            scene.Add(cube);

            GL.Enable(EnableCap.DepthTest);
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

            texture1.Use(TextureUnit.Texture0);


            foreach (SceneObject obj in scene.Objects)
            {
                obj.Draw(_editorCamera.Camera);
            }
            _scenePanel.Framebuffer.Unbind();

            GL.Viewport(0, 0, Size.X, Size.Y);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );

            RenderDockspace();

            _hierarchyPanel.Render(deltaTime);
            _scenePanel.Render();


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
