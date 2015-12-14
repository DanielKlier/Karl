using Microsoft.Xna.Framework.Graphics;

namespace Karl.Graphics.Extensions
{
    public static class SpriteBatchTextExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, TextInstance instance)
        {
            if (instance == null || instance.Font == null || instance.Text == null)
                return;

            spriteBatch.DrawString(instance.Font, instance.Text, instance.Transform.Position, instance.Color,
                instance.Transform.Rotation, instance.Origin, instance.Transform.Scale, SpriteEffects.None, 0.0f);
        }
    }
}
