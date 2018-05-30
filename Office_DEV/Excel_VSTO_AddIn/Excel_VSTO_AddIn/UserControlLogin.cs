using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Excel_VSTO_AddIn
{
    public partial class UserControlLogin : UserControl
    {
        //private Action<string, string> _loginAction;
        public Action<string, string, int> LoginAction { get; set; } = null;

        public string LoginStatusMessage { get {
                return lblLoginStatus.Text;
            }
            set {
                lblLoginStatus.Text = value;
            }
        }

        public string Username { get
            {
                return fldUsername.Text;
            }
            set
            {
                fldUsername.Text = value;
            }
        }

        public string Password { get
            {
                return fldPwd.Text;
            }
            set
            {
                fldPwd.Text = value;
            }
        }

        public string[] AccountsList {
            get {
                return null;
            }
            set
            {
                if ((value != null) && (value.Length > 0))
                {
                    listAccounts.Items.Clear();
                    listAccounts.Items.AddRange(value);
                    listAccounts.Text = value[0];
                    listAccounts.SelectedIndex = 0;
                }
            }
        }

        public UserControlLogin() {
            InitializeComponent();
        }

        public UserControlLogin(Action<string, string, int> loginAction)
        {
            LoginAction = loginAction;
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            LoginAction?.Invoke(fldUsername.Text, fldPwd.Text, listAccounts.SelectedIndex);
        }
    }
}
