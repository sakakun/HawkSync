using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkSyncShared.DTOs
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
}
