using Karl.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karl.Graphics.Extensions
{
    public static class SpriteBatchSpriteExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position)
        {
            if (sprite.Texture == null)
                return;

            spriteBatch.Draw(sprite.Texture, position, sprite.Source, sprite.Color, 0, sprite.Origin, 1, sprite.Effects, 0.0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Transform transformation)
        {
            if (sprite.Texture == null)
                return;

            spriteBatch.Draw(sprite.Texture, transformation.Position, sprite.Source, sprite.Color,
                transformation.Rotation, sprite.Origin, transformation.Scale, sprite.Effects, sprite.Depth);
        }

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Transform transformation, float depth)
        {
            if (sprite.Texture == null)
                return;

            spriteBatch.Draw(sprite.Texture, transformation.Position, sprite.Source, sprite.Color,
                transformation.Rotation, sprite.Origin, transformation.Scale, sprite.Effects, depth);
        }

        public static void Draw(this SpriteBatch spriteBatch, SpriteInstance instance)
        {
            if (instance == null)
                return;

            spriteBatch.Draw(instance.Sprite, instance.Transform);
        }
    }
}
