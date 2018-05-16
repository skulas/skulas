using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

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
            if (String.IsNullOrEmpty(currPath)) return;
            var task = new Task(async () =>
            {
                Task.Delay(2000).Wait();
                newFirstRow.Value2 = "Uploading the file";

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();

                var wbPath = Wb.Path;
                var wbFulName = Wb.FullName;
                var name = Wb.Name;
                var newName = wbPath + "\\dokkacopy___" + name;
                Wb.SaveCopyAs(newName);
                //form.Add(new StringContent(username), "username");
                //form.Add(new StringContent(useremail), "email");
                //form.Add(new StringContent(password), "password");
                var fileBytes = File.ReadAllBytes(newName);
                form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "office_file", name);
                HttpResponseMessage response = await httpClient.PostAsync("https://localhost:44300/api/values", form);
                var content = response.Content;

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
