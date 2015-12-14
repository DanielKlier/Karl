using System;
using Karl.Collision;
using Karl.Core;
using Karl.Entities;
using Karl.Game;
using Karl.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using WuschelFangen.Entities;

namespace WuschelFangen.States
{
    public class IngameState : GameState
    {
        SpriteBatch _spriteBatch;
        Song _music;
        
        World _world;
        Camera _camera;

        Layer _layerBackground, _layerEnemies;
        SpriteInstance _background;

        Entities.Karl _karl;
        TimeSpan _timeAlive;

        TextInstance _timeDisplay;
        readonly Random _random = new Random();

        public override void Initialize(GameStateManager manager)
        {
            base.Initialize(manager);

            _world = new World();

            _layerBackground = new Layer(0.1f, 1.0f);
            _world.Layers.Add(_layerBackground);

            _layerEnemies = new Layer();
            _world.Layers.Add(_layerEnemies);

            var layerPlayer = new Layer();
            _world.Layers.Add(layerPlayer);

            var layerUi = new Layer(0.0f, 1.0f);
            _world.Layers.Add(layerUi);

            _karl = new Entities.Karl(layerPlayer);
            _karl.Killed += OnKarlKilled;
            _world.AddEntity(_karl);

            SpawnWuschel();

            _timeAlive = TimeSpan.Zero;

            _timeDisplay = new TextInstance("", Align.TopRight) {Transform = new Transform(380, -230)};

            layerUi.Texts.Add(_timeDisplay);

            _camera = new Camera(Transform.Identity);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _world.LoadContent(Content);
            _background = new SpriteInstance(Content.Load<Sprite>("background"));
            _layerBackground.Sprites.Add(_background);

            _music = Content.Load<Song>("bgm\\FF7Remix");
            MediaPlayer.Play(_music);

            _timeDisplay.Font = Content.Load<SpriteFont>("Arial20");
        }

        public override void UnloadContent()
        {
            _world.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime.ElapsedGameTime);

            _timeAlive += gameTime.ElapsedGameTime;
            _timeDisplay.Text = ((int) _timeAlive.TotalSeconds) + "." + (_timeAlive.Milliseconds / 10).ToString("00");
        }

        public override void Draw(GameTime gameTime)
        {
            _world.Draw(_spriteBatch, _camera);
        }

        private void SpawnWuschel()
        {
            var circle = new Circle(100) {Mask = CollisionGroup.Player};
            do
            {
                var positionX = (float)_random.NextDouble() * 600 - 300;
                var positionY = (float)_random.NextDouble() * 400 - 200;
                circle.Transform = new Transform(positionX, positionY);
            }
            while (_world.Space.Collides(circle));

            var wuschel = new Wuschel(_layerEnemies) {Transform = circle.Transform};
            wuschel.Killed += OnWuschelKilled;
            _world.AddEntity(wuschel);
        }

        private void OnWuschelKilled(Entity entity)
        {
            // Spawn two new Wuschels for every dead Wuschel...
            SpawnWuschel();
            SpawnWuschel();
        }

        private void OnKarlKilled(Entity entity)
        {
            var highScoreEntry = new Highscore.Entry {Score = (int) _timeAlive.TotalSeconds};

            Highscore.Instance.InsertEntry(highScoreEntry);
            Highscore.Instance.Save("highscore.txt");

            GameStateManager.CurrentState = new HighscoreState();
        }
    }
}
