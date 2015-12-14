using Karl.Core.Extensions;
using Microsoft.Xna.Framework;

namespace Karl.Core
{
    public struct Transform
    {
        public static readonly Transform Identity = new Transform(Vector2.Zero, 0.0f, 1.0f);
        public static readonly Transform Zero = new Transform(Vector2.Zero, 0.0f, 0.0f);

        public Vector2 Position;
        public float Rotation;
        public float Scale;

        public Transform(Vector2 translation)
        {
            Position = translation;
            Rotation = 0;
            Scale = 1;
        }

        public Transform(Vector2 translation, float rotation)
        {
            Position = translation;
            Rotation = rotation;
            Scale = 1;
        }

        public Transform(Vector2 translation, float rotation, float scale)
        {
            Position = translation;
            Rotation = rotation;
            Scale = scale;
        }

        public Transform(float rotation)
        {
            Position = Vector2.Zero;
            Rotation = rotation;
            Scale = 1;
        }

        public Transform(float x, float y)
        {
            Position = new Vector2(x, y);
            Rotation = 0;
            Scale = 1;
        }

        public Transform(float x, float y, float rotation)
        {
            Position = new Vector2(x, y);
            Rotation = rotation;
            Scale = 1;
        }

        public static Transform Invert(Transform transform)
        {
            return new Transform((-transform.Position / transform.Scale).Rotate(-transform.Rotation),
                -transform.Rotation, 1 / transform.Scale);
        }

        public Matrix ToMatrix()
        {
            return 
                Matrix.CreateScale(Scale) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(Position.X, Position.Y, 0f);
        }

        public static Transform operator +(Transform lhs, Transform rhs)
        {
            return new Transform(
                lhs.Position + rhs.Position,
                lhs.Rotation + rhs.Rotation,
                lhs.Scale + rhs.Scale);
        }

        public static Transform operator -(Transform lhs, Transform rhs)
        {
            return new Transform(
                lhs.Position - rhs.Position,
                lhs.Rotation - rhs.Rotation,
                lhs.Scale - rhs.Scale);
        }

        public static Transform operator *(Transform transform, float scalar)
        {
            return new Transform(
                scalar * transform.Position,
                scalar * transform.Rotation,
                scalar * transform.Scale);
        }

        public static Transform operator *(float scalar, Transform transform)
        {
            return new Transform(
                scalar * transform.Position,
                scalar * transform.Rotation,
                scalar * transform.Scale);
        }

        public static Vector2 operator *(Vector2 v, Transform transform)
        {
            return (transform.Scale * v).Rotate(transform.Rotation) + transform.Position;
        }

        public static Transform operator *(Transform lhs, Transform rhs)
        {
            return new Transform(
                lhs.Position.Rotate(rhs.Rotation) * rhs.Scale + rhs.Position,
                lhs.Rotation + rhs.Rotation,
                lhs.Scale * rhs.Scale);
        }

        public static Transform operator /(Transform lhs, Transform rhs)
        {
            return new Transform(
                ((lhs.Position - rhs.Position) / rhs.Scale).Rotate(-rhs.Rotation),
                lhs.Rotation - rhs.Rotation,
                lhs.Scale / rhs.Scale);
        }
    }
}
