using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Models;

namespace linkQuest_server.Interfaces
{
    public interface IRoom
    {
        bool RoomOpen(string roomName);
        void UpdateRoom(string roomName, State state = State.NEW);
        bool GameStarted(string roomName);
        bool RoomExists(string roomName);
        Room? GetRoom(string roomName);
        bool UpdateAvailabeCount(string roomName);
        string CreateRoom(Room room);
    }
}