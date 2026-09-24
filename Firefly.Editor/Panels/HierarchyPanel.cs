using ImGuiNET;

namespace Firefly.Editor.Panels
{
    internal class HierarchyPanel
    {


        public void Render(float deltaTime)
        {
            ImGui.Begin("Firefly Editor");

            ImGuiTreeNodeFlags baseFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.SpanAvailWidth;

            if (ImGui.TreeNodeEx("World Scene", baseFlags))
            {
                if (ImGui.TreeNodeEx("Environment", baseFlags))
                {
                    ImGui.TreeNodeEx("Directional Light", baseFlags | ImGuiTreeNodeFlags.Leaf);
                    ImGui.TreePop();

                    ImGui.TreeNodeEx("Skybox", baseFlags | ImGuiTreeNodeFlags.Leaf);
                    ImGui.TreePop();

                    ImGui.TreePop();
                }

                if (ImGui.TreeNodeEx("Player Entity", baseFlags))
                {
                    if (ImGui.TreeNodeEx("Main Camera", baseFlags))
                    {
                        ImGui.TreeNodeEx("Camera Component", baseFlags | ImGuiTreeNodeFlags.Leaf);
                        ImGui.TreePop();

                        ImGui.TreePop();
                    }

                    ImGui.TreeNodeEx("Player Controller", baseFlags | ImGuiTreeNodeFlags.Leaf);
                    ImGui.TreePop();

                    ImGui.TreePop();
                }

                ImGui.TreeNodeEx("Terrain Mesh", baseFlags | ImGuiTreeNodeFlags.Leaf);
                ImGui.TreePop();

                ImGui.TreePop();
            }

            ImGui.End();

        }
    }
}
