using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Commands;
using VacationRental.Api.Controllers;
using VacationRental.Api.Models;
using Xunit;
using Xunit.Sdk;
using Exception = System.Exception;

namespace VacationRental.UnitTests.Application.Controllers
{
	public class BookingsControllerTests
	{
		private Mock<IMediator> _mediator;
		private Mock<ILogger<BookingsController>> _logger;
		private Mock<HttpContext> _context;

		public BookingsControllerTests()
		{
			_mediator = new Mock<IMediator>();
			_logger = new Mock<ILogger<BookingsController>>();
			_context = new Mock<HttpContext>();
		}

		[Fact]
		public async Task GivenAValidBookingId_WhenGet_ThenShouldReturnBookingEntity()
		{
			var bookingId = 1;

			var today = DateTime.Today;
			_mediator.Setup(m => m.Send(It.IsAny<GetBookingCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new BookingViewModel(2, today, 4));

			var bookingsController = new BookingsController(_mediator.Object, _logger.Object);
			bookingsController.ControllerContext.HttpContext = _context.Object;

			var response = await bookingsController.Get(bookingId);

			_mediator.Verify(
				m => m.Send(It.Is<GetBookingCommand>(gbc => gbc.BookingId == bookingId), It.IsAny<CancellationToken>()),
				Times.Once);

			Assert.NotNull(response);
			Assert.Equal(today, response.Start);
			Assert.Equal(4, response.Nights);
			Assert.Equal(2, response.RentalId);
		}

		[Fact]
		public async Task GivenAnInexistentBookingId_WhenGet_ThenShouldReturnError()
		{
			try
			{
				var bookingId = 1;

				var today = DateTime.Today;
				_mediator.Setup(m => m.Send(It.IsAny<GetBookingCommand>(),
						It.IsAny<CancellationToken>()))
					.ReturnsAsync(default(BookingViewModel));

				var bookingsController = new BookingsController(_mediator.Object, _logger.Object);
				bookingsController.ControllerContext.HttpContext = _context.Object;

				await bookingsController.Get(bookingId);

				_mediator.Verify(
					m => m.Send(It.Is<GetBookingCommand>(gbc => gbc.BookingId == bookingId),
						It.IsAny<CancellationToken>()),
					Times.Once);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<ApplicationException>(ex);
			}
		}

		[Fact]
		public async Task GivenAValidBookingBindingModel_WhenPost_ThenShouldReturnPersistedId()
		{
			var today = DateTime.Today;
			var bookingBindingModel = new BookingBindingModel {Start = today, Nights = 5, RentalId = 2};

			_mediator.Setup(m => m.Send(It.IsAny<CreateBookingCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new ResourceIdViewModel(3));

			var bookingsController = new BookingsController(_mediator.Object, _logger.Object);
			bookingsController.ControllerContext.HttpContext = _context.Object;

			var response = await bookingsController.Post(bookingBindingModel);

			_mediator.Verify(
				m => m.Send(
					It.Is<CreateBookingCommand>(gbc => gbc.RentalId == 2 && gbc.Date == today && gbc.Nights == 5),
					It.IsAny<CancellationToken>()),
				Times.Once);

			Assert.NotNull(response);
			Assert.Equal(3, response.Id);
		}

		[Fact]
		public async Task GivenAnInValidBookingBindingModel_WhenPost_ThenShouldReturnError()
		{
			try
			{
				var today = DateTime.Today;
				var bookingBindingModel = new BookingBindingModel {Start = today, Nights = 5, RentalId = 2};

				_mediator.Setup(m => m.Send(It.IsAny<CreateBookingCommand>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(default(ResourceIdViewModel));

				var bookingsController = new BookingsController(_mediator.Object, _logger.Object);
				bookingsController.ControllerContext.HttpContext = _context.Object;

				await bookingsController.Post(bookingBindingModel);

				_mediator.Verify(
					m => m.Send(
						It.Is<CreateBookingCommand>(gbc => gbc.RentalId == 2 && gbc.Date == today && gbc.Nights == 5),
						It.IsAny<CancellationToken>()),
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