using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Karl.Storage
{
    public interface ISaveDevice
    {
        void Save(string container, string fileName, Action<Stream> action);
        void Load(string container, string fileName, Action<Stream> action);
    }
}
