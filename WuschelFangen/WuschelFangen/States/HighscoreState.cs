using Karl.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Karl.Core;
using Karl.Game;
using Karl.Graphics;
using System.Globalization;

namespace WuschelFangen.States
{
    public class HighscoreState : GameState
    {
        SpriteBatch _spriteBatch;
        World _world;
        Camera _camera;

        Layer _layerBackground;
        SpriteInstance _background;

        TextInstance _title;
        readonly List<TextInstance> _highscoreEntries = new List<TextInstance>();
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

            _title = new TextInstance("Highscore", new Transform(0, -200), Align.TopCenter);

            var highscore = Highscore.Instance;

            var idxEntry = 0;
            foreach (var entry in highscore.Entries)
            {
                var text = new TextInstance(
                    entry.Score.ToString("0.00", CultureInfo.InvariantCulture) + " sec",
                    new Transform(0, -100 + idxEntry * 50), Align.MiddleCenter);

                _highscoreEntries.Add(text);
                layerForeground.Texts.Add(text);
                ++idxEntry;
            }

            _menuEntries.Add(new TextInstance("Hauptmenü", new Transform(0, 200), Align.MiddleCenter));
            
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

            _title.Font = Content.Load<SpriteFont>("Arial40");

            foreach (var text in _highscoreEntries)
            {
                text.Font = Content.Load<SpriteFont>("Arial20");
            }

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
                        GameStateManager.CurrentState = new MainMenuState();
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
