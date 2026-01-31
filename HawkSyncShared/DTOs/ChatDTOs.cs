using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawkSyncShared.DTOs;

public class ChatDTOs
{
    // ================================================================================
    // DTOs (Data Transfer Objects)
    // ================================================================================

    /// <summary>
    /// Chat log entry for display
    /// </summary>
    public record ChatLogEntryDTO(
        DateTime Timestamp,
        int MessageType,
        int MessageType2,
        string PlayerName,
        string MessageText,
        string TeamDisplay
    );

    /// <summary>
    /// Chat log query parameters
    /// </summary>
    public record ChatLogQueryDTO(
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        int MaxResults = 1000
    );
}

