using System.Collections.Generic;

namespace LunchService.Models
{
  public interface IUserRepository
  {
    void Add(User user);
    IEnumerable<User> GetAll();
    User Get(string id);
    User Remove(string id);
  }
}
