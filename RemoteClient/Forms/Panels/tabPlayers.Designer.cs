namespace RemoteClient.Forms.Panels
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
			SuspendLayout();
			// 
			// playerTable1
			// 
			playerTable1.ColumnCount = 5;
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			playerTable1.Dock = DockStyle.Fill;
			playerTable1.Location = new Point(0, 0);
			playerTable1.Margin = new Padding(0);
			playerTable1.Name = "playerTable1";
			playerTable1.Padding = new Padding(0, 5, 0, 0);
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
			playerTable1.Size = new Size(966, 422);
			playerTable1.TabIndex = 0;
			// 
			// tabPlayers
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(playerTable1);
			Margin = new Padding(0);
			Name = "tabPlayers";
			Size = new Size(966, 422);
			ResumeLayout(false);
		}

		#endregion

		private TableLayoutPanel playerTable1;
    }
}
