using HawkSyncShared;
using RemoteClient.Core;
using RemoteClient.Forms;

namespace RemoteClient;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        // Encoding for legacy code pages
		System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        CommonCore.InitializeCore();

        // Show login form
        using var loginForm = new LoginForm();
        
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            // Initialize API infrastructure
            var initTask = ApiCore.InitializeAsync(
                loginForm.ServerUrl,
                loginForm.JwtToken!,
                loginForm.User!
            );
            
            // Show loading message
            var loadingForm = new Form
            {
                Text = "Connecting...",
                Size = new Size(300, 100),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None
            };
            loadingForm.Controls.Add(new Label
            {
                Text = "Connecting to server...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
            loadingForm.Show();
            
            // Wait for connection
            initTask.Wait();
            loadingForm.Close();
            
            // Show main UI
            Application.Run(new ServerManagerUI());
            
            // Cleanup on exit
            ApiCore.ShutdownAsync().Wait();
        }
    }
}