using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karl.Core.Extensions
{
    public static class RandomExtensions
    {
        public static T Choice<T>(this Random random, T[] choices)
        {
            return choices[random.Next(0, choices.Length)];
        }
    }
}
