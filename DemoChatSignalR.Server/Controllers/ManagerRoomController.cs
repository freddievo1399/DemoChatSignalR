using DempChatSignalR.Shared;
using DempChatSignalR.Shared.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DemoChatSignalR.Server.Controllers
{
    [ApiController]
    [Route("api/ManagerRoom")]
    public class ManagerRoomController(CacheChatService cacheChatService) : ControllerBase, IManagerRoom
    {
        [HttpPost(nameof(Delate))]
        public async Task<Result> Delate(Guid GuidRoom)
        {
            return await cacheChatService.DeleteRoomAsync(GuidRoom);
        }

        [HttpGet(nameof(GetData))]
        public async Task<ResultsOf<InfoRoomDto>> GetData(int take, int skip)
        {
            var rlt = await cacheChatService.GetListRoomAsync(take, skip);
            if (!rlt.Success)
            {
                return rlt.Message;
            }
            return rlt.Item.Select(x => (InfoRoomDto)x).ToList();
        }
    }
}
