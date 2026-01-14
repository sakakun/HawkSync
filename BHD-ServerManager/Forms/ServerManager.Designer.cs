using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BHD_ServerManager.Forms
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
            label_TimeLeft = new ToolStripLabel();
            label_WinCondition = new ToolStripLabel();
            label_RedScore = new ToolStripLabel();
            label_BlueScore = new ToolStripLabel();
            label_PlayersOnline = new ToolStripLabel();
            mainPanel = new Panel();
            tabControl = new TabControl();
            tabProfile = new TabPage();
            tabGamePlay = new TabPage();
            tabMaps = new TabPage();
            tabPlayers = new TabPage();
            tabChat = new TabPage();
            tabBans = new TabPage();
            tabStats = new TabPage();
            openFileDialog = new OpenFileDialog();
            toolTip = new ToolTip(components);
            toolStrip.SuspendLayout();
            mainPanel.SuspendLayout();
            tabControl.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Dock = DockStyle.Bottom;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripStatus, label_TimeLeft, label_WinCondition, label_RedScore, label_BlueScore, label_PlayersOnline });
            toolStrip.Location = new Point(0, 460);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(984, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip";
            // 
            // toolStripStatus
            // 
            toolStripStatus.Name = "toolStripStatus";
            toolStripStatus.Size = new Size(117, 22);
            toolStripStatus.Text = "Current Server Status";
            // 
            // label_TimeLeft
            // 
            label_TimeLeft.Alignment = ToolStripItemAlignment.Right;
            label_TimeLeft.Name = "label_TimeLeft";
            label_TimeLeft.Size = new Size(65, 22);
            label_TimeLeft.Text = "[Time Left]";
            label_TimeLeft.ToolTipText = "Time Left";
            // 
            // label_WinCondition
            // 
            label_WinCondition.Alignment = ToolStripItemAlignment.Right;
            label_WinCondition.Name = "label_WinCondition";
            label_WinCondition.Size = new Size(92, 22);
            label_WinCondition.Text = "[Win Condition]";
            label_WinCondition.ToolTipText = "Win Condition (Game Type)";
            // 
            // label_RedScore
            // 
            label_RedScore.Alignment = ToolStripItemAlignment.Right;
            label_RedScore.Name = "label_RedScore";
            label_RedScore.Size = new Size(67, 22);
            label_RedScore.Text = "[Red Score]";
            label_RedScore.ToolTipText = "Score";
            // 
            // label_BlueScore
            // 
            label_BlueScore.Alignment = ToolStripItemAlignment.Right;
            label_BlueScore.Name = "label_BlueScore";
            label_BlueScore.Size = new Size(70, 22);
            label_BlueScore.Text = "[Blue Score]";
            label_BlueScore.ToolTipText = "Score";
            // 
            // label_PlayersOnline
            // 
            label_PlayersOnline.Alignment = ToolStripItemAlignment.Right;
            label_PlayersOnline.Name = "label_PlayersOnline";
            label_PlayersOnline.Size = new Size(90, 22);
            label_PlayersOnline.Text = "[Players Online]";
            label_PlayersOnline.ToolTipText = "Players Online";
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(tabControl);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(5);
            mainPanel.Size = new Size(984, 460);
            mainPanel.TabIndex = 2;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabProfile);
            tabControl.Controls.Add(tabGamePlay);
            tabControl.Controls.Add(tabMaps);
            tabControl.Controls.Add(tabPlayers);
            tabControl.Controls.Add(tabChat);
            tabControl.Controls.Add(tabBans);
            tabControl.Controls.Add(tabStats);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(5, 5);
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(974, 450);
            tabControl.TabIndex = 0;
            // 
            // tabProfile
            // 
            tabProfile.Location = new Point(4, 24);
            tabProfile.Name = "tabProfile";
            tabProfile.Size = new Size(966, 422);
            tabProfile.TabIndex = 8;
            tabProfile.Text = "Profile";
            tabProfile.UseVisualStyleBackColor = true;
            // 
            // tabGamePlay
            // 
            tabGamePlay.Location = new Point(4, 24);
            tabGamePlay.Name = "tabGamePlay";
            tabGamePlay.Padding = new Padding(3);
            tabGamePlay.Size = new Size(902, 362);
            tabGamePlay.TabIndex = 0;
            tabGamePlay.Text = "Game Play";
            tabGamePlay.UseVisualStyleBackColor = true;
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
            tabChat.Padding = new Padding(3);
            tabChat.Size = new Size(902, 362);
            tabChat.TabIndex = 9;
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
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // ServerManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 485);
            Controls.Add(mainPanel);
            Controls.Add(toolStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MaximumSize = new Size(1000, 524);
            MinimumSize = new Size(1000, 500);
            Name = "ServerManager";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Black Hawk Down Server Manager";
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
        private OpenFileDialog openFileDialog;
        private ToolTip toolTip;
        internal ToolStripLabel toolStripStatus;
        private ToolStripLabel label_TimeLeft;
        private ToolStripLabel label_WinCondition;
        private ToolStripLabel label_RedScore;
        private ToolStripLabel label_BlueScore;
        private ToolStripLabel label_PlayersOnline;
        internal TabControl tabControl;
        private TabPage tabProfile;
        private TabPage tabGamePlay;
        private TabPage tabMaps;
        private TabPage tabPlayers;
        private TabPage tabChat;
        private TabPage tabBans;
        private TabPage tabStats;
    }
}
