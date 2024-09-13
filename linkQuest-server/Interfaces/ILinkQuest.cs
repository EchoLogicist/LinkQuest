using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Models;

namespace linkQuest_server.Interfaces
{
    public interface ILinkQuest
    {
        LinkQuest[,] InitializeObject(string roomName);
        bool UpdateCell(CellUpdate update, string userName, string roomName);
    }
}