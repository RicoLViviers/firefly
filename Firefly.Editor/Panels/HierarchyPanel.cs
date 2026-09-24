using Firefly.Engine.Scene;
using ImGuiNET;

namespace Firefly.Editor.Panels
{
    internal class HierarchyPanel
    {
        public SceneObject SelectedObject;
        public void Render(float deltaTime, Scene scene)
        {
            ImGui.Begin("Hierarchy");

            ImGuiTreeNodeFlags baseFlags =
                ImGuiTreeNodeFlags.OpenOnArrow |
                ImGuiTreeNodeFlags.SpanAvailWidth;

            if (ImGui.TreeNodeEx("World Scene", baseFlags))
            {
                foreach (SceneObject obj in scene.Objects)
                {
                    ImGuiTreeNodeFlags flags = baseFlags;

                    if (SelectedObject == obj)
                    {
                        flags |= ImGuiTreeNodeFlags.Selected;
                    }

                    bool open = ImGui.TreeNodeEx(obj.Name, flags);

                    if (ImGui.IsItemClicked())
                    {
                        SelectedObject = obj;
                    }

                    if (open)
                    {
                        ImGui.TreePop();
                    }
                }

                ImGui.TreePop();
            }

            ImGui.End();
        }
    }
}