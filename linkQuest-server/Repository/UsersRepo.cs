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
        
        public bool updateCount(string roomName, string userName, int timeLapse){
            var user = users.Find((j) => j.Name == userName && j.RoomName == roomName)!;
            user.count += 1;
            user.turnShiftTime = DateTime.Now.AddSeconds(timeLapse);
            return true;
        }

        public bool UpdateConnectionId(string connectionId, Users user)
        {
            var tempUser = users.Find((j) => j.Name == user.Name && j.RoomName == user.RoomName)!;
            tempUser.ConnectionId = connectionId;
            return true;
        }

        public void getUserTurn(string roomName, int timeLapse){
            var tempUsers = GetUsers(roomName);
            var index = tempUsers.FindIndex((j) => j.myTurn);
            if(index == -1 || index + 1 >= tempUsers.Count){
                if(index + 1 >= tempUsers.Count) UpdateUser(tempUsers[tempUsers.Count - 1], false);
                UpdateUser(tempUsers[0], true, timeLapse);
            } 
            else{
                UpdateUser(tempUsers[index + 1], true, timeLapse);
                //user = tempUsers[index + 1];
                //tempUsers[index].myTurn = false;
                UpdateUser(tempUsers[index], false);
            }
        }

        private void UpdateUser(Users user, bool myTurn, int timeLapse = 0){
            user.myTurn = myTurn;
            if(myTurn) user.turnShiftTime = DateTime.Now.AddSeconds(timeLapse);
            else user.turnShiftTime = null;
        }
    }
}