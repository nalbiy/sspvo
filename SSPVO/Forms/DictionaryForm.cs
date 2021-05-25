using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using SSPVO.Models.Dictionary;
using System.IO;
using System.Xml.Linq;
using SSPVO.Models.Service;

namespace SSPVO.Forms
{
    /// <summary>
    /// Справочники ССПВО
    /// </summary>
    public partial class DictionaryForm : Form
    {
        /// <summary>
        /// Список справочников
        /// </summary>
        List<Dictionary> list;

        /// <summary>
        /// Конструктор
        /// </summary>
        public DictionaryForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Начальная загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictionaryForm_Load(object sender, EventArgs e)
        {
            // получаем список все справочников
            list = Dictionary.getAll();
            // настраиваем dataGridView1
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.DataSource = list;
        }

        /// <summary>
        /// Закрыть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Загрузить все српвочники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntDownloadAllFiles_Click(object sender, EventArgs e)
        {
            int cnt_add = 0;
            int cnt_miss = 0;
            // перебираем список всех справочников
            foreach (Dictionary one in list)
            {
                // формируем путь к файлу текущего справочника
                string newFileName = Properties.Settings.Default.dictionaryDir + "\\" + one.Name + ".xml";
                // проверяем на судествование этого файла
                if (!File.Exists(newFileName))
                {
                    // загружаем справочник в виде xml из ССПВО
                    TransferResult result = TransferManager.PullDictionaryFromSSPVO(one.Name);
                    if (result.isOk)
                    {
                        // если все ок загрузилось, то сохраняем в файл
                        XDocument doc = XDocument.Parse(result.data);
                        doc.Save(newFileName);
                        cnt_add++;
                    }
                    else
                    {
                        MessageBox.Show("При загрузке справочника " + one.Name + " произошла ошибка!\n" + result.errorMessage);
                    }
                }
                else
                {
                    cnt_miss++;
                }
            }
            MessageBox.Show("Загрузка завершена!" + "\n cnt_add: " + cnt_add.ToString() + "\n cnt_miss: " + cnt_miss);
        }

        /// <summary>
        /// Открыть детали справочника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // определяем название справочника
                string name = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                string description = dataGridView1.CurrentRow.Cells[2].Value.ToString();

                // строим путь к файлу справочника
                string fileName = Properties.Settings.Default.dictionaryDir + "\\" + name + ".xml";

                // если файл существует вызываем форму для его отображения
                if (File.Exists(fileName))
                {
                    // создаем форму деталей справочника, настраиваем ее и вызываем
                    DictionaryItemForm f = new DictionaryItemForm();
                    f.fileName = fileName;
                    f.name = name;
                    f.description = description;
                    f.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Файл справочника не найден!");
                }
            }
        }


    }
}
