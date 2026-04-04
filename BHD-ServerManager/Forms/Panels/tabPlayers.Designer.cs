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
			playerTable = new TableLayoutPanel();
			SuspendLayout();
			// 
			// playerTable1
			// 
			playerTable.ColumnCount = 5;
			playerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable.Dock = DockStyle.Fill;
			playerTable.Location = new Point(5, 5);
			playerTable.Margin = new Padding(0);
			playerTable.Name = "playerTable";
			playerTable.Padding = new Padding(5);
			playerTable.RowCount = 11;
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
			playerTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			playerTable.Size = new Size(956, 412);
			playerTable.TabIndex = 0;
			// 
			// tabPlayers
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(playerTable);
			Margin = new Padding(0);
			Name = "tabPlayers";
			Padding = new Padding(5);
			Size = new Size(966, 422);
			ResumeLayout(false);
		}

		#endregion

		private TableLayoutPanel playerTable;
	}
}
