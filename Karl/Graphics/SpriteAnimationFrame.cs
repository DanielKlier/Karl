using System;

namespace Karl.Graphics
{
    public struct SpriteAnimationFrame
    {
        public TimeSpan Time;
        public Sprite Sprite;

        public SpriteAnimationFrame(TimeSpan time, Sprite sprite)
        {
            Time = time;
            Sprite = sprite;
        }
    }
}
