using OpenTK.Mathematics;

namespace Firefly.Engine.Graphics
{
    public class Camera
    {
        public Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);
        private Vector3 cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 cameraUp = Vector3.UnitY;

        Matrix4 projection;
        private Matrix4 view;
        public int Width, Height;
        public Camera(int width, int height)
        {
            Width = width;
            Height = height;
            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(Position - cameraTarget);
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(cameraUp, cameraDirection));
            Vector3 realCameraUp = Vector3.Cross(cameraDirection, cameraRight);
            UpdateProjection();
            UpdateView();
        }

        private void UpdateProjection()
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Width / Height, 0.1f, 100.0f);
        }

        private void UpdateView()
        {
            view = Matrix4.LookAt(Position, Position + cameraFront, cameraUp);
        }

        public void Update()
        {
            UpdateView();
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