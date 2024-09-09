using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Models;

namespace linkQuest_server.Interfaces
{
    public interface IRoom
    {
        bool Exists(string roomName);
        void UpdateRoom(string roomName);
    }
}