using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BHD_ServerManager.API.Hubs;
using HawkSyncShared.SupportClasses;
using BHD_ServerManager.Classes.SupportClasses;

namespace BHD_ServerManager.API.Services;

/// <summary>
/// Embedded API host that runs alongside WinForms UI
/// </summary>
public class EmbeddedApiHost
{
    private IHost? _host;
    private Task? _runTask;
    private readonly CancellationTokenSource _cts = new();

    /// <summary>
    /// Start the embedded API server
    /// </summary>
    public void Start(int port = 5000)
    {
        var jwtKey = "YourSuperSecretKeyThatIsAtLeast32CharactersLongForJWT!";

        _host = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls($"http://0.0.0.0:{port}");
                webBuilder.ConfigureServices(services =>
                {
                    services.AddControllers();
                    services.AddSignalR();

                    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = "BHD.ServerManager",
                                ValidAudience = "BHD.RemoteClient",
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                            };

                            options.Events = new JwtBearerEvents
                            {
                                OnMessageReceived = context =>
                                {
                                    var accessToken = context.Request.Query["access_token"];
                                    if (!string.IsNullOrEmpty(accessToken) &&
                                        context.HttpContext.Request.Path.StartsWithSegments("/hubs/server"))
                                    {
                                        context.Token = accessToken;
                                    }
                                    return Task.CompletedTask;
                                }
                            };
                        });

                    services.AddAuthorization();

                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowAll", policy =>
                        {
                            policy.SetIsOriginAllowed(_ => true)
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials();
                        });
                    });

                    services.AddHostedService<InstanceBroadcastService>();
                });

                webBuilder.Configure(app =>
                {
                    app.UseCors("AllowAll");
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapHub<ServerHub>("/hubs/server");
                    });
                });
            })
            .Build();

        _runTask = _host.RunAsync(_cts.Token);

        AppDebug.Log("EmbeddedApiHost", $"API server started on http://0.0.0.0:{port}");
    }

    /// <summary>
    /// Stop the embedded API server
    /// </summary>
    public async Task StopAsync()
    {
        if (_host != null)
        {
            _cts.Cancel();
            await _host.StopAsync();
            _host.Dispose();
            AppDebug.Log("EmbeddedApiHost", "API server stopped");
        }
    }
}