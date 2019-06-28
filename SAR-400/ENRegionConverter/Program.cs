using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ENRegionConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
            FileInfo[] files = directoryInfo.GetFiles("*.csv");
            String fileContent = String.Empty;
            int counter = 1;
            foreach (FileInfo file in files)
            {
                fileContent = File.ReadAllText(file.FullName);
                fileContent = fileContent.Replace(",", ".");
                fileContent = fileContent.Replace(";", ",");
                File.WriteAllText(file.FullName, fileContent);

                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Processed {counter++} from {files.Length}");
            }
            Console.WriteLine("Done... Press any key to exit.");
            Console.ReadKey();
        }
    }
}
