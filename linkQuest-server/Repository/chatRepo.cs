using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Interfaces;
using linkQuest_server.Models;

namespace linkQuest_server.Repository
{
    public class chatRepo : IMessage
    {
        private static List<Message> messages = new List<Message>();

        public bool AddMessage(Message message)
        {
            messages.Add(message);
            return true;
        }

        public List<Message> GetMessages(string roomName)
        {
            var tempMessages = messages.FindAll((j) => j.RoomName == roomName);
            return tempMessages;
        }
    }
}