using System.Collections.Generic;

namespace LunchService.Models
{
    public interface IUserRepository
    {
        void Add(User user);
        IEnumerable<User> GetAll();
        User Remove(int id);
        int IDOfUser(User user);
    }
}
