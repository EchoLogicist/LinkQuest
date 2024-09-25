using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Models;

namespace linkQuest_server.Interfaces
{
    public interface IUser
    {
        List<Users>? GetUsers(string roomName);
        Users? GetUser(string ConnectionId);
        string GetConnectionId(string userName, string roomName);
        bool AddUser(Users user);
        bool IsUserExists(string userName, string roomName);
        bool UpdateConnectionId(string connectionId, Users user);
        bool updateCount(string roomName, string userName, int timeLapse);
        void getUserTurn(string roomName, int timeLapse);
    }
}