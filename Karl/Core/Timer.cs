using System;

namespace Karl.Core
{
    public class Timer
    {
        private TimeSpan _timeLeft;
        private TimeSpan _interval;
        private bool _running;
        private bool _repeating;

        public Timer()
        {
        }

        public Timer(TimeSpan time, bool run = false, bool repeat = false)
        {
            _interval = time;
            _timeLeft = time;
            _running = run;
            _repeating = repeat;
        }

        public Timer Start()
        {
            _running = true;
            return this;
        }

        public Timer Start(TimeSpan timeLeft)
        {
            _timeLeft = timeLeft;
            return Start();
        }

        public Timer Stop()
        {
            _timeLeft = Interval;
            _running = false;
            return this;
        }

        public Timer Pause()
        {
            _running = false;
            return this;
        }

        public void Update(TimeSpan elapsedTime)
        {
            if (_running)
            {
                _timeLeft -= elapsedTime;

                if (_timeLeft <= TimeSpan.Zero && null != Expired)
                {
                    while (_timeLeft <= TimeSpan.Zero)
                    {
                        Expired(this, null);

                        if (!_repeating)
                            Stop();

                        _timeLeft += _interval;
                    }
                }
            }
        }

        public TimeSpan Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        public TimeSpan TimeLeft
        {
            get { return _timeLeft; }
            set { _timeLeft = value; }
        }

        public bool Repeating 
        {
            get { return _repeating; }
            set { _repeating = value; }
        }
        
        public event EventHandler Expired;
    }
}
