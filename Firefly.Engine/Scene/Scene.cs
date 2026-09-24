using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Firefly.Engine.Scene
{
    public class Scene
    {
        private List<SceneObject> _objects = new();
        public IReadOnlyList<SceneObject> Objects => _objects;

        public Scene()
        {

        }

        public void Add(SceneObject sceneObject)
        {
            _objects.Add(sceneObject);
        }

        public void Remove(SceneObject sceneObject)
        {
            _objects.Remove(sceneObject);
        }
    }
}
