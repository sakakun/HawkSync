using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BHD_ServerManager.Forms
{
    partial class ServerManagerUI : Form
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerManagerUI));
			toolStrip = new System.Windows.Forms.ToolStrip();
			toolStripStatus = new System.Windows.Forms.ToolStripLabel();
			label_TimeLeft = new System.Windows.Forms.ToolStripLabel();
			label_WinCondition = new System.Windows.Forms.ToolStripLabel();
			label_RedScore = new System.Windows.Forms.ToolStripLabel();
			label_BlueScore = new System.Windows.Forms.ToolStripLabel();
			label_PlayersOnline = new System.Windows.Forms.ToolStripLabel();
			mainPanel = new System.Windows.Forms.Panel();
			tabControl = new System.Windows.Forms.TabControl();
			tabProfile = new System.Windows.Forms.TabPage();
			tabGamePlay = new System.Windows.Forms.TabPage();
			tabMaps = new System.Windows.Forms.TabPage();
			tabPlayers = new System.Windows.Forms.TabPage();
			tabChat = new System.Windows.Forms.TabPage();
			tabBans = new System.Windows.Forms.TabPage();
			tabStats = new System.Windows.Forms.TabPage();
			tabAdmin = new System.Windows.Forms.TabPage();
			openFileDialog = new System.Windows.Forms.OpenFileDialog();
			toolTip = new System.Windows.Forms.ToolTip(components);
			toolStrip.SuspendLayout();
			mainPanel.SuspendLayout();
			tabControl.SuspendLayout();
			SuspendLayout();
			// 
			// toolStrip
			// 
			toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
			toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatus, label_TimeLeft, label_WinCondition, label_RedScore, label_BlueScore, label_PlayersOnline });
			toolStrip.Location = new System.Drawing.Point(0, 460);
			toolStrip.Name = "toolStrip";
			toolStrip.Size = new System.Drawing.Size(984, 25);
			toolStrip.TabIndex = 1;
			toolStrip.Text = "toolStrip";
			// 
			// toolStripStatus
			// 
			toolStripStatus.Name = "toolStripStatus";
			toolStripStatus.Size = new System.Drawing.Size(117, 22);
			toolStripStatus.Text = "Current Server Status";
			// 
			// label_TimeLeft
			// 
			label_TimeLeft.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			label_TimeLeft.Name = "label_TimeLeft";
			label_TimeLeft.Size = new System.Drawing.Size(65, 22);
			label_TimeLeft.Text = "[Time Left]";
			label_TimeLeft.ToolTipText = "Time Left";
			// 
			// label_WinCondition
			// 
			label_WinCondition.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			label_WinCondition.Name = "label_WinCondition";
			label_WinCondition.Size = new System.Drawing.Size(92, 22);
			label_WinCondition.Text = "[Win Condition]";
			label_WinCondition.ToolTipText = "Win Condition (Game Type)";
			// 
			// label_RedScore
			// 
			label_RedScore.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			label_RedScore.Name = "label_RedScore";
			label_RedScore.Size = new System.Drawing.Size(67, 22);
			label_RedScore.Text = "[Red Score]";
			label_RedScore.ToolTipText = "Score";
			// 
			// label_BlueScore
			// 
			label_BlueScore.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			label_BlueScore.Name = "label_BlueScore";
			label_BlueScore.Size = new System.Drawing.Size(70, 22);
			label_BlueScore.Text = "[Blue Score]";
			label_BlueScore.ToolTipText = "Score";
			// 
			// label_PlayersOnline
			// 
			label_PlayersOnline.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			label_PlayersOnline.Name = "label_PlayersOnline";
			label_PlayersOnline.Size = new System.Drawing.Size(90, 22);
			label_PlayersOnline.Text = "[Players Online]";
			label_PlayersOnline.ToolTipText = "Players Online";
			// 
			// mainPanel
			// 
			mainPanel.Controls.Add(tabControl);
			mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			mainPanel.Location = new System.Drawing.Point(0, 0);
			mainPanel.Name = "mainPanel";
			mainPanel.Padding = new System.Windows.Forms.Padding(5);
			mainPanel.Size = new System.Drawing.Size(984, 460);
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
			tabControl.Controls.Add(tabAdmin);
			tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			tabControl.Location = new System.Drawing.Point(5, 5);
			tabControl.Multiline = true;
			tabControl.Name = "tabControl";
			tabControl.SelectedIndex = 0;
			tabControl.Size = new System.Drawing.Size(974, 450);
			tabControl.TabIndex = 0;
			// 
			// tabProfile
			// 
			tabProfile.Location = new System.Drawing.Point(4, 24);
			tabProfile.Name = "tabProfile";
			tabProfile.Size = new System.Drawing.Size(966, 422);
			tabProfile.TabIndex = 8;
			tabProfile.Text = "Profile";
			tabProfile.UseVisualStyleBackColor = true;
			// 
			// tabGamePlay
			// 
			tabGamePlay.Location = new System.Drawing.Point(4, 24);
			tabGamePlay.Name = "tabGamePlay";
			tabGamePlay.Padding = new System.Windows.Forms.Padding(3);
			tabGamePlay.Size = new System.Drawing.Size(966, 422);
			tabGamePlay.TabIndex = 0;
			tabGamePlay.Text = "Game Play";
			tabGamePlay.UseVisualStyleBackColor = true;
			// 
			// tabMaps
			// 
			tabMaps.Location = new System.Drawing.Point(4, 24);
			tabMaps.Name = "tabMaps";
			tabMaps.Padding = new System.Windows.Forms.Padding(3);
			tabMaps.Size = new System.Drawing.Size(966, 422);
			tabMaps.TabIndex = 1;
			tabMaps.Text = "Maps";
			tabMaps.UseVisualStyleBackColor = true;
			// 
			// tabPlayers
			// 
			tabPlayers.Location = new System.Drawing.Point(4, 24);
			tabPlayers.Name = "tabPlayers";
			tabPlayers.Size = new System.Drawing.Size(966, 422);
			tabPlayers.TabIndex = 2;
			tabPlayers.Text = "Players";
			tabPlayers.UseVisualStyleBackColor = true;
			// 
			// tabChat
			// 
			tabChat.Location = new System.Drawing.Point(4, 24);
			tabChat.Name = "tabChat";
			tabChat.Padding = new System.Windows.Forms.Padding(3);
			tabChat.Size = new System.Drawing.Size(966, 422);
			tabChat.TabIndex = 9;
			tabChat.Text = "Chat";
			tabChat.UseVisualStyleBackColor = true;
			// 
			// tabBans
			// 
			tabBans.Location = new System.Drawing.Point(4, 24);
			tabBans.Name = "tabBans";
			tabBans.Padding = new System.Windows.Forms.Padding(3);
			tabBans.Size = new System.Drawing.Size(966, 422);
			tabBans.TabIndex = 4;
			tabBans.Text = "Bans";
			tabBans.UseVisualStyleBackColor = true;
			// 
			// tabStats
			// 
			tabStats.Location = new System.Drawing.Point(4, 24);
			tabStats.Name = "tabStats";
			tabStats.Padding = new System.Windows.Forms.Padding(3);
			tabStats.Size = new System.Drawing.Size(966, 422);
			tabStats.TabIndex = 6;
			tabStats.Text = "Stats";
			tabStats.UseVisualStyleBackColor = true;
			// 
			// tabAdmin
			// 
			tabAdmin.Location = new System.Drawing.Point(4, 24);
			tabAdmin.Name = "tabAdmin";
			tabAdmin.Padding = new System.Windows.Forms.Padding(3);
			tabAdmin.Size = new System.Drawing.Size(966, 422);
			tabAdmin.TabIndex = 10;
			tabAdmin.Text = "Admins";
			tabAdmin.UseVisualStyleBackColor = true;
			// 
			// openFileDialog
			// 
			openFileDialog.FileName = "openFileDialog";
			// 
			// ServerManagerUI
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(984, 485);
			Controls.Add(mainPanel);
			Controls.Add(toolStrip);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
			MaximizeBox = false;
			MaximumSize = new System.Drawing.Size(1000, 524);
			MinimumSize = new System.Drawing.Size(1000, 500);
			SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
        private TabPage tabAdmin;
    }
}
