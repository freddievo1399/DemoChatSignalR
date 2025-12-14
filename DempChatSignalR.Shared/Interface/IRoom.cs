using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

public interface IRoom
{
    [Get(nameof(GetInfoRoom))]
    Task<ResultOf<InfoRoomDto>> GetInfoRoom(Guid guid);
    [Get(nameof(GetInfoRoom))]
    Task<ResultOf<InfoMessDto>> GetInfoRoom(ReqLoadMessageByRange reqLoadMessageByRange);

}
