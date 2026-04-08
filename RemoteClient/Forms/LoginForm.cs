using RemoteClient.Services;
using HawkSyncShared.DTOs.API;
using HawkSyncShared.SupportClasses;

namespace RemoteClient.Forms;

public partial class LoginForm
{
    private ApiClient? _apiClient;
    private readonly System.Windows.Forms.Timer _cooldownTimer = new() { Interval = 1000 };
    private DateTime? _cooldownUntilUtc;

    public string? JwtToken { get; private set; }
    public UserInfoDTO? User { get; private set; }
    public string ServerUrl { get; private set; } = "http://localhost:5000";

    public LoginForm()
    {
        InitializeComponent();
        _cooldownTimer.Tick += CooldownTimer_Tick;
        LoadSavedSettings();
    }

    private async void BtnLogin_Click(object sender, EventArgs e)
    {
        if (IsCooldownActive())
        {
            UpdateCooldownUi();
            return;
        }

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
                if (response.RetryAfterSeconds is { } retryAfterSeconds && retryAfterSeconds > 0)
                {
                    StartLoginCooldown(retryAfterSeconds);
                }
                else
                {
                    lblStatus.Text = "Login failed";
                    lblStatus.ForeColor = Color.Red;
                    SetControlsEnabled(true);
                }

                MessageBox.Show(response.Message, "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private bool IsCooldownActive()
    {
        return _cooldownUntilUtc.HasValue && _cooldownUntilUtc.Value > DateTime.UtcNow;
    }

    private void StartLoginCooldown(int retryAfterSeconds)
    {
        SetControlsEnabled(true);
        _cooldownUntilUtc = DateTime.UtcNow.AddSeconds(Math.Max(1, retryAfterSeconds));
        _cooldownTimer.Start();
        UpdateCooldownUi();
    }

    private void CooldownTimer_Tick(object? sender, EventArgs e)
    {
        UpdateCooldownUi();
    }

    private void UpdateCooldownUi()
    {
        if (!IsCooldownActive())
        {
            _cooldownTimer.Stop();
            _cooldownUntilUtc = null;
            btnLogin.Enabled = true;

            if (lblStatus.Text.StartsWith("Rate limited", StringComparison.OrdinalIgnoreCase))
            {
                lblStatus.Text = "Ready";
                lblStatus.ForeColor = Color.Gray;
            }

            return;
        }

        var remainingSeconds = (int)Math.Ceiling((_cooldownUntilUtc!.Value - DateTime.UtcNow).TotalSeconds);
        btnLogin.Enabled = false;
        lblStatus.Text = $"Rate limited. Try again in {Math.Max(1, remainingSeconds)}s";
        lblStatus.ForeColor = Color.DarkOrange;
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
        catch (Exception ex)
        {
            AppDebug.Log("Error Occured", AppDebug.LogLevel.Error, ex);
        }
    }

    private void SaveSettings(string serverUrl, string username)
    {
        try
        {
            string settingsPath = GetSettingsFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
            File.WriteAllLines(settingsPath, new[] { serverUrl, username });
        }
        catch (Exception ex)
        {
            AppDebug.Log("Error Occured", AppDebug.LogLevel.Error, ex);
        }
    }

    private static string GetSettingsFilePath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "HawkSync", "client-settings.txt");
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _cooldownTimer.Stop();
        _cooldownTimer.Dispose();
        base.OnFormClosed(e);
    }

}