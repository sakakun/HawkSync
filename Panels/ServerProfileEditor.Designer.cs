using System.ComponentModel;

namespace ServerManager.Panels;

partial class ServerProfileEditor
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerProfileEditor));
        this.label_profileName = new System.Windows.Forms.Label();
        this.textBox_profileName = new System.Windows.Forms.TextBox();
        this.label_serverPath = new System.Windows.Forms.Label();
        this.textBox_serverPath = new System.Windows.Forms.TextBox();
        this.button_browse = new System.Windows.Forms.Button();
        this.label_mod = new System.Windows.Forms.Label();
        this.label_modInfo = new System.Windows.Forms.Label();
        this.button_accept = new System.Windows.Forms.Button();
        this.button_cancel = new System.Windows.Forms.Button();
        this.label_message = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // label_profileName
        // 
        this.label_profileName.Location = new System.Drawing.Point(12, 9);
        this.label_profileName.Name = "label_profileName";
        this.label_profileName.Size = new System.Drawing.Size(100, 23);
        this.label_profileName.TabIndex = 0;
        this.label_profileName.Text = "Profile Name:";
        this.label_profileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // textBox_profileName
        // 
        this.textBox_profileName.Location = new System.Drawing.Point(118, 11);
        this.textBox_profileName.MaxLength = 16;
        this.textBox_profileName.Name = "textBox_profileName";
        this.textBox_profileName.Size = new System.Drawing.Size(223, 20);
        this.textBox_profileName.TabIndex = 1;
        this.textBox_profileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        this.textBox_profileName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.profileName_validateChars);
        // 
        // label_serverPath
        // 
        this.label_serverPath.Location = new System.Drawing.Point(12, 35);
        this.label_serverPath.Name = "label_serverPath";
        this.label_serverPath.Size = new System.Drawing.Size(100, 23);
        this.label_serverPath.TabIndex = 2;
        this.label_serverPath.Text = "Server Path:";
        this.label_serverPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // textBox_serverPath
        // 
        this.textBox_serverPath.Enabled = false;
        this.textBox_serverPath.Location = new System.Drawing.Point(118, 37);
        this.textBox_serverPath.Name = "textBox_serverPath";
        this.textBox_serverPath.Size = new System.Drawing.Size(223, 20);
        this.textBox_serverPath.TabIndex = 3;
        this.textBox_serverPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // button_browse
        // 
        this.button_browse.Location = new System.Drawing.Point(347, 38);
        this.button_browse.Name = "button_browse";
        this.button_browse.Size = new System.Drawing.Size(75, 20);
        this.button_browse.TabIndex = 4;
        this.button_browse.Text = "Browse...";
        this.button_browse.UseVisualStyleBackColor = true;
        this.button_browse.Click += new System.EventHandler(this.click_browserExe);
        // 
        // label_mod
        // 
        this.label_mod.Location = new System.Drawing.Point(12, 64);
        this.label_mod.Name = "label_mod";
        this.label_mod.Size = new System.Drawing.Size(100, 23);
        this.label_mod.TabIndex = 5;
        this.label_mod.Text = "Mod Installed:";
        this.label_mod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label_modInfo
        // 
        this.label_modInfo.Location = new System.Drawing.Point(118, 64);
        this.label_modInfo.Name = "label_modInfo";
        this.label_modInfo.Size = new System.Drawing.Size(223, 23);
        this.label_modInfo.TabIndex = 6;
        this.label_modInfo.Text = "Select game server executable...";
        this.label_modInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // button_accept
        // 
        this.button_accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.button_accept.Location = new System.Drawing.Point(266, 117);
        this.button_accept.Name = "button_accept";
        this.button_accept.Size = new System.Drawing.Size(75, 23);
        this.button_accept.TabIndex = 7;
        this.button_accept.Text = "Accept";
        this.button_accept.UseVisualStyleBackColor = true;
        this.button_accept.Click += new System.EventHandler(this.click_accept);
        // 
        // button_cancel
        // 
        this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.button_cancel.Location = new System.Drawing.Point(347, 117);
        this.button_cancel.Name = "button_cancel";
        this.button_cancel.Size = new System.Drawing.Size(75, 23);
        this.button_cancel.TabIndex = 8;
        this.button_cancel.Text = "Cancel";
        this.button_cancel.UseVisualStyleBackColor = true;
        // 
        // label_message
        // 
        this.label_message.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        this.label_message.Location = new System.Drawing.Point(12, 91);
        this.label_message.Name = "label_message";
        this.label_message.Size = new System.Drawing.Size(410, 23);
        this.label_message.TabIndex = 9;
        this.label_message.Text = "Message";
        this.label_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // ServerProfileEditor
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(434, 152);
        this.Controls.Add(this.label_message);
        this.Controls.Add(this.button_cancel);
        this.Controls.Add(this.button_accept);
        this.Controls.Add(this.label_modInfo);
        this.Controls.Add(this.label_mod);
        this.Controls.Add(this.button_browse);
        this.Controls.Add(this.textBox_serverPath);
        this.Controls.Add(this.label_serverPath);
        this.Controls.Add(this.textBox_profileName);
        this.Controls.Add(this.label_profileName);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "ServerProfileEditor";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Server Profile";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.Label label_message;

    private System.Windows.Forms.Button button_accept;
    private System.Windows.Forms.Button button_cancel;

    private System.Windows.Forms.Label label_mod;
    private System.Windows.Forms.Label label_modInfo;

    private System.Windows.Forms.TextBox textBox_profileName;
    private System.Windows.Forms.Label label_serverPath;
    private System.Windows.Forms.TextBox textBox_serverPath;
    private System.Windows.Forms.Button button_browse;

    private System.Windows.Forms.Label label_profileName;

    #endregion
}