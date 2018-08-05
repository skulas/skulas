using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dokka_excel_addin
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Application.WorkbookBeforeSave += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeSaveEventHandler(Application_WorkbookBeforeSave);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
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



        #region Dokka

        private void Application_WorkbookBeforeSave(Microsoft.Office.Interop.Excel.Workbook Wb, bool SaveAsUI, ref bool Cancel)
        {
            // File is only in memory (New file, never saved)
            if (String.IsNullOrEmpty(Wb.Path))
            {
                // File is not managed by Dokka.
                Trace.WriteLine("Ignoring save of new file");

                return;
            }

            // Filename not regognized as Dokka mananged file.
            if (!DokkaConfig.shared.IsDokkaManagedFilename(Wb.Name))
            {
                Trace.WriteLine($"Filename {Wb.Name} is not recognized as a Dokka managed filename");

                return;
            }

            UploadWbToDokka(Wb);
        }

        private Task UploadWbToDokka(Microsoft.Office.Interop.Excel.Workbook Wb)
        {
            var task = new Task(async () =>
            {
                Task.Delay(2000).Wait(); /// let save finish.

                var wbPath = Wb.FullName;
                var name = Wb.Name;

                await DokkaOfficeUploader.shared.UploadFileAtPath(wbPath, name, (string message, string id) =>
                {
                    Trace.TraceInformation($"Upload ended: {message}");

                    if (String.IsNullOrEmpty(message))
                    {
                        Application.StatusBar = "Upload aborted without reason";
                    }
                    else
                    {
                        Application.StatusBar = message;
                    }

                });
            });

            task.Start();

            return task;
        }

        #endregion Dokka
    }
}
