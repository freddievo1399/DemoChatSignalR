using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

public class ReqSentMessage
{
    public required string GuidUser { get; set; }
    public required string Message { get; set; }
}
