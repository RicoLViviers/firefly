using Firefly.Engine.Scene;

namespace Firefly.Engine.Graphics.Primitives
{
    public static class Pyramid
    {
        private static readonly float[] Vertices =
        [
            // Front
            -0.5f, -0.5f,  0.5f,    0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,    1.0f, 0.0f,
             0.0f,  0.5f,  0.0f,    0.5f, 1.0f,

            // Right
             0.5f, -0.5f,  0.5f,    0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,    1.0f, 0.0f,
             0.0f,  0.5f,  0.0f,    0.5f, 1.0f,

            // Back
             0.5f, -0.5f, -0.5f,    0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,    1.0f, 0.0f,
             0.0f,  0.5f,  0.0f,    0.5f, 1.0f,

            // Left
            -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,    1.0f, 0.0f,
             0.0f,  0.5f,  0.0f,    0.5f, 1.0f,

            // Bottom
            -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,    1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,    1.0f, 1.0f,

             0.5f, -0.5f,  0.5f,    1.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,    0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,    0.0f, 0.0f
        ];

        public static SceneObject Create(Shader shader)
        {
            Mesh mesh = new(Vertices);
            return new SceneObject(mesh, shader);
        }
    }
}
