using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class UsersTests : BaseTests
    {
        [TestMethod()]
        public void User_CRUD_Positive()
        {
            // create
            User user = null;
            Func<Task> asyncFunction = async () => user = await CreateUser(Client);
            asyncFunction.ShouldNotThrow();
            user.Should().NotBeNull();

            // read
            asyncFunction = async () => user = await Client.ReadUser(user.Id);
            asyncFunction.ShouldNotThrow();
            user.Should().NotBeNull();

            // read list
            IEnumerable<User> users = null;
            asyncFunction = async () => users = await Client.ReadUsers();
            asyncFunction.ShouldNotThrow();
            users.Should().NotBeNullOrEmpty();
            users.Any(x => x.Id == user.Id).Should().BeTrue();

            // update
            var update = new UpdateUser(user)
            {
                FirstName = user.FirstName + " Updated",
            };
            asyncFunction = async () => user = await Client.UpdateUser(user.Id, update);
            asyncFunction.ShouldNotThrow();
            user.Should().NotBeNull();
            user.FirstName.Should().Be(update.FirstName);
            user.LastName.Should().Be(update.LastName);

            // change permissions
            Permission permissions = Permission.Create | Permission.Update | Permission.Read;
            asyncFunction = async () => user = await Client.ChangePermissions(user.Id, permissions);
            asyncFunction.ShouldNotThrow();
            user.Should().NotBeNull();
            user.Permissions.Should().Be(permissions);

            // delete
            asyncFunction = async () => await Client.DeleteUser(user.Id);
            asyncFunction.ShouldNotThrow();

            // read list
            asyncFunction = async () => users = await Client.ReadUsers();
            asyncFunction.ShouldNotThrow();
            users.Any(x => x.Id == user.Id).Should().BeFalse();

            // clean up all test users
            users.Where(x => x.Email.StartsWith("dummy+")).ToList().ForEach(x => 
            {
                asyncFunction = async () => await Client.DeleteUser(x.Id);
                asyncFunction.ShouldNotThrow();
            });
        }
    }
}
