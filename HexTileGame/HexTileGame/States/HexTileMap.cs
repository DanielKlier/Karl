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
        private readonly float _offsetX;
        private readonly float _offsetY;

        public int NumTiles 
        {
            get { return _hexes.GetLength(0)*_hexes.GetLength(1); }
        }

        public int Width { get { return _hexes.GetLength(0); } }
        public int Height { get { return _hexes.GetLength(1); } }

        public HexTileMap(int columns, int rows, Layer hexTileLayer, World world, int hexRadius)
        {
            _hexTileLayer = hexTileLayer;
            _world = world;
            _hexRadius = hexRadius;
            _hexHeight = (int)(Math.Cos(Math.PI / 6) * _hexRadius);

            var fieldWidth = (columns - 0.25f * (columns - 1)) * _hexRadius * 2;
            _offsetX = (int)(-fieldWidth / 2);
            _offsetY = -rows * _hexHeight;

            _hexes = new Hexagon[columns, rows];
        }

        public void Create(Func<int, int, Hexagon[,]> creatorFunction)
        {
            var w = _hexes.GetLength(0);
            var h = _hexes.GetLength(1);
            var result = creatorFunction(w, h);

            for (var i = 0; i < h; ++i)
            {
                for (var j = 0; j < w; j++)
                {
                    var hex = result[j, i];
                    _hexes[j, i] = hex;
                    InitHex(hex, i, j);
                }
            }
        }

        public Hexagon this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                    return _hexes[x, y];
                else
                    return null;
            }
        }

        private void InitHex(Hexagon hex, int i, int j)
        {
            var x = 1.5f * _hexRadius * j + _offsetX;
            var y = 2 * _hexHeight * i + (j % 2) * _hexHeight + _offsetY;

            hex.Transform = new Transform()
            {
                Position = new Vector2(x, y),
                Rotation = 0,
                Scale = 1
            };

            hex.Layer = _hexTileLayer;
            _world.AddEntity(hex);
        }

        internal void AddHex(Hexagon hex, int x, int y)
        {
            if(_hexes[x, y] != null)
                _world.RemoveEntity(_hexes[x, y]);

            _hexes[x, y] = hex;
            InitHex(hex, y, x);
        }
    }
}