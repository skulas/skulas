using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Threading.Tasks;

// https://msdn.microsoft.com/en-us/library/cc668205.aspx

namespace Excel_VSTO_AddIn
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.WorkbookBeforeSave += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeSaveEventHandler(Application_WorkbookBeforeSave);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void Application_WorkbookBeforeSave(Microsoft.Office.Interop.Excel.Workbook Wb, bool SaveAsUI, ref bool Cancel)
        {
            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
            Excel.Range firstRow = activeWorksheet.get_Range("A1");
            firstRow.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
            Excel.Range newFirstRow = activeWorksheet.get_Range("A1");
            var currPath = Wb.Path;
            if (String.IsNullOrEmpty(currPath))
            {
                var login = new UserControlLogin();
                var loginCustomPane = this.CustomTaskPanes.Add(login, "Login To Dokka");
                loginCustomPane.Visible = true;
                return;
            }
            var task = new Task(async () =>
            {
                Task.Delay(2000).Wait();
                newFirstRow.Value2 = "Uploading the file";
 
                var wbPath = Wb.Path;
                var wbFulName = Wb.FullName;
                var name = Wb.Name;
                var newName = wbPath + "\\dokkacopy___" + name;
                Wb.SaveCopyAs(newName);
                //form.Add(new StringContent(username), "username");
                //form.Add(new StringContent(useremail), "email");
                //form.Add(new StringContent(password), "password");

                await ServerInterface.Instance.UploadFileAtPath(newName, name);

                // Delete the file some day, should add callback to upload for that.
            });
            task.Start();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
