using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Karl.Storage
{
    class EasyStorageSaveDeviceAdapter : ISaveDevice
    {
        private readonly EasyStorage.ISaveDevice _device;

        internal EasyStorageSaveDeviceAdapter(EasyStorage.ISaveDevice device)
        {
            _device = device;
        }

        public void Save(string container, string fileName, Action<Stream> action)
        {
            _device.Save(container, fileName, stream => action(stream));
        }

        public void Load(string container, string fileName, Action<Stream> action)
        {
            _device.Load(container, fileName, stream => action(stream));
        }

        public bool FileExists(string container, string fileName)
        {
            return _device.FileExists(container, fileName);
        }
    }
}
