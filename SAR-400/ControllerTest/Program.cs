using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CostumeController;

namespace ControllerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Costume costume = new Costume();
            try
            {
                costume.Initialize("cfg.xml");
                Console.BufferHeight = Math.Max(Console.WindowHeight, costume.Joints.Count + 3);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }

            while (costume.Initialized)
            {
                //Выводим на экран
                Console.WriteLine(string.Format("{0}|{1}", "NAME".PadRight(50), "VALUE".PadRight(10)));
                Console.WriteLine("-----------------------------------------------------------------------------");
                foreach (var item in costume.Joints)
                {
                    Console.WriteLine(string.Format("{0}|{1}", item.Name.PadRight(50), item.Value.ToString("F").PadRight(10)));
                }
                Thread.Sleep(50);
                Console.Clear();
            }
        }
    }
}
