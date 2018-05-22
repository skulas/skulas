using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace Excel_VSTO_AddIn
{
    public partial class DokkaRibbon
    {
        private void DokkaRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void BtnSaveToDokka_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.SaveFileToDokka();
        }

        private void BtnSaveToDokkaFromFile_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.SaveFileToDokka();
        }
    }
}
