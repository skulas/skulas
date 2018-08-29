using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace dokka_excel_addin
{
    public partial class ThisAddIn
    {
        object _uploadInProgressLock = new object();
        bool _uploadInProgressFlag = false;

        int _clearStatusBarPID = 0;

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
            // File being uploaded, cannot change it while being uploaded.
            if (_uploadInProgressFlag)
            {
                Trace.WriteLine("Upload in progress aborted save file");
                ShowStatusMessage("File still being uploaded to Dokka. Cannot upload.");
                Cancel = true;
            }

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

                var name = Wb.Name;
                var newPath = $"{Wb.Path}\\dokkacopy___{name}";
                // Need to save a copy as cannot upload the file that is in use by excel.
                Wb.SaveCopyAs(newPath);

                await DokkaOfficeUploader.shared.UploadFileAtPath(newPath, name, (string message, string id) =>
                {
                    SetUploadFlag(false);
                    Trace.TraceInformation($"Upload ended: {message}");

                    if (String.IsNullOrEmpty(message))
                    {
                        ShowStatusMessage("Upload aborted without reason");
                    }
                    else
                    {
                        ShowStatusMessage(message);
                    }

                });
            });

            SetUploadFlag(true);
            task.Start();

            return task;
        }

        private void SetUploadFlag(bool state)
        {
            lock (_uploadInProgressLock)
            {
                _uploadInProgressFlag = state;
            }
        }
        
        private void ShowStatusMessage(string msg)
        {
            Application.StatusBar = msg;
            _clearStatusBarPID++;
            Task.Run(() =>
            {
                Task.Delay(10000).Wait();
                ClearStatus(_clearStatusBarPID);
            });
        }

        private void ClearStatus(int pid)
        {
            if (_clearStatusBarPID == pid)
            {
                Application.StatusBar = "";
            }
        }

        #endregion Dokka
    }
}
