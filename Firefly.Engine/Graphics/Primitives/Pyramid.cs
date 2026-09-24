using Firefly.Engine.Scene;

namespace Firefly.Engine.Graphics.Primitives
{
    public static class Pyramid
    {
        private static readonly float[] Vertices =
        [
            // Front
            -0.5f, -0.5f,  0.5f,   0.0f,  0.707f,  0.707f,
             0.5f, -0.5f,  0.5f,   0.0f,  0.707f,  0.707f,
             0.0f,  0.5f,  0.0f,   0.0f,  0.707f,  0.707f,

            // Right
             0.5f, -0.5f,  0.5f,   0.707f,  0.707f,  0.0f,
             0.5f, -0.5f, -0.5f,   0.707f,  0.707f,  0.0f,
             0.0f,  0.5f,  0.0f,   0.707f,  0.707f,  0.0f,

            // Back
             0.5f, -0.5f, -0.5f,   0.0f,  0.707f, -0.707f,
            -0.5f, -0.5f, -0.5f,   0.0f,  0.707f, -0.707f,
             0.0f,  0.5f,  0.0f,   0.0f,  0.707f, -0.707f,

            // Left
            -0.5f, -0.5f, -0.5f,  -0.707f,  0.707f,  0.0f,
            -0.5f, -0.5f,  0.5f,  -0.707f,  0.707f,  0.0f,
             0.0f,  0.5f,  0.0f,  -0.707f,  0.707f,  0.0f,

            // Bottom
            -0.5f, -0.5f, -0.5f,   0.0f, -1.0f, 0.0f,
             0.5f, -0.5f, -0.5f,   0.0f, -1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,   0.0f, -1.0f, 0.0f,

             0.5f, -0.5f,  0.5f,   0.0f, -1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,   0.0f, -1.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,   0.0f, -1.0f, 0.0f
        ];
        public static SceneObject Create(string name, Material material)
        {
            Mesh mesh = new(Vertices);
            return new SceneObject(name, mesh, material);
        }
    }
}
