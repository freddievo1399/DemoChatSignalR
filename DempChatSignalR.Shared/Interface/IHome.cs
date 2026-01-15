using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared
{
    [BasePath("api/home/")]
    public interface IHome
    {
        [Post(nameof(CreateRoom))]
        Task<ResultOf<InfoRoomDto>> CreateRoom([Body]ReqCreateRoom reqCreateHome);
    }
}
