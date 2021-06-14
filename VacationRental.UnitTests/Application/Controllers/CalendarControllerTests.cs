using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using VacationRental.Api.Commands;
using VacationRental.Api.Controllers;
using VacationRental.Api.Models;
using VacationRental.Domain.AggregatesModel.RentalAggregate;
using Xunit;
using Xunit.Sdk;

namespace VacationRental.UnitTests.Application.Controllers
{
	public class CalendarControllerTests
	{
		private Mock<IMediator> _mediator;
		private Mock<ILogger<CalendarController>> _logger;
		private Mock<HttpContext> _context;

		public CalendarControllerTests()
		{
			_mediator = new Mock<IMediator>();
			_logger = new Mock<ILogger<CalendarController>>();
			_context = new Mock<HttpContext>();
		}

		[Fact]
		public async Task GivenAValidCalendarRequest_WhenGet_ThenShouldReturnCalendarEntity()
		{
			var rentalId = 1;

			var today = DateTime.Today;
			_mediator.Setup(m => m.Send(It.IsAny<GetCalendarCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(
					new CalendarViewModel(1,
						new List<BookingCalendarDate>
						{
							new BookingCalendarDate(today,
								new List<BookingUnit>
								{
									new BookingUnit(2, 3)
								},
								new List<int> {4})
						}));

			var calendarController = new CalendarController(_mediator.Object, _logger.Object);
			calendarController.ControllerContext.HttpContext = _context.Object;

			var response = await calendarController.Get(rentalId, today, 1);

			_mediator.Verify(
				m => m.Send(It.Is<GetCalendarCommand>(gcc => gcc.RentalId == rentalId && gcc.Start == today && gcc.Nights == 1), It.IsAny<CancellationToken>()),
				Times.Once);

			Assert.NotNull(response);
			Assert.Equal(1, response.RentalId);
			Assert.Single(response.Dates);
			Assert.Equal(today, response.Dates[0].Date);
			Assert.Single(response.Dates[0].Bookings);
			Assert.Equal(2, response.Dates[0].Bookings[0].Id);
			Assert.Equal(3, response.Dates[0].Bookings[0].Unit);
			Assert.Single(response.Dates[0].PreparationTimes);
			Assert.Equal(4, response.Dates[0].PreparationTimes[0].Unit);
		}

		[Fact]
		public async Task GivenAnInvalidCalendarRequest_WhenGet_ThenShouldReturnError()
		{
			try
			{
				var rentalId = 1;

				var today = DateTime.Today;
				_mediator.Setup(m => m.Send(It.IsAny<GetCalendarCommand>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(default(CalendarViewModel));

				var calendarController = new CalendarController(_mediator.Object, _logger.Object);
				calendarController.ControllerContext.HttpContext = _context.Object;

				var response = await calendarController.Get(rentalId, today, 1);

				_mediator.Verify(
					m => m.Send(It.Is<GetCalendarCommand>(gcc => gcc.RentalId == rentalId && gcc.Start == today && gcc.Nights == 1), It.IsAny<CancellationToken>()),
					Times.Once);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<ApplicationException>(ex);
			}
		}
	}
}