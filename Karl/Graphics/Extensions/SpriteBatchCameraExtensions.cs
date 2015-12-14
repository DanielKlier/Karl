using Microsoft.Xna.Framework.Graphics;

namespace Karl.Graphics.Extensions
{
    public static class SpriteBatchCameraExtensions
    {
        public static void Begin(this SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, camera.GetViewMatrix(spriteBatch.GraphicsDevice.Viewport));
        }
    }
}
