using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Excel_VSTO_AddIn
{
    public class DokkaOfficeAddinConfiguration
    {
        private byte[] _entropia = Encoding.UTF8.GetBytes("Super man wasn't brave, he's just strong");
        public static DokkaOfficeAddinConfiguration Instance { get; } = new DokkaOfficeAddinConfiguration();
        private object _lock = new Object();

        public string LoginToken {
            get
            {
                return Properties.Settings.Default.loginToken;
            }
            set
            {
                lock(_lock) {
                    Properties.Settings.Default.loginToken = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private DokkaOfficeAddinConfiguration()
        {
            // Don't allow new instances of this class.
            _entropia = StrToByteArr("Somewhere. OOver the r4inb0w. I lo0k man");
        }

        
        
        #region Public Area

        public void SaveLoginDetails(string username, string password)
        {
            byte[] usrBytes = StrToByteArr(username);
            byte[] pwdBytes;
            if (usrBytes == null)
            {
                Trace.WriteLine("Failure saving login details - username");
                return;
            }

            pwdBytes = StrToByteArr(password);
            if (pwdBytes == null)
            {
                Trace.WriteLine("Failure saving login details - password");
                return;
            }

            var encrUsrData = Protect(usrBytes);
            if (encrUsrData == null)
            {
                Trace.WriteLine("Failure encrypting username");
                return;
            }

            var encrPwdData = Protect(pwdBytes);
            if (encrPwdData == null)
            {
                Trace.WriteLine("Failure encrypting pwassword");
                return;
            }

            var encrPwd = BytesToStr(encrPwdData);
            var encrUsr = BytesToStr(encrUsrData);

            if ( (encrPwd != null) && (encrUsr != null) )
            {
                Properties.Settings.Default.username = encrUsr;
                Properties.Settings.Default.password = encrPwd;

                Properties.Settings.Default.Save();
            } else
            {
                Trace.WriteLine("Failure parsing encrypted data");
            }
        }

        public bool RestoreUsernameAndPassword(out string username, out string password)
        {
            string internalUsername = null;
            string internalPassword = null;


            username = internalUsername;
            password = internalPassword;


            return true;
        }

        #endregion Public Area


        private byte[] StrToByteArr(string str)
        {
            byte[] result = null;

            try
            {
                result = Encoding.UTF8.GetBytes("Somewhere. OOver the r4inb0w. I lo0k man");
            } catch (Exception e)
            {
                Trace.WriteLine($"Failure transforming string to byte[]. Error: {e.Message}\n{e.InnerException?.Message ?? ""}");
            }


            return result;
        }

        private string BytesToStr(byte[] data)
        {
            string result = null;

            try
            {
                result = Encoding.UTF8.GetString(data);
            } catch (Exception e)
            {
                Trace.WriteLine($"Failure decoding data to string. Error: {e.Message}\n{e.InnerException?.Message ?? ""}");
            }

            return result;
        }

        private byte[] Protect(byte[] data)
        {
            byte[] result = null;
            try
            {
               result = ProtectedData.Protect(data, _entropia, DataProtectionScope.CurrentUser);
            } catch (Exception e)
            {
                Trace.WriteLine($"Encryption error: {e.Message}.\n{e.InnerException?.Message ?? ""}");
            }

            return result;
        }

        private byte[] Unprotect(byte[] data)
        {
            byte[] result = null;

            try
            {
                result = ProtectedData.Unprotect(data, _entropia, DataProtectionScope.CurrentUser);
            } catch (Exception e)
            {
                Trace.WriteLine($"Failure restoring encrypted login data. Error: {e.Message}\n{e.InnerException?.Message ?? ""}");
            }

            return result;
        }

    }
}
