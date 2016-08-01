using System.Collections.Generic;

namespace LunchService.Models
{
  public class UserRepository : IUserRepository
  {
    private readonly IDictionary<string, User> users = new Dictionary<string, User>();

    public IEnumerable<User> GetAll()
    {
      return new List<User>(users.Values);
    }

    public void Add(User user)
    {
      users.Add(user.ID, user);
    }

    public User Get(string id)
    {
      return users[id];
    }

    public User Remove(string id)
    {
      User removedUser = Get(id);
      users.Remove(id);
      return removedUser;
    }
  }
}
