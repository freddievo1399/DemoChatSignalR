using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared
{
    public interface IHome
    {
        [Post(nameof(CreateRoom))]
        Task<ResultOf<InfoRoomDto>> CreateRoom(ReqCreateRoom reqCreateHome);
    }
}
