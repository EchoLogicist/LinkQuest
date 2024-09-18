using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Interfaces;
using linkQuest_server.Models;

namespace linkQuest_server.Repository
{
    public class RoomRepo : IRoom
    {
        private static List<Room> rooms = new List<Room>{ new Room() };

        public bool RoomOpen(string roomName)
        {
            return rooms.Exists((room) => room.name == roomName && !room.isLocked);
        }

        public bool GameStarted(string roomName)
        {
            return rooms.Exists((room) => room.name == roomName && room.gameStated);
        }

        public void UpdateRoom(string roomName, bool isLocked = false, bool isGameStarted = false)
        {
            var index = rooms.FindIndex((room) => room.name == roomName);
            rooms[index].isLocked = isLocked;
            rooms[index].gameStated = isGameStarted;
            if(isGameStarted) rooms[index].cellsPending = (int)Math.Pow(rooms[index].dimension, 2);
        }

        public bool RoomExists(string roomName)
        {
            return rooms.Exists((room) => room.name == roomName);
        }

        public Room? GetRoom(string roomName)
        {
            return rooms.Find((room) => room.name == roomName);
        }

        public bool UpdateAvailabeCount(string roomName){
            var room = rooms.Find((room) => room.name == roomName)!;
            room.cellsPending -= 1;
            return true;
        }
    }
}