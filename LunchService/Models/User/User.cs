using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LunchService.Models
{
  public class User
  {

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 1)]
    public string Name { get; set; }

    [BindNever]
    public string ID { get; set; }

    public User(string name)
    {
      Name = name;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }
      
      User user = obj as User;
      return this.Equals(user);
    }

    public bool Equals(User user)
    {
      if (user == null)
      {
        return false;
      }

      bool isNameEqual = user.Name == this.Name;
      return isNameEqual;
    }

    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }

    public override string ToString()
    {
      return JsonConvert.SerializeObject(this);
    }
  }
}
