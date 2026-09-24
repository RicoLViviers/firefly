using Firefly.Engine.Scene;

namespace Firefly.Engine.Graphics.Primitives
{
    public static class Plane
    {
        private static readonly float[] Vertices =
        [
            // Position              // UV
            -0.5f, 0.0f, -0.5f,     0.0f, 0.0f,
             0.5f, 0.0f, -0.5f,     1.0f, 0.0f,
             0.5f, 0.0f,  0.5f,     1.0f, 1.0f,

             0.5f, 0.0f,  0.5f,     1.0f, 1.0f,
            -0.5f, 0.0f,  0.5f,     0.0f, 1.0f,
            -0.5f, 0.0f, -0.5f,     0.0f, 0.0f
        ];

        public static SceneObject Create(Shader shader)
        {
            Mesh mesh = new(Vertices);
            return new SceneObject(mesh, shader);
        }
    }
}

