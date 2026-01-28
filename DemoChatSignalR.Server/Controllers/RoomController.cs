using DempChatSignalR.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DemoChatSignalR.Server.Controllers;

[ApiController]
[Route("api/room")]
public class RoomController(CacheChatService cacheChatService) : ControllerBase, IRoom
{
    [HttpPost(nameof(GetChatHistory))]
    public async Task<ResultsOf<InfoMessDto>> GetChatHistory([FromBody] ReqLoadMessageByRange reqLoadMessageByRange)
    {
        var rlt = await cacheChatService.GetMessages(reqLoadMessageByRange.GuidRoom, reqLoadMessageByRange.FromIndex, reqLoadMessageByRange.ToIndex);
        if (!rlt.Success)
        {
            return rlt.Message;
        }
        return rlt.Items.ToList<InfoMessDto>();
    }

    [HttpGet(nameof(GetInfoRoom))]
    public async Task<ResultOf<InfoRoomDto>> GetInfoRoom([FromQuery] Guid guid)
    {
        Guid GuidDemo = Guid.Parse("0a0852cc-1ceb-442c-ae11-72dba99fa01c");
        if (guid == GuidDemo)
        {
            var rltDemo = await cacheChatService.CreateRoomAsync(guid, "Demo");
            if (rltDemo.Success)
            {
                return rltDemo.Item;
            }
            return rltDemo.Message;
        }
        var rlt = await cacheChatService.GetRoomAsync(guid);
        if (!rlt.Success)
        {
            return rlt.Message;
        }
        return rlt.Item;
    }
}
