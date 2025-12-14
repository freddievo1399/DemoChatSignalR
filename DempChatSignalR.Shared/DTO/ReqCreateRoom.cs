using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

public class ReqCreateRoom
{
    [Required]
    public required string NameRoom { get; set; } = string.Empty;
}
