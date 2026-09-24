

using OpenTK.Mathematics;

namespace Firefly.Engine.Physics
{
    public class RigidBody
    {
        public float Mass = 1.0f;
        public bool useGravity = true;
        public Vector3 Velocity = Vector3.Zero;
        public float Gravity = -9.81f;

        public RigidBody()
        {

        }

        public void Update(float deltaTime)
        {
            if (useGravity)
            {
                Velocity.Y += Gravity * deltaTime;
            }
        }
    }
}
