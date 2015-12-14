using HexTileGame.States;
using Karl.Content;
using Karl.Game;
using Karl.Graphics;
using Microsoft.Xna.Framework;

namespace HexTileGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class HexTileGame : Game
    {
        /// <summary>
        /// GraphicsDeviceManager instance. Must be created in the constructor in order to be available as a service.
        /// </summary>
        GraphicsDeviceManager _graphics;


        readonly GameStateManager _gameStateManager;

        public HexTileGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _gameStateManager = new GameStateManager(this);

            var content = new ExtensibleContentManager(Services);
            content.Plugins.Add(typeof(Sprite), new SpritePlugin());
            content.Plugins.Add(typeof(SpriteAnimation), new SpriteAnimationPlugin());
            Content = content;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _gameStateManager.CurrentState = new IngameState();
            _gameStateManager.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _gameStateManager.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            _gameStateManager.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _gameStateManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.HotPink);
            _gameStateManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
