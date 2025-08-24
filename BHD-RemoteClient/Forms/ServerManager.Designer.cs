using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BHD_RemoteClient.Forms
{
    partial class ServerManager : Form
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
            components = new System.ComponentModel.Container();
            toolStrip = new ToolStrip();
            toolStripStatus = new ToolStripLabel();
            mainPanel = new Panel();
            tabControl = new TabControl();
            tabProfile = new TabPage();
            tabServer = new TabPage();
            tabMaps = new TabPage();
            tabPlayers = new TabPage();
            tabChat = new TabPage();
            tabBans = new TabPage();
            tabMessages = new TabPage();
            tabStats = new TabPage();
            tabAdmin = new TabPage();
            openFileDialog = new OpenFileDialog();
            toolTip = new ToolTip(components);
            label_TimeLeft = new ToolStripLabel();
            label_WinCondition = new ToolStripLabel();
            label_RedScore = new ToolStripLabel();
            label_BlueScore = new ToolStripLabel();
            label_PlayersOnline = new ToolStripLabel();
            toolStrip.SuspendLayout();
            mainPanel.SuspendLayout();
            tabControl.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Dock = DockStyle.Bottom;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripStatus, label_TimeLeft, label_WinCondition, label_RedScore, label_BlueScore, label_PlayersOnline });
            toolStrip.Location = new Point(0, 400);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(920, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip";
            // 
            // toolStripStatus
            // 
            toolStripStatus.Name = "toolStripStatus";
            toolStripStatus.Size = new Size(117, 22);
            toolStripStatus.Text = "Current Server Status";
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(tabControl);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(5);
            mainPanel.Size = new Size(920, 400);
            mainPanel.TabIndex = 2;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabProfile);
            tabControl.Controls.Add(tabServer);
            tabControl.Controls.Add(tabMaps);
            tabControl.Controls.Add(tabPlayers);
            tabControl.Controls.Add(tabChat);
            tabControl.Controls.Add(tabBans);
            tabControl.Controls.Add(tabMessages);
            tabControl.Controls.Add(tabStats);
            tabControl.Controls.Add(tabAdmin);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(5, 5);
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(910, 390);
            tabControl.TabIndex = 0;
            // 
            // tabProfile
            // 
            tabProfile.Location = new Point(4, 24);
            tabProfile.Name = "tabProfile";
            tabProfile.Padding = new Padding(3);
            tabProfile.Size = new Size(902, 362);
            tabProfile.TabIndex = 8;
            tabProfile.Text = "Profile";
            tabProfile.UseVisualStyleBackColor = true;
            // 
            // tabServer
            // 
            tabServer.Location = new Point(4, 24);
            tabServer.Name = "tabServer";
            tabServer.Padding = new Padding(3);
            tabServer.Size = new Size(902, 362);
            tabServer.TabIndex = 0;
            tabServer.Text = "Server";
            tabServer.UseVisualStyleBackColor = true;
            // 
            // tabMaps
            // 
            tabMaps.Location = new Point(4, 24);
            tabMaps.Name = "tabMaps";
            tabMaps.Padding = new Padding(3);
            tabMaps.Size = new Size(902, 362);
            tabMaps.TabIndex = 1;
            tabMaps.Text = "Maps";
            tabMaps.UseVisualStyleBackColor = true;
            // 
            // tabPlayers
            // 
            tabPlayers.Location = new Point(4, 24);
            tabPlayers.Name = "tabPlayers";
            tabPlayers.Size = new Size(902, 362);
            tabPlayers.TabIndex = 2;
            tabPlayers.Text = "Players";
            tabPlayers.UseVisualStyleBackColor = true;
            // 
            // tabChat
            // 
            tabChat.Location = new Point(4, 24);
            tabChat.Name = "tabChat";
            tabChat.Size = new Size(902, 362);
            tabChat.TabIndex = 3;
            tabChat.Text = "Chat";
            tabChat.UseVisualStyleBackColor = true;
            // 
            // tabBans
            // 
            tabBans.Location = new Point(4, 24);
            tabBans.Name = "tabBans";
            tabBans.Padding = new Padding(3);
            tabBans.Size = new Size(902, 362);
            tabBans.TabIndex = 4;
            tabBans.Text = "Bans";
            tabBans.UseVisualStyleBackColor = true;
            // 
            // tabMessages
            // 
            tabMessages.Location = new Point(4, 24);
            tabMessages.Name = "tabMessages";
            tabMessages.Size = new Size(902, 362);
            tabMessages.TabIndex = 5;
            tabMessages.Text = "Messages";
            tabMessages.UseVisualStyleBackColor = true;
            // 
            // tabStats
            // 
            tabStats.Location = new Point(4, 24);
            tabStats.Name = "tabStats";
            tabStats.Padding = new Padding(3);
            tabStats.Size = new Size(902, 362);
            tabStats.TabIndex = 6;
            tabStats.Text = "Stats";
            tabStats.UseVisualStyleBackColor = true;
            // 
            // tabAdmin
            // 
            tabAdmin.Location = new Point(4, 24);
            tabAdmin.Name = "tabAdmin";
            tabAdmin.Padding = new Padding(3);
            tabAdmin.Size = new Size(902, 362);
            tabAdmin.TabIndex = 7;
            tabAdmin.Text = "Admin";
            tabAdmin.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // label_TimeLeft
            // 
            label_TimeLeft.Alignment = ToolStripItemAlignment.Right;
            label_TimeLeft.Name = "label_TimeLeft";
            label_TimeLeft.Size = new Size(65, 22);
            label_TimeLeft.Text = "[Time Left]";
            label_TimeLeft.ToolTipText = "Future Feature";
            // 
            // label_WinCondition
            // 
            label_WinCondition.Alignment = ToolStripItemAlignment.Right;
            label_WinCondition.Name = "label_WinCondition";
            label_WinCondition.Size = new Size(92, 22);
            label_WinCondition.Text = "[Win Condition]";
            label_WinCondition.ToolTipText = "Future Feature";
            // 
            // label_RedScore
            // 
            label_RedScore.Alignment = ToolStripItemAlignment.Right;
            label_RedScore.Name = "label_RedScore";
            label_RedScore.Size = new Size(67, 22);
            label_RedScore.Text = "[Red Score]";
            label_RedScore.ToolTipText = "Future Feature";
            // 
            // label_BlueScore
            // 
            label_BlueScore.Alignment = ToolStripItemAlignment.Right;
            label_BlueScore.Name = "label_BlueScore";
            label_BlueScore.Size = new Size(70, 22);
            label_BlueScore.Text = "[Blue Score]";
            label_BlueScore.ToolTipText = "Future Feature";
            // 
            // label_PlayersOnline
            // 
            label_PlayersOnline.Alignment = ToolStripItemAlignment.Right;
            label_PlayersOnline.Name = "label_PlayersOnline";
            label_PlayersOnline.Size = new Size(90, 22);
            label_PlayersOnline.Text = "[Players Online]";
            label_PlayersOnline.ToolTipText = "Future Feature";
            // 
            // ServerManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(920, 425);
            Controls.Add(mainPanel);
            Controls.Add(toolStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ServerManager";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Black Hawk Down Server Manager - Remote Client";
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            mainPanel.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip;
        private Panel mainPanel;
        private TabPage tabServer;
        private TabPage tabMaps;
        private TabPage tabPlayers;
        private OpenFileDialog openFileDialog;
        private ToolTip toolTip;
        internal ToolStripLabel toolStripStatus;
        private TabPage tabBans;
        internal TabControl tabControl;
        private TabPage tabMessages;
        private TabPage tabStats;
        private TabPage tabAdmin;
        private TabPage tabProfile;
        private TabPage tabChat;
        private ToolStripLabel label_TimeLeft;
        private ToolStripLabel label_WinCondition;
        private ToolStripLabel label_RedScore;
        private ToolStripLabel label_BlueScore;
        private ToolStripLabel label_PlayersOnline;
    }
}
