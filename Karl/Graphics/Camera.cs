using Karl.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karl.Graphics
{
    public struct Camera
    {
        private Transform _transform;

        public Camera(Transform transform)
        {
            _transform = transform;
        }
                
        public Camera(Camera camera)
        {
            _transform = camera._transform;
        }

        public Matrix GetViewMatrix(Viewport viewport)
        {
            return GetViewMatrix(viewport.Width, viewport.Height);
        }

        public Matrix GetViewMatrix(int viewportWidth, int viewportHeight)
        {
            var centerOffset = new Transform(0.5f * viewportWidth, 0.5f * viewportHeight);
            return (Transform.Invert(_transform) * centerOffset).ToMatrix();
        }

        public Transform Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public Vector2 Position
        {
            get { return _transform.Position; }
            set { _transform.Position = value; }
        }

        public float Rotation
        {
            get { return _transform.Rotation; }
            set { _transform.Rotation = value; }
        }

        public float Scale
        {
            get { return _transform.Scale; }
            set { _transform.Scale = value; }
        }
    }
}
