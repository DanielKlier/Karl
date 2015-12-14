using HexTileGame.States;
using Karl;
using Karl.Content;
using Karl.Game;
using Karl.Graphics;
using Microsoft.Xna.Framework;

namespace HexTileGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class HexTileGame : KarlGame
    {
        public HexTileGame()
        {
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
            GameStateManager.CurrentState = new IngameState();

            base.Initialize();
        }
    }
}
