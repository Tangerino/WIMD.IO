using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Sensors;
using WimdioApiProxy.v2.DataTransferObjects.Devices;
using WimdioApiProxy.v2.DataTransferObjects.Calendars;
using WimdioApiProxy.v2.DataTransferObjects.TimeSeries;
using System.Threading;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class TimeSeriesTests : BaseTests
    {
        [TestMethod()]
        public void ReadData_Calendar_Positive()
        {
            // create device
            Device device = null;
            Func<Task> asyncFunction = async () => device = await CreateDevice(Client);
            asyncFunction.ShouldNotThrow();

            // create sensor
            Sensor sensor = null;
            asyncFunction = async () => sensor = await CreateSensor(Client, device);
            asyncFunction.ShouldNotThrow();

            // add data
            var data = CreateSensorData(new[] { sensor.RemoteId });
            asyncFunction = async () => await Client.AddSensorData(device.DevKey, data.Series);
            asyncFunction.ShouldNotThrow();


            // create calendar
            var random = Guid.NewGuid().ToString().Split('-').First();
            var newCalendar = new NewCalendar { Name = $"EN {random}" };
            Calendar calendar = null;
            asyncFunction = async () => calendar = await Client.CreateCalendar(newCalendar);
            asyncFunction.ShouldNotThrow();

            // create season
            var newSeason = new NewSeason { Name = $"EN {random}", StartDate = DateTime.Today };
            Season season = null;
            asyncFunction = async () => season = await Client.CreateSeason(calendar.Id, newSeason);
            asyncFunction.ShouldNotThrow();

            // create period
            var newPeriod = new NewPeriod
            {
                Name = $"EN {random}",
                StartTime = new TimeSpan(DateTime.Now.AddHours(-1).Hour, 00, 00),
                EndTime = new TimeSpan(DateTime.Now.AddHours(1).Hour, 00, 00),
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
                Sunday = true
            };
            Period period = null;
            asyncFunction = async () => period = await Client.CreatePeriod(calendar.Id, season.Id, newPeriod);
            asyncFunction.ShouldNotThrow();


            // read calendar data
            IEnumerable<CalendarData> calendarData = null;
            var maxWaitingTime = TimeSpan.FromSeconds(120);
            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.Elapsed < maxWaitingTime && ((calendarData == null) || (!calendarData.Any())))
            {
                asyncFunction = async () => calendarData = await Client.ReadCalendarData(sensor.Id, DateTime.Today.AddDays(-2).ToUniversalTime(), DateTime.Today.AddDays(2).ToUniversalTime(), DataOperation.Sum, TimeInterval.Day, calendar.Id);
                asyncFunction.ShouldNotThrow();
                if (!calendarData?.Any() ?? false)
                    Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            stopWatch.Stop();
            Debug.WriteLine($"I was waiting for calendar data for {stopWatch.Elapsed.TotalSeconds} secs");
            calendarData.Should().NotBeNullOrEmpty();
            calendarData.All(x => x.Period.Equals(period.Name) && x.Season.Equals(season.Name)).Should().BeTrue();

            // delete calendar
            asyncFunction = async () => await Client.DeleteCalendar(calendar.Id);
            asyncFunction.ShouldNotThrow();

            // delete device
            asyncFunction = async () => await Client.DeleteDevice(device.Id);
            asyncFunction.ShouldNotThrow();
        }
    }
}