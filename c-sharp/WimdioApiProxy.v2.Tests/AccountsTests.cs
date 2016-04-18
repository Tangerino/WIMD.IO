using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class AccountsTests : BaseTests
    {
        [TestMethod()]
        [Ignore()]
        public void ChangePassword_Positive()
        {
            string actual = null;
            string expected = "An e-mail was sent. Please follow instructions";
            Func<Task> asyncFunction = async () =>
            {
                actual = await Client.ChangePassword(Credentials);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().Be(expected, "Method should return specific message");
        }

        [TestMethod()]
        [Ignore()]
        public void ResetAccount_Positive()
        {
            string actual = null;
            string expected = "An e-mail was sent. Please follow instructions";
            Func<Task> asyncFunction = async () =>
            {
                actual = await Client.ResetAccount(new Account { Email = Credentials.Email });
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().Be(expected, "Method should return specific message");
        }

        [TestMethod()]
        public void Pocket_Positive()
        {
            var pocketName = "TestPocketName";
            var expected = new { favorites1 = "dashboards", favorites2 = "alarms" };
            object actual = null;

            Func<Task> asyncFunction = async () =>
            {
                await Client.CreatePocket(pocketName, expected);
                actual = await Client.ReadPocket(pocketName);
                await Client.DeletePocket(pocketName);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Pocket content expected");
        }
    }
}