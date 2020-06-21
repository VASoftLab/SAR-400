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
using System.Windows.Shapes;

namespace CostumeRecorder
{
    /// <summary>
    /// Логика взаимодействия для WindowNeuralControlPanel.xaml
    /// </summary>
    public partial class WindowNeuralControlPanel : Window
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double X3 { get; set; }
        public double X4 { get; set; }
        public double X5 { get; set; }
        public double X6 { get; set; }
        public double X7 { get; set; }
        public double X8 { get; set; }
        public double X9 { get; set; }
        public double X10 { get; set; }
        public double X11 { get; set; }
        public double X12 { get; set; }
        public WindowNeuralControlPanel()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                X1 = Convert.ToDouble(tbR1.Text);
                X2 = Convert.ToDouble(tbR2.Text);
                X3 = Convert.ToDouble(tbR3.Text);
                X4 = Convert.ToDouble(tbR4.Text);
                X5 = Convert.ToDouble(tbR5.Text);
                X6 = Convert.ToDouble(tbR6.Text);
                X7 = Convert.ToDouble(tbR7.Text);
                X8 = Convert.ToDouble(tbR8.Text);
                X9 = Convert.ToDouble(tbR9.Text);

                X10 = Convert.ToDouble(tbP1.Text);
                X11 = Convert.ToDouble(tbP2.Text);
                X12 = Convert.ToDouble(tbP3.Text);
            }
            catch
            {
                MessageBox.Show("Введено неверное значение!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            this.Close();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
