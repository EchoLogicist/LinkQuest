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

        public bool Exists(string roomName)
        {
            return rooms.Exists((room) => room.name == roomName && !room.isLocked);
        }

        public void UpdateRoom(string roomName)
        {
            var index = rooms.FindIndex((room) => room.name == roomName);
            rooms[index].isLocked = true;
        }
    }
}