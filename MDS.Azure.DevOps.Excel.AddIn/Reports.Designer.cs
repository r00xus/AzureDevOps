namespace MDS.Azure.DevOps.Excel.AddIn
{
    partial class Reports : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Reports()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btnOpenSettings = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.checkBox1 = this.Factory.CreateRibbonCheckBox();
            this.checkBox2 = this.Factory.CreateRibbonCheckBox();
            this.checkBox3 = this.Factory.CreateRibbonCheckBox();
            this.checkBox4 = this.Factory.CreateRibbonCheckBox();
            this.btnExecute = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "Отчеты DevOps";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnOpenSettings);
            this.group1.Name = "group1";
            // 
            // btnOpenSettings
            // 
            this.btnOpenSettings.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnOpenSettings.Image = global::MDS.Azure.DevOps.Excel.AddIn.Properties.Resources.page_white_gear;
            this.btnOpenSettings.Label = "Открыть настройки";
            this.btnOpenSettings.Name = "btnOpenSettings";
            this.btnOpenSettings.ShowImage = true;
            // 
            // group2
            // 
            this.group2.Items.Add(this.checkBox1);
            this.group2.Items.Add(this.checkBox2);
            this.group2.Items.Add(this.checkBox3);
            this.group2.Items.Add(this.checkBox4);
            this.group2.Items.Add(this.btnExecute);
            this.group2.Label = "Отчет";
            this.group2.Name = "group2";
            // 
            // checkBox1
            // 
            this.checkBox1.Label = "Активности";
            this.checkBox1.Name = "checkBox1";
            // 
            // checkBox2
            // 
            this.checkBox2.Label = "Задачи";
            this.checkBox2.Name = "checkBox2";
            // 
            // checkBox3
            // 
            this.checkBox3.Label = "Отработанное время";
            this.checkBox3.Name = "checkBox3";
            // 
            // checkBox4
            // 
            this.checkBox4.Label = "Расхождения";
            this.checkBox4.Name = "checkBox4";
            // 
            // btnExecute
            // 
            this.btnExecute.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnExecute.Image = global::MDS.Azure.DevOps.Excel.AddIn.Properties.Resources.lightning;
            this.btnExecute.Label = "Выполнить";
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.ShowImage = true;
            this.btnExecute.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnExecute_Click);
            // 
            // Reports
            // 
            this.Name = "Reports";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Reports_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnOpenSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnExecute;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBox1;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBox2;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBox3;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBox4;
    }

    partial class ThisRibbonCollection
    {
        internal Reports Reports
        {
            get { return this.GetRibbon<Reports>(); }
        }
    }
}
