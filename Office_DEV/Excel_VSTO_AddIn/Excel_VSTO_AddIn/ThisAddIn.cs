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

        
        #region INIT

        public class Upload
        {
            public string Name { get; set; }
            public string NewName { get; set; }
            public Microsoft.Office.Interop.Excel.Workbook Wb { get; set; }
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



        /// <summary>
        /// Initiate the upload from the UI and not by the Save command hook.
        /// </summary>
        /// <param name="resetToken">If true, the system forgets about the login token supplied by the server. Will prompt new login.</param>
        /// <remarks>
        /// When the file is being directly uploaded from RAM to Dokka, a temporary path is used instead the filename as it is saved on disk.
        /// </remarks>
        public async void SendFileToDokkaFromUI(bool resetToken = false)
        {
            Microsoft.Office.Interop.Excel.Workbook Wb = (Microsoft.Office.Interop.Excel.Workbook)Application.ActiveWorkbook;
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

                // Set the document ID to some default value so we know it's been uploaded to Dokka Before being saved.
                WbCustomPropsMngr.Instance.SetDocumentIdForWb(Wb, WbCustomPropsMngr.DOKKA_DEFAULT_DOCUMENT_ID);

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
                // file is in HD, don't upload if it's a dokka doc.
                // If it's a dokka managed document, it is uploaded uppon save.
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

            if (WbCustomPropsMngr.Instance.GetDoucmentIdForWb(Wb) == null)
            {
                // File is not managed by dokka
                // Regular save behaviour.
                WriteWbToDisk(Wb);
            }
            else
            {
                if (String.IsNullOrEmpty(Wb.Path)) {
                    // File hasn't been saved to disk, but it is managed by dokka.
                    // Emulate the 'Upload to Dokka' Ribbon button.
                    Trace.TraceInformation("File already managed by dokka, won't be saved locally");
                    SendFileToDokkaFromUI();
                } else
                {
                    // File already on disk and not managed by dokka, Vanilla, Save to disk
                    WriteWbToDisk(Wb);
                }
            }
        }

        private void WriteWbToDisk(Microsoft.Office.Interop.Excel.Workbook Wb)
        {
            Trace.WriteLine("SAVING FILE TO DISK by user save command.");
            try
            {
                Wb.Save();
            }
            catch (Exception e)
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

            if (String.IsNullOrEmpty(Wb.Path) && String.IsNullOrEmpty(tempFilePath))
            {
                // File is only in memory (New file, never saved)
                if (WbCustomPropsMngr.Instance.GetDoucmentIdForWb(Wb) == null)
                {
                    // File is not managed by Dokka.
                    Trace.WriteLine("Ignoring save of new file");

                    return null;
                }
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
                    await SendFileToServer(Wb, newName, fileName??name);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Failure attempting to upload file: {ex.Message}\n{ex.InnerException?.Message ?? ""}");
                    Application.StatusBar = "The file couldn't be uploaded to Dokka";
                }

            });

            return task;
        }

        /// <summary>
        /// Send the file to dokka async.
        /// </summary>
        /// <param name="Wb">The currently uploaded Workbook, not the copy uploaded</param>
        /// <param name="newName">The full path to the copy saved for upload purposes</param>
        /// <param name="name">The name of the file (no path) to be sent to the server in the file_name field..</param>
        /// <returns>A Task uplpading the file. This task should be awaited / executed in order the upload to happen.</returns>
        private Task SendFileToServer(Microsoft.Office.Interop.Excel.Workbook Wb, string newName, string name)
        {
            Task result;
            if (_currentUpload == null)
            {
                _currentUpload = new Upload()
                {
                    Name = name,
                    NewName = newName,
                    Wb = Wb
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

                    // Keep persistent login details
                    DokkaOfficeAddinConfiguration.Instance.SaveLoginDetails(username, password);

                    HideLogin();
                    if (!String.IsNullOrEmpty(responseStr))
                    {
                        DokkaOfficeAddinConfiguration.Instance.LoginToken = responseStr;

                        if (_currentUpload != null)
                        {
                            Trace.WriteLine("Upload detected, resuming upload after successful login");
                            var name = _currentUpload.Name;
                            var newName = _currentUpload.NewName;
                            var wb = _currentUpload.Wb;
                            _currentUpload = null;
                            var sendTask = SendFileToServer(wb, newName, name);
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

        private void UploadResultHandler(string result, bool loginRequired, string dokkaDocId)
        {
            Trace.TraceInformation($"Upload ended: {result}");

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
                    // Delete temp copy
                    var copyFilePath = _currentUpload.NewName;
                    try
                    {
                        System.IO.File.Delete(copyFilePath);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError($"Failure deleting file {copyFilePath}.\n{e.Message}\n{e.InnerException?.Message ?? ""}");
                    }

                    // Update Doc ID from dokka response
                    if (dokkaDocId != null)
                    {
                        var wb = _currentUpload.Wb;
                        var currDocId = WbCustomPropsMngr.Instance.GetDoucmentIdForWb(wb);
                        if (String.IsNullOrEmpty(currDocId))
                        {
                            Trace.TraceInformation("Current Wb doc ID property is empty");
                        } else if (currDocId.Equals(dokkaDocId))
                        {
                            Trace.TraceInformation("Document ID is in sync with Dokka cloud");
                        } else if (currDocId.Equals(WbCustomPropsMngr.DOKKA_DEFAULT_DOCUMENT_ID))
                        {
                            Trace.TraceInformation("Updating Document ID to dokka supplied ID");
                            WbCustomPropsMngr.Instance.SetDocumentIdForWb(wb, dokkaDocId);
                        } else
                        {
                            Trace.TraceWarning($"Dokka document ID is different but not default. ID from server: {dokkaDocId}. ID in file: {currDocId}");
                            Trace.TraceInformation("Updating document ID to newly sent ID from Dokka cloud");
                            WbCustomPropsMngr.Instance.SetDocumentIdForWb(wb, dokkaDocId);
                        }
                    }
                    _currentUpload = null;
                } else
                {
                    Trace.TraceWarning("WARNING: ** Current upload was NULL **");
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
            string username, password;
            if (DokkaOfficeAddinConfiguration.Instance.RestoreUsernameAndPassword(out username, out password))
            {
                _loginCtrl.Password = password;
                _loginCtrl.Username = username;
            }
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
