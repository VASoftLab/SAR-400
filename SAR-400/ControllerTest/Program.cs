using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CostumeController;

namespace ControllerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Costume MyCostume = new Costume();

            MyCostume.Initialize("cfg.xml");
        }
    }
}
