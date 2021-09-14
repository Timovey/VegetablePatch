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
    /// Логика взаимодействия для MoveCategoryWindow.xaml
    /// </summary>
    public partial class MoveCategoryWindow : Window
    {
        public Category? Result { get; private set; }

        public bool IsUnload { get; private set; }
        public MoveCategoryWindow(Category thisCategory)
        {
            InitializeComponent();
            RemoveFromListCategory(thisCategory);
            IsUnload = false;
        }

        private void RemoveFromListCategory(Category category)
        {
            switch(category)
            {
                case Category.Шаблоны:
                    Template.Visibility = Visibility.Collapsed;
                    break;
                case Category.Первичные:
                    First.Visibility = Visibility.Collapsed;
                    break;
                case Category.Вторичные:
                    Second.Visibility = Visibility.Collapsed;
                    break;
                case Category.Выздоров:
                    Recovered.Visibility = Visibility.Collapsed;
                    break;
                case Category.Вк:
                    Vk.Visibility = Visibility.Collapsed;
                    break;
                default:
                    Certificate.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (Result != null || IsUnload)
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

            if (sender is RadioButton item)
            {
                if (item.IsChecked != null && item.Name.Equals("Template"))
                {
                   Result = Category.Шаблоны;
                }
                if (item.IsChecked != null && item.Name.Equals("First"))
                {
                    Result = Category.Первичные;
                }
                if (item.IsChecked != null && item.Name.Equals("Second"))
                {
                    Result = Category.Вторичные;
                }
                if (item.IsChecked != null && item.Name.Equals("Vk"))
                {
                    Result = Category.Вк;
                }
                if (item.IsChecked != null && item.Name.Equals("Certificate"))
                {
                    Result = Category.УчСправки;
                }
                if (item.IsChecked != null && item.Name.Equals("Recovered"))
                {
                    Result = Category.Выздоров;
                }
                if (item.IsChecked != null && item.Name.Equals("Unload"))
                {
                    IsUnload = true;
                }
            }

        }
    }
}
