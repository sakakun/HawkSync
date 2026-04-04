using System.ComponentModel;

namespace BHD_ServerManager.Forms.SubPanels;

partial class PlayerCardV2
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
        label_dataPlayerNameRole = new System.Windows.Forms.Label();
        label_dataIPinfo = new System.Windows.Forms.Label();
        label_dataSlotNum = new System.Windows.Forms.Label();
        PlayerFlagIcon = new System.Windows.Forms.PictureBox();
        contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
        player_Tooltip = new System.Windows.Forms.ToolTip(components);
        ((System.ComponentModel.ISupportInitialize)playerTeamIcon).BeginInit();
        ((System.ComponentModel.ISupportInitialize)playerContextMenuIcon).BeginInit();
        ((System.ComponentModel.ISupportInitialize)PlayerFlagIcon).BeginInit();
        SuspendLayout();
        // 
        // playerTeamIcon
        // 
        playerTeamIcon.BackColor = System.Drawing.SystemColors.Control;
        playerTeamIcon.ForeColor = System.Drawing.SystemColors.ControlText;
        playerTeamIcon.IconChar = FontAwesome.Sharp.IconChar.PersonHiking;
        playerTeamIcon.IconColor = System.Drawing.SystemColors.ControlText;
        playerTeamIcon.IconFont = FontAwesome.Sharp.IconFont.Auto;
        playerTeamIcon.Location = new System.Drawing.Point(1, 3);
        playerTeamIcon.Name = "playerTeamIcon";
        playerTeamIcon.Size = new System.Drawing.Size(32, 32);
        playerTeamIcon.TabIndex = 0;
        playerTeamIcon.TabStop = false;
        // 
        // playerContextMenuIcon
        // 
        playerContextMenuIcon.BackColor = System.Drawing.SystemColors.Control;
        playerContextMenuIcon.ForeColor = System.Drawing.SystemColors.ControlText;
        playerContextMenuIcon.IconChar = FontAwesome.Sharp.IconChar.EllipsisV;
        playerContextMenuIcon.IconColor = System.Drawing.SystemColors.ControlText;
        playerContextMenuIcon.IconFont = FontAwesome.Sharp.IconFont.Auto;
        playerContextMenuIcon.IconSize = 24;
        playerContextMenuIcon.Location = new System.Drawing.Point(168, 3);
        playerContextMenuIcon.Name = "playerContextMenuIcon";
        playerContextMenuIcon.Size = new System.Drawing.Size(24, 32);
        playerContextMenuIcon.TabIndex = 1;
        playerContextMenuIcon.TabStop = false;
        // 
        // label_dataPlayerNameRole
        // 
        label_dataPlayerNameRole.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        label_dataPlayerNameRole.Location = new System.Drawing.Point(71, 4);
        label_dataPlayerNameRole.Name = "label_dataPlayerNameRole";
        label_dataPlayerNameRole.Size = new System.Drawing.Size(95, 15);
        label_dataPlayerNameRole.TabIndex = 2;
        label_dataPlayerNameRole.Text = "Slot Empty";
        label_dataPlayerNameRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label_dataIPinfo
        // 
        label_dataIPinfo.Location = new System.Drawing.Point(71, 17);
        label_dataIPinfo.Name = "label_dataIPinfo";
        label_dataIPinfo.Size = new System.Drawing.Size(95, 15);
        label_dataIPinfo.TabIndex = 3;
        label_dataIPinfo.Text = "000.000.000.000";
        label_dataIPinfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label_dataSlotNum
        // 
        label_dataSlotNum.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        label_dataSlotNum.Location = new System.Drawing.Point(32, 4);
        label_dataSlotNum.Name = "label_dataSlotNum";
        label_dataSlotNum.Size = new System.Drawing.Size(21, 15);
        label_dataSlotNum.TabIndex = 4;
        label_dataSlotNum.Text = "01";
        // 
        // PlayerFlagIcon
        // 
        PlayerFlagIcon.Location = new System.Drawing.Point(51, 4);
        PlayerFlagIcon.Name = "PlayerFlagIcon";
        PlayerFlagIcon.Size = new System.Drawing.Size(20, 15);
        PlayerFlagIcon.TabIndex = 5;
        PlayerFlagIcon.TabStop = false;
        // 
        // contextMenu
        // 
        contextMenu.Name = "contextMenu";
        contextMenu.Size = new System.Drawing.Size(61, 4);
        // 
        // PlayerCardV2
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(PlayerFlagIcon);
        Controls.Add(label_dataSlotNum);
        Controls.Add(label_dataIPinfo);
        Controls.Add(label_dataPlayerNameRole);
        Controls.Add(playerContextMenuIcon);
        Controls.Add(playerTeamIcon);
        Size = new System.Drawing.Size(195, 39);
        ((System.ComponentModel.ISupportInitialize)playerTeamIcon).EndInit();
        ((System.ComponentModel.ISupportInitialize)playerContextMenuIcon).EndInit();
        ((System.ComponentModel.ISupportInitialize)PlayerFlagIcon).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip player_Tooltip;

    private System.Windows.Forms.ContextMenuStrip contextMenu;

    private System.Windows.Forms.PictureBox PlayerFlagIcon;

    private System.Windows.Forms.Label label_dataSlotNum;

    private System.Windows.Forms.Label label_dataIPinfo;

    private System.Windows.Forms.Label label_dataPlayerNameRole;

    private FontAwesome.Sharp.IconPictureBox playerContextMenuIcon;

    private FontAwesome.Sharp.IconPictureBox playerTeamIcon;

    #endregion
}