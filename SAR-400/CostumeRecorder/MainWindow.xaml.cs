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

using CostumeController;

namespace CostumeRecorder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Costume SAR;
        string pathToConfig = string.Empty;
        bool recording = false;
        Task RecordingTask;

        public MainWindow()
        {
            InitializeComponent();

            SAR = new Costume();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeGUI();

            SAR.DataChanged += UpdateGUI;
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
        }

        private void UpdateGUI()
        {
            dispLShoulderF.Value = SAR.GetValue("L.ShoulderF").ToString("0.00");
            dispLShoulderS.Value = SAR.GetValue("L.ShoulderS").ToString("0.00");
            dispLElbowR.Value = SAR.GetValue("L.ElbowR").ToString("0.00");
            dispLElbow.Value = SAR.GetValue("L.Elbow").ToString("0.00");
            dispLWristR.Value = SAR.GetValue("L.WristR").ToString("0.00");
            dispLWristS.Value = SAR.GetValue("L.WristS").ToString("0.00");
            dispLWristF.Value = SAR.GetValue("L.WristF").ToString("0.00");
            dispLFingerIndex.Value = SAR.GetValue("L.Finger.Index").ToString("0.00");
            dispLFingerLittle.Value = SAR.GetValue("L.Finger.Little").ToString("0.00");
            dispLFingerMiddle.Value = SAR.GetValue("L.Finger.Middle").ToString("0.00");
            dispLFingerRing.Value = SAR.GetValue("L.Finger.Ring").ToString("0.00");
            dispLFingerThumbS.Value = SAR.GetValue("L.Finger.ThumbS").ToString("0.00");
            dispLFingerThumb.Value = SAR.GetValue("L.Finger.Thumb").ToString("0.00");

            dispRShoulderF.Value = SAR.GetValue("R.ShoulderF").ToString("0.00");
            dispRShoulderS.Value = SAR.GetValue("R.ShoulderS").ToString("0.00");
            dispRElbowR.Value = SAR.GetValue("R.ElbowR").ToString("0.00");
            dispRElbow.Value = SAR.GetValue("R.Elbow").ToString("0.00");
            dispRWristR.Value = SAR.GetValue("R.WristR").ToString("0.00");
            dispRWristS.Value = SAR.GetValue("R.WristS").ToString("0.00");
            dispRWristF.Value = SAR.GetValue("R.WristF").ToString("0.00");
            dispRFingerIndex.Value = SAR.GetValue("R.Finger.Index").ToString("0.00");
            dispRFingerLittle.Value = SAR.GetValue("R.Finger.Little").ToString("0.00");
            dispRFingerMiddle.Value = SAR.GetValue("R.Finger.Middle").ToString("0.00");
            dispRFingerRing.Value = SAR.GetValue("R.Finger.Ring").ToString("0.00");
            dispRFingerThumbS.Value = SAR.GetValue("R.Finger.ThumbS").ToString("0.00");
            dispRFingerThumb.Value = SAR.GetValue("R.Finger.Thumb").ToString("0.00");

            dispTorsoR.Value = SAR.GetValue("TorsoR").ToString("0.00");
            dispTorsoF.Value = SAR.GetValue("TorsoF").ToString("0.00");
            dispTorsoS.Value = SAR.GetValue("TorsoS").ToString("0.00");
        }

        private void InitializeGUI()
        {
            #region Названия
            dispLShoulderF.JointName = "L.ShoulderF";
            dispLShoulderS.JointName = "L.ShoulderS";
            dispLElbowR.JointName = "L.ElbowR";
            dispLElbow.JointName = "L.Elbow";
            dispLWristR.JointName = "L.WristR";
            dispLWristS.JointName = "L.WristS";
            dispLWristF.JointName = "L.WristF";
            dispLFingerIndex.JointName = "L.Finger.Index";
            dispLFingerLittle.JointName = "L.Finger.Little";
            dispLFingerMiddle.JointName = "L.Finger.Middle";
            dispLFingerRing.JointName = "L.Finger.Ring";
            dispLFingerThumbS.JointName = "L.Finger.ThumbS";
            dispLFingerThumb.JointName = "L.Finger.Thumb";

            dispRShoulderF.JointName = "R.ShoulderF";
            dispRShoulderS.JointName = "R.ShoulderS";
            dispRElbowR.JointName = "R.ElbowR";
            dispRElbow.JointName = "R.Elbow";
            dispRWristR.JointName = "R.WristR";
            dispRWristS.JointName = "R.WristS";
            dispRWristF.JointName = "R.WristF";
            dispRFingerIndex.JointName = "R.Finger.Index";
            dispRFingerLittle.JointName = "R.Finger.Little";
            dispRFingerMiddle.JointName = "R.Finger.Middle";
            dispRFingerRing.JointName = "R.Finger.Ring";
            dispRFingerThumbS.JointName = "R.Finger.ThumbS";
            dispRFingerThumb.JointName = "R.Finger.Thumb";

            dispTorsoR.JointName = "TorsoR";
            dispTorsoF.JointName = "TorsoF";
            dispTorsoS.JointName = "TorsoS";
            #endregion

            #region Значения
            dispLShoulderF.Value = "0.00";
            dispLShoulderS.Value = "0.00";
            dispLElbowR.Value = "0.00";
            dispLElbow.Value = "0.00";
            dispLWristR.Value = "0.00";
            dispLWristS.Value = "0.00";
            dispLWristF.Value = "0.00";
            dispLFingerIndex.Value = "0.00";
            dispLFingerLittle.Value = "0.00";
            dispLFingerMiddle.Value = "0.00";
            dispLFingerRing.Value = "0.00";
            dispLFingerThumbS.Value = "0.00";
            dispLFingerThumb.Value = "0.00";

            dispRShoulderF.Value = "0.00";
            dispRShoulderS.Value = "0.00";
            dispRElbowR.Value = "0.00";
            dispRElbow.Value = "0.00";
            dispRWristR.Value = "0.00";
            dispRWristS.Value = "0.00";
            dispRWristF.Value = "0.00";
            dispRFingerIndex.Value = "0.00";
            dispRFingerLittle.Value = "0.00";
            dispRFingerMiddle.Value = "0.00";
            dispRFingerRing.Value = "0.00";
            dispRFingerThumbS.Value = "0.00";
            dispRFingerThumb.Value = "0.00";

            dispTorsoR.Value = "0.00";
            dispTorsoF.Value = "0.00";
            dispTorsoS.Value = "0.00";
            #endregion
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
    }
}
