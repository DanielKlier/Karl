using System;

namespace Karl.Graphics
{
    public class SpriteAnimationState
    {
        private SpriteAnimation _animation;
        private TimeSpan _time;
        private int _idxFrame;
        private bool _playing;
        private bool _looping;
        
        public SpriteAnimationState()
        {
        }

        public SpriteAnimationState(SpriteAnimation animation, bool play = true)
        {
            _animation = animation;
            _playing = play;
        }

        public void Update()
        {
            Update(TimeSpan.Zero);
        }

        public void Update(TimeSpan elapsedTime)
        {
            // do nothing if not playing...
            if (!_playing)
                return;
            
            // add elapsed time
            _time += elapsedTime;
            
            // abort if there is no animation
            if (_animation == null || _animation.Length == TimeSpan.Zero)
                return;

            // find next frame
            var idxLastFrame = _animation.Frames.Count - 1;            
            while (_idxFrame < idxLastFrame)
            {
                if (_animation.Frames[_idxFrame + 1].Time > _time)
                    break;

                ++_idxFrame;
            }

            // if looping, repeat animation on or after the last frame.
            if (_looping && _idxFrame >= idxLastFrame)
            {
                // start from the beginning
                _idxFrame = 0;

                // calculate new time.
                // (if the update hasn't hit the end of the animation perfectly, 
                // we simply take the remaining time over into the next loop).
                _time = new TimeSpan(_time.Ticks % _animation.Length.Ticks);

                // make sure that idxFrame is indexing the correct frame
                // (we might have skipped some frames already if the remaining time 
                // from the last step is large enough...)
                Update();
            }
        }

        public void Play()
        {
            _playing = true;
        }

        public void Pause()
        {
            _playing = false;
        }

        public void Stop()
        {
            _playing = false;
            _time = TimeSpan.Zero;
            _idxFrame = 0;
        }

        public bool Looping
        {
            get { return _looping; }
            set { _looping = value; }
        }

        public Sprite Sprite
        {
            get
            {
                // return empty sprite if there is no animation or idxFrame is invalid
                if (_animation == null || _idxFrame >= _animation.Frames.Count)
                    return default(Sprite);

                return _animation.Frames[_idxFrame].Sprite;
            }
        }

        public SpriteAnimation Animation 
        {
            get { return _animation; }
            set 
            {
                _animation = value;
                _idxFrame = 0;
                Update();
            }
        }
    }
}
