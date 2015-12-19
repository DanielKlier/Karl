using Karl.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexTileGame.Entities
{
    class LandHexagon : Hexagon
    {
        public override bool Visible { get { return true; } }
        public override Color Color { get { return Color.Green; } }
        public SpriteInstance ShadowSprite { get; private set; }

        public LandHexagon(Layer layer) : base(layer) 
        {
        }

        public LandHexagon()
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var shadowSprite = new Sprite(Content.Load<Texture2D>("tex/hex-filled"))
            {
                Color = new Color(0, 0, 0, 0.4f),
                Depth = LayerDepth + 0.1f
            };
            ShadowSprite = new SpriteInstance(shadowSprite);
            var shadowTransform = Transform;
            shadowTransform.Position.X -= 4;
            shadowTransform.Position.Y += 4;
            ShadowSprite.Transform = shadowTransform;
            Layer.Sprites.Add(ShadowSprite);
        }
    }
}
