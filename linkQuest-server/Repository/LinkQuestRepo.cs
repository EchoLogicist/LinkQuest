using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Interfaces;
using linkQuest_server.Models;

namespace linkQuest_server.Repository
{
    public class LinkQuestRepo : ILinkQuest
    {
        private static Dictionary<string, LinkQuestContainer> _linkQuests = new Dictionary<string, LinkQuestContainer>();
        private readonly IRoom _room;
        private readonly IUser _user;

        public LinkQuestRepo(IRoom room, IUser user)
        {
            _room = room;
            _user = user;
        }
        public LinkQuestContainer InitializeObject(string roomName)
        {
            var room = _room.GetRoom(roomName)!;

            if (!_room.GameStarted(roomName))
            {
                var gameObject = new LinkQuest[room.dimension, room.dimension];

                for (int i = 0; i < gameObject.GetLength(0); i++)
                {
                    for (int j = 0; j < gameObject.GetLength(1); j++)
                    {
                        gameObject[i, j] = new LinkQuest();
                    }
                }
                _linkQuests[roomName] = new LinkQuestContainer{GameObject = gameObject};
                _room.UpdateRoom(roomName, State.STARTED);
            }
            return _linkQuests[roomName];
        }

        public bool UpdateCell(CellUpdate update, string userName, string roomName)
        {
            var gameObject = _linkQuests[roomName].GameObject;
            _linkQuests[roomName].cellInfo = update;
            LinkQuest tempObj;
            var retainMyTurn = false;

            switch (update.Cell)
            {
                case "Top":
                    gameObject[update.RowIndex, update.ColumnIndex].Top = new Cell { Checked = true, UserName = userName };
                    _linkQuests[roomName].recentUpdateCell = gameObject[update.RowIndex, update.ColumnIndex];
                    if (update.RowIndex >= 1)
                    {
                        gameObject[update.RowIndex - 1, update.ColumnIndex].Bottom = new Cell { Checked = true, UserName = userName };

                        tempObj = gameObject[update.RowIndex - 1, update.ColumnIndex];
                        if (tempObj.Top.Checked && tempObj.Bottom.Checked && tempObj.Left.Checked && tempObj.Right.Checked)
                        {
                            gameObject[update.RowIndex - 1, update.ColumnIndex].cellOwner = userName;
                            _room.UpdateAvailabeCount(roomName);
                            _user.updateCount(roomName, userName);
                            retainMyTurn = true;
                        }
                    }

                    tempObj = gameObject[update.RowIndex, update.ColumnIndex];
                    if (tempObj.Top.Checked && tempObj.Bottom.Checked && tempObj.Left.Checked && tempObj.Right.Checked)
                    {
                        gameObject[update.RowIndex, update.ColumnIndex].cellOwner = userName;
                        _room.UpdateAvailabeCount(roomName);
                        _user.updateCount(roomName, userName);
                        retainMyTurn = true;
                    }
                    if(!retainMyTurn) _user.getUserTurn();
                    //_linkQuests[roomName] = gameObject;
                    return true;

                case "Bottom":
                    gameObject[update.RowIndex, update.ColumnIndex].Bottom = new Cell { Checked = true, UserName = userName };
                    _linkQuests[roomName].recentUpdateCell = gameObject[update.RowIndex, update.ColumnIndex];

                    tempObj = gameObject[update.RowIndex, update.ColumnIndex];
                    if (tempObj.Top.Checked && tempObj.Bottom.Checked && tempObj.Left.Checked && tempObj.Right.Checked)
                    {
                        gameObject[update.RowIndex, update.ColumnIndex].cellOwner = userName;
                        _room.UpdateAvailabeCount(roomName);
                        _user.updateCount(roomName, userName);
                    }
                    else _user.getUserTurn();
                    //_linkQuests[roomName] = gameObject;
                    return true;

                case "Right":
                    gameObject[update.RowIndex, update.ColumnIndex].Right = new Cell { Checked = true, UserName = userName };
                    _linkQuests[roomName].recentUpdateCell = gameObject[update.RowIndex, update.ColumnIndex];

                    tempObj = gameObject[update.RowIndex, update.ColumnIndex];
                    if (tempObj.Top.Checked && tempObj.Bottom.Checked && tempObj.Left.Checked && tempObj.Right.Checked)
                    {
                        gameObject[update.RowIndex, update.ColumnIndex].cellOwner = userName;
                        _room.UpdateAvailabeCount(roomName);
                        _user.updateCount(roomName, userName);
                    }
                    else _user.getUserTurn();
                    //_linkQuests[roomName] = gameObject;
                    return true;

                case "Left":
                    gameObject[update.RowIndex, update.ColumnIndex].Left = new Cell { Checked = true, UserName = userName };
                    _linkQuests[roomName].recentUpdateCell = gameObject[update.RowIndex, update.ColumnIndex];

                    if (update.ColumnIndex >= 1)
                    {
                        gameObject[update.RowIndex, update.ColumnIndex - 1].Right = new Cell { Checked = true, UserName = userName };

                        tempObj = gameObject[update.RowIndex, update.ColumnIndex - 1];
                        if (tempObj.Top.Checked && tempObj.Bottom.Checked && tempObj.Left.Checked && tempObj.Right.Checked)
                        {
                            gameObject[update.RowIndex, update.ColumnIndex - 1].cellOwner = userName;
                            _room.UpdateAvailabeCount(roomName);
                            _user.updateCount(roomName, userName);
                            retainMyTurn = true;
                        }
                    }

                    tempObj = gameObject[update.RowIndex, update.ColumnIndex];
                    if (tempObj.Top.Checked && tempObj.Bottom.Checked && tempObj.Left.Checked && tempObj.Right.Checked)
                    {
                        gameObject[update.RowIndex, update.ColumnIndex].cellOwner = userName;
                        _room.UpdateAvailabeCount(roomName);
                        _user.updateCount(roomName, userName);
                        retainMyTurn = true;
                    }
                    if(!retainMyTurn) _user.getUserTurn();
                    //_linkQuests[roomName] = gameObject;
                    return true;

                default: break;
            }
            return false;
        }
    }
}