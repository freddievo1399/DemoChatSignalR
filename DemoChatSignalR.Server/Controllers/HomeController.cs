using DempChatSignalR.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DemoChatSignalR.Server.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController() : ControllerBase, IHome
    {
        [HttpPost(nameof(CreateRoom))]
        public Task<ResultOf<InfoRoomDto>> CreateRoom(ReqCreateRoom reqCreateHome)
        {
            throw new NotImplementedException();
        }
    }
}
