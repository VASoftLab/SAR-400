using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CostumeController;
using CostumeController.Robot;

namespace RobotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Robot robot = new Robot();

            if (!robot.Connect())
            {
                Console.WriteLine("Подключение не установлено!");
                Console.Write("Нажмите любую кнопку для продолжения...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Подключение установлено!");

            CostumeJoint[] joints = new CostumeJoint[2]
            {
                    new CostumeJoint()
                    {
                        Name = "R.ShoulderS"
                    },
                    new CostumeJoint()
                    {
                        Name = "R.ShoulderF"
                    }
            };
            double[] endPoints = new double[2] { -90, -70 };


            if (robot.ExecuteCommand(joints, endPoints))
                Console.WriteLine("Команда выполнена!");
            else
                Console.WriteLine("Команда не выполнена!");

            Console.Write("Нажмите любую кнопку для продолжения...");
            Console.ReadKey();
        }
    }
}
