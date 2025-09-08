using System;
using System.Collections.Generic;

namespace BHD_SharedResources.Classes.Instances
{
    public class adminInstance
    {
        public List<AdminAccount> Admins { get; set; } = new List<AdminAccount>();
        public List<AdminLog> Logs { get; set; } = new List<AdminLog>();
    }

    public class AdminAccount
    {
        public int UserId { get; set; } = 0;
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public AdminRoles Role { get; set; } = AdminRoles.None;

        public bool IsOnline { get; set; } = false;
        public DateTime LastSeen { get; set; } = DateTime.MinValue;
    }

    public class AdminLog
    {
        public int LogId { get; set; } = 0;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int UserId { get; set; } = 0;
        public required string Action { get; set; } = string.Empty;
    }

    public enum AdminRoles
    {
        None = 0,
        Moderator = 1,
        Admin = 2
    }
}