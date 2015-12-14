using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karl.Graphics
{
    public struct Sprite
    {
        public Texture2D Texture;
        public Rectangle Source;
        public Vector2 Origin;
        public Color Color;
        public float Depth;
        public SpriteEffects Effects;

        public Sprite(Texture2D texture) :
            this(texture, Vector2.Zero, Color.White, SpriteEffects.None)
        {
        }

        public Sprite(Texture2D texture, Vector2 origin) :
            this(texture, origin, Color.White, SpriteEffects.None)
        {
        }

        public Sprite(Texture2D texture, Vector2 origin, Color color, SpriteEffects effects) :
            this(texture, origin, color, effects, 0.0f)
        {
        }

        public Sprite(Texture2D texture, Vector2 origin, Color color, SpriteEffects effects, float depth)
        {
            Texture = texture;
            Origin = origin;
            Color = color;
            Effects = effects;
            Depth = depth;

            Source = texture != null ? texture.Bounds : Rectangle.Empty;
        }

        public Sprite(Texture2D texture, Rectangle source) :
            this(texture, source, Vector2.Zero, Color.White, SpriteEffects.None)
        {
        }

        public Sprite(Texture2D texture, Rectangle source, Vector2 origin) :
            this(texture, source, origin, Color.White, SpriteEffects.None)
        {
        }

        public Sprite(Texture2D texture, Rectangle source, Vector2 origin, Color color, SpriteEffects effects)
            : this(texture, source, origin, color, effects, 0.0f)
        {
        }

        public Sprite(Texture2D texture, Rectangle source, Vector2 origin, Color color, SpriteEffects effects, float depth)
        {
            Texture = texture;
            Source = source;
            Origin = origin;
            Color = color;
            Effects = effects;
            Depth = depth;
        }

        public Vector2 Center
        {
            get { return new Vector2(0.5f * Width, 0.5f * Height); }
        }

        public float Width
        {
            get { return Source.Width; }
        }

        public float Height
        {
            get { return Source.Height; }
        }
    }
}
