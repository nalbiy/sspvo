using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSPVO.Forms
{
    /// <summary>
    /// Форма просмотра деталей справочника
    /// </summary>
    public partial class DictionaryItemForm : Form
    {
        /// <summary>
        /// Название справочника
        /// </summary>
        public string name;

        /// <summary>
        /// Описание справочника
        /// </summary>
        public string description;

        /// <summary>
        /// Путь к файлу справочника
        /// </summary>
        public string fileName;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public DictionaryItemForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Начальная загрузка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictionaryItemForm_Load(object sender, EventArgs e)
        {
            this.Text = "Детали справочника " + this.name;
            this.richTextBox1.Text = this.description;
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(fileName);
            dataGridView1.DataSource = dataSet.Tables[0];
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
    }
}
