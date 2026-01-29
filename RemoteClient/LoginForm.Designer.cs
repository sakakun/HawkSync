namespace RemoteClient
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            textBox_remotePassword = new TextBox();
            textBox_remoteUsername = new TextBox();
            button_login = new Button();
            checkBox_rememberLogin = new CheckBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(textBox_remotePassword, 2, 3);
            tableLayoutPanel1.Controls.Add(textBox_remoteUsername, 2, 2);
            tableLayoutPanel1.Controls.Add(button_login, 2, 4);
            tableLayoutPanel1.Controls.Add(checkBox_rememberLogin, 2, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 8;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(473, 250);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // textBox_remotePassword
            // 
            textBox_remotePassword.BorderStyle = BorderStyle.FixedSingle;
            textBox_remotePassword.Dock = DockStyle.Fill;
            textBox_remotePassword.Font = new Font("Segoe UI", 14F);
            textBox_remotePassword.Location = new Point(131, 95);
            textBox_remotePassword.Margin = new Padding(3, 5, 3, 3);
            textBox_remotePassword.MaxLength = 32;
            textBox_remotePassword.Name = "textBox_remotePassword";
            textBox_remotePassword.PlaceholderText = "Password";
            textBox_remotePassword.Size = new Size(210, 32);
            textBox_remotePassword.TabIndex = 1;
            textBox_remotePassword.TextAlign = HorizontalAlignment.Center;
            textBox_remotePassword.UseSystemPasswordChar = true;
            // 
            // textBox_remoteUsername
            // 
            textBox_remoteUsername.BorderStyle = BorderStyle.FixedSingle;
            textBox_remoteUsername.Dock = DockStyle.Fill;
            textBox_remoteUsername.Font = new Font("Segoe UI", 14F);
            textBox_remoteUsername.Location = new Point(131, 55);
            textBox_remoteUsername.Margin = new Padding(3, 5, 3, 3);
            textBox_remoteUsername.MaxLength = 32;
            textBox_remoteUsername.Name = "textBox_remoteUsername";
            textBox_remoteUsername.PlaceholderText = "Username";
            textBox_remoteUsername.Size = new Size(210, 32);
            textBox_remoteUsername.TabIndex = 0;
            textBox_remoteUsername.TextAlign = HorizontalAlignment.Center;
            // 
            // button_login
            // 
            button_login.BackColor = Color.White;
            button_login.Dock = DockStyle.Fill;
            button_login.FlatStyle = FlatStyle.Flat;
            button_login.Font = new Font("Segoe UI", 12F);
            button_login.Location = new Point(131, 133);
            button_login.Name = "button_login";
            button_login.Size = new Size(210, 34);
            button_login.TabIndex = 2;
            button_login.Text = "LOGIN";
            button_login.UseVisualStyleBackColor = false;
            // 
            // checkBox_rememberLogin
            // 
            checkBox_rememberLogin.AutoSize = true;
            checkBox_rememberLogin.Dock = DockStyle.Fill;
            checkBox_rememberLogin.Font = new Font("Segoe UI", 12F);
            checkBox_rememberLogin.Location = new Point(128, 170);
            checkBox_rememberLogin.Margin = new Padding(0);
            checkBox_rememberLogin.Name = "checkBox_rememberLogin";
            checkBox_rememberLogin.Padding = new Padding(50, 0, 40, 0);
            checkBox_rememberLogin.Size = new Size(216, 30);
            checkBox_rememberLogin.TabIndex = 3;
            checkBox_rememberLogin.Text = "Remember";
            checkBox_rememberLogin.TextAlign = ContentAlignment.MiddleCenter;
            checkBox_rememberLogin.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(473, 250);
            Controls.Add(tableLayoutPanel1);
            Name = "LoginForm";
            Text = "Hawk Sync Remote Client";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TextBox textBox_remotePassword;
        private Button button_login;
        private CheckBox checkBox_rememberLogin;
        private TextBox textBox_remoteUsername;
    }
}
