using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

public class ReqLoadMessageByRange
{
    public Guid GuidRoom { get; set; }
    public int FromIndex { get; set; }
    public int ToIndex { get; set; }
}
