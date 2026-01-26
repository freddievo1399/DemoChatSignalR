using DempChatSignalR.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.RegularExpressions;

namespace DemoChatSignalR.Server
{
    public class HubChatRoom(CacheChatService cacheChatService) : Hub
    {
        private Guid GuidUserInContext =>
    Context.Items.TryGetValue("GuidUser", out var val)
    ? Guid.Parse(val?.ToString() ?? "")
    : Guid.Empty;
        private Guid roomIdInContext =>
    Context.Items.TryGetValue("RoomId", out var val)
    ? Guid.Parse(val?.ToString() ?? "")
    : Guid.Empty;

        /// <summary>
        /// Sent JoinRoom
        /// </summary>
        /// <param name="GuidUser"></param>
        /// <param name="RoomId"></param>
        /// <returns></returns>
        public async Task<Result> JoinRoom(JoinRoomDTO joinRoomDTO)
        {
            try
            {
                Context.Items["GuidUser"] = joinRoomDTO.GuidUser;
                Context.Items["RoomId"] = joinRoomDTO.RoomId;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"ChatRoom_{joinRoomDTO.RoomId}");
                var rltUpdateName = await ReqUpdateName(new() { UserGuid = joinRoomDTO.GuidUser.ToString(), UserName = joinRoomDTO.userName, IsActive = true });
                if (rltUpdateName.Success)
                {
                    if (rltUpdateName.Item == 1)
                    {
                        var rlt = await SentMessage(new() { GuidUser = "System", Message = $"User name: {joinRoomDTO.GuidUser} has join group Chat" });
                    }
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        private async Task<ResultOf<int>> ReqUpdateName(InfoUser UserNew)
        {
            try
            {
                Context.Items["GuidUser"] = UserNew.UserGuid;
                var rlt = await cacheChatService.CreateOrUpdateUser(roomIdInContext, UserNew.UserName, UserNew.UserGuid);
                if (rlt.Success || rlt.Item == 2)
                {
                    await Clients.OthersInGroup($"ChatRoom_{roomIdInContext}").SendAsync("ResUpdateName", UserNew);
                }
                return rlt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Sent SentMessage
        /// Receive ReceiveMessage
        /// </summary>
        /// <param name="reqSentMessage"></param>
        /// <returns></returns>
        public async Task<Result> SentMessage(ReqSentMessage reqSentMessage)
        {
            try
            {

                var rlt = await cacheChatService.AddMessageAsync(roomIdInContext, reqSentMessage.GuidUser, reqSentMessage.Message);
                if (!rlt.Success)
                {
                    return rlt.Message;
                }
                await Clients.Group($"ChatRoom_{roomIdInContext}").SendAsync("ReceiveMessage", (InfoMessDto)rlt.Item);
                return true;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Sent NotfiMemberGroup
        /// Receive ReceiveNotfiMemberGroup
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="GuidUser"></param>
        /// <param name="isIn"></param>
        /// <returns></returns>
        public async Task<Result> NotfiOnlineUser(OnlineUserDto OnlineUser)
        {
            try
            {
                if (roomIdInContext == Guid.Empty)
                    return "Guid room is empty";
                await Clients.Group($"ChatRoom_{roomIdInContext}").SendAsync("ReceiveNotfiMemberGroup", OnlineUser);

                return true;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Event out
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await NotfiOnlineUser(new() { ConnectionId = Context.ConnectionId, GuidUser = GuidUserInContext.ToString(), IsOnline = false });
            await base.OnDisconnectedAsync(exception);
        }

    }
}
