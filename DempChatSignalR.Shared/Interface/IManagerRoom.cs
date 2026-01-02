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
        [Get(nameof(GetData))]
        Task<ResultsOf<InfoRoomDto>> GetData(int take,int skip);
        [Post(nameof(Delate))]
        Task<Result> Delate(Guid GuidRoom);
    }
}
