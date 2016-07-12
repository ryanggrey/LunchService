using Xunit;
using LunchService.Models;

namespace UnitTest
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dotnet-core.html
    public class UserTest
    {
        [Fact]
        public void can_instantiate_User_with_name()
        {
            string exptectedName = "Ryan";
            User user = new User(exptectedName);
            string observedName = user.Name;
            Assert.Equal(exptectedName, observedName);
        }

        [Fact]
        public void can_get_User_Name()
        {
            string exptectedName = "Ryan";
            User user = new User(exptectedName);
            string observedName = user.Name;
            Assert.Equal(exptectedName, observedName);
        }
    }
}
