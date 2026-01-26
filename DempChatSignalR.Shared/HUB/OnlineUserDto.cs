using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared
{
    public class OnlineUserDto
    {
        public required string ConnectionId { get; set; }
        public required string GuidUser { get; set; }
        public required bool IsOnline { get; set; }
    }
}
