using Karl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuadtreeDemo.States;

namespace QuadtreeDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class QuadtreeDemoGame : KarlGame
    {
        public QuadtreeDemoGame()
        {
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GameStateManager.CurrentState = new DemoGameState();

            base.Initialize();
        }
    }
}
