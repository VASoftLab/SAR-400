using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CostumeController;
using CostumeController.BasicClasses;
using CostumeController.Robot;

namespace RobotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000);
            Robot robot = new Robot();
            Answer result;

            if (!robot.Connect())
            {
                Console.WriteLine("Подключение не установлено!");
                Console.Write("Нажмите любую кнопку для продолжения...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Подключение установлено!");

            //string[] data = File.ReadAllLines("test_lhand_forward.csv");

            CostumeJoint[] joints = new CostumeJoint[2]
            {
                    new CostumeJoint()
                    {
                        Name = "L.ShoulderF"
                    },
                    new CostumeJoint()
                    {
                        Name = "L.ShoulderS"
                    }
            };

            double[] startPosition = new double[2] { -10.65, 15.14};
            double[] endPosition = new double[2] { -86.56, 7.39 };

            result = robot.ExecuteCommand(joints, startPosition, 2);
            Console.WriteLine($"Результат: {result}");

            result = robot.ExecuteCommand(joints, endPosition, 2);
            Console.WriteLine($"Результат: {result}");

            Console.Write("Нажмите любую кнопку для продолжения...");
            Console.ReadKey();
        }
    }
}
