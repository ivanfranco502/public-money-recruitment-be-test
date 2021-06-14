using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.FunctionalTests
{
	public class GetCalendarTests:IDisposable
    {
        private readonly HttpClient _client;

        public GetCalendarTests()
        {
			var _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
			_client = _server.CreateClient();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

			DateTime nextMonth = DateTime.Today.AddMonths(1);
            DateTime nextMonthAndOneDay = nextMonth.AddDays(1);
            var postBooking1Request = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 2,
                 Start = nextMonthAndOneDay
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

			DateTime nextMonthAndTwoDays = nextMonth.AddDays(2);
            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = nextMonthAndTwoDays
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start={nextMonth:yyyy-MMM-dd}&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(6, getCalendarResult.Dates.Count);

                Assert.Equal(nextMonth, getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);
                
                Assert.Equal(nextMonth.AddDays(1), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id && x.Unit == 1);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(nextMonth.AddDays(2), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id && x.Unit == 1);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id && x.Unit == 2);
                
                Assert.Equal(nextMonth.AddDays(3), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id && x.Unit == 2);
				Assert.Single(getCalendarResult.Dates[3].PreparationTimes);
				Assert.Contains(getCalendarResult.Dates[3].PreparationTimes, pt => pt.Unit == 1);
                
                Assert.Equal(nextMonth.AddDays(4), getCalendarResult.Dates[4].Date);
				Assert.Empty(getCalendarResult.Dates[4].Bookings);
				Assert.Single(getCalendarResult.Dates[4].PreparationTimes);
				Assert.Contains(getCalendarResult.Dates[4].PreparationTimes, pt => pt.Unit == 2);

				Assert.Equal(nextMonth.AddDays(5), getCalendarResult.Dates[5].Date);
				Assert.Empty(getCalendarResult.Dates[5].Bookings);
                Assert.Empty(getCalendarResult.Dates[5].PreparationTimes);
            }
        }

		public void Dispose()
		{
			_client?.Dispose();
		}
	}
}
