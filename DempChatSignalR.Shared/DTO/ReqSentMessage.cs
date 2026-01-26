using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DempChatSignalR.Shared;

public class ReqSentMessage
{
    [Required]
    public required string GuidUser { get; set; }
    [Required]
    [MinLength(1, ErrorMessage = "Tối thiểu 1 ký tự")]
    public required string Message { get; set; }
}
