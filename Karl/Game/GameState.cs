using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Karl.Game
{
    public abstract class GameState
    {
        public virtual void Initialize(GameStateManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            GameStateManager = manager;
        }

        public virtual void Shutdown()
        {
            GameStateManager = null;
        }

        public virtual void LoadContent()
        {
        }

        public virtual void UnloadContent()
        {
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    
        public ContentManager Content
        {
            get
            {
                return GameStateManager == null ? null : GameStateManager.Game.Content;
            }
        }

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return GameStateManager == null ? null : GameStateManager.Game.GraphicsDevice;
            }
        }

        public GameStateManager GameStateManager { get; private set; }
    }
}
