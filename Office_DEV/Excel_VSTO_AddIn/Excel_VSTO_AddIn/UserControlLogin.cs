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
        public UserControlLogin()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            await ServerInterface.Instance.LoginWithCredentials(fldUsername.Text, fldPwd.Text, (succss) =>
            {
                if (succss)
                {
                    Trace.WriteLine("Successfull login");
                } else
                {
                    Trace.WriteLine("Login failed");
                }
            });
        }
    }
}
