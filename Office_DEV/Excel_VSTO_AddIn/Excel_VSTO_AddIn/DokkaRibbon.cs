using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace Excel_VSTO_AddIn
{
    public partial class DokkaRibbon
    {
        public bool ControlKeyIsDown
        {
            get
            {
                return Control.ModifierKeys.HasFlag(Keys.Control);
            }
        }

        private void DokkaRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        // Send from Home Tab
        private void BtnSaveToDokka_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.SendFileToDokkaFromUI(ControlKeyIsDown);
        }

        // Send from File Tab
        private void BtnSaveToDokkaFromFile_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.SendFileToDokkaFromUI(ControlKeyIsDown);
        }
    }
}
