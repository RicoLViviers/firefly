using ImGuiNET;
using OpenTK.Mathematics;
using System;
using System.Reflection;

namespace Firefly.Editor.Panels
{
    internal class InspectorPanel
    {
        public void Render(HierarchyPanel hierarchyPanel)
        {
            ImGui.Begin("Inspector");

            object selectedObject = hierarchyPanel.SelectedObject;

            if (selectedObject == null)
            {
                ImGui.TextDisabled("No object selected.");
                ImGui.End();
                return;
            }

            Type type = selectedObject.GetType();

            ImGui.Text(type.Name);
            ImGui.Separator();

            DrawObject(selectedObject, type);

            ImGui.End();
        }

        private void DrawObject(object target, Type type)
        {
            FieldInfo[] fields = type.GetFields(
                BindingFlags.Public |
                BindingFlags.Instance
            );

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(target);

                DrawField(target, field, value);
            }
        }

        private void DrawField(object target, FieldInfo field, object value)
        {
            Type fieldType = field.FieldType;
            string name = field.Name;

            ImGui.PushID(name);

            if (fieldType == typeof(float))
            {
                float v = (float)value;

                if (ImGui.DragFloat(name, ref v, 0.1f))
                    field.SetValue(target, v);
            }
            else if (fieldType == typeof(int))
            {
                int v = (int)value;

                if (ImGui.DragInt(name, ref v))
                    field.SetValue(target, v);
            }
            else if (fieldType == typeof(bool))
            {
                bool v = (bool)value;

                if (ImGui.Checkbox(name, ref v))
                    field.SetValue(target, v);
            }
            else if (fieldType == typeof(string))
            {
                string v = value as string ?? "";

                if (ImGui.InputText(name, ref v, 256))
                    field.SetValue(target, v);
            }
            else if (fieldType == typeof(Vector2))
            {
                Vector2 vector = (Vector2)value;

                System.Numerics.Vector2 v = new System.Numerics.Vector2(
                    vector.X,
                    vector.Y
                );

                if (ImGui.DragFloat2(name, ref v, 0.1f))
                {
                    field.SetValue(
                        target,
                        new Vector2(v.X, v.Y)
                    );
                }
            }
            else if (fieldType == typeof(Vector3))
            {
                Vector3 vector = (Vector3)value;

                System.Numerics.Vector3 v = new System.Numerics.Vector3(
                    vector.X,
                    vector.Y,
                    vector.Z
                );

                if (ImGui.DragFloat3(name, ref v, 0.1f))
                {
                    field.SetValue(
                        target,
                        new Vector3(v.X, v.Y, v.Z)
                    );
                }
            }
            else if (fieldType == typeof(Vector4))
            {
                Vector4 vector = (Vector4)value;

                System.Numerics.Vector4 v = new System.Numerics.Vector4(
                    vector.X,
                    vector.Y,
                    vector.Z,
                    vector.W
                );

                if (ImGui.DragFloat4(name, ref v, 0.1f))
                {
                    field.SetValue(
                        target,
                        new Vector4(v.X, v.Y, v.Z, v.W)
                    );
                }
            }
            else if (fieldType.IsEnum)
            {
                Array values = Enum.GetValues(fieldType);
                string[] names = Enum.GetNames(fieldType);

                int currentIndex = Array.IndexOf(values, value);

                if (currentIndex < 0)
                    currentIndex = 0;

                if (ImGui.Combo(name, ref currentIndex, names, names.Length))
                {
                    field.SetValue(
                        target,
                        values.GetValue(currentIndex)
                    );
                }
            }
            else if (IsInspectableObject(fieldType))
            {
                if (value == null)
                {
                    ImGui.TextDisabled($"{name}: null");
                }
                else
                {
                    ImGuiTreeNodeFlags flags =
                        ImGuiTreeNodeFlags.DefaultOpen |
                        ImGuiTreeNodeFlags.SpanAvailWidth;

                    if (ImGui.TreeNodeEx(name, flags))
                    {
                        DrawObject(value, fieldType);
                        ImGui.TreePop();
                    }
                }
            }
            else
            {
                ImGui.Text($"{name}: {value}");
            }

            ImGui.PopID();
        }

        private bool IsInspectableObject(Type type)
        {
            if (type.IsPrimitive)
                return false;

            if (type == typeof(string))
                return false;

            if (type.IsEnum)
                return false;

            if (type == typeof(Vector2))
                return false;

            if (type == typeof(Vector3))
                return false;

            if (type == typeof(Vector4))
                return false;

            if (type.Namespace != null &&
                type.Namespace.StartsWith("Firefly"))
                return true;

            return false;
        }
    }
}