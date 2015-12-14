using System.Collections.Generic;
using Karl.Graphics.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace Karl.Graphics
{
    public class Layer
    {
        private readonly List<SpriteInstance> _sprites = new List<SpriteInstance>();
        private readonly List<TextInstance> _texts = new List<TextInstance>();

        public Layer()
        {
            Speed = 1;
            Scale = 1;
        }

        public Layer(float speed, float scale)
        {
            Speed = speed;
            Scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            camera.Position *= Speed;
            camera.Scale /= Scale;

            spriteBatch.Begin(camera);
            foreach (var sprite in _sprites)
            {
                spriteBatch.Draw(sprite);   
            }

            foreach (var text in _texts)
            {
                spriteBatch.Draw(text);
            }
            spriteBatch.End();
        }

        public IList<SpriteInstance> Sprites
        {
            get { return _sprites; }
        }

        public IList<TextInstance> Texts
        {
            get { return _texts; }
        }

        public float Speed { get; set; }

        public float Scale { get; set; }
    }
}
