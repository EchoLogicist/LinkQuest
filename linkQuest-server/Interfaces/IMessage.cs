using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Models;

namespace linkQuest_server.Interfaces
{
    public interface IMessage
    {
        bool AddMessage(Message message);
        List<Message> GetMessages(string roomName);
    }
}