using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared
{
    public class InfoUser
    {
        [MinLength(6, ErrorMessage ="Tối thiểu 6 ký tự")]
        public required string UserName { get; set; }
        public required string UserGuid { get; set; }
        public bool IsActive { get; set; }
    }
}
