using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using ServerManager.API.Hubs;

namespace ServerManager.API.Services;

/// <summary>
/// Embedded API host that runs alongside WinForms UI
/// </summary>
public class EmbeddedApiHost
{
    private IHost? _host;
    private readonly CancellationTokenSource _cts = new();

    /// <summary>
    /// Start the embedded API server
    /// </summary>
    public void Start(int port = 5000)
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls($"http://0.0.0.0:{port}");
                webBuilder.ConfigureServices(services =>
                {
                    services.AddControllers();
                    services.AddSignalR();

                    // Rate limiting policy for login endpoint:
                    // 5 attempts per minute per remote IP
                    services.AddRateLimiter(options =>
                    {
                        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                        options.OnRejected = async (context, _) =>
                        {
                            context.HttpContext.Response.ContentType = "application/json";
                            await context.HttpContext.Response.WriteAsync(
                                "{\"success\":false,\"message\":\"Too many login attempts. Please wait 60 seconds and try again.\"}");
                        };

                        options.AddPolicy("LoginPolicy", httpContext =>
                        {
                            var partitionKey = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                            return RateLimitPartition.GetSlidingWindowLimiter(
                                partitionKey,
                                _ => new SlidingWindowRateLimiterOptions
                                {
                                    PermitLimit = 5,
                                    Window = TimeSpan.FromMinutes(1),
                                    SegmentsPerWindow = 6,
                                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                    QueueLimit = 0,
                                    AutoReplenishment = true
                                });
                        });
                        
                        options.OnRejected = async (context, _) =>
                        {
                            var response = context.HttpContext.Response;
                            response.StatusCode = StatusCodes.Status429TooManyRequests;
                            response.ContentType = "application/json";
                        
                            // Use limiter metadata if available; fallback to 60 seconds.
                            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                            {
                                var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
                                response.Headers["Retry-After"] = seconds.ToString();
                            }
                            else
                            {
                                response.Headers["Retry-After"] = "60";
                            }
                        
                            await response.WriteAsync(
                                "{\"success\":false,\"message\":\"Too many login attempts. Please wait before trying again.\"}");
                        };
                        
                    });

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
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKeyProvider.JwtKey))
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

                    // Must be after routing and before endpoint execution
                    app.UseRateLimiter();

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

        _host.RunAsync(_cts.Token);
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
        }
    }
}
