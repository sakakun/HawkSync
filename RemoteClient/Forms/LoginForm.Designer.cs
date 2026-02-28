namespace RemoteClient.Forms;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;
    private TextBox txtServerUrl;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private CheckBox chkRememberServer;
    private Button btnLogin;
    private Button btnCancel;
    private Label lblStatus;

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
		lblTitle.Location = new Point(0, 0);
		lblTitle.Name = "lblTitle";
		lblTitle.Size = new Size(100, 23);
		lblTitle.TabIndex = 0;
		// 
		// lblServerUrl
		// 
		lblServerUrl.Location = new Point(0, 0);
		lblServerUrl.Name = "lblServerUrl";
		lblServerUrl.Size = new Size(100, 23);
		lblServerUrl.TabIndex = 1;
		// 
		// txtServerUrl
		// 
		txtServerUrl.Location = new Point(0, 0);
		txtServerUrl.Name = "txtServerUrl";
		txtServerUrl.Size = new Size(100, 23);
		txtServerUrl.TabIndex = 2;
		// 
		// lblUsername
		// 
		lblUsername.Location = new Point(0, 0);
		lblUsername.Name = "lblUsername";
		lblUsername.Size = new Size(100, 23);
		lblUsername.TabIndex = 3;
		// 
		// txtUsername
		// 
		txtUsername.Location = new Point(0, 0);
		txtUsername.Name = "txtUsername";
		txtUsername.Size = new Size(100, 23);
		txtUsername.TabIndex = 4;
		// 
		// lblPassword
		// 
		lblPassword.Location = new Point(0, 0);
		lblPassword.Name = "lblPassword";
		lblPassword.Size = new Size(100, 23);
		lblPassword.TabIndex = 5;
		// 
		// txtPassword
		// 
		txtPassword.Location = new Point(0, 0);
		txtPassword.Name = "txtPassword";
		txtPassword.Size = new Size(100, 23);
		txtPassword.TabIndex = 6;
		// 
		// chkRememberServer
		// 
		chkRememberServer.Location = new Point(0, 0);
		chkRememberServer.Name = "chkRememberServer";
		chkRememberServer.Size = new Size(104, 24);
		chkRememberServer.TabIndex = 7;
		// 
		// lblStatus
		// 
		lblStatus.Location = new Point(0, 0);
		lblStatus.Name = "lblStatus";
		lblStatus.Size = new Size(100, 23);
		lblStatus.TabIndex = 8;
		// 
		// btnLogin
		// 
		btnLogin.Location = new Point(0, 0);
		btnLogin.Name = "btnLogin";
		btnLogin.Size = new Size(75, 23);
		btnLogin.TabIndex = 9;
		btnLogin.Click += BtnLogin_Click;
		// 
		// btnCancel
		// 
		btnCancel.Location = new Point(0, 0);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new Size(75, 23);
		btnCancel.TabIndex = 10;
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

	private Label lblTitle;
	private Label lblServerUrl;
	private Label lblUsername;
	private Label lblPassword;
}