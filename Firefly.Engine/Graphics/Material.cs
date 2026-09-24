using OpenTK.Mathematics;



namespace Firefly.Engine.Graphics
{
    public class Material
    {
        public string Name;
        public Shader Shader;
        public Vector3 Color = Vector3.One;

        public float Metallic = 0.0f;
        public float Roughness = 0.5f;

        public Material(string name, Shader shader)
        {
            Name = name;
            Shader = shader;
        }

        public void Apply()
        {
            Shader.Use();

            Shader.SetVec3("materialColor", Color);
        }
    }
}
