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
        private static List<Room> rooms = new List<Room>();

        private readonly IConfiguration _configuration;

        public RoomRepo(IConfiguration configuration)
        {
            _configuration = configuration;            
        }
        public bool RoomOpen(string roomName)
        {
            return rooms.Exists((room) => room.name == roomName && !room.isLocked);
        }

        public bool GameStarted(string roomName)
        {
            return rooms.Exists((room) => room.name == roomName && room.gameState == State.STARTED);
        }

        public void UpdateRoom(string roomName, State state = State.NEW)
        {
            var index = rooms.FindIndex((room) => room.name == roomName);
            if(index != -1){                
                rooms[index].isLocked = true;
                if(state == State.STARTED) rooms[index].gameState = state;
            }
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

        public string CreateRoom(RoomInfo room)
        {
            try
            {
                var tempRoom = new Room();
                tempRoom.cellsPending = (int)Math.Pow(room.Dimension, 2);
                tempRoom.name = GenerateRandomNo();
                tempRoom.dimension = room.Dimension;
                tempRoom.playersCount = room.playersCount;
                tempRoom.ElapseTime = Convert.ToInt16(_configuration.GetSection("LapsTime").Value);
                tempRoom.colorPalette = room.colorPalette.Split(',').Select((color) => new ColorPalette{ Color = color, Picked = false}).ToList();
                rooms.Add(tempRoom);
                return tempRoom.name;                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max).ToString();
        }
    }
}