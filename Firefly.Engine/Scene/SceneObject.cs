using Firefly.Engine.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firefly.Engine.Scene
{
    public class SceneObject
    {
        public Transform Transform { get; } = new Transform();
        public Mesh Mesh;
        public Shader Shader;

        public SceneObject(Mesh mesh, Shader shader)
        {
            Mesh = mesh;
            Shader = shader;
        }

        public void Update(float deltaTime)
        {

        }

        public void Draw(Camera camera)
        {
            Shader.Use();

            camera.Use(Shader);
            camera.Update();

            Shader.SetMatrix4("model", Transform.GetMatrix());

            Mesh.Bind();

            GL.DrawArrays(
                PrimitiveType.Triangles,
                0,
                Mesh.VertexCount
            );
        }
    }
}
