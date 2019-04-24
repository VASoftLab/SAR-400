using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostumeRecorder.Classes
{
    public static class AppSettings
    {
        public static string CostumeConfigPath
        {
            get
            {
                return Properties.Settings.Default.CostumeConfigPath;
            }
            set
            {
                Properties.Settings.Default.CostumeConfigPath = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
