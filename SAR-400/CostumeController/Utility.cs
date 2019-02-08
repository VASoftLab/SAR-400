using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostumeController
{
    public static class Utility
    {
        private static readonly CultureInfo ci = new CultureInfo("en-US", false);

        public static int Round(float value)
        {
            return (int)Math.Round(value);
        }

        public static float ConvertToFloat(string str)
        {
            float.TryParse(str.Replace(',', '.'), NumberStyles.Float, ci, out float retVal);
            return retVal;
        }
    }
}
