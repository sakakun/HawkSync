using HawkSyncShared.DTOs.tabAdmin;
using HawkSyncShared.DTOs.API;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RemoteClient.Services.Commands;

public class AdminService
{
    private readonly ApiClient _apiClient;

    public AdminService(ApiClient ApiClient)
    {
        _apiClient = ApiClient;
    }
    
    public async Task<AdminCommandResult> CreateUserAsync(CreateUserRequestDTO request)
    {
        var result = await _apiClient.SendCommandAsync("/api/admin/user/create", request);
        return new AdminCommandResult { Success = result.Success, Message = result.Message };
    }

    public async Task<AdminCommandResult> UpdateUserAsync(UpdateUserRequestDTO request)
    {
        var result = await _apiClient.SendCommandAsync("/api/admin/user/update", request);
        return new AdminCommandResult { Success = result.Success, Message = result.Message };
    }

    public async Task<AdminCommandResult> DeleteUserAsync(int userId)
    {
        var request = new DeleteUserRequestDTO { UserID = userId };
        var result = await _apiClient.SendCommandAsync("/api/admin/user/delete", request);
        return new AdminCommandResult { Success = result.Success, Message = result.Message };
    }
        
}

