using Karl.Core;
using Microsoft.Xna.Framework;

namespace Karl.Collision
{
    public abstract class Shape : IBoundingBox
    {
        private Rectangle _cachedBoundingBox;
        private bool _invalidBoundingBox = true;
        private Transform _transform;

        protected Shape()
        {
            _transform = Transform.Identity;
        }

        public abstract bool Intersects(Shape other);

        internal void OnCollision(Space space, Shape other)
        {
            if (Collision != null)
                Collision(space, other);
        }

        internal void OnSeparation(Space space, Shape other)
        {
            if (Separation != null)
                Separation(space, other);
        }

        public uint Group { get; set; }

        public uint Mask { get; set; }

        public object Tag { get; set; }

        public delegate void CollisionHandler(Space space, Shape other);
        public event CollisionHandler Collision;
        public event CollisionHandler Separation;

        protected abstract Rectangle CalculateBoundingBox();

        public Rectangle BoundingBox
        {
            get
            {
                if (_invalidBoundingBox)
                {
                    _invalidBoundingBox = false;
                    _cachedBoundingBox = CalculateBoundingBox();
                }
                return _cachedBoundingBox;
            }
        }

        public Transform Transform
        {
            get { return _transform; }
            set 
            {
                _invalidBoundingBox = true;
                _transform = value;
                
            }
        }
    }
}
