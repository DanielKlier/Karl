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
        private HexTileMap _hexTileMap;
        Layer _hexTileLayer;

        public IngameState()
        {
        }

        public override void Initialize(GameStateManager manager)
        {
            base.Initialize(manager);

            _world = new World();
            _camera = new Camera(Transform.Identity);
            _hexTileLayer = new Layer(1, 1);
            _hexTileMap = new HexTileMap(31, 16, _hexTileLayer, _world, 32);

            _world.Layers.Add(_hexTileLayer);
            
            _hexTileMap.Create();
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
