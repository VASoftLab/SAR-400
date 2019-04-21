using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using SAR.Control.Costume;
using SAR.Control.Recorder;
using SAR.Control.Robot;

namespace CostumeRecorder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Костюм
        Costume Costume;
        string _pathToCostumeConfig = string.Empty;

        // Робот
        Robot Robot;
        List<RecorderCommand> commands;

        bool recording = false;
        Task RecordingTask;

        Log AppLog;

        System.Timers.Timer timerGUI = new System.Timers.Timer { Interval = 500 };

        public MainWindow()
        {
            InitializeComponent();

            AppLog = new Log(TextDebug, "Session.Log", true);

            Costume = new Costume();

            Robot = new Robot();
            Robot.ErrorOccured += (msg) => {
                AppLog.Write(msg);
            };

            timerGUI.Elapsed += UpdateGUI;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppLog.Write("Программа запущена.");
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog opf = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = opf.ShowDialog();

            if (result == true)
            {
                _pathToCostumeConfig = opf.FileName;
                AppLog.Write($"Путь к файлу настроек костюма задан. Новый путь: {_pathToCostumeConfig}.");
            }

            if (!string.IsNullOrEmpty(_pathToCostumeConfig))
                Costume.Initialize(_pathToCostumeConfig);
            else
            {
                AppLog.Write("Загрузка настроек костюма не выполнена. Не выбран файл настроек костюма.");
                return;
            }

            if (Costume.Initialized)
            {
                AppLog.Write("Настройки костюма успешно загружены!");
            }
            else
            {
                AppLog.Write("Ошибка при загрузке настроек костюма!");
                return;
            }

            DataGridJoints.ItemsSource = Costume.Joints;
            LabelCostumeIP.Content = $"IP: {Costume.Address}";
            LabelCostumeStatus.Content = $"Состояние: Готов к отслеживанию движений";
        }

        private void ButtonConnectCostume_Click(object sender, RoutedEventArgs e)
        {
            if (Costume.Connect())
            {
                timerGUI.Start();
                AppLog.Write("Движения костюма остлеживаются!");
            }
            else
            {
                AppLog.Write("Невозможно начать остлеживание движения костюма!");
            }
        }

        private void UpdateGUI(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                DataGridJoints.Items.Refresh();
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Costume.Initialized)
                Costume.Dispose();
        }

        private void ButtonStartRecording_Click(object sender, RoutedEventArgs e)
        {
            if (RecordingTask != null)
            {
                if (RecordingTask.Status == TaskStatus.Running)
                    return;
            }
            
            RecordingTask = Task.Factory.StartNew(() =>
            {
                Stopwatch sw = new Stopwatch();

                string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\test.csv";

                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create)))
                {
                    string header = "Time;" + Costume.GetJointNames();
                    writer.WriteLine(header);

                    recording = true;
                    sw.Start();
                    TimeSpan lastRecordTime = new TimeSpan();

                    while (recording)
                    {
                        TimeSpan timeSpan = sw.Elapsed;

                        if ((timeSpan - lastRecordTime).TotalMilliseconds >= 100)
                        {
                            string record = $"{Convert.ToInt64(timeSpan.TotalMilliseconds)};" + Costume.GetCostumeSnapshot();
                            writer.WriteLine(record);

                            lastRecordTime = timeSpan;
                        }
                    }

                    sw.Stop();
                }
            });
            ButtonStartRecording.Visibility = Visibility.Collapsed;
            ButtonStopRecording.Visibility = Visibility.Visible;
        }

        private void ButtonStopRecording_Click(object sender, RoutedEventArgs e)
        {
            recording = false;

            ButtonStartRecording.Visibility = Visibility.Visible;
            ButtonStopRecording.Visibility = Visibility.Collapsed;
        }

        private void ButtonLoadRecord_Click(object sender, RoutedEventArgs e)
        {
            string path = string.Empty;
            Microsoft.Win32.OpenFileDialog opf = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = opf.ShowDialog();

            if (result == true)
            {
                path = opf.FileName;

                Recorder _recorder = new Recorder();

                try
                {
                    commands = _recorder.ReadFromFile(path);
                    AppLog.Write($"Запись загружена. Количетсво команд в записи: {commands.Count}.");
                }
                catch (Exception E)
                {
                    commands = null;
                    AppLog.Write($"Не удалось загрузить запись. Причина: {E.Message}.");
                }
            }
        }

        private void ButtonPlayRecord_Click(object sender, RoutedEventArgs e)
        {
            if (commands == null)
            {
                AppLog.Write("Невозможно воспроизвести запись. Не загружен список команд для робота.");
                return;
            }

            if (!Robot.Connected)
            {
                AppLog.Write("Невозможно воспроизвести запись. Отсутствует соединение с роботом.");
                return;
            }

            foreach (RecorderCommand command in commands)
            {
                RobotAnswer answer = Robot.ExecuteCommand(command.Joints,command.Duration);
                AppLog.Write(answer.ToString());
            }

        }

        private void ButtonPlayTestMessage_Click(object sender, RoutedEventArgs e)
        {
            AppLog.Write("Тестовое сообщение");
        }

        private void ButtonRobotConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Robot.Connect())
            {
                AppLog.Write("Соединение с роботом установлено.");
                return;
            }
            else
            {
                AppLog.Write("Не удалось установить соединение с роботом.");
            }
        }
    }
}
