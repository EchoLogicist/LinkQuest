using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Interfaces;
using linkQuest_server.Models;

namespace linkQuest_server.Repository
{
    public class UsersRepo : IUser
    {
        private static List<Users> users = new List<Users>();

        public bool AddUser(Users user)
        {
            users.Add(user);
            return true;
        }

        public string GetConnectionId(string userName, string roomName)
        {
            return users.Find((j) => j.Name == userName && j.RoomName == roomName)?.ConnectionId ?? "";
        }

        public List<Users>? GetUsers(string roomName)
        {
            return users.FindAll((j) => j.RoomName == roomName);
        }

        public Users? GetUser(string connectionId)
        {
            return users.Find((j) => j.ConnectionId == connectionId);
        }

        public bool IsUserExists(string userName, string roomName)
        {
            return users.Exists((j) => j.Name == userName && j.RoomName == roomName);
        }
        
        public bool updateCount(string roomName, string userName){
            var user = users.Find((j) => j.Name == userName && j.RoomName == roomName)!;
            user.count += 1;
            return true;
        }

        public bool UpdateConnectionId(string connectionId, Users user)
        {
            var tempUser = users.Find((j) => j.Name == user.Name && j.RoomName == user.RoomName)!;
            tempUser.ConnectionId = connectionId;
            return true;
        }

        public void getUserTurn(){
            var index = users.FindIndex((j) => j.myTurn);
            Users user = null;
            if(index == -1 || index + 1 >= users.Count){
                if(index + 1 >= users.Count) users[users.Count - 1].myTurn = false;
                user = users[0];
            } 
            else{
                user = users[index + 1];
                users[index].myTurn = false;
            }
            user.myTurn = true;
        }
    }
}