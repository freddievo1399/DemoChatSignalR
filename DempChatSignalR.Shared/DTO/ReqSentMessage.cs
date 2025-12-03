using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

public class ReqSentMessage
{
    public Guid GuidRoom { get; set; }
    public required string UserName { get; set; }
    public required string Message { get; set; }
}
