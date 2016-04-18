using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Etls;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class EtlsTests : BaseTests
    {
        [TestMethod()]
        public void Etl_CRUD_Positive()
        {
            // create
            Place place = null;
            Etl etl = null;
            Func<Task> asyncFunction = async () =>
            {
                place = await CreatePlace(Client);
                etl = await CreateEtl(Client, place);
            };
            asyncFunction.ShouldNotThrow();
            etl.Should().NotBeNull();
            etl.PlaceId.Should().Be(place.Id);

            // read
            asyncFunction = async () => await Client.ReadEtl(etl.Id);
            etl.Should().NotBeNull();
            etl.PlaceId.Should().Be(place.Id);

            // real list
            IEnumerable<Etl> etls = null;
            asyncFunction = async () => etls = await Client.ReadEtls();
            asyncFunction.ShouldNotThrow();
            etls.Should().NotBeNullOrEmpty();
            etls.Any(x => x.Id == etl.Id).Should().BeTrue();

            // update
            var update = new UpdateEtl(etl)
            {
                Name = etl.Name + " Updated",
            };
            asyncFunction = async () => etl = await Client.UpdateEtl(etl.Id, update);
            asyncFunction.ShouldNotThrow();
            etl.PlaceId.Should().Be(place.Id);
            etl.Name.Should().Be(update.Name);
            etl.DatabaseName.Should().Be(update.DatabaseName);

            // delete
            asyncFunction = async () => 
            {
                await Client.DeleteEtl(etl.Id);
                await Client.DeletePlace(place.Id);
            };
            asyncFunction.ShouldNotThrow();

            // read list
            asyncFunction = async () => etls = await Client.ReadEtls();
            asyncFunction.ShouldNotThrow();
            etls.Any(x => x.Id == etl.Id).Should().BeFalse();
        }
    }
}
