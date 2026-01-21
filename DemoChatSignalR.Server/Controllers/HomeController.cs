using DempChatSignalR.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DemoChatSignalR.Server.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController(CacheChatService cacheChatService) : ControllerBase, IHome
{
    [HttpPost(nameof(CreateRoom))]
    public async Task<ResultOf<InfoRoomDto>> CreateRoom(ReqCreateRoom reqCreateHome)
    {
        var rlt = await cacheChatService.CreateRoomAsync(Guid.NewGuid(),reqCreateHome.NameRoom);
        if (rlt.Success)
        {
            return rlt.Item;
        }
        return rlt.Message;
    }
}
