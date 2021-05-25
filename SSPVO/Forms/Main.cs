using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSPVO.Models.Service;
using Newtonsoft.Json;

namespace SSPVO.Forms
{
    /// <summary>
    /// Основная форма
    /// </summary>
    public partial class Main : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Начальная загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Закрыть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Справочники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDictionary_Click(object sender, EventArgs e)
        {
            DictionaryForm f = new DictionaryForm();
            f.Show();
        }

        /// <summary>
        /// Сформировать файл подписи для ЛК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getSaveSignToFile_Click(object sender, EventArgs e)
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp
            };
            string headerString = JsonConvert.SerializeObject(header);
            string payload = "";
            string jw = Cript.Base64.EncodeFromString(headerString) + "." + Cript.Base64.EncodeFromString(payload);
            Cript.CtiptManager.SignMsgToFile(jw, "sign.sig");
            MessageBox.Show("Файл успешно сформирован!");
        }

        /// <summary>
        /// Проверка привязки сертификата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCertCheck_Click(object sender, EventArgs e)
        {
            TransferResult result = TransferManager.CheckCertificate();
            if (result.isOk)
            {
                MessageBox.Show("yes");
                MessageBox.Show(result.data);
            }
            else
            {
                MessageBox.Show("no");
                MessageBox.Show(result.errorMessage);
            }
        }

        /// <summary>
        /// Очередь Service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnServiceApplication_Click(object sender, EventArgs e)
        {
            TransferResult result = TransferManager.ServiceResultInfo();
            if (result.isOk)
            {
                MessageBox.Show(result.data);

                // можно так обработать полученные idJwt
                dynamic obj = JsonConvert.DeserializeObject(result.data);
                if (obj["messages"] > 1)
                {
                    foreach (var item in obj["idJwts"])
                    {
                        long idJwt = (long)item;
                        // делаем что хотим с idJwt

                    }
                }
            }
            else
            {
                MessageBox.Show(result.errorMessage);
            }
        }

        /// <summary>
        /// Очередь EPGU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEPGUApplication_Click(object sender, EventArgs e)
        {
            TransferResult result = TransferManager.EpguResultInfo();
            if (result.isOk)
            {
                MessageBox.Show(result.data);
            }
            else
            {
                MessageBox.Show(result.errorMessage);
            }
        }

        /// <summary>
        /// Произвольный запрос методом new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomQuery_Click(object sender, EventArgs e)
        {
            SendCustomRequestForm f = new SendCustomRequestForm();
            f.Show();
        }

    }
}
