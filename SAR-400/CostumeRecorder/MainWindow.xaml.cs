using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        Costume SAR;
        string pathToConfig = string.Empty;
        List<RecorderCommand> commands;

        bool recording = false;
        Task RecordingTask;

        System.Timers.Timer timerGUI = new System.Timers.Timer { Interval = 500 };

        public MainWindow()
        {
            InitializeComponent();

            SAR = new Costume();
            timerGUI.Elapsed += UpdateGUI;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog opf = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = opf.ShowDialog();

            if (result == true)
            {
                pathToConfig = opf.FileName;
            }
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pathToConfig) == false)
                SAR.Initialize(pathToConfig);

            if (SAR.Initialized)
                MessageBox.Show("Успешно подключено!", "Подключение", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Ошибка при подключении!", "Подключение", MessageBoxButton.OK, MessageBoxImage.Error);

            DataGridJoints.ItemsSource = SAR.Joints;
            timerGUI.Start();
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
            if (SAR.Initialized)
                SAR.Dispose();
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
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\test.csv";

                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create)))
                {
                    string header = "Time;" + SAR.GetJointNames();
                    writer.WriteLine(header);

                    recording = true;
                    sw.Start();
                    TimeSpan lastRecordTime = new TimeSpan();

                    while (recording)
                    {
                        TimeSpan timeSpan = sw.Elapsed;

                        if ((timeSpan - lastRecordTime).TotalMilliseconds >= 100)
                        {
                            string record = $"{Convert.ToInt64(timeSpan.TotalMilliseconds)};" + SAR.GetCostumeSnapshot();
                            writer.WriteLine(record);

                            lastRecordTime = timeSpan;
                        }
                    }

                    sw.Stop();
                }
            });
        }

        private void ButtonStopRecording_Click(object sender, RoutedEventArgs e)
        {
            recording = false;
        }

        private void ButtonLoadRecord_Click(object sender, RoutedEventArgs e)
        {
            Recorder _recorder = new Recorder();
            string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\lar_1_1s.csv";
            try
            {
                commands = _recorder.ReadFromFile(path);
                TextDebug.Text += $"Загружено успешно" + Environment.NewLine;
            }
            catch(Exception E)
            {
                commands = null;
                MessageBox.Show(E.Message);
            }
        }

        private void ButtonPlayRecord_Click(object sender, RoutedEventArgs e)
        {
            if (commands == null)
            {
                MessageBox.Show("Не загружен список команд для робота!");
                return;
            }

            Robot robot = new Robot();
            robot.ErrorOccured +=(msg) => {
                TextDebug.Text += msg + Environment.NewLine;
            };

            if (!robot.Connect())
            {
                MessageBox.Show("Не удалось установить подключение к роботу");
                return;
            }

            foreach (RecorderCommand command in commands)
            {
                RobotAnswer answer = robot.ExecuteCommand(command.Joints,command.Duration);
                TextDebug.Text += answer.ToString() + Environment.NewLine;
                //int duration = (int)Math.Ceiling(command.Duration.TotalMilliseconds);
                //System.Threading.Thread.Sleep(duration);
            }

        }
    }
}
