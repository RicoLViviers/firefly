using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Firefly.Engine
{
    public abstract class Engine : GameWindow
    {
        protected Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) 
            : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected abstract void Initialize();
        protected abstract void Update(float deltaTime);
        protected abstract void Render();

        protected override void OnLoad()
        {
            base.OnLoad();
            Initialize();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            Update(((float)args.Time));
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            Render();
        }
    }
}
