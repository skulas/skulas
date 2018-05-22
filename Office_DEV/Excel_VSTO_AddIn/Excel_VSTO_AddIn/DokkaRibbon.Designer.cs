namespace Excel_VSTO_AddIn
{
    partial class DokkaRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public DokkaRibbon()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DokkaRibbon));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btnSaveToDokka = this.Factory.CreateRibbonButton();
            this.btnSaveToDokkaFromFile = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnSaveToDokka);
            this.group1.Label = "Upploads";
            this.group1.Name = "group1";
            // 
            // btnSaveToDokka
            // 
            this.btnSaveToDokka.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSaveToDokka.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveToDokka.Image")));
            this.btnSaveToDokka.Label = "Save to Dokka";
            this.btnSaveToDokka.Name = "btnSaveToDokka";
            this.btnSaveToDokka.ShowImage = true;
            this.btnSaveToDokka.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSaveToDokka_Click);
            // 
            // btnSaveToDokkaFromFile
            // 
            this.btnSaveToDokkaFromFile.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveToDokkaFromFile.Image")));
            this.btnSaveToDokkaFromFile.ImageName = "Save to Dokka";
            this.btnSaveToDokkaFromFile.Label = "Save to Dokka";
            this.btnSaveToDokkaFromFile.Name = "btnSaveToDokkaFromFile";
            this.btnSaveToDokkaFromFile.ShowImage = true;
            this.btnSaveToDokkaFromFile.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSaveToDokkaFromFile_Click);
            // 
            // DokkaRibbon
            // 
            this.Name = "DokkaRibbon";
            // 
            // DokkaRibbon.OfficeMenu
            // 
            this.OfficeMenu.Items.Add(this.btnSaveToDokkaFromFile);
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.DokkaRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSaveToDokka;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSaveToDokkaFromFile;
    }

    partial class ThisRibbonCollection
    {
        internal DokkaRibbon DokkaRibbon
        {
            get { return this.GetRibbon<DokkaRibbon>(); }
        }
    }
}
