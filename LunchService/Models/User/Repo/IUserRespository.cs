using System.Collections.Generic;

namespace LunchService.Models
{
    public interface IRepository
    {
        void Add(User user);
        IEnumerable<User> GetAll();
    }
}
