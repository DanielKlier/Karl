using System;
using System.Net.Configuration;
using Karl.Core;
using Karl.Entities;
using Karl.Game;
using Karl.Graphics;
using Karl.Graphics.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuadtreeDemo.Entities;
using QuadtreeDemo.Quadtree;

namespace QuadtreeDemo.States
{
    public class DemoGameState : GameState
    {
        struct UiFlags
        {
            public bool ShowQuadtreeNodes;
            public bool ShowEntityNodeAssociations;
            public bool ShowEntityBoundingBoxes;
            public bool ShowEntityIndex;
        }

        private const int NumWuschels = 40000;
        private int _wuschelCount;
        private readonly Timer _wuschelCreationTimer = new Timer();
        private Layer _wuschelLayer;
        private World _world;
        private SpriteBatch _spriteBatch;
        private Camera _camera;
        private bool _enableAutoSpawn = true;
        private readonly TimeSpan _wuschelSpawnInterval = TimeSpan.FromMilliseconds(20);
        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;
        private Quadtree<BaseEntity> _quadtree;
        private DrawingUtils _drawingUtils;
        private readonly Random _random = new Random(1);
        private readonly Rectangle _creationBounds = new Rectangle(-1000, -1000, 2000, 2000);
        private SpriteFont _uiFont;

        private UiFlags _uiFlags = new UiFlags()
        {
            ShowEntityBoundingBoxes = false,
            ShowEntityIndex = false,
            ShowEntityNodeAssociations = false,
            ShowQuadtreeNodes = true
        };

        public override void Initialize(GameStateManager manager)
        {
            base.Initialize(manager);

            _drawingUtils = new DrawingUtils();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new Camera(Transform.Identity);

            _world = new World();
            _wuschelLayer = new Layer(1.0f, 1.0f);
            _world.Layers.Add(_wuschelLayer);
            _quadtree = new Quadtree<BaseEntity>();
        }

        public override void LoadContent()
        {
            _uiFont = Content.Load<SpriteFont>("Arial20");
            _world.LoadContent(Content);
        }

        private void CreateWuschel()
        {
            _wuschelCount += 1;
            _wuschelCreationTimer.Start(_wuschelSpawnInterval);

            var wuschel = new Wuschel(_wuschelLayer) {Index = _wuschelCount};

            var pos = new Vector2(
                _random.Next(_creationBounds.Left, _creationBounds.Right),
                _random.Next(_creationBounds.Top, _creationBounds.Bottom)
                );
            wuschel.Transform = new Transform(pos);

            _world.AddEntity(wuschel);
            _quadtree.AddObject(wuschel);
        }

        public override void Update(GameTime gameTime)
        {
            var elapsedTime = gameTime.ElapsedGameTime;
            
            UpdateInput(elapsedTime);
            UpdateTimers(elapsedTime);

            if (_wuschelCount < NumWuschels && _wuschelCreationTimer.TimeLeft <= TimeSpan.Zero && _enableAutoSpawn)
            {
                CreateWuschel();
            }

            _world.Update(elapsedTime);
        }

        private void UpdateTimers(TimeSpan elapsedTime)
        {
            _wuschelCreationTimer.Update(elapsedTime);
        }

        private void UpdateInput(TimeSpan elapsedTime)
        {
            _currentKeyboardState = Keyboard.GetState();

            if (KeyPressed(Keys.F1))
            {
                CreateWuschel();
            }

            if (KeyPressed(Keys.F2))
                _uiFlags.ShowEntityBoundingBoxes = !_uiFlags.ShowEntityBoundingBoxes;
            if (KeyPressed(Keys.F3))
                _uiFlags.ShowEntityIndex = !_uiFlags.ShowEntityIndex;
            if (KeyPressed(Keys.F4))
                _uiFlags.ShowEntityNodeAssociations = !_uiFlags.ShowEntityNodeAssociations;
            if (KeyPressed(Keys.F5))
                _uiFlags.ShowQuadtreeNodes = !_uiFlags.ShowQuadtreeNodes;

            var cameraDelta = Vector2.Zero;
            float camSpeed = (float)(200 * elapsedTime.TotalSeconds);
            if (KeyDown(Keys.A))
                cameraDelta.X -= camSpeed;
            if(KeyDown(Keys.D))
                cameraDelta.X += camSpeed;
            if(KeyDown(Keys.W))
                cameraDelta.Y -= camSpeed;
            if (KeyDown(Keys.S))
                cameraDelta.Y += camSpeed;
            if (KeyDown(Keys.Q))
                _camera.Scale = (float)Math.Max(_camera.Scale - 1*elapsedTime.TotalSeconds, 0.001f);
            if (KeyDown(Keys.E))
                _camera.Scale = (float)Math.Min(_camera.Scale + 1 * elapsedTime.TotalSeconds, 100f);

            _camera.Position += cameraDelta;

            _previousKeyboardState = _currentKeyboardState;
        }

        private bool KeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        private bool KeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _world.Draw(_spriteBatch, _camera);

            VisualizeQuadtree();
        }

        private void VisualizeQuadtree()
        {
            var tempCamera = _camera;
            tempCamera.Position *= _wuschelLayer.Speed;
            tempCamera.Scale /= _wuschelLayer.Scale;

            _spriteBatch.Begin(tempCamera);
            _quadtree.RunVisitor((node, depth) =>
            {
                if(_uiFlags.ShowQuadtreeNodes)
                    _drawingUtils.Draw(_spriteBatch, node.BoundingBox, Color.BurlyWood);

                var nodeCenter = new Vector2( node.BoundingBox.Center.X, node.BoundingBox.Center.Y );

                foreach (var entity in node.Data)
                {
                    if(_uiFlags.ShowEntityBoundingBoxes)
                        _drawingUtils.Draw(_spriteBatch, entity.BoundingBox, Color.DarkGreen);
                    if(_uiFlags.ShowEntityNodeAssociations)
                        _drawingUtils.DrawLine(_spriteBatch, entity.Transform.Position, nodeCenter, Color.BlueViolet);
                    if(_uiFlags.ShowEntityIndex)
                        _spriteBatch.DrawString(_uiFont, entity.Index.ToString(), entity.Transform.Position, Color.AntiqueWhite);
                }

                return true;
            });
            _spriteBatch.End();
        }
    }
}