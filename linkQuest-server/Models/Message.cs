using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linkQuest_server.Models
{
    public class Message
    {
        public int id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string message {get; set;} = string.Empty;
    }
}