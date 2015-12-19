using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karl.Graphics
{
    public class DrawingUtils
    {
        private Texture2D _pixel;

        public void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            var pixelTex = GetTexture(spriteBatch.GraphicsDevice);

            var line = end - start;

            var angle = (float)Math.Atan2(line.Y, line.X);

            spriteBatch.Draw(pixelTex,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)line.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            var topLeft = new Vector2(rectangle.Left, rectangle.Top);
            var bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
            var bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);
            var topRight = new Vector2(rectangle.Right, rectangle.Top);

            DrawLine(spriteBatch, topLeft, bottomLeft, color);
            DrawLine(spriteBatch, bottomLeft, bottomRight, color);
            DrawLine(spriteBatch, bottomRight, topRight, color);
            DrawLine(spriteBatch, topRight, topLeft, color);
        }

        private Texture2D GetTexture(GraphicsDevice graphicsDevice)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _pixel.SetData(new [] { Color.White } );
            }
            return _pixel;
        }
    }
}