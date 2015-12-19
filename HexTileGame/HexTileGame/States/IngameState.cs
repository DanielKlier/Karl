using System;
using System.Threading;
using HexTileGame.Entities;
using Karl.Core;
using Karl.Core.Extensions;
using Karl.Entities;
using Karl.Game;
using Karl.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Timer = Karl.Core.Timer;

namespace HexTileGame.States
{
    class IngameState : GameState
    {
        SpriteBatch _spriteBatch;
        World _world;
        Camera _camera;
        private HexTileMap _hexTileMap;
        Layer _hexTileLayer;

        private int _hexIterationCount = 0;
        private readonly Random _rand = new Random(1);
        private WalkIterationResult _previousHexIterationResult;
        private readonly Timer _hexIterationTimer = new Timer();
        private bool _hexMapDone = false;

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
            
            _previousHexIterationResult = new WalkIterationResult() {X = 15, Y = 8, Progress = 0};
            //_hexTileMap.Create(SimpleHexGenerator);
            //_hexTileMap.Create(RandomWalkHexGenerator);
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

            _hexIterationTimer.Update(gameTime.ElapsedGameTime);

            if (_hexIterationCount < _hexTileMap.NumTiles / 2 && _hexIterationTimer.TimeLeft <= TimeSpan.Zero || _previousHexIterationResult.Progress == 0)
            {
                InteractiveHexGenerationStep();
                _hexIterationCount += _previousHexIterationResult.Progress;
                _hexIterationTimer.Start(TimeSpan.FromMilliseconds(50));
            }
            else if (_hexIterationCount >= _hexTileMap.NumTiles && !_hexMapDone)
            {
                _hexMapDone = true;
                for (var y = 0; y < _hexTileMap.Height; y++)
                {
                    for (var x = 0; x < _hexTileMap.Width; x++)
                    {
                        if(_hexTileMap[x, y] == null)
                            _hexTileMap.AddHex(new WaterHexagon(), x, y);
                    }
                }
            }

            _world.Update(gameTime.ElapsedGameTime);
        }

        private void InteractiveHexGenerationStep()
        {
            var newResult = NextWalkIteration(_previousHexIterationResult.X, _previousHexIterationResult.Y, _rand);

            var x = _previousHexIterationResult.X;
            var y = _previousHexIterationResult.Y;
            if (_hexTileMap[x, y] == null && x >= 0 && x < _hexTileMap.Width && y >= 0 && y < _hexTileMap.Height -1)
            {
                var hex = new LandHexagon();
                _hexTileMap.AddHex(hex, x, y);
                newResult.Progress = 1;
            }
            else
            {
                newResult = _previousHexIterationResult;
                newResult.Progress = 0;
            }
            _previousHexIterationResult = newResult;
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

        private Hexagon[,] SimpleHexGenerator(int w, int h)
        {
            Random rand = new Random();
            var hexTypes = new int[w, h];

            for (var i = 0; i < h; ++i)
            {
                for (var j = 0; j < w; j++)
                {
                    var type = rand.Next(0, 2);
                    hexTypes[j, i] = type > 0 ? 1 : 0;
                }
            }

            return MakeHexes(hexTypes);
        }

        private Hexagon[,] RandomWalkHexGenerator(int w, int h)
        {
            var numIterations = w*h*100;
            var hexTypes = new int[w, h];
            
            var rand = new Random();

            // Fill everything with water
            for (var i = 0; i < h; ++i)
            {
                for (var j = 0; j < w; j++)
                {
                    hexTypes[j, i] = 0;
                }
            }

            // Do the random walk

            var x = (int)(w/2f);
            var y = (int)(h/2f);
            var minX = 0;
            var maxX = w - 1;
            var minY = 0;
            var maxY = h - 1;

            var result = NextWalkIteration(x, y, _rand);
            x = result.X;
            y = result.Y;

            hexTypes[y, x] = 1;

            return MakeHexes(hexTypes);
        }

        private WalkIterationResult NextWalkIteration(int x, int y, Random rand)
        {
            var directionChanges = new[] {-1, -1, -1, 0, 1, 1, 1};

            var dx = 0;
            var dy = 0;

            while(dx + dy != 1)
            { 
                dx = rand.Choice(directionChanges);
                dy = rand.Choice(directionChanges);
            }

            x += dx;
            y += dy;

            return new WalkIterationResult() {X = x, Y = y};
        }

        private struct WalkIterationResult
        {
            public int X;
            public int Y;
            public int Progress;
        }

        private Hexagon[,] MakeHexes(int[,] hexTypes)
        {
            var w = hexTypes.GetLength(0);
            var h = hexTypes.GetLength(1);
            var hexes = new Hexagon[w,h];

            for (var i = 0; i < h; ++i)
            {
                for (var j = 0; j < w; j++)
                {
                    var type = hexTypes[j, i];
                    Hexagon hex;

                    switch (type)
                    {
                        case 0:
                        default:
                            hex = new WaterHexagon();
                            break;
                        case 1:
                            hex = new LandHexagon();
                            break;
                    }

                    hexes[j, i] = hex;
                }
            }

            return hexes;
        }
    }
}
