using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.DropBox;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class DropboxTests : BaseTests
    {
        [TestMethod()]
        public void Dropbox_ReadFileInfo_Positive()
        {
            IEnumerable<FileInfo> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client);
                actual = await Client.ReadFilesInformation(device.Id, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1));
            };
            asyncFunction.ShouldNotThrow();
            actual.Should().NotBeNull();
        }

        [TestMethod()]
        public void Dropbox_FirmwareUpgrade_Positive()
        {
            DeviceFileInfo actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var file = new NewFile
                {
                    Url = new Uri("http://veryshorthistory.com/wp-content/uploads/2015/04/knights-templar.jpg"),
                    Action = FileAction.POST,
                    Type = FileType.FIRMWARE_UPGRADE
                };
                var device = await CreateDevice(Client);
                await Client.SendFileToDevice(device.Id, file);
                actual = await Client.DeviceReadFileInfo(device.DevKey);
            };
            asyncFunction.ShouldNotThrow();
            actual.Should().NotBeNull();
        }
    }
}
