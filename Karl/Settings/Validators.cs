using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karl.Settings
{
    public delegate object ValidatorDelegate(string readValue);
    public delegate string SaveFormatDelegate(object readValue);

    public static class Validators
    {
        public static object String(string value)
        {
            return value;
        }

        public static object Float(string value)
        {
            float result;
            if (!float.TryParse(value, out result))
                result = float.NaN;
            return result;
        }

        public static object Int(string value)
        {
            int result;
            if (!int.TryParse(value, out result))
                result = 0;
            return result;
        }

        public static object Bool(string value)
        {
            bool result;
            if (!bool.TryParse(value, out result))
                result = true;
            return result;
        }
    }
}
