using System;
using System.Linq;
using System.Collections.Generic;

namespace Karl.Graphics
{
    public class SpriteAnimation
    {   
        private readonly List<SpriteAnimationFrame> _frames = new List<SpriteAnimationFrame>();

        public bool AddFrame(SpriteAnimationFrame newFrame)
        {
            // frames with negative time are not allowed
            if (newFrame.Time < TimeSpan.Zero)
                return false;

            // find correct position for new frame
            for (int idxFrame = 0; idxFrame < _frames.Count; ++idxFrame)
            {
                SpriteAnimationFrame currFrame = _frames[idxFrame];

                // insert frame before the current frame if its time precedes the current frame
                if (newFrame.Time < currFrame.Time)
                {                    
                    _frames.Insert(idxFrame, newFrame);
                    return true;
                }

                // do not allow frames with identical times
                if (newFrame.Time == currFrame.Time)
                    return false;
            }

            // append frame to the end 
            _frames.Add(newFrame);
            return true;
        }

        public IList<SpriteAnimationFrame> Frames
        {
            get 
            {
                // make readonly, so that the list can not changed by using this property
                return _frames.AsReadOnly();
            }
        }

        public TimeSpan Length
        {
            get 
            {
                // the length of the animation equals the time of the last frame 
                // or zero, if there is no frame at all.
                return _frames.LastOrDefault().Time;
            }
        }
    }
}
