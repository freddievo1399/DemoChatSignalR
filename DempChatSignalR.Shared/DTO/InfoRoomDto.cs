using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

public class InfoRoomDto
{
    /// <summary>
    /// identifier of the room
    /// </summary>
    public Guid Guid { get; set; }
    /// <summary>
    /// name of the room
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// total count of messages in the room
    /// </summary>
    public int TotalCount { get; set; } = 0;
}
