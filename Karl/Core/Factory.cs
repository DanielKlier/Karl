using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karl.Core
{
    public static class Factory<T>
    {
        private static readonly Dictionary<Type, Func<T>> Constructors = new Dictionary<Type, Func<T>>();

        public static T Create()
        {
            Func<T> constructor;
            if (Constructors.TryGetValue(typeof(T), out constructor))
                return constructor();

            throw new ArgumentException("No constructor registered for that type");
        }

        public static void Register(Func<T> constructor)
        {
            Constructors.Add(typeof(T), constructor);
        }
    }
}
