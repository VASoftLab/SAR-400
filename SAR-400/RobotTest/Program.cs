using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SAR.Control.Costume;
using SAR.Control.Robot;

namespace RobotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000);
            Robot robot = new Robot();
            RobotAnswer result;

            if (!robot.Connect())
            {
                Console.WriteLine("Подключение не установлено!");
                Console.Write("Нажмите любую кнопку для продолжения...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Подключение установлено!");

            string[] jointNames = new string[29] { "L.ShoulderF", "L.ShoulderS", "L.ElbowR", "L.Elbow", "L.WristR", "L.WristS", "L.WristF", "L.Finger.Index", "L.Finger.Little", "L.Finger.Middle", "L.Finger.Ring", "L.Finger.ThumbS", "L.Finger.Thumb", "R.ShoulderF", "R.ShoulderS", "R.ElbowR", "R.Elbow", "R.WristR", "R.WristS", "R.WristF", "R.Finger.Index", "R.Finger.Little", "R.Finger.Middle", "R.Finger.Ring", "R.Finger.ThumbS", "R.Finger.Thumb", "TorsoR", "TorsoF", "TorsoS" };

            // Проверка на доступность каждого из узлов робота
            foreach (string name in jointNames)
            {
                List<CostumeJoint> joints = new List<CostumeJoint>
                {
                    new CostumeJoint()
                    {
                        Name = name,
                        Value = 0.00f
                    }
                };

                result = robot.ExecuteCommand(joints);

                Console.WriteLine($"Результат для {name}: {result}");
            }

            Console.Write("Нажмите любую кнопку для продолжения...");
            Console.ReadKey();
        }
    }
}
