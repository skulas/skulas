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
        public Action<string, string> LoginAction { get; set; } = null;

        public string LoginStatusMessage { get {
                return lblLoginStatus.Text;
            }
            set {
                lblLoginStatus.Text = value;
            }
        }

        public UserControlLogin() {
            InitializeComponent();
        }

        public UserControlLogin(Action<string, string> loginAction)
        {
            LoginAction = loginAction;
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            LoginAction?.Invoke(fldUsername.Text, fldPwd.Text);
        }
    }
}
