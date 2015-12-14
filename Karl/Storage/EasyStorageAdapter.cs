using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyStorage;
using Microsoft.Xna.Framework;

namespace Karl.Storage
{
    class EasyStorageAdapter : IStorageProvider, IGameComponent, IUpdateable
    {
        private readonly Queue<WaitPromise> _pendingSharedRequests = new Queue<WaitPromise>();

        private SharedSaveDevice _sharedSaveDevice;

        public EasyStorageAdapter()
        {
        }

        public WaitPromise RequestSharedDevice()
        {
            var promise = new WaitPromise();
            _pendingSharedRequests.Enqueue(promise);
            return promise;
        }

        public void Initialize()
        {
            _sharedSaveDevice = new SharedSaveDevice();
            _sharedSaveDevice.Initialize();
            _sharedSaveDevice.PromptForDevice();
        }

        public bool Enabled
        {
            get { return true; }
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public void Update(GameTime gameTime)
        {
            _sharedSaveDevice.Update(gameTime);

            if (_sharedSaveDevice.IsReady)
            {
                if (_pendingSharedRequests.Count > 0)
                {
                    var adapterDevice = new EasyStorageSaveDeviceAdapter(_sharedSaveDevice);
                    var nextRequest = _pendingSharedRequests.Dequeue();
                    nextRequest.Resolve(adapterDevice);
                }
                
            }
        }

        public int UpdateOrder
        {
            get { return 0; }
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;
    }
}
