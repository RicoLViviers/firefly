using Firefly.Engine.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firefly.Engine.Scene
{
    public class SceneObject
    {
        public string Name;
        public Transform Transform = new Transform();
        private Mesh Mesh;
        public Material Material;

        public SceneObject(string name, Mesh mesh, Material material)
        {
            Name = name;
            Mesh = mesh;
            Material = material;
        }

        public void Update(float deltaTime)
        {

        }

        public void Draw(Camera camera)
        {
            Material.Apply();

            camera.Use(Material.Shader);
            camera.Update();

            Material.Shader.SetMatrix4("model", Transform.GetMatrix());

            Mesh.Bind();

            GL.DrawArrays(
                PrimitiveType.Triangles,
                0,
                Mesh.VertexCount
            );
        }
    }
}
