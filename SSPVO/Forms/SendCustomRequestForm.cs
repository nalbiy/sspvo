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

namespace SSPVO.Forms
{
    /// <summary>
    /// Форма отправки произвольного запроса в ССПВО методом new
    /// </summary>
    public partial class SendCustomRequestForm : Form
    {
        /// <summary>
        /// Список сущностей
        /// </summary>
        List<string> entityList = new List<string>();

        /// <summary>
        /// Список типов запроса
        /// </summary>
        List<string> requestTypeList = new List<string>();

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public SendCustomRequestForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Начальная загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendCustomRequestForm_Load(object sender, EventArgs e)
        {
            // заполняем список сущностей
            entityList.Add("subdivisionOrg");
            entityList.Add("campaign");
            entityList.Add("achievement");
            entityList.Add("admissionVolume");
            entityList.Add("distributedAdmissionVolume");
            entityList.Add("competitiveGroup");
            entityList.Add("competitiveGroupProgram");
            entityList.Add("competitiveBenefit");
            entityList.Add("entranceTest");
            entityList.Add("entranceTestBenefit");
            entityList.Add("entranceTestLocation");
            entityList.Add("rntranceTestSheet");
            entityList.Add("rntrant");
            entityList.Add("identification");
            entityList.Add("document");
            entityList.Add("applicationList");
            entityList.Add("editApplicationStatusList");
            entityList.Add("entranceTestAgreedList");
            entityList.Add("entranceTestResultList");
            entityList.Add("erderAdmissionList");
            entityList.Add("eompetitiveGroupsApplicationsRating");
            entityList.Add("appAchievements");
            entityList.Add("sentToEpguEtc");

            // заполняем список типов запроса
            requestTypeList.Add("add");
            requestTypeList.Add("edit");
            requestTypeList.Add("remove");
            requestTypeList.Add("get");

            // устанавливаем источники данных для комбиков
            cmbEntity.DataSource = entityList;
            cmbRequestType.DataSource = requestTypeList;
        }

        /// <summary>
        /// Закрыть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// тправить запрос в ССПВО методом new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            TransferResult result = TransferManager.New((NewAction)Enum.Parse(typeof(NewAction), cmbRequestType.Text.Trim(), true), cmbEntity.Text.Trim(), txtPayload.Text.Trim(), "", "", 0, "", false);
            if (result.isOk)
            {
                txtRequestId.Text = result.getIdJwt().ToString();
                MessageBox.Show(result.data);
            }
            else
            {
                MessageBox.Show(result.errorMessage);
            }
        }

        /// <summary>
        /// Получить результат (если в txtIdJwt есть номер пакета)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetResult_Click(object sender, EventArgs e)
        {
            long idJwt = Convert.ToInt64(txtRequestId.Text);
            if (idJwt == 0)
            {
                MessageBox.Show("Нет запроса для получения результата!");
                return;
            }
            showResult(TransferManager.ServiceResultInfoByIdJwt(idJwt));
        }

        /// <summary>
        /// Показать результат запроса
        /// </summary>
        private void showResult(TransferResult result)
        {
            if (result.isOk)
            {
                MessageBox.Show(result.data);
                if (result.serviceInfo != null)
                {
                    MessageBox.Show(result.serviceInfo.header);
                    MessageBox.Show(result.serviceInfo.payload);
                }
            }
            else
            {
                MessageBox.Show(result.errorMessage);
            }
        }
    }
}
