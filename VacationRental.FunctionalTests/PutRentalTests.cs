using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api;
using VacationRental.Api.Models;
using VacationRental.Domain.Exceptions;
using Xunit;

namespace VacationRental.FunctionalTests
{
	public class PutRentalTests : IDisposable
    {
        private readonly HttpClient _client;

		public PutRentalTests()
		{
			var _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
			_client = _server.CreateClient();
		}

        [Fact]
        public async Task GivenCompleteRequestWithoutBookingConflicts_WhenPutRental_ThenAGetReturnsTheUpdatedBookings()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

			var today = DateTime.Today;
			var postBookingRequest = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = today
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingSecondRequest = new BookingBindingModel
			{
				RentalId = postRentalResult.Id,
				Nights = 1,
				Start = today.AddDays(4)
			};

			ResourceIdViewModel postBookingSecondResult;
			using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingSecondRequest))
			{
				Assert.True(postBookingResponse.IsSuccessStatusCode);
				postBookingSecondResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
			}

			var putRentalRequest = new RentalBindingModel
			{
				Units = 1,
				PreparationTimeInDays = 2
			};

			ResourceIdViewModel putRentalResult;
			using (var putRentalResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postRentalResult.Id}", putRentalRequest))
			{
				Assert.True(putRentalResponse.IsSuccessStatusCode);
				putRentalResult = await putRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();

				Assert.Equal(postRentalResult.Id, putRentalResult.Id);
			}

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={putRentalResult.Id}&start={today:yyyy-MMM-dd}&nights=6"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(putRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(8, getCalendarResult.Dates.Count);

                Assert.Equal(today, getCalendarResult.Dates[0].Date);
                Assert.Single(getCalendarResult.Dates[0].Bookings);
				Assert.Contains(getCalendarResult.Dates[0].Bookings, x => x.Id == postBookingResult.Id && x.Unit == 1);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(today.AddDays(1), getCalendarResult.Dates[1].Date);
                Assert.Empty(getCalendarResult.Dates[1].Bookings);
                Assert.Single(getCalendarResult.Dates[1].PreparationTimes);
				Assert.Contains(getCalendarResult.Dates[1].PreparationTimes, pt => pt.Unit == 1);

                Assert.Equal(today.AddDays(2), getCalendarResult.Dates[2].Date);
                Assert.Empty(getCalendarResult.Dates[2].Bookings);
                Assert.Single(getCalendarResult.Dates[2].PreparationTimes);
				Assert.Contains(getCalendarResult.Dates[2].PreparationTimes, pt => pt.Unit == 1);

                Assert.Equal(today.AddDays(3), getCalendarResult.Dates[3].Date);
                Assert.Empty(getCalendarResult.Dates[3].Bookings);
                Assert.Empty(getCalendarResult.Dates[3].PreparationTimes);

                Assert.Equal(today.AddDays(4), getCalendarResult.Dates[4].Date);
                Assert.Single(getCalendarResult.Dates[4].Bookings);
				Assert.Contains(getCalendarResult.Dates[4].Bookings, b => b.Id == postBookingSecondResult.Id && b.Unit == 1);
				Assert.Empty(getCalendarResult.Dates[4].PreparationTimes);

                Assert.Equal(today.AddDays(5), getCalendarResult.Dates[5].Date);
                Assert.Empty(getCalendarResult.Dates[5].Bookings);
                Assert.Single(getCalendarResult.Dates[5].PreparationTimes);
				Assert.Contains(getCalendarResult.Dates[5].PreparationTimes, pt => pt.Unit == 1);

				Assert.Equal(today.AddDays(6), getCalendarResult.Dates[6].Date);
				Assert.Empty(getCalendarResult.Dates[6].Bookings);
				Assert.Single(getCalendarResult.Dates[6].PreparationTimes);
				Assert.Contains(getCalendarResult.Dates[6].PreparationTimes, pt => pt.Unit == 1);

				Assert.Equal(today.AddDays(7), getCalendarResult.Dates[7].Date);
				Assert.Empty(getCalendarResult.Dates[7].Bookings);
				Assert.Empty(getCalendarResult.Dates[7].PreparationTimes);

            }

        }

		public void Dispose()
		{
			_client?.Dispose();
		}
	}
}
