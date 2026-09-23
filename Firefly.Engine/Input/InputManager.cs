using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Numerics;

namespace Firefly.Engine.Input
{
    public class InputManager
    {
        private KeyboardState _keyboard = null;
        private MouseState _mouse = null;

        private readonly HashSet<Keys> _previousKeys = new();
        private readonly HashSet<Keys> _currentKeys = new();

        private Vector2 _previousMousePos;
        private Vector2 _mouseDelta;

        public Vector2 MouseDelta => _mouseDelta;
        public Vector2 MousePosition => new Vector2(_mouse.X, _mouse.Y);
        public Vector2 ScrollDelta => new Vector2(_mouse.Scroll.X, _mouse.Scroll.Y);

        public void Update(KeyboardState keyboard, MouseState mouse)
        {
            _keyboard = keyboard;
            _mouse = mouse;

            _previousKeys.Clear();
            foreach (var k in _currentKeys) _previousKeys.Add(k);
            _currentKeys.Clear();

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                // Avoid checking 'Unknown' key
                if (key != Keys.Unknown && keyboard.IsKeyDown(key))
                {
                    _currentKeys.Add(key);
                }
            }

            var current = new Vector2(mouse.X, mouse.Y);
            _mouseDelta = current - _previousMousePos;
            _previousMousePos = current;
        }

        public bool IsKeyDown(Keys key) => _currentKeys.Contains(key);
        public bool IsKeyPressed(Keys key) => _currentKeys.Contains(key) && !_previousKeys.Contains(key);
        public bool IsKeyReleased(Keys key) => !_currentKeys.Contains(key) && _previousKeys.Contains(key);

        public bool IsMouseButtonDown(MouseButton button) => _mouse.IsButtonDown(button);

        public bool IsMouseButtonPressed(MouseButton button) =>
            _mouse.IsButtonPressed(button);

        public bool IsMouseButtonReleased(MouseButton button) =>
            _mouse.IsButtonReleased(button);

    }
}
