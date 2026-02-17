namespace BHD_ServerManager.Forms.SubPanels
{
    partial class FileManager
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
            listViewFiles = new ListView();
            columnFileName = new ColumnHeader();
            columnFileSize = new ColumnHeader();
            columnLastModified = new ColumnHeader();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnRefresh = new Button();
            btnUpload = new Button();
            btnDownload = new Button();
            btnDelete = new Button();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // listViewFiles
            // 
            listViewFiles.Columns.AddRange(new ColumnHeader[] { columnFileName, columnFileSize, columnLastModified });
            listViewFiles.Dock = DockStyle.Fill;
            listViewFiles.FullRowSelect = true;
            listViewFiles.Location = new Point(0, 0);
            listViewFiles.Name = "listViewFiles";
            listViewFiles.Size = new Size(434, 343);
            listViewFiles.TabIndex = 0;
            listViewFiles.UseCompatibleStateImageBehavior = false;
            listViewFiles.View = View.Details;
            listViewFiles.SelectedIndexChanged += ListViewFiles_SelectedIndexChanged;
            // 
            // columnFileName
            // 
            columnFileName.Text = "File Name";
            columnFileName.Width = 200;
            // 
            // columnFileSize
            // 
            columnFileSize.Text = "Size";
            columnFileSize.Width = 100;
            // 
            // columnLastModified
            // 
            columnLastModified.Text = "Last Modified";
            columnLastModified.Width = 130;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(btnRefresh, 0, 0);
            tableLayoutPanel1.Controls.Add(btnUpload, 1, 0);
            tableLayoutPanel1.Controls.Add(btnDownload, 2, 0);
            tableLayoutPanel1.Controls.Add(btnDelete, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 343);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(434, 45);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Dock = DockStyle.Fill;
            btnRefresh.Location = new Point(3, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(102, 39);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnUpload
            // 
            btnUpload.Dock = DockStyle.Fill;
            btnUpload.Location = new Point(111, 3);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(102, 39);
            btnUpload.TabIndex = 1;
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += BtnUpload_Click;
            // 
            // btnDownload
            // 
            btnDownload.Dock = DockStyle.Fill;
            btnDownload.Enabled = false;
            btnDownload.Location = new Point(219, 3);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(102, 39);
            btnDownload.TabIndex = 2;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += BtnDownload_Click;
            // 
            // btnDelete
            // 
            btnDelete.Dock = DockStyle.Fill;
            btnDelete.Enabled = false;
            btnDelete.Location = new Point(327, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(104, 39);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += BtnDelete_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "All Supported Files|*.bms;*.mis;*.til;*.zip|Map Files|*.bms;*.mis;*.til|Zip Files|*.zip|All Files|*.*";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select Files to Upload";
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "Map Files|*.bms;*.mis;*.til|All Files|*.*";
            saveFileDialog.Title = "Save File";
            // 
            // FileManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(listViewFiles);
            Controls.Add(tableLayoutPanel1);
            MaximumSize = new Size(434, 388);
            MinimumSize = new Size(434, 388);
            Name = "FileManager";
            Size = new Size(434, 388);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private ListView listViewFiles;
        private ColumnHeader columnFileName;
        private ColumnHeader columnFileSize;
        private ColumnHeader columnLastModified;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnRefresh;
        private Button btnUpload;
        private Button btnDownload;
        private Button btnDelete;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        #endregion
    }
}
