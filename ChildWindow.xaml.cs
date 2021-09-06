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

namespace VegetablePatch
{
    /// <summary>
    /// Логика взаимодействия для ChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        public bool? Result { get; private set; }

        public ChildWindow()
        {
            InitializeComponent();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (Result != null)
            {
                this.DialogResult = true;
                Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int i = 0;
            if (sender is RadioButton item)
            {
                if(item.IsChecked != null)
                {
                    if(i == 0)
                    {
                        Result = false;
                    }
                    else
                    {
                        Result = true;
                    }
                }
                i++;
            }
        }
    }
}
