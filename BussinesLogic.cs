using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Windows.Media;

namespace VegetablePatch
{
    public class BussinesLogic
    {
        private static string nameListDocs = "docs.xml";
        private static Color ChildColor = Color.FromArgb(70, 238, 241, 122);
        private static Color ParentColor = Color.FromArgb(70, 131, 233, 220);
        //public static void Backup(string path)
        //{
        //    List<string> names = GetListDocs();
        //    try
        //    {
        //        if (!File.Exists(nameListDocs))
        //        {
        //            return;
        //        }

        //        XDocument xDocument = XDocument.Load(nameListDocs);
        //        var xElements = xDocument.Root.Elements("Doc").ToList();
        //        foreach (var elem in xElements)
        //        {
        //            File.Copy(elem.Value, path, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public static List<ListViewModel> GetListDocs(Category category)
        {
            var list = new List<ListViewModel>();
            try
            {
                if (!File.Exists(nameListDocs))
                {
                    return null;
                }
               
                XDocument xDocument = XDocument.Load(nameListDocs);
                var ChildElements = xDocument.Root.Element(category.ToString()).Element("Детские").Elements("Doc").ToList();
                var ParentElements = xDocument.Root.Element(category.ToString()).Element("Взрослые").Elements("Doc").ToList();
                foreach (var elem in ChildElements)
                {
                    list.Add(new ListViewModel{
                    Name = elem.Value ,
                    Child = true,
                    Color = new SolidColorBrush(ChildColor) 
                    });
                }
                foreach (var elem in ParentElements)
                {
                    list.Add(new ListViewModel
                    {
                        Name = elem.Value,
                        Child = false,
                        Color = new SolidColorBrush(ParentColor)

                    });
                }
                //list.Sort();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public static void AddFile(string fileName, Category category, bool child)
        {
            string old = "Взрослые";
            if(child)
            {
                old = "Детские";
            }
            
            try
            {
                CreateDocXML();
                CreateFolders();
                string shortName = TakeNameFile(fileName);
                if (!File.Exists(nameListDocs))
                {
                    throw new Exception("А файла то нема");
                }

                bool repeatName = false;
                    XDocument xDocument = XDocument.Load(nameListDocs);
                    var xElements = xDocument.Root.Element(category.ToString()).Element(old).Elements("Doc").ToList();
                    foreach (var elem in xElements)
                    {
                        if (elem.Value.Equals(shortName))
                        {
                            repeatName = true;
                        }
                    }
                    if (!repeatName)
                    {
                        var Category = xDocument.Root.Element(category.ToString()).Element(old);
                        XElement xElement = new XElement("Doc", shortName);
                        Category.Add(xElement);
                        xDocument.Save(nameListDocs);
                    }

                string path = PathSelector(category.ToString(), old, shortName);
                File.Copy(fileName, path, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteFile(string fileName)
        {
            try
            {
                XDocument xDocument = XDocument.Load(nameListDocs);
                var xElements = xDocument.Root.Elements("Doc").ToList();
                int count = -1;
                foreach (var elem in xElements)
                {
                    count++;
                    if (elem.Value.Equals(fileName))
                    {
                        break;
                    }
                }
                if (count != -1)
                {
                    xDocument.Root.Elements("Doc").ElementAt(count).Remove();
                    xDocument.Save(nameListDocs);
                    File.Delete(fileName);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static async void TakeFile(string filePath, string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception("Файл не найден");
            }
            try
            {
                File.Copy(fileName, filePath, true);

                Application app = new Application();
                app.Visible = true;
                await System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        Document doc = app.Documents.Open(FileName: filePath, Visible: true);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });



            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void CreateFolders()
        {
            string currentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currentPath, Category.Шаблоны.ToString())))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Шаблоны.ToString())));
            
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.Первичные.ToString()))))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Первичные.ToString())));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.Вторичные.ToString()))))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Вторичные.ToString())));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.Выздоров.ToString()))))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Выздоров.ToString())));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.Вк.ToString()))))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Вк.ToString())));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.УчСправки.ToString()))))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.УчСправки.ToString())));
            }

            if (!Directory.Exists(Path.Combine(currentPath, (Category.Шаблоны.ToString()), "Детские")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Шаблоны.ToString()), "Детские"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.Шаблоны.ToString()), "Взрослые")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Шаблоны.ToString()), "Взрослые"));
            }

            if (!Directory.Exists(Path.Combine(currentPath, (Category.Первичные.ToString()), "Детские")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Первичные.ToString()), "Детские"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.Первичные.ToString()), "Взрослые")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Первичные.ToString()), "Взрослые"));
            }

            if (!Directory.Exists(Path.Combine(currentPath, (Category.Вторичные.ToString()), "Детские")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Вторичные.ToString()), "Детские"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.Вторичные.ToString()), "Взрослые")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Вторичные.ToString()), "Взрослые"));
            }

            if (!Directory.Exists(Path.Combine(currentPath, (Category.Вк.ToString()), "Детские")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Вк.ToString()), "Детские"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.Вк.ToString()), "Взрослые")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.Вк.ToString()), "Взрослые"));
            }

            if (!Directory.Exists(Path.Combine(currentPath, (Category.УчСправки.ToString()), "Детские")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.УчСправки.ToString()), "Детские"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, (Category.УчСправки.ToString()), "Взрослые")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, (Category.УчСправки.ToString()), "Взрослые"));
            }
        }
        private static void CreateDocXML()
        {
            try
            {
                if (!File.Exists(nameListDocs))
                {
                    var Adult = new XElement("Взрослые");
                    var Child = new XElement("Детские");

                    var xElement = new XElement("Docs");

                    var Template = new XElement(Category.Шаблоны.ToString());
                    Template.Add(Adult);
                    Template.Add(Child);

                    var First = new XElement(Category.Первичные.ToString());
                    First.Add(Adult);
                    First.Add(Child);

                    var Second = new XElement(Category.Вторичные.ToString());
                    Second.Add(Adult);
                    Second.Add(Child);

                    var Recovered = new XElement(Category.Выздоров.ToString());
                    Recovered.Add(Adult);
                    Recovered.Add(Child);

                    var Vk = new XElement(Category.Вк.ToString());
                    Vk.Add(Adult);
                    Vk.Add(Child);

                    var Ask = new XElement(Category.УчСправки.ToString());
                    Ask.Add(Adult);
                    Ask.Add(Child);

                    xElement.Add(Template);
                    xElement.Add(First);
                    xElement.Add(Second);
                    xElement.Add(Recovered);
                    xElement.Add(Vk);
                    xElement.Add(Ask);
                    XDocument xDocument = new XDocument(xElement);
                    xDocument.Save(nameListDocs);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private static string PathSelector(params string[] strings)
        {
            string res = "";
            int i = 0;
            foreach(string s in strings)
            {
                if (strings.Length != i + 1)
                {
                    res += s + "\\";
                }
                else
                {
                    res += s;
                }

                i++;
            }
            return res;
        }

        public static string TakeNameFile(string filename)
        {
            string[] splitString = filename.Split('\\');
            string name = splitString.Last();

            return name;
        }
    }
}
