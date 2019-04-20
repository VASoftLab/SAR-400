using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CostumeRecorder
{
    public class Log
    {
        public bool RecordToFile { get; set; }

        public TextBlock Display { get; set; }

        public string FileName { get; set; }

        #region Конструкторы
        public Log(TextBlock control)
        {
            Display = control;
            FileName = "Session.log";
            RecordToFile = false;

            Reset();
        }

        public Log(TextBlock control, string fileName)
        {
            Display = control;
            FileName = fileName;
            RecordToFile = true;

            Reset();
        }

        public Log(TextBlock control, string fileName, bool recordToFile)
        {
            Display = control;
            FileName = fileName;
            RecordToFile = recordToFile;

            Reset();
        }
        #endregion

        public bool Write(string msg)
        {
            try
            {
                // Сформировать строчку лога
                string _logLine = $"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}] {msg}{Environment.NewLine}";

                // Вывести на экран в начале текста
                Display.Text = Display.Text.Insert(0, _logLine);
                
                // При необходимости сохранить в файл
                if (RecordToFile)
                {
                    using (StreamWriter wr = new StreamWriter(FileName, true))
                    {
                        wr.WriteLine(_logLine);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool Reset()
        {
            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
