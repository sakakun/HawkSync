using BHD_RemoteClient.Classes.RemoteFunctions;
using BHD_RemoteClient.Classes.RemoteFunctions.CommandProcesses;
using BHD_SharedResources.Classes.CoreObjects;
using BHD_SharedResources.Classes.InstanceManagers;
using BHD_SharedResources.Classes.Instances;
using BHD_SharedResources.Classes.SupportClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BHD_RemoteClient.Forms
{
    public partial class LoginWindow : Form
    {
        // Login Details
        private string username { get; set; } = "";
        private string password { get; set; } = "";
        private string serverAddress { get; set; } = "";
        private int serverPort { get; set; } = 8083;
        private bool rememberMe { get; set; } = false;

        public LoginWindow()
        {
            InitializeComponent();
            this.Load += LoginWindow_Load;
        }

        private void LoginWindow_Load(object? sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            string settingsPath = BHD_SharedResources.Classes.CoreObjects.CommonCore.AppDataPath + "loginSettings.json";
            try
            {
                if (!File.Exists(settingsPath))
                {
                    var defaultSettings = new LoginSettings();
                    string json = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true });
                    Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
                    File.WriteAllText(settingsPath, json);
                    username = defaultSettings.Username;
                    password = defaultSettings.Password;
                    serverAddress = defaultSettings.ServerAddress;
                    serverPort = defaultSettings.ServerPort;
                    rememberMe = defaultSettings.RememberMe;
                    function_UpdateUI(false);
                    SaveSettings();
                    return;
                }

                string fileContent = File.ReadAllText(settingsPath);
                var settings = JsonSerializer.Deserialize<LoginSettings>(fileContent) ?? new LoginSettings();
                username = settings.Username;
                password = settings.Password;
                serverAddress = settings.ServerAddress;
                serverPort = settings.ServerPort;
                rememberMe = settings.RememberMe;
                function_UpdateUI(false);

            }
            catch (Exception ex)
            {
                AppDebug.Log("LoginWindow", $"Error loading settings: {ex.Message}");
                // Optionally log ex.Message
                // Fallback to defaults
                username = "";
                password = "";
                serverAddress = "";
                serverPort = 8083;
                rememberMe = false;
                function_UpdateUI(false);
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            string settingsPath = BHD_SharedResources.Classes.CoreObjects.CommonCore.AppDataPath + "loginSettings.json";
            try
            {
                var settings = new LoginSettings
                {
                    Username = username,
                    Password = password,
                    ServerAddress = serverAddress,
                    ServerPort = serverPort,
                    RememberMe = rememberMe
                };
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                AppDebug.Log("LoginWindow", $"Error loading settings: {ex.Message}");
                // Optionally log ex.Message
                // Swallow exception to prevent crash
            }
        }

        private void function_UpdateUI(bool setVariables = false)
        {
            // If setVariables is true, update the private variables with the UI control values
            if (setVariables)
            {
                username = tb_username.Text;
                password = tb_password.Text;
                serverAddress = tb_serverAddress.Text;
                serverPort = (int)num_serverPort.Value;
                rememberMe = cb_rememberPassword.Checked;
            }
            else
            {
                // Otherwise, update the UI controls with the private variable values
                tb_username.Text = username;
                tb_password.Text = password;
                tb_serverAddress.Text = serverAddress;
                num_serverPort.Value = serverPort;
                cb_rememberPassword.Checked = rememberMe;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (cb_rememberPassword.Checked)
            {
                function_UpdateUI(true);
                SaveSettings();
            }
            else
            {
                // If not remembering, clear the password field
                username = "";
                password = "";
                serverAddress = "";
                serverPort = 8083;
                rememberMe = false;
                function_UpdateUI(false);
                SaveSettings();
            }

            btn_login.Enabled = false;

            try
            {
                Program.theRemoteClient = new RemoteClient(serverAddress, serverPort, serverPort + 1);

                bool loginSuccess = await Task.Run(() => CmdValidateUser.Login(username, password));
                if (loginSuccess)
                {
                    MessageBox.Show("Login successful!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (cb_rememberPassword.Checked)
                    {
                        // Save the login details if "Remember Me" is checked
                        SaveSettings();
                    }
                    else
                    {
                        // Clear the login details if not remembering
                        username = "";
                        password = "";
                        serverAddress = "";
                        serverPort = 8083;
                        rememberMe = false;
                        function_UpdateUI(false);
                    }

                    btn_login.Text = "Disconnect";
                    btn_login.Enabled = true;
                    btn_open.Visible = true;
                    btn_login.Click -= btnLogin_Click!;
                    btn_login.Click += btn_Disconnect_Click!;

                    MessageBox.Show("You are now connected to the server.", "Connection Established", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Initialize the server manager
                    Program.ServerManagerUI = new ServerManager();

                    // Start Tickers
                    theInstanceManager.InitializeTickers();

                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btn_login.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn_login.Enabled = true;
            }
        }

        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            if (Program.theRemoteClient != null)
            {
                Program.theRemoteClient.Disconnect();
                Program.theRemoteClient = null;

                btn_login.Enabled = true;
                btn_login.Text = "Login";
                btn_open.Visible = false;
                btn_login.Click -= btn_Disconnect_Click!;
                btn_login.Click += btnLogin_Click!;

                MessageBox.Show("You have been disconnected from the server.", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void actionClick_openServerManager(object sender, EventArgs e)
        {
            Program.ServerManagerUI!.FormClosed -= ServerManagerUI_FormClosed; // Prevent multiple subscriptions
            Program.ServerManagerUI!.FormClosed += ServerManagerUI_FormClosed;
            this.Hide(); // Hide the LoginWindow
            Program.ServerManagerUI.Show(); // Show the ServerManagerUI
        }

        private void ServerManagerUI_FormClosed(object? sender, FormClosedEventArgs e)
        {
            this.Show(); // Show the LoginWindow again
            btn_Disconnect_Click(sender!, e); // Ensure the disconnect logic is executed
        }
    }

    public class LoginSettings
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string ServerAddress { get; set; } = "";
        public int ServerPort { get; set; } = 0;
        public bool RememberMe { get; set; } = false;
    }
}
