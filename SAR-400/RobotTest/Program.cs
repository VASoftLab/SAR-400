using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CostumeController.Robot;

namespace RobotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Robot robot = new Robot();
            robot.Connect();
            robot.ExecuteCommand();
        }
    }
}
