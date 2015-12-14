using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karl.Storage
{
    public interface IStorageProvider
    {
        WaitPromise RequestSharedDevice();
    }
}
