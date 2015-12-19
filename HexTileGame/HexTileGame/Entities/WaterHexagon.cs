using Karl.Graphics;
using Microsoft.Xna.Framework;

namespace HexTileGame.Entities
{
    class WaterHexagon : Hexagon
    {
        public WaterHexagon(Layer layer) : base(layer) { }

        public WaterHexagon() : base()
        {
        }

        public override bool Visible { get { return true; } }
        public override Color Color { get { return Color.Blue; } }
        public override float LayerDepth { get { return 1f; } }
    }
}
