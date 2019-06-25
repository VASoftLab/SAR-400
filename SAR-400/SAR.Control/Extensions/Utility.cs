using SAR.Control.Costume;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAR.Control.Extensions
{
    public static class Utility
    {
        private static readonly CultureInfo ci = new CultureInfo("en-US", false);
        private static string[] exceptions = { "L.Finger.Little", "R.Finger.Little", "TorsoF", "TorsoR", "TorsoS" };
        public static int Round(float value)
        {
            return (int)Math.Round(value);
        }

        public static float ConvertToFloat(string str)
        {
            float.TryParse(str.Replace(',', '.'), NumberStyles.Float, ci, out float retVal);
            return retVal;
        }

        public static float[] GetValuesArray(this List<CostumeJoint> source)
        {
            List<float> result = new List<float>();

            foreach (CostumeJoint joint in source)
            {
                bool skip = false;
                foreach (string name in exceptions)
                {
                    if (name == joint.Name)
                    {
                        skip = true;
                        break;
                    }
                }

                if (skip)
                    continue;

                result.Add(joint.Value);
            }
            //for (int i = 0; i < source.Count; i++)
            //{
            //    bool skip = false;
            //    foreach (string name in exceptions)
            //    {
            //        if (name == source[i].Name)
            //        {
            //            skip = true;
            //            break;
            //        }
            //    }

            //    if (skip)
            //        continue;

                
            //}

            return result.ToArray();
        }

        public static string[] GetNamesArray(this List<CostumeJoint> source)
        {
            List<string> result = new List<string>();

            foreach (CostumeJoint joint in source)
            {
                bool skip = false;
                foreach (string name in exceptions)
                {
                    if (name == joint.Name)
                    {
                        skip = true;
                        break;
                    }
                }

                if (skip)
                    continue;

                result.Add(joint.Name);
            }

            //for (int i = 0; i < source.Count; i++)
            //{
            //    bool skip = false;
            //    foreach (string name in exceptions)
            //    {
            //        if (name == source[i].Name)
            //        {
            //            skip = true;
            //            break;
            //        }
            //    }

            //    if (skip)
            //        continue;

            //    result[i] = source[i].Name;
            //}

            return result.ToArray();
        }
    }
}
