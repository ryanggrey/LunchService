using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LunchService.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        List<string> users = new List<string>();

        // GET api/users
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return users;
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return users[id];
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody]string user)
        {
            users.Add(user);
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string user)
        {
            users.Insert(id, user);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            users.RemoveAt(id);
        }
    }
}
