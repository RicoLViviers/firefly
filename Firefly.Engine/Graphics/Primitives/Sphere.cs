using Firefly.Engine.Scene;

namespace Firefly.Engine.Graphics.Primitives
{

    public static class Sphere
    {
        public static SceneObject Create(
            Shader shader,
            int latitudeSegments = 32,
            int longitudeSegments = 32)
        {
            List<float> vertices = [];

            for (int lat = 0; lat < latitudeSegments; lat++)
            {
                float v1 = (float)lat / latitudeSegments;
                float v2 = (float)(lat + 1) / latitudeSegments;

                float theta1 = v1 * MathF.PI;
                float theta2 = v2 * MathF.PI;

                for (int lon = 0; lon < longitudeSegments; lon++)
                {
                    float u1 = (float)lon / longitudeSegments;
                    float u2 = (float)(lon + 1) / longitudeSegments;

                    float phi1 = u1 * MathF.PI * 2f;
                    float phi2 = u2 * MathF.PI * 2f;

                    AddVertex(vertices, theta1, phi1, u1, v1);
                    AddVertex(vertices, theta2, phi1, u1, v2);
                    AddVertex(vertices, theta2, phi2, u2, v2);

                    AddVertex(vertices, theta2, phi2, u2, v2);
                    AddVertex(vertices, theta1, phi2, u2, v1);
                    AddVertex(vertices, theta1, phi1, u1, v1);
                }
            }

            Mesh mesh = new(vertices.ToArray());

            return new SceneObject(mesh, shader);
        }

        private static void AddVertex(
            List<float> vertices,
            float theta,
            float phi,
            float u,
            float v)
        {
            float x = MathF.Sin(theta) * MathF.Cos(phi);
            float y = MathF.Cos(theta);
            float z = MathF.Sin(theta) * MathF.Sin(phi);

            vertices.Add(x * 0.5f);
            vertices.Add(y * 0.5f);
            vertices.Add(z * 0.5f);

            vertices.Add(u);
            vertices.Add(v);
        }
    }
}
