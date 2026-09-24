using ImGuiNET;
using Firefly.Engine.Graphics;
using System.Numerics;

namespace Firefly.Editor.Panels
{
    internal class ScenePanel
    {
        private Framebuffer _framebuffer;
        private Vector2 _lastSize = Vector2.Zero;

        public bool IsResized;
        public bool IsHovered;

        public ScenePanel()
        {
            _framebuffer = new Framebuffer(1280, 720);
        }

        public Framebuffer Framebuffer => _framebuffer;

        public void Render()
        {
            ImGui.Begin("Scene");

            Vector2 size = ImGui.GetContentRegionAvail();
            IsHovered = ImGui.IsWindowHovered();


            int width = (int)size.X;
            int height = (int)size.Y;

            IsResized =
                width != _framebuffer.Width ||
                height != _framebuffer.Height;

            if (IsResized && width > 0 && height > 0)
            {
                _framebuffer.Resize(width, height);
            }

            ImGui.Image(
                _framebuffer.ColorTexture,
                size,
                new Vector2(0, 1),
                new Vector2(1, 0)
            );

            ImGui.End();
        }
    }
}