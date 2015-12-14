namespace Karl.Collision
{
    public class Circle : Shape
    {
        public Circle()
        {
            Radius = 0;
        }

        public Circle(float radius)
        {
            Radius = radius;
        }

        public override bool Intersects(Shape other)
        {
            var circle = other as Circle;
            return circle != null && Intersects(circle);
        }

        public bool Intersects(Circle other)
        {
            var sqDist = (other.Transform.Position - Transform.Position).LengthSquared();
            var r1 = Transform.Scale * Radius;
            var r2 = other.Transform.Scale * other.Radius;
            var sqRadi = (r1 + r2) * (r1 + r2);
            return sqDist < sqRadi;
        }

        public float Radius { get; set; }
    }
}
