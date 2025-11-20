using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace DemoChatSignalR.Server
{
    public class CacheChatService(IMemoryCache memoryCache)
    {
        private IMemoryCache MemoryCache => memoryCache;
        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _roomLocks = new();
    }
}
