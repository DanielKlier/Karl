using Microsoft.Xna.Framework;

namespace Karl.Game
{
    public class GameStateManager
    {
        private readonly Microsoft.Xna.Framework.Game _game;
        private GameState _currState, _nextState;
        private bool _initialized, _contentLoaded, _switchState;

        public GameStateManager(Microsoft.Xna.Framework.Game game)
        {
            _game = game;
        }

        public void Initialize()
        {
            _initialized = true;

            if (_currState != null)
                _currState.Initialize(this);
        }

        public void Shutdown()
        {
            if (_currState != null)
                _currState.Shutdown();

            _initialized = false;
        }

        public void LoadContent()
        {
            _contentLoaded = true;

            if (_currState != null)
                _currState.LoadContent();
        }

        public void UnloadContent()
        {
            if (_currState != null)
                _currState.UnloadContent();

            _contentLoaded = false;
        }

        public void Update(GameTime gameTime)
        {
            if (_switchState)
            {
                if (_currState != null)
                {
                    if (_contentLoaded)
                        _currState.UnloadContent();
                    if (_initialized)
                        _currState.Shutdown();
                }

                _currState = _nextState;
                _switchState = false;

                if (_currState != null)
                {
                    if (_initialized)
                        _currState.Initialize(this);
                    if (_contentLoaded)
                        _currState.LoadContent();
                }
            }

            if (_initialized && _contentLoaded && _currState != null)
                _currState.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (_initialized && _contentLoaded && _currState != null)
                _currState.Draw(gameTime);
        }

        public GameState CurrentState
        {
            get { return _currState; }
            set
            {
                _nextState = value;
                _switchState = true;
            }
        }

        public Microsoft.Xna.Framework.Game Game
        {
            get { return _game; }
        }
    }
}
