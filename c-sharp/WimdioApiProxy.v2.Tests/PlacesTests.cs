using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Places;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class PlacesTests : BaseTests
    {
        [TestMethod()]
        public void Place_CRUD_Positive()
        {
            // create
            Place place = null;
            Func<Task> asyncFunction = async () => place = await CreatePlace(Client);
            asyncFunction.ShouldNotThrow();
            place.Should().NotBeNull();

            // read
            asyncFunction = async () => place = await Client.ReadPlace(place.Id);
            asyncFunction.ShouldNotThrow();
            place.Should().NotBeNull();

            // real list
            IEnumerable<Place> places = null;
            asyncFunction = async () => places = await Client.ReadPlaces();
            asyncFunction.ShouldNotThrow();
            places.Should().NotBeNullOrEmpty();
            places.Any(x => x.Id == place.Id).Should().BeTrue();

            // update
            var updated = new UpdatePlace(place)
            {
                Name = place.Name + " Updated",
            };
            asyncFunction = async () => place = await Client.UpdatePlace(place.Id, updated);
            asyncFunction.ShouldNotThrow();
            place.Should().NotBeNull();
            place.Name.Should().Be(updated.Name);

            // link unlink
            User user = null;
            asyncFunction = async () =>
            {
                user = await CreateUser(Client);
                await Client.LinkPlace(place.Id, user.Id);
                await Client.UnlinkPlace(place.Id, user.Id);
                await Client.DeleteUser(user.Id);
            };
            asyncFunction.ShouldNotThrow();

            // delete
            asyncFunction = async () => await Client.DeletePlace(place.Id);
            asyncFunction.ShouldNotThrow();

            // real list
            asyncFunction = async () => places = await Client.ReadPlaces();
            asyncFunction.ShouldNotThrow();
            places.Any(x => x.Id == place.Id).Should().BeFalse();
        }
    }
}
