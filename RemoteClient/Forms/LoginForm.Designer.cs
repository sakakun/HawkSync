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
        this.components = new System.ComponentModel.Container();
        
        // Form
        this.Text = "HawkSync Remote - Login";
        this.Size = new Size(450, 320);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Title Label
        var lblTitle = new Label
        {
            Text = "HawkSync Remote Client",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            Location = new Point(20, 20),
            Size = new Size(400, 30),
            TextAlign = ContentAlignment.MiddleCenter
        };

        // Server URL
        var lblServerUrl = new Label
        {
            Text = "Server URL:",
            Location = new Point(30, 70),
            Size = new Size(100, 23),
            TextAlign = ContentAlignment.MiddleLeft
        };

        this.txtServerUrl = new TextBox
        {
            Location = new Point(140, 70),
            Size = new Size(270, 23),
            Text = "http://localhost:5000"
        };

        // Username
        var lblUsername = new Label
        {
            Text = "Username:",
            Location = new Point(30, 110),
            Size = new Size(100, 23),
            TextAlign = ContentAlignment.MiddleLeft
        };

        this.txtUsername = new TextBox
        {
            Location = new Point(140, 110),
            Size = new Size(270, 23)
        };

        // Password
        var lblPassword = new Label
        {
            Text = "Password:",
            Location = new Point(30, 150),
            Size = new Size(100, 23),
            TextAlign = ContentAlignment.MiddleLeft
        };

        this.txtPassword = new TextBox
        {
            Location = new Point(140, 150),
            Size = new Size(270, 23),
            UseSystemPasswordChar = true
        };

        // Remember Server
        this.chkRememberServer = new CheckBox
        {
            Text = "Remember server and username",
            Location = new Point(140, 185),
            Size = new Size(250, 23)
        };

        // Status Label
        this.lblStatus = new Label
        {
            Text = "Ready to connect",
            Location = new Point(30, 215),
            Size = new Size(380, 20),
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.Gray
        };

        // Login Button
        this.btnLogin = new Button
        {
            Text = "Login",
            Location = new Point(220, 245),
            Size = new Size(90, 30),
            DialogResult = DialogResult.None
        };
        this.btnLogin.Click += BtnLogin_Click;

        // Cancel Button
        this.btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(320, 245),
            Size = new Size(90, 30),
            DialogResult = DialogResult.Cancel
        };
        this.btnCancel.Click += BtnCancel_Click;

        // Add controls
        this.Controls.AddRange(new Control[]
        {
            lblTitle,
            lblServerUrl, txtServerUrl,
            lblUsername, txtUsername,
            lblPassword, txtPassword,
            chkRememberServer,
            lblStatus,
            btnLogin, btnCancel
        });

        this.AcceptButton = btnLogin;
        this.CancelButton = btnCancel;
    }
}