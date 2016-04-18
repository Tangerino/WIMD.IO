using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Things;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class ThingsTests : BaseTests
    {
        [TestMethod()]
        public void Thing_CRUD_Positive()
        {
            // create
            Place place = null;
            Thing thing = null;
            Func<Task> asyncFunction = async () =>
            {
                place = await CreatePlace(Client);
                thing = await CreateThing(Client, place);
            };
            asyncFunction.ShouldNotThrow();
            thing.Should().NotBeNull();

            // read
            asyncFunction = async () => thing = await Client.ReadThing(thing.Id);
            asyncFunction.ShouldNotThrow();
            thing.Should().NotBeNull();

            // read list
            IEnumerable<Thing> things = null;
            asyncFunction = async () => things = await Client.ReadThings(place.Id);
            asyncFunction.ShouldNotThrow();
            things.Should().NotBeNullOrEmpty();
            things.Any(x => x.Id == thing.Id).Should().BeTrue();

            // update
            var update = new UpdateThing(thing)
            {
                Name = thing.Name + " Updated"
            };
            asyncFunction = async () => thing = await Client.UpdateThing(thing.Id, update);
            asyncFunction.ShouldNotThrow();
            thing.Should().NotBeNull();
            thing.Name.Should().NotBeNull(update.Name);

            // delete
            asyncFunction = async () => await Client.DeleteThing(thing.Id);
            asyncFunction.ShouldNotThrow();

            // read list
            asyncFunction = async () => things = await Client.ReadThings(place.Id);
            asyncFunction.ShouldNotThrow();
            things.Should().BeNullOrEmpty();

            // delete place
            asyncFunction = async () => await Client.DeletePlace(place.Id);
            asyncFunction.ShouldNotThrow();
        }
    }
}
