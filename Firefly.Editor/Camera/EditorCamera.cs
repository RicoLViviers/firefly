using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Firefly.Editor.Camera
{
    internal class EditorCamera
    {
        public Engine.Graphics.Camera Camera { get; }

        public float MoveSpeed { get; set; } = 2.0f;
        public float LookSensitivity { get; set; } = 0.1f;
        public float FastMultiplier { get; set; } = 3.0f;

        private float _yaw = -90.0f;
        private float _pitch = 0.0f;

        public EditorCamera(int width, int height)
        {
            Camera = new Engine.Graphics.Camera(width, height);
        }

        public void Update(
            float deltaTime,
            KeyboardState keyboard,
            MouseState mouse,
            bool hovered)
        {
            if (!hovered)
                return;

            // Unity-style fly mode only while RMB is held
            if (!mouse.IsButtonDown(MouseButton.Right))
                return;

            UpdateRotation(mouse);
            UpdateMovement(deltaTime, keyboard);
        }

        private void UpdateRotation(MouseState mouse)
        {
            Vector2 delta = mouse.Delta;

            _yaw += delta.X * LookSensitivity;
            _pitch -= delta.Y * LookSensitivity;

            _pitch = MathHelper.Clamp(_pitch, -89.0f, 89.0f);

            Camera.Yaw = _yaw;
            Camera.Pitch = _pitch;
        }

        private void UpdateMovement(float deltaTime, KeyboardState keyboard)
        {
            float speed = MoveSpeed * deltaTime;

            if (keyboard.IsKeyDown(Keys.LeftShift))
                speed *= FastMultiplier;

            if (keyboard.IsKeyDown(Keys.W))
                Camera.Position += Camera.Front * speed;

            if (keyboard.IsKeyDown(Keys.S))
                Camera.Position -= Camera.Front * speed;

            if (keyboard.IsKeyDown(Keys.D))
                Camera.Position += Camera.Right * speed;

            if (keyboard.IsKeyDown(Keys.A))
                Camera.Position -= Camera.Right * speed;

            // Unity-style Q/E vertical movement
            if (keyboard.IsKeyDown(Keys.E))
                Camera.Position += Vector3.UnitY * speed;

            if (keyboard.IsKeyDown(Keys.Q))
                Camera.Position -= Vector3.UnitY * speed;
        }

        public void Resize(int width, int height)
        {
            Camera.Resize(width, height);
        }
    }
}