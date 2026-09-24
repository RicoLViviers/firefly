using OpenTK.Mathematics;

namespace Firefly.Engine.Graphics
{
    public class Camera
    {
        public Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);
        public Vector3 Front = -Vector3.UnitZ;
        public Vector3 Right = Vector3.UnitX;
        public Vector3 Up = Vector3.UnitY;

        public float Yaw = -90.0f;
        public float Pitch = 0.0f;

        public float Fov = 45.0f;
        public float NearPlane = 0.1f;
        public float FarPlane = 1000.0f;

        public int Width;
        public int Height;

        public Matrix4 View;
        public Matrix4 Projection;

        public Camera(int width, int height)
        {
            Width = width;
            Height = height;

            Update();
            UpdateProjection();
        }

        public void Update()
        {
            float yaw = MathHelper.DegreesToRadians(Yaw);
            float pitch = MathHelper.DegreesToRadians(Pitch);

            Front = Vector3.Normalize(new Vector3(
                MathF.Cos(yaw) * MathF.Cos(pitch),
                MathF.Sin(pitch),
                MathF.Sin(yaw) * MathF.Cos(pitch)
            ));

            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));

            View = Matrix4.LookAt(Position, Position + Front, Up);
        }

        public void UpdateProjection()
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(Fov),
                (float)Width / Height,
                NearPlane,
                FarPlane
            );
        }

        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0)
                return;

            Width = width;
            Height = height;

            UpdateProjection();
        }

        public void Use(Shader shader)
        {
            shader.SetMatrix4("view", View);
            shader.SetMatrix4("projection", Projection);
        }
    }
}