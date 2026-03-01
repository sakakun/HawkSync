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

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
			lblTitle = new Label();
			lblServerUrl = new Label();
			txtServerUrl = new TextBox();
			lblUsername = new Label();
			txtUsername = new TextBox();
			lblPassword = new Label();
			txtPassword = new TextBox();
			chkRememberServer = new CheckBox();
			lblStatus = new Label();
			btnLogin = new Button();
			btnCancel = new Button();
			SuspendLayout();
			// 
			// lblTitle
			// 
			lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
			lblTitle.Location = new Point(20, 20);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new Size(400, 30);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "HawkSync Remote Client";
			lblTitle.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lblServerUrl
			// 
			lblServerUrl.Location = new Point(30, 70);
			lblServerUrl.Name = "lblServerUrl";
			lblServerUrl.Size = new Size(100, 23);
			lblServerUrl.TabIndex = 1;
			lblServerUrl.Text = "Server URL:";
			lblServerUrl.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// txtServerUrl
			// 
			txtServerUrl.Location = new Point(140, 70);
			txtServerUrl.Name = "txtServerUrl";
			txtServerUrl.Size = new Size(270, 23);
			txtServerUrl.TabIndex = 2;
			txtServerUrl.Text = "http://localhost:5000";
			// 
			// lblUsername
			// 
			lblUsername.Location = new Point(30, 110);
			lblUsername.Name = "lblUsername";
			lblUsername.Size = new Size(100, 23);
			lblUsername.TabIndex = 3;
			lblUsername.Text = "Username:";
			lblUsername.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// txtUsername
			// 
			txtUsername.Location = new Point(140, 110);
			txtUsername.Name = "txtUsername";
			txtUsername.Size = new Size(270, 23);
			txtUsername.TabIndex = 4;
			// 
			// lblPassword
			// 
			lblPassword.Location = new Point(30, 150);
			lblPassword.Name = "lblPassword";
			lblPassword.Size = new Size(100, 23);
			lblPassword.TabIndex = 5;
			lblPassword.Text = "Password:";
			lblPassword.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// txtPassword
			// 
			txtPassword.Location = new Point(140, 150);
			txtPassword.Name = "txtPassword";
			txtPassword.Size = new Size(270, 23);
			txtPassword.TabIndex = 6;
			txtPassword.UseSystemPasswordChar = true;
			// 
			// chkRememberServer
			// 
			chkRememberServer.Location = new Point(140, 185);
			chkRememberServer.Name = "chkRememberServer";
			chkRememberServer.Size = new Size(250, 23);
			chkRememberServer.TabIndex = 7;
			chkRememberServer.Text = "Remember server and username";
			// 
			// lblStatus
			// 
			lblStatus.ForeColor = Color.Gray;
			lblStatus.Location = new Point(30, 215);
			lblStatus.Name = "lblStatus";
			lblStatus.Size = new Size(380, 20);
			lblStatus.TabIndex = 8;
			lblStatus.Text = "Ready to connect";
			lblStatus.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// btnLogin
			// 
			btnLogin.Location = new Point(220, 245);
			btnLogin.Name = "btnLogin";
			btnLogin.Size = new Size(90, 30);
			btnLogin.TabIndex = 9;
			btnLogin.Text = "Login";
			btnLogin.Click += BtnLogin_Click;
			// 
			// btnCancel
			// 
			btnCancel.DialogResult = DialogResult.Cancel;
			btnCancel.Location = new Point(320, 245);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(90, 30);
			btnCancel.TabIndex = 10;
			btnCancel.Text = "Cancel";
			btnCancel.Click += BtnCancel_Click;
			// 
			// LoginForm
			// 
			AcceptButton = btnLogin;
			CancelButton = btnCancel;
			ClientSize = new Size(434, 281);
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
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "LoginForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "HawkSync Remote - Login";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
	}
}