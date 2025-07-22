namespace BHD_RemoteClient.Forms
{
    partial class LoginWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tb_serverAddress = new TextBox();
            num_serverPort = new NumericUpDown();
            tb_username = new TextBox();
            tb_password = new TextBox();
            cb_rememberPassword = new CheckBox();
            btn_login = new Button();
            btn_open = new Button();
            ((System.ComponentModel.ISupportInitialize)num_serverPort).BeginInit();
            SuspendLayout();
            // 
            // tb_serverAddress
            // 
            tb_serverAddress.Location = new Point(12, 12);
            tb_serverAddress.Name = "tb_serverAddress";
            tb_serverAddress.PlaceholderText = "Server Address";
            tb_serverAddress.Size = new Size(182, 23);
            tb_serverAddress.TabIndex = 0;
            // 
            // num_serverPort
            // 
            num_serverPort.Location = new Point(200, 12);
            num_serverPort.Maximum = new decimal(new int[] { 49151, 0, 0, 0 });
            num_serverPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            num_serverPort.Name = "num_serverPort";
            num_serverPort.Size = new Size(58, 23);
            num_serverPort.TabIndex = 1;
            num_serverPort.TextAlign = HorizontalAlignment.Right;
            num_serverPort.Value = new decimal(new int[] { 8083, 0, 0, 0 });
            // 
            // tb_username
            // 
            tb_username.Location = new Point(12, 41);
            tb_username.Name = "tb_username";
            tb_username.PlaceholderText = "Username";
            tb_username.Size = new Size(120, 23);
            tb_username.TabIndex = 2;
            // 
            // tb_password
            // 
            tb_password.Location = new Point(138, 41);
            tb_password.Name = "tb_password";
            tb_password.PasswordChar = '*';
            tb_password.PlaceholderText = "Password";
            tb_password.Size = new Size(120, 23);
            tb_password.TabIndex = 3;
            // 
            // cb_rememberPassword
            // 
            cb_rememberPassword.AutoSize = true;
            cb_rememberPassword.Location = new Point(174, 70);
            cb_rememberPassword.Name = "cb_rememberPassword";
            cb_rememberPassword.Size = new Size(84, 19);
            cb_rememberPassword.TabIndex = 4;
            cb_rememberPassword.Text = "Remember";
            cb_rememberPassword.UseVisualStyleBackColor = true;
            // 
            // btn_login
            // 
            btn_login.Location = new Point(183, 95);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(75, 23);
            btn_login.TabIndex = 5;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += btnLogin_Click;
            // 
            // btn_open
            // 
            btn_open.Location = new Point(12, 95);
            btn_open.Name = "btn_open";
            btn_open.Size = new Size(75, 23);
            btn_open.TabIndex = 6;
            btn_open.Text = "Open";
            btn_open.UseVisualStyleBackColor = true;
            btn_open.Visible = false;
            btn_open.Click += actionClick_openServerManager;
            // 
            // LoginWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(272, 131);
            Controls.Add(btn_open);
            Controls.Add(btn_login);
            Controls.Add(cb_rememberPassword);
            Controls.Add(tb_password);
            Controls.Add(tb_username);
            Controls.Add(num_serverPort);
            Controls.Add(tb_serverAddress);
            MaximizeBox = false;
            MaximumSize = new Size(288, 170);
            MinimumSize = new Size(288, 170);
            Name = "LoginWindow";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Black Hawk Down Server Manager - Remote Client";
            ((System.ComponentModel.ISupportInitialize)num_serverPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_serverAddress;
        private NumericUpDown num_serverPort;
        private TextBox tb_username;
        private TextBox tb_password;
        private CheckBox cb_rememberPassword;
        private Button btn_login;
        private Button btn_open;
    }
}