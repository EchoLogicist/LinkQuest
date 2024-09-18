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
        private readonly IConfiguration _configuration;
        private IRoom _rooms;
        private readonly ILinkQuest _linkQuest;

        public communicationHub(IUser user, IConfiguration configuration, IRoom rooms, ILinkQuest linkQuest)
        {
            _user = user;
            _configuration = configuration;
            _rooms = rooms;
            _linkQuest = linkQuest;
        }
        public async Task<object> JoinRoom(Users user)
        {
           
            if(_rooms.RoomOpen(user.RoomName)){
                await Groups.AddToGroupAsync(Context.ConnectionId, user.RoomName);
                _user.AddUser(user);
                await Clients.Group(user.RoomName)
                .SendAsync("GroupNotification", "Lets Program Bot", $"{user.Name} has Joined the Room", DateTime.Now);

                var users = _user.GetUsers( user.RoomName)!;
                if(users.Count.ToString() == _configuration.GetSection("NumberOfUser").Value){
                    _rooms.UpdateRoom(user.RoomName);   
                    await Clients.Group(user.RoomName).SendAsync("StartGame", "Lets Program Bot", $"Game Start", DateTime.Now);                 
                    _user.getUserTurn();
                    await GameObject(user.RoomName);    
                }
                await SendConnectedUser(user.RoomName);
                return await Task.Run(() => "Success");
            }
            else {
                if(_rooms.RoomExists(user.RoomName)){
                    if(_user.IsUserExists(user.Name, user.RoomName)){
                        await Groups.AddToGroupAsync(Context.ConnectionId, user.RoomName);
                        UpdateContextId(Context.ConnectionId, user);
                        await Clients.Caller.SendAsync("StartGame", "Rejoined");
                        await GameObject(user.RoomName);
                        await SendConnectedUser(user.RoomName);
                        return await Task.Run(() => "Rejoined");
                    }                             
                }
                return await Task.Run(() => $"No active room is present with the name {user.RoomName}");
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

        private void UpdateContextId(string contextId, Users user){
            var connectonId = _user.GetConnectionId(user.Name, user.RoomName);
            Groups.RemoveFromGroupAsync(connectonId, user.RoomName);
            _user.UpdateConnectionId(contextId, user);  
        }
    }
}