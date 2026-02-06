using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkSyncShared.DTOs.tabAdmin
{
    public record UserDTO
    {
        public int UserID { get; init; }
        public string Username { get; init; } = string.Empty;
        public List<string> Permissions { get; init; } = new();
        public bool IsActive { get; init; } = true;
        public DateTime Created { get; init; }
        public DateTime? LastLogin { get; init; }
        public string Notes { get; init; } = string.Empty;
    }

    public class CreateUserRequestDTO
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public List<string> Permissions { get; set; } = new();
        public bool IsActive { get; set; }
        public string Notes { get; set; } = "";
    }

    public class UpdateUserRequestDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; } = "";
        public string? NewPassword { get; set; }
        public List<string> Permissions { get; set; } = new();
        public bool IsActive { get; set; }
        public string Notes { get; set; } = "";
    }

    public class DeleteUserRequestDTO
    {
        public int UserID { get; set; }
    }

    public class AdminCommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }
}
