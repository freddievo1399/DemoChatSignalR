using DempChatSignalR.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.RegularExpressions;

namespace DemoChatSignalR.Server
{
    public class HubChatRoom(CacheChatService cacheChatService) : Hub
    {
        private static HashSet<string> ConnectedUsers = new();
        private Guid GuidUserInContext => Guid.Parse(Context.Items["GuidUser"]?.ToString() ?? throw new Exception("GuidUser is null"));
        private Guid roomIdInContext => Guid.Parse(Context.Items["RoomId"]?.ToString() ?? throw new Exception("RoomId is null"));

        /// <summary>
        /// Sent JoinRoom
        /// </summary>
        /// <param name="GuidUser"></param>
        /// <param name="RoomId"></param>
        /// <returns></returns>
        public async Task<Result> JoinRoom(Guid GuidUser, Guid RoomId)
        {
            try
            {
                Context.Items["GuidUser"] = GuidUser;
                Context.Items["RoomId"] = RoomId;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"ChatRoom_{RoomId}");
                var rlt = await SentMessage(new() { GuidUser = "System", Message = $"User name: {GuidUser} has join group Chat" });
                if (!rlt.Success)
                {
                    return rlt;
                }
                await NotfiMemberGroup(Context.ConnectionId, GuidUser.ToString(), true);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        /// <summary>
        /// Sent ResUpdateName
        /// </summary>
        /// <param name="UserNew"></param>
        /// <returns></returns>
        public async Task<Result> ReqUpdateName(InfoUser UserNew)
        {
            try
            {
                Context.Items["GuidUser"] = UserNew.Guid;
                var rlt = await cacheChatService.CreateOrUpdateUser(roomIdInContext, UserNew.UserName, UserNew.Guid);
                await Clients.OthersInGroup($"ChatRoom_{roomIdInContext}").SendAsync("ResUpdateName", UserNew);
                return true;
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
        /// Receive ReceiveNotfiMemberGroup
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="GuidUser"></param>
        /// <param name="isIn"></param>
        /// <returns></returns>
        private async Task<Result> NotfiMemberGroup(string connectionId, string GuidUser, bool isIn)
        {
            try
            {
                if (roomIdInContext == Guid.Empty)
                    return "Guid room is empty";
                await Clients.Group($"ChatRoom_{roomIdInContext}").SendAsync("ReceiveNotfiMemberGroup", isIn ? "online" : "offline", GuidUser, connectionId);

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
            await NotfiMemberGroup(Context.ConnectionId, GuidUserInContext.ToString(), false);
        }
    }
}
