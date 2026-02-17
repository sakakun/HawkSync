using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemoteClient.Services.Commands;
using RemoteClient.Core;
using RemoteClient.Services;

namespace RemoteClient.Forms.SubPanels
{
    public partial class FileManager : UserControl
    {

        public FileManager()
        {
            InitializeComponent();
            LoadFiles();
        }

        private async void LoadFiles()
        {
            listViewFiles.Items.Clear();
            btnRefresh.Enabled = false;

            try
            {
                var response = await ApiCore.ApiClient?.FileSystem.GetFilesAsync()!;

                if (response == null || !response.Success)
                {
                    MessageBox.Show(response?.Message ?? "Failed to load files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (var file in response.Files)
                {
                    var item = new ListViewItem(file.FileName);
                    item.SubItems.Add(FormatFileSize(file.Size));
                    item.SubItems.Add(file.LastModified.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.Tag = file.FileName;
                    listViewFiles.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadFiles();
        }

        private async void BtnUpload_Click(object? sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                btnUpload.Enabled = false;
                try
                {
                    int uploadedCount = 0;
                    foreach (var sourceFile in openFileDialog.FileNames)
                    {
                        var response = await ApiCore.ApiClient?.FileSystem.UploadFileAsync(sourceFile)!;
                        
                        if (response != null && response.Success)
                        {
                            uploadedCount += response.Count;
                        }
                        else
                        {
                            MessageBox.Show(response?.Message ?? "Upload failed", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (uploadedCount > 0)
                    {
                        MessageBox.Show($"{uploadedCount} file(s) uploaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadFiles();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnUpload.Enabled = true;
                }
            }
        }

        private async void BtnDownload_Click(object? sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0)
            {
                return;
            }

            btnDownload.Enabled = false;

            try
            {
                if (listViewFiles.SelectedItems.Count == 1)
                {
                    var selectedItem = listViewFiles.SelectedItems[0];
                    var fileName = selectedItem.Tag as string;
                    
                    if (string.IsNullOrEmpty(fileName))
                    {
                        MessageBox.Show("Invalid file selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    saveFileDialog.FileName = fileName;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var success = await ApiCore.ApiClient?.FileSystem.DownloadFileAsync(fileName, saveFileDialog.FileName)!;
                        
                        if (success)
                        {
                            MessageBox.Show("File downloaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to download file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    using (var folderBrowser = new FolderBrowserDialog())
                    {
                        folderBrowser.Description = "Select destination folder for downloaded files";
                        if (folderBrowser.ShowDialog() == DialogResult.OK)
                        {
                            int downloadedCount = 0;
                            foreach (ListViewItem item in listViewFiles.SelectedItems)
                            {
                                var fileName = item.Tag as string;
                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    var destFile = Path.Combine(folderBrowser.SelectedPath, fileName);
                                    var success = await ApiCore.ApiClient?.FileSystem.DownloadFileAsync(fileName, destFile)!;
                                    if (success)
                                    {
                                        downloadedCount++;
                                    }
                                }
                            }
                            MessageBox.Show($"{downloadedCount} file(s) downloaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDownload.Enabled = true;
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0)
            {
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete {listViewFiles.SelectedItems.Count} file(s)?", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                btnDelete.Enabled = false;

                try
                {
                    var fileNames = new List<string>();
                    foreach (ListViewItem item in listViewFiles.SelectedItems)
                    {
                        var fileName = item.Tag as string;
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            fileNames.Add(fileName);
                        }
                    }

                    var response = await ApiCore.ApiClient?.FileSystem.DeleteFilesAsync(fileNames)!;
                    
                    if (response != null && response.Success)
                    {
                        MessageBox.Show(response.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadFiles();
                    }
                    else
                    {
                        MessageBox.Show(response?.Message ?? "Delete failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnDelete.Enabled = true;
                }
            }
        }

        private void ListViewFiles_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool hasSelection = listViewFiles.SelectedItems.Count > 0;
            btnDownload.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }
    }
}

