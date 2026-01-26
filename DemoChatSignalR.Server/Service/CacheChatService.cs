using DempChatSignalR.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DemoChatSignalR.Server;

public class CacheChatService(IMemoryCache memoryCache)
{
    private IMemoryCache MemoryCache => memoryCache;
    private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _roomLocks = new();

    public Task<ResultOf<RoomChacheModel>> GetRoomAsync(Guid RoomGuid)
    {
        MemoryCache.TryGetValue(RoomGuid, out RoomChacheModel? room);
        if (room == null)
        {
            return Task.FromResult((ResultOf<RoomChacheModel>)"Not found room");
        }
        return Task.FromResult((ResultOf<RoomChacheModel>)room);
    }
    public async Task<ResultOf<RoomChacheModel>> CreateRoomAsync(Guid GuidRoom, string NameRoom)
    {
        var room = new RoomChacheModel() { TotalCount = 1, Guid = GuidRoom, Name = NameRoom }; ;
        MemoryCache.Set(GuidRoom, room, TimeSpan.FromMinutes(40));
        await AddMessageAsync(GuidRoom, $"Room {NameRoom} created", "System");
        return room;
    }

    public async Task<ResultsOf<MessageChacheModel>> GetMessages(Guid GuidRoom, int FromIndex, int ToIndex)
    {
        var roomRlt = await GetRoomAsync(GuidRoom);
        if (!roomRlt.Success)
        {
            return roomRlt.Message;
        }
        if (FromIndex > ToIndex)
        {
            return "ToIndex must biger than FromIndex ";
        }
        if (FromIndex < 0 || roomRlt.Item.TotalCount < ToIndex)
        {
            Console.WriteLine(roomRlt.Item.TotalCount);
            return "Request out range";
        }
        var rlt = new List<MessageChacheModel>();
        for (int i = FromIndex; i < ToIndex; i++)
        {
            MemoryCache.TryGetValue($"Message_{GuidRoom}_{i}", out MessageChacheModel? Message);
            if (Message != null)
            {
                rlt.Add(Message);
            }
        }
        if (rlt.Count == 0)
        {
            return "Not found message";
        }
        return rlt;
    }
    public async Task<ResultOf<MessageChacheModel>> AddMessageAsync(Guid GuidRoom, string UserGuid, string Message)
    {
        var roomRlt = await GetRoomAsync(GuidRoom);
        if (!roomRlt.Success)
        {
            return roomRlt.Message;
        }
        var room = roomRlt.Item;
        var roomLock = _roomLocks.GetOrAdd(GuidRoom, _ => new SemaphoreSlim(1, 1));
        await roomLock.WaitAsync();
        try
        {
            var messageIndex = room.TotalCount;
            var messageValue = new MessageChacheModel()
            {
                Id = messageIndex,
                Message = Message,
                UserGuid = UserGuid,
                DateTimeSent = DateTime.UtcNow
            };
            MemoryCache.Set($"Message_{GuidRoom}_{messageIndex}", messageValue
            , TimeSpan.FromMinutes(30));
            room.TotalCount++;
            MemoryCache.Set(room.Guid, room, TimeSpan.FromMinutes(30));
            return messageValue;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            roomLock.Release();
        }
    }
    /// <summary>
    /// 0: no change 
    /// 1: created
    /// 2: updated
    /// </summary>
    /// <param name="GuidRoom"></param>
    /// <param name="UserName"></param>
    /// <param name="GuidUser"></param>
    /// <returns></returns>
    public async Task<ResultOf<int>> CreateOrUpdateUser(Guid GuidRoom, string UserName, string GuidUser)
    {
        var roomRlt = await GetRoomAsync(GuidRoom);
        if (!roomRlt.Success)
        {
            return roomRlt.Message;
        }
        var room = roomRlt.Item;
        var roomLock = _roomLocks.GetOrAdd(GuidRoom, _ => new SemaphoreSlim(1, 1));
        await roomLock.WaitAsync();
        try
        {
            var usersTemp = room!.Users;
            var userExist = usersTemp.FirstOrDefault(u => u.UserGuid == GuidUser);
            if (userExist == null)
            {
                userExist = new InfoUser() { UserGuid = GuidUser, UserName = UserName };
                usersTemp.Add(userExist);
                MemoryCache.Set(GuidRoom, room, TimeSpan.FromMinutes(40));
                return 1;
            }
            if (userExist.UserName == UserName)
            {
                return 0;
            }
            room.Users = usersTemp;
            MemoryCache.Set(GuidRoom, room, TimeSpan.FromMinutes(40));
            return 2;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            roomLock.Release();
        }
    }

}
