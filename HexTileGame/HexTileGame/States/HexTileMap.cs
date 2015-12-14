using System;
using HexTileGame.Entities;
using Karl.Core;
using Karl.Entities;
using Karl.Graphics;
using Microsoft.Xna.Framework;

namespace HexTileGame.States
{
    internal class HexTileMap
    {
        private readonly Hexagon[,] _hexes;
        private readonly Layer _hexTileLayer;
        private readonly World _world;
        private readonly int _hexRadius;
        private readonly int _hexHeight;
        readonly Random _randomGenerator = new Random();

        public HexTileMap(int columns, int rows, Layer hexTileLayer, World world, int hexRadius)
        {
            _hexTileLayer = hexTileLayer;
            _world = world;
            _hexRadius = hexRadius;
            _hexHeight = (int)(Math.Cos(Math.PI / 6) * _hexRadius);

            _hexes = new Hexagon[columns, rows];
        }

        public void Create()
        {
            CreateHexagons();
        }

        private void CreateHexagons()
        {
            var w = _hexes.GetLength(0);
            var h = _hexes.GetLength(1);
            var fieldWidth = (w - 0.25f * (w - 1)) * _hexRadius * 2;
            var offsetX = (int)(-fieldWidth / 2);
            var offsetY = -h * _hexHeight;

            for (var i = 0; i < h; ++i)
            {
                for (var j = 0; j < w; j++)
                {
                    var hex = MakeHex(i, j, offsetX, offsetY);
                    _hexes[j, i] = hex;
                    _world.AddEntity(hex);
                }
            }
        }

        private Hexagon MakeHex(int i, int j, int offX, int offY)
        {
            var x = 1.5f * _hexRadius * j + offX;
            float y = 2 * _hexHeight * i + (j % 2) * _hexHeight + offY;

            var type = _randomGenerator.Next(3);
            Hexagon hex;
            if (type > 1)
                hex = new LandHexagon(_hexTileLayer);
            else
                hex = new WaterHexagon(_hexTileLayer);

            hex.Transform = new Transform()
            {
                Position = new Vector2(x, y),
                Rotation = 0,
                Scale = 1
            };

            return hex;
        }
    }
}