using System;
using HexTileGame.Entities;
using Karl.Core;
using Karl.Entities;
using Karl.Game;
using Karl.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexTileGame.States
{
    class IngameState : GameState
    {
        SpriteBatch _spriteBatch;
        World _world;
        Camera _camera;

        Layer _hexTileLayer;

        const int Width = 31;
        const int Height = 16;

        const int HexRadius = 32;
        readonly int _hexHeight;

        readonly Random _randomGenerator = new Random();

        Hexagon[,] _hexes = new Hexagon[Width, Height];

        public IngameState()
        {
            _hexHeight = (int)(Math.Cos(Math.PI / 6) * HexRadius);
        }

        public override void Initialize(GameStateManager manager)
        {
            base.Initialize(manager);

            _world = new World();
            _camera = new Camera(Transform.Identity);

            _hexTileLayer = new Layer(1, 1);

            _world.Layers.Add(_hexTileLayer);

            _hexes = CreateHexagons(_hexes);
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _world.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var currKeyboardState = Keyboard.GetState();

            UpdateCameraFromInput(currKeyboardState, dt);

            _world.Update(gameTime.ElapsedGameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _world.Draw(_spriteBatch, _camera);
        }

        private Hexagon[,] CreateHexagons(Hexagon[,] hexes)
        {
            var w = hexes.GetLength(0);
            var h = hexes.GetLength(1);
            var fieldWidth = (w - 0.25f * (w - 1)) * HexRadius * 2;
            var offsetX = (int)(-fieldWidth / 2);
            var offsetY = -h * _hexHeight;

            for (var i = 0; i < h; ++i)
            {
                for (var j = 0; j < w; j++)
                {
                    var hex = MakeHex(i, j, offsetX, offsetY);
                    hexes[j, i] = hex;
                    _world.AddEntity(hex);
                }
            }

            return hexes;
        }

        private Hexagon MakeHex(int i, int j, int offX, int offY)
        {
            var x = 1.5f * HexRadius * j + offX;
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

        private void UpdateCameraFromInput(KeyboardState currKeyboardState, float dt)
        {
            var deltaCamPosition = Vector2.Zero;
            float deltaCamRotation = 0;

            if (currKeyboardState.IsKeyDown(Keys.A))
                deltaCamPosition.X -= 1;
            if (currKeyboardState.IsKeyDown(Keys.D))
                deltaCamPosition.X += 1;
            if (currKeyboardState.IsKeyDown(Keys.W))
                deltaCamPosition.Y -= 1;
            if (currKeyboardState.IsKeyDown(Keys.S))
                deltaCamPosition.Y += 1;
            if (currKeyboardState.IsKeyDown(Keys.Q))
                deltaCamRotation -= 1;
            if (currKeyboardState.IsKeyDown(Keys.E))
                deltaCamRotation += 1;

            _camera.Position += dt * deltaCamPosition * 500;
            _camera.Rotation += dt * deltaCamRotation * MathHelper.Pi;
        }
    }
}
