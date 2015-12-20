    
using System;
using Karl.Content;
using Karl.Game;
using Karl.Graphics;
using Karl.Settings;
using Karl.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karl
{
    public class KarlGame : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// GraphicsDeviceManager instance. Must be created in the constructor in order to be available as a service.
        /// </summary>
        private readonly GraphicsDeviceManager _graphics;

        private readonly EasyStorageAdapter _storageProvider;

        private readonly VideoSettings _videoSettings;

        protected GameStateManager GameStateManager { get; private set; }

        protected KarlGame()
        {
            RegisterFactories();

            _graphics = new GraphicsDeviceManager(this);
            GameStateManager = new GameStateManager(this);

            var content = new ExtensibleContentManager(Services);
            content.Plugins.Add(typeof(Sprite), new SpritePlugin());
            content.Plugins.Add(typeof(SpriteAnimation), new SpriteAnimationPlugin());
            Content = content;

            _videoSettings = new VideoSettings();

            _storageProvider = new EasyStorageAdapter();
            Components.Add(_storageProvider);
        }

        private void RegisterFactories()
        {
        }

        protected override void Initialize()
        {
            _videoSettings.Load(_storageProvider, ApplyVideoSettings);

            GameStateManager.Initialize();

            base.Initialize();
        }

        private void ApplyVideoSettings()
        {
            var windowMode = _videoSettings.GetString("Window.Mode");
            var windowWidth = _videoSettings.GetInt("Window.Width");
            var windowHeight = _videoSettings.GetInt("Window.Height");
            var fullscreen = windowMode == "fullscreen";
            var vSync = _videoSettings.GetBool("Device.VSync");

            if (windowMode == "borderless")
            {
                windowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                windowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                var hWnd = Window.Handle;
                var control = System.Windows.Forms.Control.FromHandle(hWnd);
                var form = control.FindForm();
                if (form != null) form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            }

            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.PreferredBackBufferHeight = windowHeight;
            _graphics.IsFullScreen = fullscreen;
            _graphics.SynchronizeWithVerticalRetrace = vSync;

            _graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GameStateManager.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            GameStateManager.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameStateManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.HotPink);
            GameStateManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            _videoSettings.Persist(_storageProvider);

            _storageProvider.Update(new GameTime());

            base.OnExiting(sender, args);
        }
    }
}
