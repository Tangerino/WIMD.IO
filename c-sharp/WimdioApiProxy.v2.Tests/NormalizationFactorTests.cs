using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class NormalizationFactorTests : BaseTests
    {
        [TestMethod()]
        public void NormalizationFactor_CRUD_Positive()
        {
            // create
            Place place = null;
            NormalizationFactor normalizationFactor = null;
            Func<Task> asyncFunction = async () =>
            {
                place = await CreatePlace(Client);
                normalizationFactor = await CreateNormalizationFactor(Client, place);
            };
            asyncFunction.ShouldNotThrow();
            normalizationFactor.Should().NotBeNull();

            // read
            asyncFunction = async () => await Client.ReadNormalizationFactor(normalizationFactor.Id);
            asyncFunction.ShouldNotThrow();
            normalizationFactor.Should().NotBeNull();

            // read list
            IEnumerable<NormalizationFactor> normalizationFactors = null;
            asyncFunction = async () => normalizationFactors = await Client.ReadNormalizationFactors(place.Id);
            asyncFunction.ShouldNotThrow();
            normalizationFactors.Should().NotBeNullOrEmpty();
            normalizationFactors.Any(x => x.Id == normalizationFactor.Id);

            // update
            var updated = new UpdateNormalizationFactor(normalizationFactor)
            {
                Name = normalizationFactor.Name + " Updated",
                Aggregation = AggregationType.Sum
            };
            asyncFunction = async () => normalizationFactor = await Client.UpdateNormalizationFactor(normalizationFactor.Id, updated);
            asyncFunction.ShouldNotThrow();
            normalizationFactor.Should().NotBeNull();
            normalizationFactor.Name.Should().Be(updated.Name);
            normalizationFactor.Aggregation.Should().Be(updated.Aggregation);
            normalizationFactor.Description.Should().Be(updated.Description);
            normalizationFactor.Operation.Should().Be(updated.Operation);
            normalizationFactor.Unit.Should().Be(updated.Unit);

            // delete
            asyncFunction = async () => await Client.DeleteNormalizationFactor(normalizationFactor.Id);
            asyncFunction.ShouldNotThrow();

            // read list
            asyncFunction = async () => normalizationFactors = await Client.ReadNormalizationFactors(place.Id);
            asyncFunction.ShouldNotThrow();
            normalizationFactors.Should().BeNullOrEmpty();

            // delete place
            asyncFunction = async () => await Client.DeletePlace(place.Id);
            asyncFunction.ShouldNotThrow();
        }
    }
}
