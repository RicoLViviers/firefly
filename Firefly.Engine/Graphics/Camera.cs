using OpenTK.Mathematics;

namespace Firefly.Engine.Graphics
{
    public class Camera
    {
        public Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);
        public Vector3 Front = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 Up = Vector3.UnitY;
        public Vector3 Right;

        Matrix4 projection;
        private Matrix4 view;
        public int Width, Height;
        public Camera(int width, int height)
        {
            Width = width;
            Height = height;
            Vector3 Target = Vector3.Zero;
            Vector3 Direction = Vector3.Normalize(Position - Target);
            Right = Vector3.Normalize(Vector3.Cross(Front, Direction));
            Vector3 realCameraUp = Vector3.Cross(Direction, Right);
            UpdateProjection();
            UpdateView();
        }

        private void UpdateProjection()
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Width / Height, 0.1f, 100.0f);
        }

        private void UpdateView()
        {
            view = Matrix4.LookAt(Position, Position + Front, Up);
        }

        public void Update()
        {
            UpdateView();
            Right = Vector3.Normalize(Vector3.Cross(Front, Up));
        }
        public void Use(Shader shader)
        {
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
            UpdateProjection();
        }
    }
}