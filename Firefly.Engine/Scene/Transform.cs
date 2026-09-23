using OpenTK.Mathematics;

namespace Firefly.Engine.Scene
{
    public class Transform
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public Matrix4 GetMatrix()
        {
            return
                Matrix4.CreateScale(Scale) *
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) *
                Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z)) *
                Matrix4.CreateTranslation(Position);
        }
    }
}
