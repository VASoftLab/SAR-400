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

namespace CostumeRecorder.UserControls
{
    /// <summary>
    /// Логика взаимодействия для JointDisplay.xaml
    /// </summary>
    public partial class JointDisplay : UserControl
    {
        public string Value
        {
            set
            {
                _value = value;
                try
                {
                    Dispatcher.Invoke(() => TextBoxValue.Text = _value);
                }
                catch
                {
                    Dispatcher.Invoke(() => TextBoxValue.Text = _value);
                }
                
            }
            get
            {
                return _value;
            }
        }

        private string _value;
        public JointDisplay()
        {
            InitializeComponent();
        }
    }
}
