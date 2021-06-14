using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VacationRental.Api.Commands;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
		private readonly IMediator _mediator;
		private readonly ILogger<BookingsController> _logger;

        public BookingsController(
			IMediator mediator,
			ILogger<BookingsController> logger)
		{
			_mediator = mediator;
			_logger = logger;
        }

		[HttpGet]
		[Route("{bookingId:int}")]
		public async Task<BookingViewModel> Get(int bookingId)
		{
			var getBookingCommand = new GetBookingCommand(bookingId);

			BookingViewModel booking = await _mediator.Send(getBookingCommand);

			if (booking == default(BookingViewModel))
				throw new ApplicationException("Booking not found");

			return booking;
		}

		[HttpPost]
        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
			var createBookingCommand = new CreateBookingCommand(model.RentalId, model.Start, model.Nights);
			var createdBooking = await _mediator.Send(createBookingCommand);

			if (createdBooking == default(ResourceIdViewModel))
				throw new ApplicationException("Rental was not created");

			return createdBooking;
		}
    }
}
