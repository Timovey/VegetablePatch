using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            var window = new ChildWindow(BussinesLogic.GetParentColor(), BussinesLogic.GetChildColor());
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
            ListboxFirst.ItemsSource = BussinesLogic.GetListDocs(Category.Первичные);
            ListboxSecond.ItemsSource = BussinesLogic.GetListDocs(Category.Вторичные);
            ListboxVk.ItemsSource = BussinesLogic.GetListDocs(Category.Вк);
            ListboxRecovered.ItemsSource = BussinesLogic.GetListDocs(Category.Выздоров);
            ListboxCertificate.ItemsSource = BussinesLogic.GetListDocs(Category.УчСправки);
        }

        private void Listbox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox listbox= (ListBox)sender;
            string NameListBox = listbox.Name;
            if (listbox.SelectedIndex == -1)
            {
                MessageBox.Show("Овощей пока еще не завезли", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Category category;
            var window = new MoveCategoryWindow(GetNameCategoryListBox(NameListBox));
            if (window.ShowDialog().Value)
            {
                if (window.IsUnload)
                {
                        var dialog = new SaveFileDialog();
                        dialog.Filter = "docx|*.docx";
                        dialog.FileName = ((ListViewModel)ListboxTemplate.SelectedValue).Name;
                        if ((bool)dialog.ShowDialog())
                        {
                            BussinesLogic.TakeFile(dialog.FileName, ((ListViewModel)listbox.SelectedValue).Name, GetNameCategoryListBox(NameListBox),
                                ((ListViewModel)listbox.SelectedValue).Child);
                        }
                    }
                else
                {
                    category = (Category)window.Result;
                        BussinesLogic.MoveFile(GetNameCategoryListBox(NameListBox), 
                            ((ListViewModel)listbox.SelectedValue).Name, category, ((ListViewModel)listbox.SelectedValue).Child);
                    }
                //var mb = MessageBox.Show("Открыть файл?", "Выбери правильную сторону, юный падаван", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    if()
                }
            else
            {
                return;
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (GetNameListBox() == null)
            {
                MessageBox.Show("Овощей пока еще не завезли", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var item = (ListViewModel)GetListBox().SelectedValue;
            BussinesLogic.DeleteFile(item.Name, GetNameCategoryListBox(GetNameListBox()), item.Child);
            MessageBox.Show("Произошла жатва, овощ удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            Load();
        }
        private Category GetNameCategoryListBox(string Name)
        {
            if(Name.Equals(ListboxVk.Name))
            {
                return Category.Вк;
            }
            if(Name.Equals(ListboxTemplate.Name))
            {
                return Category.Шаблоны;
            }
            if (Name.Equals(ListboxFirst.Name))
            {
                return Category.Первичные;
            }
            if (Name.Equals(ListboxSecond.Name))
            {
                return Category.Вторичные;
            }
            if (Name.Equals(ListboxRecovered.Name))
            {
                return Category.Выздоров;
            }
            return Category.УчСправки;
        }

        private string GetNameListBox()
        {
            if (ListboxTemplate.SelectedIndex > -1)
            {
                return ListboxTemplate.Name;
            }
            if (ListboxFirst.SelectedIndex > -1)
            {
                return ListboxFirst.Name;
            }
            if (ListboxSecond.SelectedIndex > -1)
            {
                return ListboxSecond.Name;
            }
            if (ListboxRecovered.SelectedIndex > -1)
            {
                return ListboxRecovered.Name;
            }
            if (ListboxVk.SelectedIndex > -1)
            {
                return ListboxVk.Name;
            }
            if (ListboxCertificate.SelectedIndex > -1)
            {
                return ListboxCertificate.Name;
            }
            return null;
        }

        private ListBox GetListBox()
        {
            if (ListboxTemplate.SelectedIndex > -1)
            {
                return ListboxTemplate;
            }
            if (ListboxFirst.SelectedIndex > -1)
            {
                return ListboxFirst;
            }
            if (ListboxSecond.SelectedIndex > -1)
            {
                return ListboxSecond;
            }
            if (ListboxRecovered.SelectedIndex > -1)
            {
                return ListboxRecovered;
            }
            if (ListboxVk.SelectedIndex > -1)
            {
                return ListboxVk;
            }
            if (ListboxCertificate.SelectedIndex > -1)
            {
                return ListboxCertificate;
            }
            return null;
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

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (GetNameListBox() == null)
            {
                MessageBox.Show("Овощей пока еще не завезли", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var item = (ListViewModel)GetListBox().SelectedValue;
            BussinesLogic.OpenFile(GetNameCategoryListBox(GetNameListBox()), item.Name, item.Child);
           
        }
    }
}
