using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BHD_ServerManager.Classes.SupportClasses;
using HawkSyncShared;
using HawkSyncShared.Instances;

namespace BHD_ServerManager.Forms.SubPanels
{
    public partial class FileManager : UserControl
    {
        private readonly string[] allowedExtensions = { ".bms", ".mis", ".til" };
        private theInstance? theInstance => CommonCore.theInstance;

        public FileManager()
        {
            InitializeComponent();
            LoadFiles();
        }

        private void LoadFiles()
        {
            listViewFiles.Items.Clear();

            if (theInstance == null || string.IsNullOrEmpty(theInstance.profileServerPath))
            {
                return;
            }

            if (!Directory.Exists(theInstance.profileServerPath))
            {
                MessageBox.Show($"Directory not found: {theInstance.profileServerPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var files = Directory.GetFiles(theInstance.profileServerPath)
                    .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLower()))
                    .OrderBy(f => Path.GetFileName(f));

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var item = new ListViewItem(fileInfo.Name);
                    item.SubItems.Add(FormatFileSize(fileInfo.Length));
                    item.SubItems.Add(fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.Tag = file;
                    listViewFiles.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BtnUpload_Click(object? sender, EventArgs e)
        {
            if (theInstance == null || string.IsNullOrEmpty(theInstance.profileServerPath))
            {
                MessageBox.Show("Server path is not configured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(theInstance.profileServerPath))
            {
                MessageBox.Show($"Directory not found: {theInstance.profileServerPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int uploadedCount = 0;
                    foreach (var sourceFile in openFileDialog.FileNames)
                    {
                        var fileName = Path.GetFileName(sourceFile);
                        var extension = Path.GetExtension(fileName).ToLower();

                        // Handle ZIP files
                        if (extension == ".zip")
                        {
                            uploadedCount += ExtractZipFiles(sourceFile);
                            continue;
                        }

                        // Handle individual files
                        if (!allowedExtensions.Contains(extension))
                        {
                            MessageBox.Show($"File type not allowed: {fileName}\nAllowed types: .bms, .mis, .til, .zip", "Invalid File Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        var destFile = Path.Combine(theInstance.profileServerPath, fileName);

                        if (File.Exists(destFile))
                        {
                            var result = MessageBox.Show($"File '{fileName}' already exists. Overwrite?", "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result != DialogResult.Yes)
                            {
                                continue;
                            }
                        }

                        File.Copy(sourceFile, destFile, true);
                        uploadedCount++;
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
            }
        }

        private int ExtractZipFiles(string zipFilePath)
        {
            int extractedCount = 0;

            try
            {
                using (var archive = ZipFile.OpenRead(zipFilePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        // Skip directories
                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            continue;
                        }

                        var extension = Path.GetExtension(entry.Name).ToLower();
                        
                        // Only extract allowed file types
                        if (allowedExtensions.Contains(extension))
                        {
                            // Extract to root of profileServerPath, regardless of subfolder in zip
                            var destFile = Path.Combine(theInstance!.profileServerPath, entry.Name);
                            
                            // Force overwrite
                            entry.ExtractToFile(destFile, overwrite: true);
                            extractedCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error extracting zip file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return extractedCount;
        }

        private void BtnDownload_Click(object? sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0)
            {
                return;
            }

            if (listViewFiles.SelectedItems.Count == 1)
            {
                var selectedItem = listViewFiles.SelectedItems[0];
                var sourceFile = selectedItem.Tag as string;
                if (sourceFile == null || !File.Exists(sourceFile))
                {
                    MessageBox.Show("Selected file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                saveFileDialog.FileName = Path.GetFileName(sourceFile);
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.Copy(sourceFile, saveFileDialog.FileName, true);
                        MessageBox.Show("File downloaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error downloading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        try
                        {
                            int downloadedCount = 0;
                            foreach (ListViewItem item in listViewFiles.SelectedItems)
                            {
                                var sourceFile = item.Tag as string;
                                if (sourceFile != null && File.Exists(sourceFile))
                                {
                                    var fileName = Path.GetFileName(sourceFile);
                                    var destFile = Path.Combine(folderBrowser.SelectedPath, fileName);
                                    File.Copy(sourceFile, destFile, true);
                                    downloadedCount++;
                                }
                            }
                            MessageBox.Show($"{downloadedCount} file(s) downloaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error downloading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0)
            {
                return;
            }

            // Check if any .bms files are in use by playlists
            var filesInUse = new List<string>();
            foreach (ListViewItem item in listViewFiles.SelectedItems)
            {
                var file = item.Tag as string;
                if (file != null && Path.GetExtension(file).ToLower() == ".bms")
                {
                    var fileName = Path.GetFileName(file);
                    if (IsMapInPlaylists(fileName))
                    {
                        filesInUse.Add(fileName);
                    }
                }
            }

            if (filesInUse.Count > 0)
            {
                MessageBox.Show(
                    $"The following map files cannot be deleted because they are in use by one or more playlists:\n\n{string.Join("\n", filesInUse)}\n\nPlease remove them from the playlists first.",
                    "Cannot Delete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete {listViewFiles.SelectedItems.Count} file(s)?", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int deletedCount = 0;
                    foreach (ListViewItem item in listViewFiles.SelectedItems)
                    {
                        var file = item.Tag as string;
                        if (file != null && File.Exists(file))
                        {
                            File.Delete(file);
                            deletedCount++;
                        }
                    }
                    MessageBox.Show($"{deletedCount} file(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFiles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsMapInPlaylists(string fileName)
        {
            var mapInstance = CommonCore.instanceMaps;
            if (mapInstance == null || mapInstance.Playlists == null)
            {
                return false;
            }

            // Check playlists 1-5
            for (int i = 1; i <= 5; i++)
            {
                if (mapInstance.Playlists.TryGetValue(i, out var playlist))
                {
                    if (playlist.Any(map => string.Equals(map.MapFile, fileName, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ListViewFiles_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool hasSelection = listViewFiles.SelectedItems.Count > 0;
            btnDownload.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }
    }
}
