using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared.Interface
{
    public interface IManagerRoom
    {
        [Get(nameof(GetDataAll))]
        Task<ResultsOf<InfoRoomDto>> GetDataAll();
        [Get(nameof(Delate))]
        Task<ResultOf<InfoRoomDto>> Delate(Guid GuidRoom);
    }
}
