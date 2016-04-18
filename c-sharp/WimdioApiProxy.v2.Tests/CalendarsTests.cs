using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WimdioApiProxy.v2.DataTransferObjects.Calendars;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class CalendarsTests : BaseTests
    {
        [TestMethod()]
        public void Calendar_CRUD_Positive()
        {
            // create
            var random = Guid.NewGuid().ToString().Split('-').First();

            var newCalendar = new NewCalendar { Name = $"EN {random}" };
            Calendar calendar = null;
            Func<Task> asyncFunction = async () => calendar = await Client.CreateCalendar(newCalendar);
            asyncFunction.ShouldNotThrow();
            calendar.Should().NotBeNull();
            calendar.Name.Should().Be(newCalendar.Name);

            // read list
            IEnumerable<Calendar> calendars = null;
            asyncFunction = async () => calendars = await Client.ReadCalendars();
            asyncFunction.ShouldNotThrow();
            calendars.Should().NotBeNullOrEmpty();
            calendars.Any(x => x.Id == calendar.Id).Should().BeTrue();

            // update
            var update = new NewCalendar(calendar)
            {
                Name = calendar.Name + " Updated",
            };
            asyncFunction = async () => calendar = await Client.UpdateCalendar(calendar.Id, update);
            asyncFunction.ShouldNotThrow();
            calendar.Should().NotBeNull();
            calendar.Name.Should().Be(update.Name);


            // create special day
            var newDay = new NewSpecialDay { Name = $"EN {random}", Day = DateTime.Today, IsRecurrent = true };
            SpecialDay day = null;
            asyncFunction = async () => day = await Client.CreateSpecialDay(calendar.Id, newDay);
            asyncFunction.ShouldNotThrow();
            day.Should().NotBeNull();
            day.Name.Should().Be(newDay.Name);
            day.Day.Should().Be(newDay.Day);
            day.IsRecurrent.Should().Be(newDay.IsRecurrent);

            // update special day
            var updateDay = new NewSpecialDay(day) { Name = day.Name + " Updated" };
            asyncFunction = async () => await Client.UpdateSpecialDay(calendar.Id, day.Id, updateDay);
            asyncFunction.ShouldNotThrow();

            // delete special day
            asyncFunction = async () => await Client.DeleteSpecialDay(calendar.Id, day.Id);
            asyncFunction.ShouldNotThrow();


            // create season
            var newSeason = new NewSeason { Name = $"EN {random}", StartDate = DateTime.Today };
            Season season = null;
            asyncFunction = async () => season = await Client.CreateSeason(calendar.Id, newSeason);
            asyncFunction.ShouldNotThrow();
            season.Should().NotBeNull();
            season.Name.Should().Be(newSeason.Name);
            season.StartDate.Should().Be(newSeason.StartDate);

            // read seasons list
            IEnumerable<Season> seasons = null;
            asyncFunction = async () => seasons = await Client.ReadSeasons(calendar.Id);
            asyncFunction.ShouldNotThrow();
            seasons.Should().NotBeNullOrEmpty();
            seasons.Any(x => x.Id == season.Id).Should().BeTrue();

            // update season
            var updateSeason = new NewSeason(season) { Name = season.Name + " Updated" };
            asyncFunction = async () => season = await Client.UpdateSeason(calendar.Id, season.Id, updateSeason);
            asyncFunction.ShouldNotThrow();
            season.Should().NotBeNull();
            season.Name.Should().Be(updateSeason.Name);
            season.StartDate.Should().Be(updateSeason.StartDate);


            // create period
            var newPeriod = new NewPeriod
            {
                Name = $"EN {random}",
                StartTime = new TimeSpan(06, 00, 00),
                EndTime = new TimeSpan(18, 00, 00),
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true
            };
            Period period = null;
            asyncFunction = async () => period = await Client.CreatePeriod(calendar.Id, season.Id, newPeriod);
            asyncFunction.ShouldNotThrow();
            period.Should().NotBeNull();
            period.Name.Should().Be(newPeriod.Name);
            period.StartTime.Should().Be(newPeriod.StartTime);
            period.EndTime.Should().Be(newPeriod.EndTime);
            period.Monday.Should().Be(newPeriod.Monday);
            period.Tuesday.Should().Be(newPeriod.Tuesday);
            period.Wednesday.Should().Be(newPeriod.Wednesday);
            period.Thursday.Should().Be(newPeriod.Thursday);
            period.Friday.Should().Be(newPeriod.Friday);
            period.Saturday.Should().Be(newPeriod.Saturday);
            period.Sunday.Should().Be(newPeriod.Sunday);
            period.Special.Should().Be(newPeriod.Special);

            // read periods
            IEnumerable<Period> periods = null;
            asyncFunction = async () => periods = await Client.ReadPeriods(calendar.Id, season.Id);
            asyncFunction.ShouldNotThrow();
            periods.Should().NotBeNullOrEmpty();
            periods.Any(x => x.Id == period.Id).Should().BeTrue();

            // update period
            var updatePeriod = new NewPeriod(period) { Name = period.Name + " Name", EndTime = period.EndTime.Add(TimeSpan.FromHours(3)), Friday = true };
            asyncFunction = async () => period = await Client.UpdatePeriod(calendar.Id, season.Id, period.Id, updatePeriod);
            asyncFunction.ShouldNotThrow();
            period.Should().NotBeNull();
            period.Name.Should().Be(updatePeriod.Name);
            period.StartTime.Should().Be(updatePeriod.StartTime);
            period.EndTime.Should().Be(updatePeriod.EndTime);
            period.Monday.Should().Be(updatePeriod.Monday);
            period.Tuesday.Should().Be(updatePeriod.Tuesday);
            period.Wednesday.Should().Be(updatePeriod.Wednesday);
            period.Thursday.Should().Be(updatePeriod.Thursday);
            period.Friday.Should().Be(updatePeriod.Friday);
            period.Saturday.Should().Be(updatePeriod.Saturday);
            period.Sunday.Should().Be(updatePeriod.Sunday);
            period.Special.Should().Be(updatePeriod.Special);

            // delete period
            periods.ToList().ForEach(x =>
            {
                asyncFunction = async () => await Client.DeletePeriod(calendar.Id, season.Id, x.Id);
                asyncFunction.ShouldNotThrow();
            });

            // delete seasons
            seasons.ToList().ForEach(x =>
            {
                asyncFunction = async () => await Client.DeleteSeason(calendar.Id, x.Id);
                asyncFunction.ShouldNotThrow();
            });

            // delete
            calendars.ToList().ForEach(x => 
            {
                asyncFunction = async () => await Client.DeleteCalendar(x.Id);
                asyncFunction.ShouldNotThrow();
            });
        }
    }
}