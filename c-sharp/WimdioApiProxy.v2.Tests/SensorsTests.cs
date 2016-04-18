using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Sensors;
using WimdioApiProxy.v2.DataTransferObjects.Devices;
using WimdioApiProxy.v2.Helpers;
using WimdioApiProxy.v2.DataTransferObjects.Things;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class SensorsTests : BaseTests
    {
        [TestMethod()]
        public void Sensor_CRUD_Positive()
        {
            // create
            Device device = null;
            Sensor sensor = null;
            Func<Task> asyncFunction = async () =>
            {
                device = await CreateDevice(Client);
                sensor = await CreateSensor(Client, device);
            };
            asyncFunction.ShouldNotThrow();
            sensor.Should().NotBeNull();

            // read
            asyncFunction = async () => sensor = await Client.ReadSensor(sensor.Id);
            asyncFunction.ShouldNotThrow();
            sensor.Should().NotBeNull();

            // read list
            IEnumerable<Sensor> sensors = null;
            asyncFunction = async () => sensors = await Client.ReadSensors(device.Id);
            asyncFunction.ShouldNotThrow();
            sensors.Should().NotBeNullOrEmpty();
            sensors.Any(x => x.Id == sensor.Id).Should().BeTrue();

            // update
            var update = new UpdateSensor(sensor)
            {
                Description = sensor.Description + "Updated",
                Tseoi = 1,
            };
            asyncFunction = async () => sensor = await Client.UpdateSensor(device.DevKey, sensor.RemoteId, update);
            asyncFunction.ShouldNotThrow();
            sensor.Should().NotBeNull();
            sensor.Name.Should().Be(update.Name);
            sensor.Description.Should().Be(update.Description);
            sensor.Tseoi.Should().Be(update.Tseoi);

            // link list unlink
            Place place = null;
            Thing thing = null;
            asyncFunction = async () =>
            {
                place = await CreatePlace(Client);
                thing = await CreateThing(Client, place);

                await Client.LinkSensor(thing.Id, sensor.Id);
                sensors = await Client.ListSensors(thing.Id);
                await Client.UnlinkSensor(thing.Id, sensor.Id);
            };
            asyncFunction.ShouldNotThrow();
            sensors.Should().NotBeNullOrEmpty();
            sensors.Any(x => x.Id == sensor.Id).Should().BeTrue();

            // add data
            asyncFunction = async () => 
            {
                var data = CreateSensorData(new[] { sensor.RemoteId });
                await Client.AddSensorData(device.DevKey, data.Series);
            };
            asyncFunction.ShouldNotThrow();

            // read rule
            Rule rule = null;
            asyncFunction = async () => rule = await Client.ReadSensorRule(sensor.Id);
            asyncFunction.ShouldNotThrow();
            rule.Should().NotBeNull();

            // update rule
            var updatedRule = new UpdateRule(rule)
            {
                IsEnabled = !rule.IsEnabled,
                Name = rule.Name + " Updated",
                Description = rule.Name + " Updated",
                IsIncremental = !rule.IsIncremental,
                CheckGap = !rule.CheckGap,
                LogInterval = !rule.LogInterval,
                IndexToAbsolute = !rule.IndexToAbsolute,
                HasMinimumValue = !rule.HasMinimumValue,
                MinimumValue = rule.MinimumValue - 1,
                HasMaximumValue = !rule.HasMaximumValue,
                MaximumValue = rule.MaximumValue + 1
            };
            asyncFunction = async () => rule = await Client.UpdateSensorRule(sensor.Id, updatedRule);
            asyncFunction.ShouldNotThrow();
            rule.Should().NotBeNull();
            rule.IsEnabled.Should().Be(updatedRule.IsEnabled);
            rule.Name.Should().Be(updatedRule.Name);
            rule.Description.Should().Be(updatedRule.Description);
            rule.IsIncremental.Should().Be(updatedRule.IsIncremental);
            rule.CheckGap.Should().Be(updatedRule.CheckGap);
            rule.LogInterval.Should().Be(updatedRule.LogInterval);
            rule.IndexToAbsolute.Should().Be(updatedRule.IndexToAbsolute);
            rule.HasMinimumValue.Should().Be(updatedRule.HasMinimumValue);
            rule.MinimumValue.Should().Be(updatedRule.MinimumValue);
            rule.HasMaximumValue.Should().Be(updatedRule.HasMaximumValue);
            rule.MaximumValue.Should().Be(updatedRule.MaximumValue);

            // delete
            asyncFunction = async () => 
            {
                await Client.DeleteSensor(device.DevKey, sensor.RemoteId);
                await Client.DeleteThing(thing.Id);
                await Client.DeletePlace(place.Id);
            };
            asyncFunction.ShouldNotThrow();

            // read list
            asyncFunction = async () => sensors = await Client.ReadSensors(device.Id);
            asyncFunction.ShouldNotThrow();
            sensors.Should().BeNullOrEmpty();

            // delete device
            asyncFunction = async () => await Client.DeleteDevice(device.Id);
            asyncFunction.ShouldNotThrow();
        }

        [TestMethod()]
        public void VirtualSensorVariables_CRUD_Positive()
        {
            // create
            Device device = null;
            Sensor sensor = null;
            IEnumerable<SensorVariable> sensorVariables = null;
            Func<Task> asyncFunction = async () =>
            {
                device = await CreateDevice(Client);
                sensor = await CreateSensor(Client, device, true);
                sensorVariables = new List<SensorVariable> { new SensorVariable { Id = sensor.Id.ToString(), Variable = "Dummy" } };
                await Client.AddVirtualSensorVariables(sensor.Id, sensorVariables);
            };
            asyncFunction.ShouldNotThrow();

            // read list
            asyncFunction = async () => sensorVariables = await Client.ReadVirtualSensorVariables(sensor.Id);
            asyncFunction.ShouldNotThrow();
            sensorVariables.Should().NotBeNullOrEmpty();

            // read virtual sensors
            IEnumerable<Sensor> sensors = null;
            asyncFunction = async () => sensors = await Client.ReadVirtualSensors(device.Id);
            asyncFunction.ShouldNotThrow();
            sensorVariables.Should().NotBeNullOrEmpty();

            // delete
            sensorVariables.ToList().ForEach(x => 
            {
                asyncFunction = async () => await Client.DeleteVirtualSensorVariables(sensor.Id, x.Id);
                asyncFunction.ShouldNotThrow();
            });

            // read list
            asyncFunction = async () => sensorVariables = await Client.ReadVirtualSensorVariables(sensor.Id);
            asyncFunction.ShouldNotThrow();
            sensorVariables.Should().BeNullOrEmpty();

            // delete device
            asyncFunction = async () => await Client.DeleteDevice(device.Id);
            asyncFunction.ShouldNotThrow();
        }

        [TestMethod()]
        public void SensorData_Serialize_Positive()
        {
            var expected = CreateSensorData(new[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() });
            SensorData actual = null;
            var serializer = new JsonSerializer();
            Action act = () =>
            {
                var json = serializer.Serialize(expected);
                actual = serializer.Deserialize<SensorData>(json);
            };
            act.ShouldNotThrow();
            actual.Should().NotBeNull();
            actual.Series?.FirstOrDefault()?.RemoteId.Should().Be(expected.Series.FirstOrDefault().RemoteId);
            actual.Series?.FirstOrDefault()?.Values?.FirstOrDefault()?[0].ToString().Should().Be(expected.Series.FirstOrDefault().Values.FirstOrDefault()[0].ToString());
            actual.Series?.FirstOrDefault()?.Values?.FirstOrDefault()?[1].ToString().Should().Be(expected.Series.FirstOrDefault().Values.FirstOrDefault()[1].ToString());
        }
    }
}
