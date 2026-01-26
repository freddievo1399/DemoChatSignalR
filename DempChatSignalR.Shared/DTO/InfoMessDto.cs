using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

public class InfoMessDto
{
    /// <summary>
    /// identifier of the message
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// sent date and time of the message
    /// </summary>
    public DateTime DateTimeSent { get; set; }
    /// <summary>
    /// message text
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    /// user name who sent the message
    /// If System sent the message, this value is "System"
    /// </summary>
    public required string UserGuid { get; set; }
}
