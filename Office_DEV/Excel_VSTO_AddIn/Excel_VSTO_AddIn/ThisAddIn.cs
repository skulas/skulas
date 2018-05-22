using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

// https://msdn.microsoft.com/en-us/library/cc668205.aspx

namespace Excel_VSTO_AddIn
{
    public partial class ThisAddIn
    {
        public class UploadProgress
        {
            public string Name { get; set; }
            public string NewName { get; set; }
        }

        private UploadProgress _currentUpload = null;
        private const string DOKKA_PREFIX = "Dokka#managed#file";
        //private const string FILENAME_FREE_PART = "FREE_STRING"; // Any string
        //private const string CUSTOMER_REF_PART = "0000000000"; // 10 hexadecimal digits
        //private const string FILE_EXTNESION_PART = "FILE_EXTENSION";
        //private const string FILE_CONFIG_PART = "FILE_CONFIG_PARRT"; // Any string
        //private string FILENAME_PROTO = $"{DOKKA_PREFIX}_{FILENAME_FREE_PART}_{CUSTOMER_REF_PART}_{FILE_CONFIG_PART}.{FILE_EXTNESION_PART}";
        Microsoft.Office.Tools.CustomTaskPane _loginPane = null;
        UserControlLogin _loginCtrl = null;
        // NOTE: This regexp doesn't allow spaces in the filename. Fix needed if requirements allow space in filename.
        private string _filenameRegexPattern = $"{DOKKA_PREFIX}_.*_{"[a-fA-F0-9]{10}"}_.*\\..*";
        
        Thread _mainThread = Thread.CurrentThread;

        public SynchronizationContext TheWindowsFormsSynchronizationContext { get; private set; }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            TheWindowsFormsSynchronizationContext = WindowsFormsSynchronizationContext.Current
                                           ?? new WindowsFormsSynchronizationContext();
            Application.WorkbookBeforeSave += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeSaveEventHandler(Application_WorkbookBeforeSave);
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
                Trace.WriteLine("Ignoring save of new file");

                return;
            }

            if (!IsDokkaManagedFilename(Wb.Name))
            {
                Trace.WriteLine($"Filename {Wb.Name} is not recognized as a Dokka managed filename");

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

                try
                {
                    await SendFileToServer(newName, name);
                } catch (Exception ex)
                {
                    Trace.WriteLine($"Failure attempting to upload file: {ex.Message}\n{ex.InnerException?.Message??""}");
                }
                

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



        #region Utils and Logic

        private Task SendFileToServer(string newName, string name)
        {
            Task result;
            if (_currentUpload == null)
            {
                _currentUpload = new UploadProgress()
                {
                    Name = name,
                    NewName = newName
                };

                result = ServerInterface.Instance.UploadFileAtPath(newName, name, ShowLoginWrapper, UploadResultHandler);
            }
            else
            {
                Trace.WriteLine("UPLOAD ALREADY IN PROGRESS: Abording requested upload");
                result = new Task(() => { });
            }

            return result;
        }

        private async void LoginActionCallback(string username, string password)
        {

            await ServerInterface.Instance.LoginWithCredentials(username, password, (succss, loginTokenStr) =>
            {
                if (succss)
                {
                    Trace.WriteLine("Successfull login");
                    HideLogin();
                    if (!String.IsNullOrEmpty(loginTokenStr))
                    {
                        Properties.Settings.Default.loginToken = loginTokenStr;

                        if (_currentUpload != null)
                        {
                            Trace.WriteLine("Upload detected, resuming upload after successful login");
                            var name = _currentUpload.Name;
                            var newName = _currentUpload.NewName;
                            _currentUpload = null;
                            var sendTask = SendFileToServer(newName, name);
                            sendTask.Wait();
                        }
                    }
                }
                else
                {
                    Trace.WriteLine("Login failed");
                    ShowLogin("Login Failure. Please try again");
                }
            });
        }

        private void UploadResultHandler(string result, bool loginRequired)
        {
            Trace.WriteLine($"Upload ended: {result}");

            if (loginRequired)
            {
                ShowLogin("Upload Failed, login is required");
            } else
            {
                Trace.WriteLine("CLEARING CURRENT UPLOAD");
                _currentUpload = null;
            }
        }

        private void ShowLoginWrapper(string message)
        {
            // When invoked as a delegate function, use this wrapper for future changes.
            ShowLogin(message);
        }

        private void ShowLogin(string message = "")
        {
            if (_loginPane == null)
            {
                TheWindowsFormsSynchronizationContext.Send(d =>
                {
                    CreateLogin();
                }, null);
            }
            else
            {
                TheWindowsFormsSynchronizationContext.Send(d =>
                {
                    _loginCtrl.LoginStatusMessage = message;
                    _loginPane.Visible = true;
                }, null);
            }
        }

        private void HideLogin()
        {
            TheWindowsFormsSynchronizationContext.Send(d =>
            {
                _loginCtrl.LoginStatusMessage = "";
                _loginPane.Visible = false;
            }, null);
        }

        private void CreateLogin()
        {
            _loginCtrl = new UserControlLogin(LoginActionCallback);
            _loginPane = this.CustomTaskPanes.Add(_loginCtrl, "Login To Dokka");
            _loginPane.Visible = true;
        }

        private bool IsDokkaManagedFilename(string filename)
        {
            var isDokkaFilename = Regex.IsMatch(filename, _filenameRegexPattern);

            return isDokkaFilename;
        }

        #endregion Utils and Logic
    }
}
