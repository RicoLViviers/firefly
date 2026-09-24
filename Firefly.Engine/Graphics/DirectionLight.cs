using OpenTK.Mathematics;

namespace Firefly.Engine.Graphics
{
    public class DirectionLight
    {
        public Vector3 Direction = new Vector3(-0.2f, -1.0f, -0.3f);
        public Vector3 Color = Vector3.One;
        public float Intensity = 1.0f;
    }
}
