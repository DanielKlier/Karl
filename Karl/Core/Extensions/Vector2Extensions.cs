using System;
using Microsoft.Xna.Framework;

namespace Karl.Core.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Rotate(this Vector2 v, float angle)
        {
            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);
            return new Vector2(v.X * cos - v.Y * sin,
                               v.X * sin + v.Y * cos);
        }

        public static void Rotate(this Vector2 v, float angle, out Vector2 result)
        {
            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);
            result.X = v.X * cos - v.Y * sin;
            result.Y = v.X * sin + v.Y * cos;
        }
    }
}
