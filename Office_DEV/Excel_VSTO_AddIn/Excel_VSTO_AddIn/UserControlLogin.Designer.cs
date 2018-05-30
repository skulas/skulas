namespace Excel_VSTO_AddIn
{
    partial class UserControlLogin
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPwd = new System.Windows.Forms.Label();
            this.fldUsername = new System.Windows.Forms.TextBox();
            this.fldPwd = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblLoginStatus = new System.Windows.Forms.Label();
            this.listAccounts = new System.Windows.Forms.ComboBox();
            this.lblAccount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(94, 34);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(31, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "email";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new System.Drawing.Point(83, 75);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(52, 13);
            this.lblPwd.TabIndex = 2;
            this.lblPwd.Text = "password";
            this.lblPwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fldUsername
            // 
            this.fldUsername.Location = new System.Drawing.Point(31, 50);
            this.fldUsername.Name = "fldUsername";
            this.fldUsername.Size = new System.Drawing.Size(156, 20);
            this.fldUsername.TabIndex = 3;
            // 
            // fldPwd
            // 
            this.fldPwd.Location = new System.Drawing.Point(31, 91);
            this.fldPwd.Name = "fldPwd";
            this.fldPwd.Size = new System.Drawing.Size(156, 20);
            this.fldPwd.TabIndex = 4;
            this.fldPwd.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Monotype Corsiva", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Location = new System.Drawing.Point(71, 175);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(76, 48);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "GO";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // lblLoginStatus
            // 
            this.lblLoginStatus.AutoSize = true;
            this.lblLoginStatus.Location = new System.Drawing.Point(26, 229);
            this.lblLoginStatus.MaximumSize = new System.Drawing.Size(170, 0);
            this.lblLoginStatus.MinimumSize = new System.Drawing.Size(170, 0);
            this.lblLoginStatus.Name = "lblLoginStatus";
            this.lblLoginStatus.Size = new System.Drawing.Size(170, 13);
            this.lblLoginStatus.TabIndex = 6;
            this.lblLoginStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listAccounts
            // 
            this.listAccounts.FormattingEnabled = true;
            this.listAccounts.Items.AddRange(new object[] {
            "acc1",
            "acc2"});
            this.listAccounts.Location = new System.Drawing.Point(31, 141);
            this.listAccounts.Name = "listAccounts";
            this.listAccounts.Size = new System.Drawing.Size(156, 21);
            this.listAccounts.TabIndex = 7;
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new System.Drawing.Point(86, 123);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(46, 13);
            this.lblAccount.TabIndex = 8;
            this.lblAccount.Text = "account";
            this.lblAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserControlLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAccount);
            this.Controls.Add(this.listAccounts);
            this.Controls.Add(this.lblLoginStatus);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.fldPwd);
            this.Controls.Add(this.fldUsername);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.lblUsername);
            this.Name = "UserControlLogin";
            this.Size = new System.Drawing.Size(221, 273);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.TextBox fldUsername;
        private System.Windows.Forms.TextBox fldPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblLoginStatus;
        private System.Windows.Forms.ComboBox listAccounts;
        private System.Windows.Forms.Label lblAccount;
    }
}
