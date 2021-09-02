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

namespace VegetablePatch
{
    public class BussinesLogic
    {
        private static string nameListDocs = "docs.xml";

        public static List<string> GetListDocs()
        {
            var list = new List<string>();
            try
            {
                if (!File.Exists(nameListDocs))
                {
                    return null;
                }

                XDocument xDocument = XDocument.Load(nameListDocs);
                var xElements = xDocument.Root.Elements("Doc").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(elem.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public static void AddFile(string fileName)
        {
            try
            {
                string shortName = TakeNameFile(fileName);
                if (!File.Exists(nameListDocs))
                {
                    var xElement = new XElement("Docs");
                    xElement.Add(new XElement("Doc", shortName));
                    XDocument xDocument = new XDocument(xElement);
                    xDocument.Save(nameListDocs);
                }
                else
                {
                    bool repeatName = false;
                    XDocument xDocument = XDocument.Load(nameListDocs);
                    var xElements = xDocument.Root.Elements("Doc").ToList();
                    foreach (var elem in xElements)
                    {
                        if (elem.Value.Equals(shortName))
                        {
                            repeatName = true;
                        }
                    }
                    if (!repeatName)
                    {
                        XElement xElement = new XElement("Doc", shortName);
                        xDocument.Root.Add(xElement);
                        xDocument.Save(nameListDocs);
                    }
                }
                File.Copy(fileName, shortName, true);
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

        public static string TakeNameFile(string filename)
        {
            string[] splitString = filename.Split('\\');
            string name = splitString.Last();

            return name;
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
    }
}
