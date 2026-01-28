using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHD_ServerManager.Classes.ObjectClasses
{ 
    /// <summary>
    /// Represents a user record from the database.
    /// </summary>
    public class UserRecord
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
