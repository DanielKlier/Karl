using Karl.Core;

namespace Karl.Collision
{
    public abstract class Shape
    {
        protected Shape()
        {
            Transform = Transform.Identity;
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

        public Transform Transform { get; set; }

        public delegate void CollisionHandler(Space space, Shape other);
        public event CollisionHandler Collision;
        public event CollisionHandler Separation;
    }
}
