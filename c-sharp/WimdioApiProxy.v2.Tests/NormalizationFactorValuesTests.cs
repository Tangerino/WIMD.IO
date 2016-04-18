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
    public class NormalizationFactorValuesTests : BaseTests
    {
        [TestMethod()]
        public void NormalizationFactorValue_CRUD_Positive()
        {
            // create
            Place place = null;
            NormalizationFactor normalizationFactor = null;
            NormalizationFactorValue normalizationFactorValue = null;
            Func<Task> asyncFunction = async () =>
            {
                place = await CreatePlace(Client);
                normalizationFactor = await CreateNormalizationFactor(Client, place);
                normalizationFactorValue = await CreateNormalizationFactorValue(Client, normalizationFactor);
            };
            asyncFunction.ShouldNotThrow();
            normalizationFactorValue.Should().NotBeNull();

            // read
            IEnumerable<NormalizationFactorValue> normalizationFactorValues = null;
            asyncFunction = async () => normalizationFactorValues = await Client.ReadNormalizationFactorValues(normalizationFactor.Id);
            asyncFunction.ShouldNotThrow();
            normalizationFactorValues.Should().NotBeNullOrEmpty();
            normalizationFactorValues.Any(x => x.Timestamp == normalizationFactorValue.Timestamp && x.Value == normalizationFactorValue.Value).Should().BeTrue();

            // update
            var update = new UpdateNormalizationFactorValue(normalizationFactorValue)
            {
                Value = normalizationFactorValue.Value + "12345"
            };
            asyncFunction = async () => normalizationFactorValue = await Client.UpdateNormalizationFactorValue(normalizationFactor.Id, update);
            asyncFunction.ShouldNotThrow();
            normalizationFactorValue.Should().NotBeNull();
            normalizationFactorValue.Timestamp.Should().Be(update.Timestamp);
            normalizationFactorValue.Value.Should().Be(update.Value);

            // delete
            asyncFunction = async () => await Client.DeleteNormalizationFactorValue(normalizationFactor.Id, normalizationFactorValue.Timestamp);
            asyncFunction.ShouldNotThrow();

            // read
            asyncFunction = async () => normalizationFactorValues = await Client.ReadNormalizationFactorValues(normalizationFactor.Id);
            asyncFunction.ShouldNotThrow();
            normalizationFactorValues.Should().NotBeNullOrEmpty();

            // delete normalization factor and place
            asyncFunction = async () =>
            {
                await Client.DeleteNormalizationFactor(normalizationFactor.Id);
                await Client.DeletePlace(place.Id);
            };
            asyncFunction.ShouldNotThrow();
        }
    }
}
