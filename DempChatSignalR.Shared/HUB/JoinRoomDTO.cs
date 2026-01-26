using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared
{
    public class JoinRoomDTO
    {
        public required Guid GuidUser { get; set; }
        public required string userName { get; set; }
        public required Guid RoomId { get; set; }
    }
}
