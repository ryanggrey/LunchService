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

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (user == null) {
                return BadRequest();
            }
            users.Add(user);
            return CreatedAtRoute(null, null);
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
