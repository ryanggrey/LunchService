using System;
using System.Collections.Generic;

namespace LunchService.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> users = new List<User>();

        public IEnumerable<User> GetAll()
        {
            return new List<User>(users);
        }

        public void Add(User user)
        {
            users.Add(user);
        }

        private User Find(int id)
        {
            if(id >= users.Count)
            {
                return null;
            }

            return users[id];
        }

        public User Remove(int id)
        {
            User removedUser = Find(id);
            if (removedUser != null)
            {
                users.RemoveAt(id);
            }
            return removedUser;
        }
    }
}
