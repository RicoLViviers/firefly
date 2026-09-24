using Firefly.Engine.Scene;

namespace Firefly.Engine.Graphics.Primitives
{
    public static class Plane
    {
        private static readonly float[] Vertices =
        [
            -0.5f, 0.0f, -0.5f,   0.0f, 1.0f, 0.0f,
             0.5f, 0.0f, -0.5f,   0.0f, 1.0f, 0.0f,
             0.5f, 0.0f,  0.5f,   0.0f, 1.0f, 0.0f,

             0.5f, 0.0f,  0.5f,   0.0f, 1.0f, 0.0f,
            -0.5f, 0.0f,  0.5f,   0.0f, 1.0f, 0.0f,
            -0.5f, 0.0f, -0.5f,   0.0f, 1.0f, 0.0f
        ];

        public static SceneObject Create(string name, Material material)
        {
            Mesh mesh = new(Vertices);
            return new SceneObject(name, mesh, material);
        }
    }
}

