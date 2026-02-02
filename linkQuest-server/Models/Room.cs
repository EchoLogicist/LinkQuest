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
        public short ElapseTime {get; set;}
        public int cellsPending {get; set;}
        public List<ColorPalette> colorPalette {get; set;} = new List<ColorPalette>();
    }

    public enum State
    {
        NEW,
        STARTED,
        ENDED
    }

    public class ColorPalette
    {
        public string Color { get; set; } = string.Empty;
        public bool Picked { get; set; }
    }

    public class RoomInfo
    {
        public string PlayerName { get; set; } = string.Empty;
        public int playersCount {get; set;}
        public int Dimension {get; set;}
        public string colorPalette {get; set;} = string.Empty;
    }
    
    public class PlayerColor
    {
        public string PlayerName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}