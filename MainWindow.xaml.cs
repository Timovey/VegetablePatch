using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace VegetablePatch
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load();
            ParentLabel.Background = BussinesLogic.GetParentColor();
            ChildLabel.Background = BussinesLogic.GetChildColor();
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            bool result;
            var window = new ChildWindow();
            if (window.ShowDialog().Value)
            {
                 result = (bool)window.Result;
            }
            else
            {
                return;
            }

            var dialog = new OpenFileDialog();
            dialog.Filter = "docx|*.docx";

            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    BussinesLogic.AddFile(dialog.FileName, Category.Шаблоны, result);
                    MessageBox.Show("В грядку попал новый овощ", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Load();
        }
        private void Load()
        {
            ListboxTemplate.ItemsSource = BussinesLogic.GetListDocs(Category.Шаблоны);
        }

        private void Listbox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListboxTemplate.SelectedIndex == -1)
            {
                MessageBox.Show("Овощей пока еще не завезли", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "docx|*.docx";
                dialog.FileName = ((ListViewModel)ListboxTemplate.SelectedValue).Name;
                if ((bool)dialog.ShowDialog())
                {
                    BussinesLogic.TakeFile(dialog.FileName, ((ListViewModel)ListboxTemplate.SelectedValue).Name, GetNameListBox(),
                        ((ListViewModel)ListboxTemplate.SelectedValue).Child);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (ListboxTemplate.SelectedIndex == -1)
            {
                MessageBox.Show("Овощей пока еще не завезли", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BussinesLogic.DeleteFile(((ListViewModel)ListboxTemplate.SelectedValue).Name, GetNameListBox(), ((ListViewModel)ListboxTemplate.SelectedValue).Child);
            MessageBox.Show("Произошла жатва, овощ удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            Load();
        }
        private Category GetNameListBox()
        {
            if(ListboxTemplate.SelectedIndex > -1)
            {
                return Category.Шаблоны;
            }
            return Category.УчСправки;
        }
        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            if(ListboxTemplate.Items.Count < 1)
            {
                MessageBox.Show("Сначала посади овощ, потом требуй урожай", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var dialog = new OpenFileDialog();

            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    //BussinesLogic.AddFile(dialog.FileName, Category.Шаблоны,);
                    MessageBox.Show("Овощи выгружены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
