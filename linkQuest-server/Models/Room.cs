using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linkQuest_server.Models
{
    public class Room
    {
        public int id {get; set;}
        public string name {get; set;} = string.Empty;
        public int playersCount {get; set;}
        public bool isLocked {get; set;} = false;
        public State gameState {get; set;} = State.NEW;
        public int dimension {get; set;}
        public int cellsPending {get; set;}
    }

    public enum State
    {
        NEW,
        STARTED,
        ENDED
    }
}