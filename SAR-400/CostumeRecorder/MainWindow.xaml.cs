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
    }
}
