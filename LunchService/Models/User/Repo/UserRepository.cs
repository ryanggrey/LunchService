using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace LunchService.Models
{
    public class UserRepository : IUserRepository
    {
        private static List<User> users = new List<User>();

        public TodoRepository()
        {
        }

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
