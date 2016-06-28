using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LunchService.Models;

namespace LunchService.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        List<User> users = new List<User>();

        // GET api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return users;
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody]User user)
        {
            users.Add(user);
        }
    }
}
