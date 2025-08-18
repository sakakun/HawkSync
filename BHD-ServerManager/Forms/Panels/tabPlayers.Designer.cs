namespace BHD_ServerManager.Forms.Panels
{
    partial class tabPlayers
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
            playerLayout = new TableLayoutPanel();
            SuspendLayout();
            // 
            // playerLayout
            // 
            playerLayout.ColumnCount = 5;
            playerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            playerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            playerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            playerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            playerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            playerLayout.Dock = DockStyle.Fill;
            playerLayout.Location = new Point(0, 0);
            playerLayout.Name = "playerLayout";
            playerLayout.RowCount = 10;
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            playerLayout.Size = new Size(902, 362);
            playerLayout.TabIndex = 0;
            // 
            // tabPlayers
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(playerLayout);
            MaximumSize = new Size(902, 362);
            MinimumSize = new Size(902, 362);
            Name = "tabPlayers";
            Size = new Size(902, 362);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel playerLayout;
    }
}
