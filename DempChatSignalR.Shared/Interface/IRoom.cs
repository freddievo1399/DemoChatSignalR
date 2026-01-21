using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

[BasePath("api/room/")]
public interface IRoom
{
    [Get(nameof(GetInfoRoom))]
    Task<ResultOf<InfoRoomDto>> GetInfoRoom([Query]Guid guid);
    [Get(nameof(GetChatHistory))]
    Task<ResultsOf<InfoMessDto>> GetChatHistory([Query] ReqLoadMessageByRange reqLoadMessageByRange);
}
