using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LunchService.Models;
using LunchService.Filters;
using System.Numerics;

namespace LunchService.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository users;
        static BigInteger counter = new BigInteger(0);

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
        public IActionResult Get(string id)
        {
            return Ok(users.Get(id));
        }

        private string UniqueUserID()
        {
            return counter++.ToString();
        }

        // POST api/users
        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody]User user)
        {
            if (user == null) {
                return BadRequest("User required in request body.");
            }

            user.ID = UniqueUserID();

            if (string.IsNullOrEmpty(user.ID)) {
                return StatusCode(500);
            }

            users.Add(user);

            string routeName = "GetUser";
            Object routeValues = new { id = user.ID };
            return CreatedAtRoute(routeName, routeValues, user);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            User deletedUser = users.Remove(id);
            if (deletedUser == null) {
                return NotFound();
            }
            return new ObjectResult(deletedUser);
        }
    }
}
