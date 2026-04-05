namespace RemoteClient.Forms
{
    partial class LoginForm : Form
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblServerUrl;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtServerUrl;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkRememberServer;
        private Button btnLogin;
        private Button btnCancel;
        private Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
			lblTitle = new System.Windows.Forms.Label();
			lblServerUrl = new System.Windows.Forms.Label();
			txtServerUrl = new System.Windows.Forms.TextBox();
			lblUsername = new System.Windows.Forms.Label();
			txtUsername = new System.Windows.Forms.TextBox();
			lblPassword = new System.Windows.Forms.Label();
			txtPassword = new System.Windows.Forms.TextBox();
			chkRememberServer = new System.Windows.Forms.CheckBox();
			lblStatus = new System.Windows.Forms.Label();
			btnLogin = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			// 
			// lblTitle
			// 
			lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			lblTitle.Location = new System.Drawing.Point(20, 20);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new System.Drawing.Size(400, 30);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "HawkSync Remote Client";
			lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblServerUrl
			// 
			lblServerUrl.Location = new System.Drawing.Point(30, 70);
			lblServerUrl.Name = "lblServerUrl";
			lblServerUrl.Size = new System.Drawing.Size(100, 23);
			lblServerUrl.TabIndex = 1;
			lblServerUrl.Text = "Server URL:";
			lblServerUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtServerUrl
			// 
			txtServerUrl.Location = new System.Drawing.Point(140, 70);
			txtServerUrl.Name = "txtServerUrl";
			txtServerUrl.Size = new System.Drawing.Size(270, 23);
			txtServerUrl.TabIndex = 2;
			txtServerUrl.Text = "http://localhost:5000";
			// 
			// lblUsername
			// 
			lblUsername.Location = new System.Drawing.Point(30, 110);
			lblUsername.Name = "lblUsername";
			lblUsername.Size = new System.Drawing.Size(100, 23);
			lblUsername.TabIndex = 3;
			lblUsername.Text = "Username:";
			lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtUsername
			// 
			txtUsername.Location = new System.Drawing.Point(140, 110);
			txtUsername.Name = "txtUsername";
			txtUsername.Size = new System.Drawing.Size(270, 23);
			txtUsername.TabIndex = 4;
			// 
			// lblPassword
			// 
			lblPassword.Location = new System.Drawing.Point(30, 150);
			lblPassword.Name = "lblPassword";
			lblPassword.Size = new System.Drawing.Size(100, 23);
			lblPassword.TabIndex = 5;
			lblPassword.Text = "Password:";
			lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPassword
			// 
			txtPassword.Location = new System.Drawing.Point(140, 150);
			txtPassword.Name = "txtPassword";
			txtPassword.Size = new System.Drawing.Size(270, 23);
			txtPassword.TabIndex = 6;
			txtPassword.UseSystemPasswordChar = true;
			// 
			// chkRememberServer
			// 
			chkRememberServer.Location = new System.Drawing.Point(140, 185);
			chkRememberServer.Name = "chkRememberServer";
			chkRememberServer.Size = new System.Drawing.Size(250, 23);
			chkRememberServer.TabIndex = 7;
			chkRememberServer.Text = "Remember server and username";
			// 
			// lblStatus
			// 
			lblStatus.ForeColor = System.Drawing.Color.Gray;
			lblStatus.Location = new System.Drawing.Point(30, 215);
			lblStatus.Name = "lblStatus";
			lblStatus.Size = new System.Drawing.Size(380, 20);
			lblStatus.TabIndex = 8;
			lblStatus.Text = "Ready to connect";
			lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnLogin
			// 
			btnLogin.Location = new System.Drawing.Point(220, 245);
			btnLogin.Name = "btnLogin";
			btnLogin.Size = new System.Drawing.Size(90, 30);
			btnLogin.TabIndex = 9;
			btnLogin.Text = "Login";
			btnLogin.Click += BtnLogin_Click;
			// 
			// btnCancel
			// 
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnCancel.Location = new System.Drawing.Point(320, 245);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(90, 30);
			btnCancel.TabIndex = 10;
			btnCancel.Text = "Cancel";
			btnCancel.Click += BtnCancel_Click;
			// 
			// LoginForm
			// 
			AcceptButton = btnLogin;
			CancelButton = btnCancel;
			ClientSize = new System.Drawing.Size(434, 281);
			Controls.Add(lblTitle);
			Controls.Add(lblServerUrl);
			Controls.Add(txtServerUrl);
			Controls.Add(lblUsername);
			Controls.Add(txtUsername);
			Controls.Add(lblPassword);
			Controls.Add(txtPassword);
			Controls.Add(chkRememberServer);
			Controls.Add(lblStatus);
			Controls.Add(btnLogin);
			Controls.Add(btnCancel);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
			MaximizeBox = false;
			MinimizeBox = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "HawkSync Remote - Login";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
	}
}