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
            this.Dokka = this.Factory.CreateRibbonTab();
            this.GroupDokka = this.Factory.CreateRibbonGroup();
            this.btnSaveToDokkaFromFile = this.Factory.CreateRibbonButton();
            this.btnSaveToDokka = this.Factory.CreateRibbonButton();
            this.Dokka.SuspendLayout();
            this.GroupDokka.SuspendLayout();
            this.SuspendLayout();
            // 
            // Dokka
            // 
            this.Dokka.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.Dokka.ControlId.OfficeId = "TabHome";
            this.Dokka.Groups.Add(this.GroupDokka);
            this.Dokka.Label = "TabHome";
            this.Dokka.Name = "Dokka";
            // 
            // GroupDokka
            // 
            this.GroupDokka.Items.Add(this.btnSaveToDokka);
            this.GroupDokka.Label = "Dokka";
            this.GroupDokka.Name = "GroupDokka";
            this.GroupDokka.Position = this.Factory.RibbonPosition.AfterOfficeId("GroupFont");
            // 
            // btnSaveToDokkaFromFile
            // 
            this.btnSaveToDokkaFromFile.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveToDokkaFromFile.Image")));
            this.btnSaveToDokkaFromFile.ImageName = "Save to Dokka";
            this.btnSaveToDokkaFromFile.Label = "Save to Dokka";
            this.btnSaveToDokkaFromFile.Name = "btnSaveToDokkaFromFile";
            this.btnSaveToDokkaFromFile.Position = this.Factory.RibbonPosition.AfterOfficeId("FileSaveToDocumentManagementServer");
            this.btnSaveToDokkaFromFile.ScreenTip = "Upload File to Dokka";
            this.btnSaveToDokkaFromFile.ShowImage = true;
            this.btnSaveToDokkaFromFile.SuperTip = "Sends the file to Dokka, even if it wasn\'t saved locally.";
            this.btnSaveToDokkaFromFile.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSaveToDokkaFromFile_Click);
            // 
            // btnSaveToDokka
            // 
            this.btnSaveToDokka.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSaveToDokka.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveToDokka.Image")));
            this.btnSaveToDokka.Label = "Upload";
            this.btnSaveToDokka.Name = "btnSaveToDokka";
            this.btnSaveToDokka.ScreenTip = "Send File to Dokka";
            this.btnSaveToDokka.ShowImage = true;
            this.btnSaveToDokka.SuperTip = "Sends the current file to dokka, event if wasn\'t saved locally.";
            this.btnSaveToDokka.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSaveToDokka_Click);
            // 
            // DokkaRibbon
            // 
            this.Name = "DokkaRibbon";
            // 
            // DokkaRibbon.OfficeMenu
            // 
            this.OfficeMenu.Items.Add(this.btnSaveToDokkaFromFile);
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.Dokka);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.DokkaRibbon_Load);
            this.Dokka.ResumeLayout(false);
            this.Dokka.PerformLayout();
            this.GroupDokka.ResumeLayout(false);
            this.GroupDokka.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab Dokka;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupDokka;
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
