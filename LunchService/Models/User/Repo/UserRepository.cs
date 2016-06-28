using System;
using System.Collections.Generic;

namespace LunchService.Models
{
    public class UserRepository : IUserRepository
    {
        private static List<User> users = new List<User>();

        public IEnumerable<User> GetAll()
        {
            return new List<User>(users);
        }

        public void Add(User user)
        {
            users.Add(user);
        }
    }
}
