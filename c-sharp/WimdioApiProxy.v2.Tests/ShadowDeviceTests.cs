using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Devices;
using WimdioApiProxy.v2.DataTransferObjects.ShadowDevice;
using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class ShadowDeviceTests : BaseTests
    {
        [TestMethod()]
        public void ShadowCommands_CarlosTestCase_Positive()
        {
            // Please do not delete it
            // It is a real device sending data to the WIMD and has a lot of objetcs in it
            var testDeviceId = Guid.Parse("01344f33-edb8-11e5-8a0f-04017fd5d401");
            Device device = null;
            Func<Task> asyncFunction = async () => device = await Client.ReadDevice(testDeviceId);
            asyncFunction.ShouldNotThrow($"ReadDevice({testDeviceId}) shoud not throw");
            device.Should().NotBeNull($"test device ID = {testDeviceId} should exist");
            device.Id.Should().Be(testDeviceId);

            // Gateway requests commands
            var limit = 10;
            IEnumerable<Command> commands = null;
            asyncFunction = async () => commands = await Client.ReadDeviceCommands(device.DevKey, limit);
            asyncFunction.ShouldNotThrow($"ReadDeviceCommands({device.DevKey}, {limit}) shoud not throw");
            commands.Should().NotBeNull("ReadDeviceCommands should return empty array");

            // Gateway send some configuration
            var settings = CarlosTestSettings();
            asyncFunction = async () => await Client.SendDeviceSettings(device.DevKey, settings);
            asyncFunction.ShouldNotThrow($"SendDeviceSettings({device.DevKey}, settings) shoud not throw");

            // NORTH API read devices
            testDeviceId = Guid.Parse("01344f33-edb8-11e5-8a0f-04017fd5d401");
            IEnumerable <Device> devices = null;
            asyncFunction = async () => devices = await Client.ReadDevices();
            asyncFunction.ShouldNotThrow($"ReadDevices() shoud not throw");
            devices.Should().NotBeNullOrEmpty("ReadDevices() should return non empty list of devices");
            devices.Any(x => x.Id == testDeviceId).Should().BeTrue($"ReadDevices() result should contain a device ID = {testDeviceId}");
            device = devices.First(x => x.Id == testDeviceId);

            // read the available objects from device
            var testObjectName = "cloudservice";
            IEnumerable<ShadowObjectName> shadowObjectNames = null;
            asyncFunction = async () => shadowObjectNames = await Client.ReadDeviceObjects(device.Id);
            asyncFunction.ShouldNotThrow($"ReadDeviceObjects({device.Id}) should not throw");
            shadowObjectNames.Should().NotBeNullOrEmpty($"ReadDeviceObjects({device.Id}) should return not empty list");
            shadowObjectNames.Any(x => x.ObjectName.Equals(testObjectName, StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue($"ReadDeviceObjects({device.Id}) result should contain an object '{testObjectName}'");
            var testObject = shadowObjectNames.First(x => x.ObjectName.Equals(testObjectName, StringComparison.InvariantCultureIgnoreCase));

            // Read a particular object
            IEnumerable<ShadowObject> shadowObjects = null;
            asyncFunction = async () => shadowObjects = await Client.ReadDeviceObjects(device.Id, testObject.ObjectName, 1, 3);
            asyncFunction.ShouldNotThrow($"ReadDeviceObjects({device.Id}, '{testObject.ObjectName}') should not throw");
            shadowObjects.Should().NotBeNullOrEmpty($"ReadDeviceObjects({device.Id}, '{testObject.ObjectName}') should return non empty list");
            shadowObjects.Count().Should().Be(2, $"ReadDeviceObjects({device.Id}, '{testObject.ObjectName}') result list should contain 2 objects");
            var shadowObject = shadowObjects.First();

            // Create a command
            // As we do have the actual gateway settings, I'd like to change some values, in this case I'll update the parameter "cleanSession" to 1
            var newCommand = new NewCommand
            {
                ObjectName = testObject.ObjectName,
                ObjectId = shadowObject.Id,
                Action = CommandAction.UPDATE,
                ObjectContent = new ShadowObjectContent { CleanSession = true }
            };
            CommandDeatils command = null;
            asyncFunction = async () => command = await Client.CreateDeviceCommand(device.Id, newCommand);
            asyncFunction.ShouldNotThrow($"CreateDeviceCommand({device.Id}, newCommand) should not throw");
            command.Should().NotBeNull($"CreateDeviceCommand({device.Id}, newCommand) should not return NULL");
            command.ObjectName.Should().Be(testObject.ObjectName);

            // Again, I'll update the "timeout" to 60 and add a comment to the new command. This will create a new command
            newCommand = new NewCommand
            {
                ObjectName = testObject.ObjectName,
                ObjectId = shadowObject.Id,
                Action = CommandAction.UPDATE,
                ObjectContent = new ShadowObjectContent { Timeout = 60 },
                Comment = "Increase timeout"
            };
            asyncFunction = async () => command = await Client.CreateDeviceCommand(device.Id, newCommand);
            asyncFunction.ShouldNotThrow($"CreateDeviceCommand({device.Id}, newCommand) should not throw");
            command.Should().NotBeNull($"CreateDeviceCommand({device.Id}, newCommand) should not return NULL");
            command.ObjectName.Should().Be(testObjectName);
            command.Comment.Should().Be(newCommand.Comment);

            // I'll read now the commands available to the remote device
            var from = DateTime.Today.AddDays(-1);
            var to = DateTime.Today.AddDays(1);
            IEnumerable<CommandDeatils> commandDetails = null;
            asyncFunction = async () => commandDetails = await Client.ReadDeviceCommands(device.Id, from, to);
            asyncFunction.ShouldNotThrow($"ReadDeviceCommands({device.DevKey}, {from}, {to}) shoud not throw");
            commandDetails.Should().NotBeNullOrEmpty($"ReadDeviceCommands({device.DevKey}, {from}, {to} should not return empty array");

            // Now the gateway read one command, execute it and acknowledge (device uses its DEVKEY)
            limit = 2;
            asyncFunction = async () => commands = await Client.ReadDeviceCommands(device.DevKey, limit);
            asyncFunction.ShouldNotThrow($"ReadDeviceCommands({device.DevKey}, {limit}) shoud not throw");
            commands.Should().NotBeNullOrEmpty("ReadDeviceCommands should return empty array");
            commands.All(x => x.ObjectId == shadowObject.Id).Should().BeTrue();

            // The gateway will execute the commands and send back the result to the server
            var commandStates = new List<CommandState>
            {
                new CommandState { Id = commands.First().Id, Status = 0 },
                new CommandState { Id = commands.Last().Id, Status = 13 },
            };
            asyncFunction = async () => await Client.AcknowledgeDeviceCommands(device.DevKey, commandStates);
            asyncFunction.ShouldNotThrow($"AcknowledgeDeviceCommands({device.DevKey}, commandStates) shoud not throw");

            // I'll now read back the commands I've sent. They are the same as before but the field acknowledge has a date and staus is also set
            asyncFunction = async () => commandDetails = await Client.ReadDeviceCommands(device.Id, from, to);
            asyncFunction.ShouldNotThrow($"ReadDeviceCommands({device.DevKey}, {from}, {to}) shoud not throw");
            commandDetails.Should().NotBeNullOrEmpty($"ReadDeviceCommands({device.DevKey}, {from}, {to} should not return empty array");
            commandDetails.All(x => x.Acknowledge.HasValue).Should().BeTrue();
            commandDetails.All(x => x.Status.HasValue).Should().BeTrue();
        }

        [TestMethod()]
        public void ShadowObject_Deserialize_Positive()
        {
            var json = "[{\n\t\t\"id\":\t20,\n\t\t\"objectcontent\":\t\"{\\\"name\\\":\\\"wimd\\\",\\\"type\\\":18,\\\"enabled\\\":1,\\\"host\\\":1,\\\"publishinterval\\\":1440,\\\"tagposition\\\":31310287,\\\"lastrun\\\":\\\"2016-03-19T08:46:05\\\",\\\"nextrun\\\":\\\"9999-12-31T23:59:59.9999999\\\",\\\"status\\\":200,\\\"pause\\\":0,\\\"zipit\\\":0,\\\"cleanSession\\\":0,\\\"timeout\\\":30,\\\"activationcode\\\":\\\"d109a897-d26f-11e5-8d5d-04017fd5d401\\\",\\\"eventposition\\\":0,\\\"alarmposition\\\":0,\\\"configposition\\\":2}\"\n\t}, {\n\t\t\"id\":\t21,\n\t\t\"objectcontent\":\t\"{\\\"name\\\":\\\"WIMD.IO\\\",\\\"type\\\":18,\\\"enabled\\\":1,\\\"host\\\":11,\\\"publishinterval\\\":1,\\\"tagposition\\\":-1,\\\"lastrun\\\":\\\"2016-03-19 10:54:04\\\",\\\"nextrun\\\":\\\"2016-03-19 10:55:00\\\",\\\"status\\\":200,\\\"pause\\\":0,\\\"zipit\\\":0,\\\"apiKey\\\":null,\\\"cleanSession\\\":1,\\\"timeout\\\":30,\\\"activationcode\\\":\\\"013451c7-edb8-11e5-8a0f-04017fd5d401\\\",\\\"feedid\\\":null,\\\"mailto\\\":null,\\\"mailcc\\\":null,\\\"mailbcc\\\":null,\\\"eventposition\\\":-1,\\\"alarmposition\\\":-1,\\\"configposition\\\":-1}\"\n\t}]";
            var serializer = new JsonSerializer();

            IEnumerable<ShadowObject> expected = null;
            Action act = () => expected = serializer.Deserialize<ShadowObject[]>(json);
            act.ShouldNotThrow("Deserialize<ShadowObject[]>(expected) should not throw");
            expected.Should().NotBeNullOrEmpty("Deserialize<ShadowObject[]>(expected) should contain some objects");

            var actualStr = string.Empty;
            act = () => actualStr = serializer.Serialize(expected);
            act.ShouldNotThrow("Serialize(actual) should not throw");

            IEnumerable<ShadowObject> actual = null;
            act = () => actual = serializer.Deserialize<ShadowObject[]>(actualStr);
            act.ShouldNotThrow("Deserialize<ShadowObject[]>(expected) should not throw");
            actual.Should().NotBeNullOrEmpty("Deserialize<ShadowObject[]>(expected) should contain some objects");
        }

        [TestMethod()]
        public void ShadowObject_DeserializeCommand_Positive()
        {
            var json = "[{\n\t\t\"id\":\t\"6ad1c14b-ee61-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:03:07\",\n\t\t\"sent\":\t\"2016-03-20 06:04:03\",\n\t\t\"acknowledge\":\t\"2016-03-20 06:04:03\",\n\t\t\"status\":\t2,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"cleanSession\\\": 1\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}, {\n\t\t\"id\":\t\"8eea2f20-ee61-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:04:08\",\n\t\t\"sent\":\t\"2016-03-20 06:05:03\",\n\t\t\"acknowledge\":\t\"2016-03-20 06:05:03\",\n\t\t\"status\":\t2,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"cleanSession\\\": 1\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}, {\n\t\t\"id\":\t\"a69813cc-ee61-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:04:47\",\n\t\t\"sent\":\t\"2016-03-20 06:05:03\",\n\t\t\"acknowledge\":\t\"2016-03-20 06:05:03\",\n\t\t\"status\":\t2,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"Increase timeout\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"timeout\\\": 60\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}, {\n\t\t\"id\":\t\"0e5b2285-ee62-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:07:41\",\n\t\t\"sent\":\t\"2016-03-20 06:08:03\",\n\t\t\"acknowledge\":\t\"2016-03-20 06:08:03\",\n\t\t\"status\":\t2,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"cleanSession\\\": 1\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}, {\n\t\t\"id\":\t\"0ecb4f2c-ee62-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:07:42\",\n\t\t\"sent\":\t\"2016-03-20 06:08:03\",\n\t\t\"acknowledge\":\t\"2016-03-20 06:08:03\",\n\t\t\"status\":\t2,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"Increase timeout\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"timeout\\\": 60\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}, {\n\t\t\"id\":\t\"911bc11a-ee62-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:11:21\",\n\t\t\"sent\":\t\"2016-03-20 06:12:03\",\n\t\t\"acknowledge\":\t\"2016-03-20 06:12:03\",\n\t\t\"status\":\t2,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"cleanSession\\\": 1\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}, {\n\t\t\"id\":\t\"91910a27-ee62-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:11:22\",\n\t\t\"sent\":\t\"2016-03-20 06:12:03\",\n\t\t\"acknowledge\":\t\"2016-03-20 06:12:03\",\n\t\t\"status\":\t2,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"Increase timeout\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"timeout\\\": 60\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}, {\n\t\t\"id\":\t\"b66ac73b-ee62-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:12:23\",\n\t\t\"sent\":\t\"\",\n\t\t\"acknowledge\":\t\"\",\n\t\t\"status\":\t0,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"cleanSession\\\": 1\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}, {\n\t\t\"id\":\t\"b6db36d0-ee62-11e5-8a0f-04017fd5d401\",\n\t\t\"created\":\t\"2016-03-20 06:12:24\",\n\t\t\"sent\":\t\"\",\n\t\t\"acknowledge\":\t\"\",\n\t\t\"status\":\t0,\n\t\t\"objectname\":\t\"cloudservice\",\n\t\t\"objectid\":\t20,\n\t\t\"action\":\t\"UPDATE\",\n\t\t\"comment\":\t\"Increase timeout\",\n\t\t\"createdby\":\t\"a5865a97-dd15-11e5-8db7-04017fd5d401\",\n\t\t\"objectcontent\":\t\"{\\r\\n  \\\"timeout\\\": 60\\r\\n}\",\n\t\t\"duedate\":\t\"1980-01-01 00:00:00\",\n\t\t\"deleted\":\t0,\n\t\t\"deletedby\":\t\"\"\n\t}]";

            var serializer = new JsonSerializer();

            IEnumerable<CommandDeatils> expected = null;
            Action act = () => expected = serializer.Deserialize<CommandDeatils[]>(json);
            act.ShouldNotThrow("Deserialize<Command[]>(expected) should not throw");
            expected.Should().NotBeNullOrEmpty("Deserialize<Command[]>(expected) should contain some objects");

            var actualStr = string.Empty;
            act = () => actualStr = serializer.Serialize(expected);
            act.ShouldNotThrow("Serialize(actual) should not throw");

            IEnumerable<CommandDeatils> actual = null;
            act = () => actual = serializer.Deserialize<CommandDeatils[]>(actualStr);
            act.ShouldNotThrow("Deserialize<Command[]>(expected) should not throw");
            actual.Should().NotBeNullOrEmpty("Deserialize<Command[]>(expected) should contain some objects");
        }
    }
}
