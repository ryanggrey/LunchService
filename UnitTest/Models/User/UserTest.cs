using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using LunchService.Models;

namespace UnitTest
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dotnet-core.html
    public class UserTest
    {
        [Fact]
        public void can_set_User_Name()
        {
            User user = new User();
            string exptectedName = "Ryan";
            user.Name = exptectedName;
            string observedName = user.Name;
            Assert.Equal(exptectedName, observedName);
        }

        [Fact]
        public void can_get_User_Name()
        {
            User user = new User();
            string exptectedName = "Ryan";
            user.Name = exptectedName;
            string observedName = user.Name;
            Assert.Equal(exptectedName, observedName);
        }
    }
}
