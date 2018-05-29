﻿using System;
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

        
        #region INIT

        public class Upload
        {
            public string Name { get; set; }
            public string NewName { get; set; }
        }

        private Upload _currentUpload = null;
        private const string DOKKA_PREFIX = "Dokka#managed#file";

        Microsoft.Office.Tools.CustomTaskPane _loginPane = null;
        UserControlLogin _loginCtrl = null;

        // NOTE: This regexp doesn't allow spaces in the filename. Fix needed if requirements allow space in filename.
        private string _filenameRegexPattern = $"^{DOKKA_PREFIX}_.*_{"[a-fA-F0-9]{10}"}_.*\\..*";

        private Dictionary<string, string> _accountsStub = new Dictionary<string, string>()
        {
            { "account 8", "company.email8@foo.com" },
            { "account 9", "company.email9@foo.com" }
        };
        
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

        #endregion INIT



        public async void SendFileToDokka(bool resetToken = false)
        {
            Microsoft.Office.Interop.Excel.Workbook Wb = Application.ActiveWorkbook;
            string tempPath = null;
            string filename = null;

            if (resetToken)
            {
                // Reset the login token.
                Trace.WriteLine("Resetting Login Token");
                ServerInterface.Instance.ResetLoginToken();
            }

            if (String.IsNullOrEmpty(Wb.Path))
            {
                // Sending a file that hasn't been saved yet to local storage.
                tempPath = System.IO.Path.GetTempPath();
                //string[] parts = Wb.FullName.Split(".".ToCharArray());
                string namepart = "noname";
                string extension = @"xlsx"; 
                string formatStr = Wb.FileFormat.ToString();
                if (!String.IsNullOrEmpty(Wb.Name))
                {
                    //extension = parts[1];
                    namepart = Wb.Name;
                }

                filename = $"{namepart}_{DateTime.Now.ToString("yyyy_MMM_dd__HH_mm")}.{extension}";
                tempPath = $"{tempPath}{filename}";
            } else
            {
                // file is in HD, don't upload if it's a dokka doc
                if (IsDokkaManagedFilename(Wb.Name))
                {
                    Trace.WriteLine($"The file {Wb.Name} is already managed by dokka, no need to upload it again");
                    Application.StatusBar = "This file is managed by Dokka. No need to upload it.";
                    return;
                }
            }

            var uploadTask = UploadFile(Wb, tempPath, false, filename);
            if (uploadTask != null)
            {
                uploadTask.Start();
                await uploadTask;
            } else
            {
                Trace.WriteLine("Upload Aborted ....");
                Application.StatusBar = "Cannot upload file to Dokka";
            }
        }

        private void Application_WorkbookBeforeSave(Microsoft.Office.Interop.Excel.Workbook Wb, bool SaveAsUI, ref bool Cancel)
        {
            // NOTE: Uncomment these lines to debug on the body of the excel file.
            //Excel.Worksheet activeWorksheet = ((Excel.Worksheet)Application.ActiveSheet);
            //Excel.Range firstRow = activeWorksheet.get_Range("A1");
            //firstRow.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
            //Excel.Range newFirstRow = activeWorksheet.get_Range("A1");
            //newFirstRow.Value2 = "Uploading the file";

            var uploadTask = UploadFile(Wb, null, true);
            if (uploadTask != null)
            {
                uploadTask.Start();
            } else
            {
                Trace.WriteLine("NOT UPLOADING THE FILE ..");
            }

            Trace.WriteLine("SAVING FILE TO DISK by user save command.");
            try
            {
                Wb.Save();
            } catch (Exception e)
            {
                Trace.WriteLine($"Error while saving, probably the user clicked cancel when saing a new file. Error: {e.Message}\n{e.InnerException?.Message ?? ""}");
            }
            
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

        private Task UploadFile(Microsoft.Office.Interop.Excel.Workbook Wb, 
            string tempFilePath = null, 
            bool validateDokkaSignature = false,
            string fileName = null)
        {
            Trace.WriteLine(" === Upload file process triggered === ");

            var currPath = Wb.Path;
            if (String.IsNullOrEmpty(currPath) && String.IsNullOrEmpty(tempFilePath))
            {
                Trace.WriteLine("Ignoring save of new file");

                return null;
            }

            if (validateDokkaSignature && !IsDokkaManagedFilename(Wb.Name))
            {
                Trace.WriteLine($"Filename {Wb.Name} is not recognized as a Dokka managed filename");

                return null;
            }

            var task = new Task(async () =>
            {
                Task.Delay(2000).Wait(); /// let save finish.

                var wbPath = Wb.Path;
                var name = Wb.Name;
                var newName = tempFilePath??$"{wbPath}\\dokkacopy___{name}";
                Wb.SaveCopyAs(newName);

                try
                {
                    await SendFileToServer(newName, fileName??name);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Failure attempting to upload file: {ex.Message}\n{ex.InnerException?.Message ?? ""}");
                    Application.StatusBar = "The file couldn't be uploaded to Dokka";
                }

            });

            return task;
        }

        private Task SendFileToServer(string newName, string name)
        {
            Task result;
            if (_currentUpload == null)
            {
                _currentUpload = new Upload()
                {
                    Name = name,
                    NewName = newName
                };

                result = ServerInterface.Instance.UploadFileAtPath(newName, name, ShowLoginWrapper, UploadResultHandler);
            }
            else
            {
                Trace.WriteLine("UPLOAD ALREADY IN PROGRESS: Abording requested upload.");
                result = new Task(() => { });
            }

            return result;
        }

        private async void LoginActionCallback(string username, string password, int companyIx)
        {
            var selectedKey = _accountsStub.Keys.ElementAt(companyIx);
            var companyEmail = _accountsStub[selectedKey];
            await ServerInterface.Instance.LoginWithCredentials(username, password, companyEmail, (succss, responseStr) =>
            {
                if (succss)
                {
                    Trace.WriteLine("Successfull login");
                    this.Application.StatusBar = "You are now logged in to Dokka";

                    HideLogin();
                    if (!String.IsNullOrEmpty(responseStr))
                    {
                        Properties.Settings.Default.loginToken = responseStr;

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
                    Application.StatusBar = "Login to Dokka failed.";
                    ShowLogin(responseStr??"Login Failure. Please try again");
                }
            });
        }

        private void UploadResultHandler(string result, bool loginRequired)
        {
            Trace.WriteLine($"Upload ended: {result}");

            if (loginRequired)
            {
                ShowLogin("Upload Failed, login is required before you can upload a file.");
            } else
            {
                Trace.WriteLine("CLEARING CURRENT UPLOAD");
                if (result != null)
                {
                    Application.StatusBar = result;
                }

                if (_currentUpload != null)
                {
                    var copyFilePath = _currentUpload.NewName;
                    try
                    {
                        System.IO.File.Delete(copyFilePath);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine($"Failure deleting file {copyFilePath}.\n{e.Message}\n{e.InnerException?.Message ?? ""}");
                    }
                    _currentUpload = null;
                } else
                {
                    Trace.WriteLine("WARNING: ** Current upload was NULL **");
                }
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
                    _loginCtrl.LoginStatusMessage = message;
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

            Trace.WriteLine("Showing Login UI");
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
            _loginCtrl = new UserControlLogin(LoginActionCallback)
            {
                AccountsList = _accountsStub.Keys.ToArray()
            };
            _loginPane = this.CustomTaskPanes.Add(_loginCtrl, "Login To Dokka");
            _loginPane.Visible = true;
            Trace.WriteLine("Creating Login UI");
        }

        private bool IsDokkaManagedFilename(string filename)
        {
            var isDokkaFilename = Regex.IsMatch(filename, _filenameRegexPattern);

            return isDokkaFilename;
        }

        #endregion Utils and Logic
    }
}