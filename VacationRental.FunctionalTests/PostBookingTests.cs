﻿using Microsoft.AspNetCore.Hosting;
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
	public class PostBookingTests: IDisposable
    {
        private readonly HttpClient _client;

		public PostBookingTests()
		{
			var _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
			_client = _server.CreateClient();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 3,
                 Start = DateTime.Today
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
			}
		}

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbookingCausedByBooking()
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

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = DateTime.Today
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.Today.AddDays(1)
            };

            await Assert.ThrowsAsync<BookingDomainException>(async () =>
            {
                using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
                {
                }
            });
        }

		[Fact]
		public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbookingCausedByPreparationTime()
		{
			var postRentalRequest = new RentalBindingModel
			{
				Units = 1,
				PreparationTimeInDays = 5
			};

			ResourceIdViewModel postRentalResult;
			using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
			{
				Assert.True(postRentalResponse.IsSuccessStatusCode);
				postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
			}

			var postBooking1Request = new BookingBindingModel
			{
				RentalId = postRentalResult.Id,
				Nights = 1,
				Start = DateTime.Today
			};

			using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
			{
				Assert.True(postBooking1Response.IsSuccessStatusCode);
			}

			var postBooking2Request = new BookingBindingModel
			{
				RentalId = postRentalResult.Id,
				Nights = 1,
				Start = DateTime.Today.AddDays(3)
			};

			await Assert.ThrowsAsync<BookingDomainException>(async () =>
			{
				using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
				{
				}
			});
		}

        public void Dispose()
		{
			_client?.Dispose();
		}
	}
}
