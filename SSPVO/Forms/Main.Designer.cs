namespace SSPVO.Forms
{
    partial class Main
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEPGUApplication = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCustomQuery = new System.Windows.Forms.Button();
            this.btnCertCheck = new System.Windows.Forms.Button();
            this.btnDictionary = new System.Windows.Forms.Button();
            this.btnServiceApplication = new System.Windows.Forms.Button();
            this.getSaveSignToFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEPGUApplication
            // 
            this.btnEPGUApplication.Location = new System.Drawing.Point(12, 128);
            this.btnEPGUApplication.Name = "btnEPGUApplication";
            this.btnEPGUApplication.Size = new System.Drawing.Size(338, 23);
            this.btnEPGUApplication.TabIndex = 10;
            this.btnEPGUApplication.Text = "Очередь EPGU";
            this.btnEPGUApplication.UseVisualStyleBackColor = true;
            this.btnEPGUApplication.Click += new System.EventHandler(this.btnEPGUApplication_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(12, 297);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(338, 23);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCustomQuery
            // 
            this.btnCustomQuery.Location = new System.Drawing.Point(12, 157);
            this.btnCustomQuery.Name = "btnCustomQuery";
            this.btnCustomQuery.Size = new System.Drawing.Size(338, 23);
            this.btnCustomQuery.TabIndex = 9;
            this.btnCustomQuery.Text = "Произвольный запрос методом New";
            this.btnCustomQuery.UseVisualStyleBackColor = true;
            this.btnCustomQuery.Click += new System.EventHandler(this.btnCustomQuery_Click);
            // 
            // btnCertCheck
            // 
            this.btnCertCheck.Location = new System.Drawing.Point(12, 70);
            this.btnCertCheck.Name = "btnCertCheck";
            this.btnCertCheck.Size = new System.Drawing.Size(338, 23);
            this.btnCertCheck.TabIndex = 1;
            this.btnCertCheck.Text = "Проверка привязки сетрификата";
            this.btnCertCheck.UseVisualStyleBackColor = true;
            this.btnCertCheck.Click += new System.EventHandler(this.btnCertCheck_Click);
            // 
            // btnDictionary
            // 
            this.btnDictionary.Location = new System.Drawing.Point(12, 12);
            this.btnDictionary.Name = "btnDictionary";
            this.btnDictionary.Size = new System.Drawing.Size(338, 23);
            this.btnDictionary.TabIndex = 0;
            this.btnDictionary.Text = "Справочники";
            this.btnDictionary.UseVisualStyleBackColor = true;
            this.btnDictionary.Click += new System.EventHandler(this.btnDictionary_Click);
            // 
            // btnServiceApplication
            // 
            this.btnServiceApplication.Location = new System.Drawing.Point(12, 99);
            this.btnServiceApplication.Name = "btnServiceApplication";
            this.btnServiceApplication.Size = new System.Drawing.Size(338, 23);
            this.btnServiceApplication.TabIndex = 16;
            this.btnServiceApplication.Text = "Очередь Service";
            this.btnServiceApplication.UseVisualStyleBackColor = true;
            this.btnServiceApplication.Click += new System.EventHandler(this.btnServiceApplication_Click);
            // 
            // getSaveSignToFile
            // 
            this.getSaveSignToFile.Location = new System.Drawing.Point(12, 41);
            this.getSaveSignToFile.Name = "getSaveSignToFile";
            this.getSaveSignToFile.Size = new System.Drawing.Size(338, 23);
            this.getSaveSignToFile.TabIndex = 19;
            this.getSaveSignToFile.Text = "Сформировать файл подписи для ЛК";
            this.getSaveSignToFile.UseVisualStyleBackColor = true;
            this.getSaveSignToFile.Click += new System.EventHandler(this.getSaveSignToFile_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 341);
            this.Controls.Add(this.getSaveSignToFile);
            this.Controls.Add(this.btnServiceApplication);
            this.Controls.Add(this.btnEPGUApplication);
            this.Controls.Add(this.btnCustomQuery);
            this.Controls.Add(this.btnCertCheck);
            this.Controls.Add(this.btnDictionary);
            this.Controls.Add(this.btnExit);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Интеграция с ССПВО";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEPGUApplication;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCustomQuery;
        private System.Windows.Forms.Button btnCertCheck;
        private System.Windows.Forms.Button btnDictionary;
        private System.Windows.Forms.Button btnServiceApplication;
        private System.Windows.Forms.Button getSaveSignToFile;
    }
}

