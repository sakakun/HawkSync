using RemoteClient.Services;
using HawkSyncShared.DTOs.API;

namespace RemoteClient.Forms;

public partial class LoginForm : Form
{
    private ApiClient? _apiClient;

    public string? JwtToken { get; private set; }
    public UserInfoDTO? User { get; private set; }
    public string ServerUrl { get; private set; } = "http://localhost:5000";

    public LoginForm()
    {
        InitializeComponent();
        LoadSavedSettings();
    }

    private async void BtnLogin_Click(object sender, EventArgs e)
    {
        string serverUrl = txtServerUrl.Text.Trim();
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        // Validation
        if (string.IsNullOrEmpty(serverUrl))
        {
            MessageBox.Show("Please enter server URL.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtServerUrl.Focus();
            return;
        }

        if (string.IsNullOrEmpty(username))
        {
            MessageBox.Show("Please enter username.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUsername.Focus();
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Please enter password.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPassword.Focus();
            return;
        }

        // Disable UI during login
        SetControlsEnabled(false);
        lblStatus.Text = "Connecting to server...";
        lblStatus.ForeColor = Color.Blue;

        try
        {
            _apiClient = new ApiClient(serverUrl);
            
            lblStatus.Text = "Authenticating...";
            var response = await _apiClient.LoginAsync(username, password);

            if (response.Success && response.User != null)
            {
                JwtToken = response.Token;
                User = response.User;
                ServerUrl = serverUrl;

                lblStatus.Text = "Login successful!";
                lblStatus.ForeColor = Color.Green;

                // Save settings if remember is checked
                if (chkRememberServer.Checked)
                {
                    SaveSettings(serverUrl, username);
                }

                // Close with OK result
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblStatus.Text = "Login failed";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show(response.Message, "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetControlsEnabled(true);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Connection error";
            lblStatus.ForeColor = Color.Red;
            MessageBox.Show($"Failed to connect to server:\n\n{ex.Message}",
                "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetControlsEnabled(true);
        }
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void SetControlsEnabled(bool enabled)
    {
        txtServerUrl.Enabled = enabled;
        txtUsername.Enabled = enabled;
        txtPassword.Enabled = enabled;
        chkRememberServer.Enabled = enabled;
        btnLogin.Enabled = enabled;
        btnCancel.Enabled = enabled;
    }

    private void LoadSavedSettings()
    {
        try
        {
            string settingsPath = GetSettingsFilePath();
            if (File.Exists(settingsPath))
            {
                var lines = File.ReadAllLines(settingsPath);
                if (lines.Length >= 1)
                {
                    txtServerUrl.Text = lines[0];
                    chkRememberServer.Checked = true;
                }
                if (lines.Length >= 2)
                {
                    txtUsername.Text = lines[1];
                }
            }
        }
        catch { }
    }

    private void SaveSettings(string serverUrl, string username)
    {
        try
        {
            string settingsPath = GetSettingsFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
            File.WriteAllLines(settingsPath, new[] { serverUrl, username });
        }
        catch { }
    }

    private static string GetSettingsFilePath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "HawkSync", "client-settings.txt");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _apiClient?.Dispose();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }
}