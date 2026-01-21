using DempChatSignalR.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DemoChatSignalR.Server.Controllers;

[ApiController]
[Route("api/room")]
public class RoomController(CacheChatService cacheChatService) : ControllerBase, IRoom
{
    [HttpGet(nameof(GetChatHistory))]
    public async Task<ResultsOf<InfoMessDto>> GetChatHistory([FromQuery] ReqLoadMessageByRange reqLoadMessageByRange)
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
        var rlt = await cacheChatService.GetRoomAsync(guid);
        if (!rlt.Success)
        {
            return rlt.Message;
        }
        return rlt.Item;
    }
}
