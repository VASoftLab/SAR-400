using System;
using System.Collections.Generic;
using System.Linq;
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

        public MainWindow()
        {
            InitializeComponent();

            SAR = new Costume();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            dispLShoulderF.Value = SAR.GetValue("L.ShoulderF").ToString();
            dispLShoulderS.Value = SAR.GetValue("L.ShoulderS").ToString();
            dispLElbowR.Value = SAR.GetValue("L.ElbowR").ToString();
            dispLElbow.Value = SAR.GetValue("L.Elbow").ToString();
            dispLWristR.Value = SAR.GetValue("L.WristR").ToString();
            dispLWristS.Value = SAR.GetValue("L.WristS").ToString();
            dispLWristF.Value = SAR.GetValue("L.WristF").ToString();
            dispLFingerIndex.Value = SAR.GetValue("L.Finger.Index").ToString();
            dispLFingerLittle.Value = SAR.GetValue("L.Finger.Little").ToString();
            dispLFingerMiddle.Value = SAR.GetValue("L.Finger.Middle").ToString();
            dispLFingerRing.Value = SAR.GetValue("L.Finger.Ring").ToString();
            dispLFingerThumbS.Value = SAR.GetValue("L.Finger.ThumbS").ToString();
            dispLFingerThumb.Value = SAR.GetValue("L.Finger.Thumb").ToString();

            dispRShoulderF.Value = SAR.GetValue("R.ShoulderF").ToString();
            dispRShoulderS.Value = SAR.GetValue("R.ShoulderS").ToString();
            dispRElbowR.Value = SAR.GetValue("R.ElbowR").ToString();
            dispRElbow.Value = SAR.GetValue("R.Elbow").ToString();
            dispRWristR.Value = SAR.GetValue("R.WristR").ToString();
            dispRWristS.Value = SAR.GetValue("R.WristS").ToString();
            dispRWristF.Value = SAR.GetValue("R.WristF").ToString();
            dispRFingerIndex.Value = SAR.GetValue("R.Finger.Index").ToString();
            dispRFingerLittle.Value = SAR.GetValue("R.Finger.Little").ToString();
            dispRFingerMiddle.Value = SAR.GetValue("R.Finger.Middle").ToString();
            dispRFingerRing.Value = SAR.GetValue("R.Finger.Ring").ToString();
            dispRFingerThumbS.Value = SAR.GetValue("R.Finger.ThumbS").ToString();
            dispRFingerThumb.Value = SAR.GetValue("R.Finger.Thumb").ToString();

            dispTorsoR.Value = SAR.GetValue("TorsoR").ToString();
            dispTorsoF.Value = SAR.GetValue("TorsoF").ToString();
            dispTorsoS.Value = SAR.GetValue("TorsoS").ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SAR.Initialized)
                SAR.Dispose();
        }
    }
}
