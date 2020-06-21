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

using CostumeRecorder.Classes;

using SARNeuralLibrary;
using MathWorks.MATLAB.NET.Arrays;

namespace CostumeRecorder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Costume Costume;
        Robot Robot;


        List<RecorderCommand> commands;

        bool recording = false;
        string pathToRecord = string.Empty;
        Task RecordingTask;

        Log AppLog;

        System.Timers.Timer timerGUI = new System.Timers.Timer { Interval = 500 };

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация лога программы
            AppLog = new Log(TextDebug, "Session.Log", true);

            AppLog.Write("Программа запущена.");

            // Инициализация костюма
            Costume = new Costume();

            // Загрузка конфига костюма
            if (File.Exists(AppSettings.CostumeConfigPath))
                Costume.Initialize(AppSettings.CostumeConfigPath);

            if (Costume.Initialized)
            {
                LabelCostumeIP.Content = $"IP: {Costume.Address}";
                LabelCostumeStatus.Content = "Состояние: Готов";

                DataGridJoints.ItemsSource = Costume.Joints;
                AppLog.Write($"Настройки костюма загружены.");
            }
            else
            {
                AppLog.Write($"Не удалось загрузить настройки костюма. Проверьте файл настроек.");
            }

            
            // Инициализация робота
            Robot = new Robot();
            Robot.ErrorOccured += (msg) => {
                AppLog.Write(msg);
            };

            LabelRobotIP.Content = $"IP: {Robot.Address}";
            LabelRobotStatus.Content = "Состояние: Не подключен";

            AppLog.Write($"Настройки робота загружены.");

            timerGUI.Elapsed += UpdateGUI;
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog opf = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = opf.ShowDialog();

            if (result == true)
            {

                AppSettings.CostumeConfigPath = opf.FileName;
                AppLog.Write($"Путь к файлу настроек костюма задан. Новый путь: {AppSettings.CostumeConfigPath}.");
            }

            if (!string.IsNullOrEmpty(AppSettings.CostumeConfigPath))
                Costume.Initialize(AppSettings.CostumeConfigPath);
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
            LabelCostumeStatus.Content = $"Состояние: Готов";
        }

        private void ButtonConnectCostume_Click(object sender, RoutedEventArgs e)
        {
            if (Costume.Connect())
            {
                LabelCostumeStatus.Content = "Состояние: Отслеживается";
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
            try
            {
                Dispatcher.Invoke(() =>
                {
                    DataGridJoints.Items.Refresh();
                });
            }
            catch (Exception E)
            {
                Dispatcher.Invoke(() => { AppLog.Write($"При работе с обновлением GUI возникла ошибка. {E.Message}"); });
                return;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Costume.Initialized)
                Costume.Dispose();
        }

        private void ButtonRobotConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Robot.Connect())
                {
                    AppLog.Write("Соединение с роботом установлено.");
                    LabelRobotStatus.Content = "Состяние: Подключен";
                    return;
                }
                else
                {
                    AppLog.Write("Не удалось установить соединение с роботом.");
                    LabelRobotStatus.Content = "Состяние: Ошибка";
                }
            }
            catch (Exception E)
            {
                Dispatcher.Invoke(() => { AppLog.Write($"При подключении к роботу возникла ошибка. {E.Message}"); });
                return;
            }
        }

        private void ButtonRecordStop_Click(object sender, RoutedEventArgs e)
        {
            recording = false;
        }

        private void ButtonRecordStart_Click(object sender, RoutedEventArgs e)
        {
            if (RecordingTask != null)
            {
                if (RecordingTask.Status == TaskStatus.Running)
                    return;
            }

            if (string.IsNullOrEmpty(pathToRecord))
            {
                Dispatcher.Invoke(() => { AppLog.Write("Запись двжиения невозможна. Не выбран конечный файл."); });
                return;
            }

            AppLog.Write("Начата запись движения.");

            RecordingTask = Task.Factory.StartNew(() =>
            {
                Stopwatch sw = new Stopwatch();

                try
                {
                    using (StreamWriter writer = new StreamWriter(new FileStream(pathToRecord, FileMode.Create)))
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
                                try
                                {
                                    string record = $"{Convert.ToInt64(timeSpan.TotalMilliseconds)};" + Costume.GetCostumeSnapshot();
                                    writer.WriteLine(record);

                                    lastRecordTime = timeSpan;


                                    Dispatcher.Invoke(() => { LabelRecordStatus.Content = $"Состояние: Идет запись. ({sw.Elapsed})"; });
                                }
                                catch (Exception E)
                                {
                                    Dispatcher.Invoke(() => { AppLog.Write($"При записи движения возникла ошибка. {E.Message}. Запись остановлена"); });
                                    recording = false;
                                }

                            }
                        }

                        // Добавить последнюю запись
                        try
                        {
                            string record = $"{Convert.ToInt64(sw.Elapsed.TotalMilliseconds)};" + Costume.GetCostumeSnapshot();
                            writer.WriteLine(record);

                            Dispatcher.Invoke(() => { LabelRecordStatus.Content = $"Состояние: Идет запись. ({sw.Elapsed})"; });
                        }
                        catch (Exception E)
                        {
                            Dispatcher.Invoke(() => { AppLog.Write($"При записи движения возникла ошибка. {E.Message}. Запись остановлена"); });
                            recording = false;
                        }

                        sw.Stop();
                        Dispatcher.Invoke(() =>
                        {
                            LabelRecordStatus.Content = $"Состояние: Запись окончена. Длительность: {sw.Elapsed}";
                            AppLog.Write("Запись движения окончена.");
                        });
                    }
                }
                catch(Exception E)
                {
                    Dispatcher.Invoke(()=> { AppLog.Write($"При работе с записью движения возникла ошибка. {E.Message}. Запись остановлена"); });
                    return;
                }
            });
        }

        private void ButtonPlayStart_Click(object sender, RoutedEventArgs e)
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
                RobotAnswer answer = Robot.ExecuteCommand(command.Joints, command.Duration);
                AppLog.Write(answer.ToString());
            }
        }

        private void BittonPlaySelectFile_Click(object sender, RoutedEventArgs e)
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
                    AppLog.Write($"Запись загружена. Количество команд в записи: {commands.Count}.");
                }
                catch (Exception E)
                {
                    commands = null;
                    AppLog.Write($"Не удалось загрузить запись. Причина: {E.Message}.");
                }

                LabelPlayFile.Content = $"Файл: {path}";
            }
        }

        private void BittonRecordSelectFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sf = new Microsoft.Win32.SaveFileDialog();

            Nullable<bool> result = sf.ShowDialog();

            if (result == true)
            {
                pathToRecord = sf.FileName;
                LabelRecordFile.Content = $"Файл: {pathToRecord}";
                LabelRecordStatus.Content = "Состояние: Файл задан";
            }
        }

        private void ButtonNeuralOpenPanel_Click(object sender, RoutedEventArgs e)
        {
            WindowNeuralControlPanel wnd = new WindowNeuralControlPanel();

            wnd.Show();

            if (wnd.DialogResult == true)
            {
                SARNeural neural = new SARNeural();
                MWArray output = neural.Predict(wnd.X1, wnd.X2, wnd.X3, wnd.X4, wnd.X5, wnd.X6, wnd.X7, wnd.X8, wnd.X9, wnd.X10, wnd.X11, wnd.X12);
                double[,] values = (double[,])((MWNumericArray)output).ToArray(MWArrayComponent.Real);


                RecorderCommand command = new RecorderCommand
                {
                    Duration = TimeSpan.FromSeconds(2.0),
                    Joints = new List<CostumeJoint>
                    {
                        new CostumeJoint() { Name = "R.ShoulderF", Value = (float)values[0, 0] },
                        new CostumeJoint() { Name = "R.ShoulderS", Value = (float)values[0, 1] },
                        new CostumeJoint() { Name = "R.ElbowR", Value = (float)values[0, 2] },
                        new CostumeJoint() { Name = "R.Elbow", Value = (float)values[0, 3] },
                        new CostumeJoint() { Name = "R.WristR", Value = (float)values[0, 4] },
                        new CostumeJoint() { Name = "R.WritsS", Value = (float)values[0, 5] }
                    }
                };

                RobotAnswer answer = Robot.ExecuteCommand(command.Joints, command.Duration);
                AppLog.Write(answer.ToString());
            }
        }
    }
}
