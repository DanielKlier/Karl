using Karl.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Karl.Core;
using Karl.Game;
using Karl.Graphics;

namespace WuschelFangen.States
{
    public class MainMenuState : GameState
    {
        SpriteBatch _spriteBatch;
        World _world;
        Camera _camera;

        Layer _layerBackground;
        SpriteInstance _background;
        SpriteInstance _wuschel;

        Sprite _sprWuschel1;
        Sprite _sprWuschel2;
        
        TextInstance _title;
        readonly List<TextInstance> _menuEntries = new List<TextInstance>();
        int _idxSelectedMenuEntry;
        
        KeyboardState _lastKeyboardState;

        public override void Initialize(GameStateManager manager)
        {
            base.Initialize(manager);

            _world = new World();

            _layerBackground = new Layer(0.1f, 1.0f);
            _world.Layers.Add(_layerBackground);

            var layerForeground = new Layer();
            _world.Layers.Add(layerForeground);

            _wuschel = new SpriteInstance {Transform = new Transform(0, -50)};
            layerForeground.Sprites.Add(_wuschel);

            _title = new TextInstance("Wuschel-Fange",new Transform(0, -200), Align.TopCenter);
            _menuEntries.Add(new TextInstance("Spiel starten", new Transform(0, 50), Align.MiddleCenter));
            _menuEntries.Add(new TextInstance("Highscore", new Transform(0, 100), Align.MiddleCenter));
            _menuEntries.Add(new TextInstance("Beenden", new Transform(0, 150), Align.MiddleCenter));

            layerForeground.Texts.Add(_title);
            foreach (var text in _menuEntries)
            {
                layerForeground.Texts.Add(text);
            }

            _camera = new Camera(Transform.Identity);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _world.LoadContent(Content);
            _background = new SpriteInstance(Content.Load<Sprite>("background"));
            _layerBackground.Sprites.Add(_background);

            _sprWuschel1 = Content.Load<Sprite>("wuschel1");
            _sprWuschel2 = Content.Load<Sprite>("wuschel3");
            _wuschel.Sprite = _sprWuschel1;

            _title.Font = Content.Load<SpriteFont>("Arial40");

            foreach (var text in _menuEntries)
            {
                text.Font = Content.Load<SpriteFont>("Arial20");
            }
        }

        public override void UnloadContent()
        {
            _world.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            const string text = "Wuschel-Fange";
            var numChars = Math.Min((int) (gameTime.TotalGameTime.TotalSeconds * 10), text.Length);
            _title.Text = text.Substring(0, numChars);

            if (numChars == text.Length)
                _wuschel.Sprite = _sprWuschel2;

            var currKeyboardState = Keyboard.GetState();

            if (_lastKeyboardState.IsKeyUp(Keys.Up) &&
                currKeyboardState.IsKeyDown(Keys.Up))
                --_idxSelectedMenuEntry;
            if (_lastKeyboardState.IsKeyUp(Keys.Down) &&
                currKeyboardState.IsKeyDown(Keys.Down))
                ++_idxSelectedMenuEntry;
            if (_lastKeyboardState.IsKeyDown(Keys.Enter) &&
                currKeyboardState.IsKeyUp(Keys.Enter))
            {
                switch (_idxSelectedMenuEntry)
                {
                    case 0:
                        GameStateManager.CurrentState = new IngameState();
                        return;

                    case 1:
                        GameStateManager.CurrentState = new HighscoreState();
                        return;

                    case 2:
                        GameStateManager.Game.Exit();
                        return;
                }
            }

            _world.Update(gameTime.ElapsedGameTime);

            if (_idxSelectedMenuEntry < 0)
                _idxSelectedMenuEntry = _menuEntries.Count - 1;
            if (_idxSelectedMenuEntry >= _menuEntries.Count)
                _idxSelectedMenuEntry = 0;

            for (var idxMenuEntry = 0; idxMenuEntry < _menuEntries.Count; ++idxMenuEntry)
            {
                _menuEntries[idxMenuEntry].Color = idxMenuEntry == _idxSelectedMenuEntry ? Color.Red : Color.White;
            }

            _lastKeyboardState = currKeyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            _world.Draw(_spriteBatch, _camera);
        }
    }
}
