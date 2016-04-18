using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Devices;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class DevicesTests : BaseTests
    {
        [TestMethod()]
        public void ReadDevices_Positive()
        {
            IEnumerable<Device> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await Client.ReadDevices();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void Device_CRUD_Positive()
        {
            // create
            Device device = null;
            Func<Task> asyncFunction = async () => device = await CreateDevice(Client);
            asyncFunction.ShouldNotThrow();
            device.Should().NotBeNull();

            // read
            asyncFunction = async () => device = await Client.ReadDevice(device.Id);
            asyncFunction.ShouldNotThrow();
            device.Should().NotBeNull();

            // read list
            IEnumerable<Device> devices = null;
            asyncFunction = async () => devices = await Client.ReadDevices();
            asyncFunction.ShouldNotThrow();
            devices.Should().NotBeNullOrEmpty();
            devices.Any(x => x.Id == device.Id).Should().BeTrue();

            // update
            var update = new UpdateDevice(device)
            {
                Name = device.Name + " Updated",
                Description = device.Description + " Updated",
            };
            asyncFunction = async () => device = await Client.UpdateDevice(device.Id, update);
            asyncFunction.ShouldNotThrow();
            device.Should().NotBeNull();
            device.Name.Should().Be(update.Name);
            device.Description.Should().Be(update.Description);
            device.Mac.Should().Be(update.Mac);

            // delete
            asyncFunction = async () => await Client.DeleteDevice(device.Id);
            asyncFunction.ShouldNotThrow();

            // read list
            asyncFunction = async () => devices = await Client.ReadDevices();
            asyncFunction.ShouldNotThrow();
            devices.Should().NotBeNull();
            devices.Any(x => x.Id == device.Id).Should().BeFalse();
        }
    }
}
