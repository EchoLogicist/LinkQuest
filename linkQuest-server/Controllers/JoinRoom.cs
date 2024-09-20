using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using linkQuest_server.Interfaces;
using linkQuest_server.Models;
using Microsoft.AspNetCore.Mvc;

namespace linkQuest_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JoinRoom : ControllerBase
    {
        private readonly IRoom _room;

        public JoinRoom(IRoom room)
        {
            _room = room;
        }
        public ActionResult CreateRoom(Room room)
        {
            try
            {
                if (_room.RoomExists(room.name)) return StatusCode(409, "Room already exists");
                var roomCode = _room.CreateRoom(room);
                if (!string.IsNullOrEmpty(roomCode)) return Ok(roomCode);
                return StatusCode(500, "Unable to process you request.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private ActionResult OK()
        {
            throw new NotImplementedException();
        }
    }
}