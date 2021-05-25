using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace SSPVO.Models.Dictionary
{
    /// <summary>
    /// Справочники
    /// </summary>
    public class Dictionary
    {
        /// <summary>
        /// Имя файла с списком справочников
        /// </summary>
        static string fileBaseName = "list.xml";

        /// <summary>
        /// Путь к файлу с справочниками (относительно запускаемого файла)
        /// </summary>
        static string fileName = Properties.Settings.Default.dictionaryDir + "\\" + Dictionary.fileBaseName;

        /// <summary>
        /// Порядковый индекс справочника
        /// </summary>
        [XmlElement("Index")]
        public Int32 Index { get; set; }

        /// <summary>
        /// Название справочника
        /// </summary>
        [XmlElement("Name")]
        public String Name { get; set; }

        /// <summary>
        /// Описание справочника
        /// </summary>
        [XmlElement("Description")]
        public String Description { get; set; }

        /// <summary>
        /// Получить список справочников
        /// </summary>
        /// <returns>Возвращает полный список справочников</returns>
        public static List<Dictionary> getAll()
        {
            using (var reader = new StreamReader(Dictionary.fileName))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<Dictionary>), new XmlRootAttribute("DictionaryList"));
                return (List<Dictionary>)deserializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Получить один справочник по названию
        /// </summary>
        /// <param name="Название справочника"></param>
        /// <returns>Возвращается один справочник или null в случае, если не найдено по названию ничего</returns>
        public static Dictionary getOne(string name)
        {
            foreach (Dictionary one in Dictionary.getAll())
            {
                if (one.Name == name)
                {
                    return one;
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает один справочник в виде объекта по XML элементу
        /// </summary>
        /// <param name="dictionaryElement">XML элемент содержащий описание одного справочника</param>
        /// <returns>Возвращает один справочник в виде объекта по XML элементу</returns>
        private static Dictionary serializeOne(XElement dictionaryElement)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Dictionary), new XmlRootAttribute("Dictionary"));
            StringReader stringReader = new StringReader(dictionaryElement.ToString());
            return (Dictionary)serializer.Deserialize(stringReader);
        }

    }

}
