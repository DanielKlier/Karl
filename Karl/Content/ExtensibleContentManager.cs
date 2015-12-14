using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Karl.Content
{
    public class ExtensibleContentManager : ContentManager
    {
        private readonly Dictionary<Type, IPlugin> _plugins = new Dictionary<Type,IPlugin>();

        public ExtensibleContentManager(IServiceProvider services)
            : base(services)
        {
        }

        public ExtensibleContentManager(IServiceProvider services, string rootDirectory)
            : base(services, rootDirectory)
        {
        }

        public override T Load<T>(string assetName)
        {
            Type type = typeof(T);

            IPlugin plugin;
            return _plugins.TryGetValue(type, out plugin) ? plugin.Load<T>(this, assetName) : base.Load<T>(assetName);
        }

        public Dictionary<Type, IPlugin> Plugins
        {
            get { return _plugins; }
        }

        public interface IPlugin
        {
            T Load<T>(ExtensibleContentManager content, string assetName);
        }
    }
}
