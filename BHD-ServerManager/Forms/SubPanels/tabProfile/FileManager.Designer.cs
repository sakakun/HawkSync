namespace ServerManager.Forms.SubPanels.tabProfile
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
            listViewFiles = new System.Windows.Forms.ListView();
            columnFileName = new System.Windows.Forms.ColumnHeader();
            columnFileSize = new System.Windows.Forms.ColumnHeader();
            columnLastModified = new System.Windows.Forms.ColumnHeader();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            btnRefresh = new System.Windows.Forms.Button();
            btnUpload = new System.Windows.Forms.Button();
            btnDownload = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // listViewFiles
            // 
            listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnFileName, columnFileSize, columnLastModified });
            listViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewFiles.FullRowSelect = true;
            listViewFiles.Location = new System.Drawing.Point(0, 0);
            listViewFiles.Name = "listViewFiles";
            listViewFiles.Size = new System.Drawing.Size(512, 310);
            listViewFiles.TabIndex = 0;
            listViewFiles.UseCompatibleStateImageBehavior = false;
            listViewFiles.View = System.Windows.Forms.View.Details;
            listViewFiles.SelectedIndexChanged += ListViewFiles_SelectedIndexChanged;
            // 
            // columnFileName
            // 
            columnFileName.Name = "columnFileName";
            columnFileName.Text = "File Name";
            columnFileName.Width = 232;
            // 
            // columnFileSize
            // 
            columnFileSize.Name = "columnFileSize";
            columnFileSize.Text = "Size";
            columnFileSize.Width = 110;
            // 
            // columnLastModified
            // 
            columnLastModified.Name = "columnLastModified";
            columnLastModified.Text = "Last Modified";
            columnLastModified.Width = 152;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(btnRefresh, 0, 0);
            tableLayoutPanel1.Controls.Add(btnUpload, 1, 0);
            tableLayoutPanel1.Controls.Add(btnDownload, 2, 0);
            tableLayoutPanel1.Controls.Add(btnDelete, 3, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 310);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(512, 45);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            btnRefresh.Location = new System.Drawing.Point(3, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(122, 39);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnUpload
            // 
            btnUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            btnUpload.Location = new System.Drawing.Point(131, 3);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new System.Drawing.Size(122, 39);
            btnUpload.TabIndex = 1;
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += BtnUpload_Click;
            // 
            // btnDownload
            // 
            btnDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            btnDownload.Enabled = false;
            btnDownload.Location = new System.Drawing.Point(259, 3);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new System.Drawing.Size(122, 39);
            btnDownload.TabIndex = 2;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += BtnDownload_Click;
            // 
            // btnDelete
            // 
            btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            btnDelete.Enabled = false;
            btnDelete.Location = new System.Drawing.Point(387, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(122, 39);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += BtnDelete_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = ("All Supported Files|*.bms;*.mis;*.til;*.zip|Map Files|*.bms;*.mis;*.til|Zip Files" + "|*.zip|All Files|*.*");
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
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(listViewFiles);
            Controls.Add(tableLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(0);
            MaximumSize = new System.Drawing.Size(512, 355);
            MinimumSize = new System.Drawing.Size(512, 355);
            Size = new System.Drawing.Size(512, 355);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.ListView listViewFiles;
        private ColumnHeader columnFileName;
        private ColumnHeader columnFileSize;
        private ColumnHeader columnLastModified;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnDelete;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        #endregion
    }
}
