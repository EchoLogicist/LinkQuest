using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linkQuest_server.Models
{
    public class Users
    {      
        public string Name {get; set;} = string.Empty;
        public string Color {get; set;} = string.Empty;
        public string RoomName {get; set;} = string.Empty;
        public string ConnectionId {get; set;} = string.Empty;
        public int count {get; set;} = 0;
        public bool myTurn {get; set;} = false;
    }
}