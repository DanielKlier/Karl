using System;

namespace Karl.Storage
{
    public class WaitPromise
    {
        private event Action<ISaveDevice> ReadyEvent;

        public void Ready(Action<ISaveDevice> action)
        {
            ReadyEvent += action;
        }

        internal void Resolve(ISaveDevice device)
        {
            if (ReadyEvent != null)
            {
                ReadyEvent(device);
            }
        }
    }
}