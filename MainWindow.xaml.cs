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
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "docx|*.docx";

            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    BussinesLogic.AddFile(dialog.FileName);
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
            Listbox.ItemsSource = BussinesLogic.GetListDocs();
        }

        private void Listbox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Овощей пока еще не завезли", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var dialog = new SaveFileDialog();
                dialog.Filter = "docx|*.docx";
                dialog.FileName = Listbox.SelectedValue.ToString();
                if ((bool)dialog.ShowDialog())
                {
                    BussinesLogic.TakeFile(dialog.FileName, Listbox.SelectedValue.ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (Listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Овощей пока еще не завезли", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BussinesLogic.DeleteFile(Listbox.SelectedValue.ToString());
            MessageBox.Show("Произошла жатва, овощ удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            Load();
        }
    }
}
