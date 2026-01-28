namespace BHD_ServerManager.Classes.PlayerManagementClasses
{
    partial class PlayerCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            playerTeamIcon = new FontAwesome.Sharp.IconPictureBox();
            playerContextMenuIcon = new FontAwesome.Sharp.IconPictureBox();
            label_dataPlayerNameRole = new Label();
            label_dataIPinfo = new Label();
            label_dataSlotNum = new Label();
            player_Tooltip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)playerTeamIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)playerContextMenuIcon).BeginInit();
            SuspendLayout();
            // 
            // playerTeamIcon
            // 
            playerTeamIcon.BackColor = Color.Transparent;
            playerTeamIcon.BackgroundImageLayout = ImageLayout.None;
            playerTeamIcon.ForeColor = SystemColors.ControlText;
            playerTeamIcon.IconChar = FontAwesome.Sharp.IconChar.PersonHiking;
            playerTeamIcon.IconColor = SystemColors.ControlText;
            playerTeamIcon.IconFont = FontAwesome.Sharp.IconFont.Auto;
            playerTeamIcon.IconSize = 35;
            playerTeamIcon.Location = new Point(3, 4);
            playerTeamIcon.Name = "playerTeamIcon";
            playerTeamIcon.Size = new Size(37, 35);
            playerTeamIcon.TabIndex = 0;
            playerTeamIcon.TabStop = false;
            // 
            // playerContextMenuIcon
            // 
            playerContextMenuIcon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            playerContextMenuIcon.BackColor = Color.Transparent;
            playerContextMenuIcon.BackgroundImageLayout = ImageLayout.Center;
            playerContextMenuIcon.Cursor = Cursors.Hand;
            playerContextMenuIcon.ForeColor = SystemColors.ControlText;
            playerContextMenuIcon.IconChar = FontAwesome.Sharp.IconChar.EllipsisV;
            playerContextMenuIcon.IconColor = SystemColors.ControlText;
            playerContextMenuIcon.IconFont = FontAwesome.Sharp.IconFont.Auto;
            playerContextMenuIcon.IconSize = 33;
            playerContextMenuIcon.Location = new Point(161, 7);
            playerContextMenuIcon.Margin = new Padding(0);
            playerContextMenuIcon.Name = "playerContextMenuIcon";
            playerContextMenuIcon.Size = new Size(33, 35);
            playerContextMenuIcon.TabIndex = 1;
            playerContextMenuIcon.TabStop = false;
            // 
            // label_dataPlayerNameRole
            // 
            label_dataPlayerNameRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label_dataPlayerNameRole.BackColor = Color.Transparent;
            label_dataPlayerNameRole.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_dataPlayerNameRole.Location = new Point(50, 4);
            label_dataPlayerNameRole.Name = "label_dataPlayerNameRole";
            label_dataPlayerNameRole.Size = new Size(100, 15);
            label_dataPlayerNameRole.TabIndex = 2;
            label_dataPlayerNameRole.Text = "Slot Empty";
            label_dataPlayerNameRole.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label_dataIPinfo
            // 
            label_dataIPinfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label_dataIPinfo.BackColor = Color.Transparent;
            label_dataIPinfo.Location = new Point(50, 19);
            label_dataIPinfo.Name = "label_dataIPinfo";
            label_dataIPinfo.Size = new Size(100, 15);
            label_dataIPinfo.TabIndex = 3;
            label_dataIPinfo.Text = "000.000.000.000";
            label_dataIPinfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label_dataSlotNum
            // 
            label_dataSlotNum.AutoSize = true;
            label_dataSlotNum.FlatStyle = FlatStyle.Flat;
            label_dataSlotNum.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_dataSlotNum.Location = new Point(32, 4);
            label_dataSlotNum.Margin = new Padding(0);
            label_dataSlotNum.Name = "label_dataSlotNum";
            label_dataSlotNum.Size = new Size(21, 15);
            label_dataSlotNum.TabIndex = 4;
            label_dataSlotNum.Text = "01";
            label_dataSlotNum.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // PlayerCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(playerContextMenuIcon);
            Controls.Add(label_dataSlotNum);
            Controls.Add(playerTeamIcon);
            Controls.Add(label_dataPlayerNameRole);
            Controls.Add(label_dataIPinfo);
            Name = "PlayerCard";
            Size = new Size(193, 42);
            ((System.ComponentModel.ISupportInitialize)playerTeamIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)playerContextMenuIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FontAwesome.Sharp.IconPictureBox playerTeamIcon;
        private FontAwesome.Sharp.IconPictureBox playerContextMenuIcon;
        private Label label_dataPlayerNameRole;
        private Label label_dataIPinfo;
        private Label label_dataSlotNum;
        private ToolTip player_Tooltip;
    }
}
