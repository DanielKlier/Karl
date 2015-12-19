using System;
using Karl.Entities;
using Karl.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexTileGame.Entities
{
    abstract class Hexagon : LayerEntity
    {
        public abstract bool Visible { get; }
        public abstract Color Color { get; }
        public virtual float LayerDepth { get { return 0.5f; } }
        public virtual Color BorderColor { get { return Color.Black; } }

        public SpriteInstance BorderSprite { get; private set; }

        protected Hexagon() { }
        protected Hexagon(Layer layer) : base(layer) { }

        protected override void LoadContent()
        {
            var sprite = new Sprite(Content.Load<Texture2D>("tex/hex-filled"))
            {
                Color = Color,
                Depth = LayerDepth
            };
            Sprite = sprite;

            BorderSprite = new SpriteInstance(new Sprite(Content.Load<Texture2D>("tex/hex-border")))
            {
                Sprite =
                {
                    Color = BorderColor,
                    Depth = LayerDepth - 0.1f
                }
            };

            Layer.Sprites.Add(BorderSprite);
        }

        protected override void Update(TimeSpan elapsedTime)
        {
            BorderSprite.Transform = Transform;
            base.Update(elapsedTime);
        }
    }
}
