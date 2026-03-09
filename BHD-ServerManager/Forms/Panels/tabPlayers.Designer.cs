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
			playerTable1 = new TableLayoutPanel();
			playerPanels = new TabControl();
			tabPage1 = new TabPage();
			tabPage2 = new TabPage();
			playerTable2 = new TableLayoutPanel();
			playerPanels.SuspendLayout();
			tabPage1.SuspendLayout();
			tabPage2.SuspendLayout();
			SuspendLayout();
			// 
			// playerTable1
			// 
			playerTable1.ColumnCount = 4;
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
			playerTable1.Dock = DockStyle.Fill;
			playerTable1.Location = new Point(70, 10);
			playerTable1.Margin = new Padding(0);
			playerTable1.Name = "playerTable1";
			playerTable1.RowCount = 11;
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			playerTable1.Size = new Size(795, 404);
			playerTable1.TabIndex = 0;
			// 
			// playerPanels
			// 
			playerPanels.Alignment = TabAlignment.Right;
			playerPanels.Controls.Add(tabPage1);
			playerPanels.Controls.Add(tabPage2);
			playerPanels.Dock = DockStyle.Fill;
			playerPanels.Location = new Point(0, 0);
			playerPanels.Margin = new Padding(0);
			playerPanels.Multiline = true;
			playerPanels.Name = "playerPanels";
			playerPanels.Padding = new Point(0, 0);
			playerPanels.SelectedIndex = 0;
			playerPanels.Size = new Size(966, 422);
			playerPanels.TabIndex = 1;
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(playerTable1);
			tabPage1.Location = new Point(4, 4);
			tabPage1.Margin = new Padding(0);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new Padding(70, 10, 70, 0);
			tabPage1.Size = new Size(935, 414);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "Players (01-40)";
			tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			tabPage2.Controls.Add(playerTable2);
			tabPage2.Location = new Point(4, 4);
			tabPage2.Margin = new Padding(0);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new Padding(70, 10, 70, 0);
			tabPage2.Size = new Size(935, 414);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "Players (41-80)";
			tabPage2.UseVisualStyleBackColor = true;
			// 
			// playerTable2
			// 
			playerTable2.ColumnCount = 4;
			playerTable2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
			playerTable2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
			playerTable2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
			playerTable2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
			playerTable2.Dock = DockStyle.Fill;
			playerTable2.Location = new Point(70, 10);
			playerTable2.Margin = new Padding(0);
			playerTable2.Name = "playerTable2";
			playerTable2.RowCount = 11;
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			playerTable2.Size = new Size(795, 404);
			playerTable2.TabIndex = 1;
			// 
			// tabPlayers
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(playerPanels);
			Margin = new Padding(0);
			Name = "tabPlayers";
			Size = new Size(966, 422);
			playerPanels.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			tabPage2.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private TableLayoutPanel playerTable1;
		private TabControl playerPanels;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private TableLayoutPanel playerTable2;
	}
}
