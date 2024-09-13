using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linkQuest_server.Models
{
    public class Room
    {
        public int id {get; set;} = 1;
        public string name {get; set;} = "testRoom1";
        public int playersCount {get; set;} = 4;
        public bool isLocked {get; set;} = false;
        public bool gameStated {get; set;} = false;
        public int dimension {get; set;} = 5;
        public int cellsPending {get; set;}
    }
}