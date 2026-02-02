using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using linkQuest_server.Interfaces;
using linkQuest_server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace linkQuest_server
{
    public class communicationHub : Hub
    {
        private readonly IUser _user;
        private IRoom _rooms;
        private readonly ILinkQuest _linkQuest;
        private readonly IMessage _message;

        public communicationHub(IUser user, IRoom rooms, ILinkQuest linkQuest, IMessage message)
        {
            _user = user;
            _rooms = rooms;
            _linkQuest = linkQuest;
            _message = message;
        }
        public async Task<object> JoinRoom(Users user)
        {
            var roomInfo = _rooms.GetRoom(user.RoomName)!;

            if(roomInfo != null && roomInfo.name == user.RoomName && !roomInfo.isLocked){
                await Groups.AddToGroupAsync(Context.ConnectionId, user.RoomName);
                _user.AddUser(user);
                await Clients.Group(user.RoomName).SendAsync("GroupNotification", "Lets Program Bot", $"{user.Name} has Joined the Room", DateTime.Now);

                var users = _user.GetUsers( user.RoomName)!;
                if(users.Count == roomInfo.playersCount)
                {
                    _rooms.UpdateRoom(user.RoomName);   
                    CanStartGame(user.RoomName, false);
                    //await Clients.Group(user.RoomName).SendAsync("StartGame", "Lets Program Bot", $"Game Start", DateTime.Now);                 
                }

                await Clients.Group(user.RoomName).SendAsync("colorSelected", roomInfo.colorPalette);  
                await SendConnectedUser(user.RoomName);
                return await Task.Run(() => new { data = roomInfo, status = "joined" });
            }
            else {
                if(roomInfo != null)
                {
                    if(_user.IsUserExists(user.Name, user.RoomName)){
                        await Groups.AddToGroupAsync(Context.ConnectionId, user.RoomName);
                        UpdateContextId(Context.ConnectionId, user);
                        var userInfo = _user.GetUser(Context.ConnectionId);
                        CanStartGame(user.RoomName, true);
                        //await Clients.Caller.SendAsync("StartGame", "Rejoined");
                        await GameObject(user.RoomName);
                        return await Task.Run(() => new { data = userInfo , status = "rejoined" });
                    }                             
                }
                return await Task.Run(() => new { data = $"No active room is present with the name {user.RoomName}", status = "No Room Present" });
            }
        }
        public Task SendConnectedUser(string roomName)
        {
            var users = _user.GetUsers(roomName);
            return Clients.Group(roomName).SendAsync("ConnectedUser", users);
        }

        public Task GameObject(string roomName){
            var objects = _linkQuest.InitializeObject(roomName);
            return Clients.Group(roomName).SendAsync("GameObjects", JsonConvert.SerializeObject(objects));
        }

        public void UpdateCell(CellUpdate update){
            var user = _user.GetUser(Context.ConnectionId)!;
            if(_linkQuest.UpdateCell(update, user.Name, user.RoomName)){
                GameObject(user.RoomName);
                SendConnectedUser(user.RoomName);
                var room = _rooms.GetRoom(user.RoomName)!;
                if(room.cellsPending == 0) Clients.Group(user.RoomName).SendAsync("EndGame", "Game Ended", JsonConvert.SerializeObject(_linkQuest.InitializeObject(user.RoomName)), _user.GetUsers( user.RoomName));
            }
        }

        public void UpdateColor(string color){
            var user = _user.GetUser(Context.ConnectionId);
            if(user != null){
                _user.UpdateColor(user, color);
                SendConnectedUser(user.RoomName);                
            }
        }

        public void SwitchTurns(string roomName){
            var roomInfo = _rooms.GetRoom(roomName)!;
            _user.getUserTurn(roomName, roomInfo.ElapseTime);
            SendConnectedUser(roomName);
        }

        private void UpdateContextId(string contextId, Users user){
            var connectonId = _user.GetConnectionId(user.Name, user.RoomName);
            Groups.RemoveFromGroupAsync(connectonId, user.RoomName);
            _user.UpdateConnectionId(contextId, user);           
        }

        public Task SendMessage(Message message){
            var messages = new List<Message>();
            if(_message.AddMessage(message)) messages = _message.GetMessages(message.RoomName);
            return Clients.Group(message.RoomName).SendAsync("RecieveChat", messages);
        }

        public async Task<object> CreateRoomAndJoin(RoomInfo room){
            var roomCode = _rooms.CreateRoom(room);
            if (!string.IsNullOrEmpty(roomCode)){
                await JoinRoom(new Users{ Name = room.PlayerName, RoomName = roomCode, isHost = true, ConnectionId = Context.ConnectionId});
            }
            return await Task.Run(() => new { data = _rooms.GetRoom(roomCode), roomCode});
        }

        public void PickColor(PlayerColor playerColor){
            var color =_user.UpdateColor(playerColor);
            SendConnectedUser(playerColor.RoomName);
            var room = _rooms.GetRoom(playerColor.RoomName);
            if(!string.IsNullOrEmpty(color)){
                room.colorPalette.Find(j => j.Color == color).Picked = false;
            }
            room.colorPalette.Find(j => j.Color == playerColor.Color).Picked = true;
            CanStartGame(playerColor.RoomName, false);
            Clients.Group(playerColor.RoomName).SendAsync("colorSelected", room.colorPalette);
        }

        private void CanStartGame(string roomName, bool rejoined){
            var room = _rooms.GetRoom(roomName);
            var remaningColors = room.colorPalette.FindAll((j) => !j.Picked);
            if(remaningColors.Count == 0 && room.isLocked){
                // if(remaningColors.Count == 1) {
                //     _user.UpdateColor()
                // }
                Clients.Group(roomName).SendAsync("StartGame", $"Game Start", DateTime.Now);  
                GameObject(roomName);
                if(!rejoined)  _user.getUserTurn(roomName, room.ElapseTime);  
                SendConnectedUser(roomName);
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        
        // public Task<string> GetTime(string roomName){
        //     var user = _user.GetUsers(roomName).Find((j) => j.myTurn);
        //     var room = _rooms.GetRoom(roomName);
        //     if(user == null){
        //         _user.getUserTurn(roomName, room.ElapseTime);
        //     }
        //     else{
        //         user.turnShiftTime -= 5;

        //     }
        // }
    }
}