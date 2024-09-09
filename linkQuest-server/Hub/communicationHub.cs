using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Interfaces;
using linkQuest_server.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

namespace linkQuest_server
{
    public class communicationHub : Hub
    {
        private readonly IDictionary<string, Users> _connection;
        private readonly IConfiguration _configuration;
        private IRoom _rooms;

        public communicationHub(IDictionary<string, Users> connection, IConfiguration configuration, IRoom rooms)
        {
            _connection = connection;
            _configuration = configuration;
            _rooms = rooms;
        }
        public async Task JoinRoom(Users user)
        {
           
            if(_rooms.Exists(user.RoomName)){
                await Groups.AddToGroupAsync(Context.ConnectionId, user.RoomName);
                _connection[Context.ConnectionId] = user;
                await Clients.Group(user.RoomName)
                .SendAsync("ReceiveMessage", "Lets Program Bot", $"{user.Name} has Joined the Room", DateTime.Now);
                await SendConnectedUser(user.RoomName);

                var users = _connection.Values.Where(u => u.RoomName == user.RoomName);
                if(users.Count().ToString() == _configuration.GetSection("NumberOfUser").Value){
                    _rooms.UpdateRoom(user.RoomName);
                    await Clients.Group(user.RoomName).SendAsync("StartGame", "Lets Program Bot", $"Game Start", DateTime.Now);
                }
            }
            else {
                await SendNotifications($"No active room is present with the name {user.RoomName}");
            }
        }
        public Task SendConnectedUser(string roomName)
        {
            var users = _connection.Values
            .Where(u => u.RoomName == roomName)
            .Select(s => s.Name);
            return Clients.Group(roomName).SendAsync("ConnectedUser", users);
        }

        public Task SendNotifications(string roomName)
        {
            return Clients.Caller.SendAsync("messages", roomName);
        }
    }
}