using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LunchService.Models;

namespace LunchService.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository users;

        public UsersController(IUserRepository users)
        {
            this.users = users;
        }

        // GET api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return users.GetAll();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (user == null) {
                return BadRequest();
            }

            users.Add(user);
            int userID = users.IDOfUser(user);
            if (userID == -1) {
                return StatusCode(500);
            }

            string routeName = "GetUser";
            Object routeValues = new { id = userID };
            return CreatedAtRoute(routeName, routeValues, user);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User deletedUser = users.Remove(id);
            if (deletedUser == null) {
                return NotFound();
            }
            return new ObjectResult(deletedUser);
        }
    }
}
