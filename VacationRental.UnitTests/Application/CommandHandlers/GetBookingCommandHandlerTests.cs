using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using VacationRental.Api.Commands;
using VacationRental.Domain.AggregatesModel.RentalAggregate;
using VacationRental.Domain.Exceptions;
using Xunit;
using Xunit.Sdk;

namespace VacationRental.UnitTests.Application.CommandHandlers
{
	public class GetBookingCommandHandlerTests
	{
		private Mock<IRentalRepository> _rentalRepository;
		private Mock<ILogger<GetBookingCommandHandler>> _logger;

		public GetBookingCommandHandlerTests()
		{
			_rentalRepository = new Mock<IRentalRepository>();
			_logger = new Mock<ILogger<GetBookingCommandHandler>>();
		}

		[Fact]
		public async Task
			GivenAValidBookingRequest_WhenHandleCommand_ThenEntityReturned()
		{
			var request = new GetBookingCommand(1);
			var dateTime = DateTime.Today.AddDays(5);
			_rentalRepository.Setup(r => r.GetBookingAsync(1)).ReturnsAsync(new Booking(2, 5, 1, dateTime, 1));

			var handler = new GetBookingCommandHandler(_rentalRepository.Object, _logger.Object);

			var result = await handler.Handle(request, CancellationToken.None);

			Assert.NotNull(result);
			Assert.Equal(2, result.RentalId);
			Assert.Equal(dateTime, result.Start);
			Assert.Equal(5, result.Nights);
		}


		[Fact]
		public async Task
			GivenAnInexistentBooking_WhenHandleCommand_ThenBookingDomainException()
		{
			try
			{
				var request = new GetBookingCommand(1);
				_rentalRepository.Setup(r => r.GetBookingAsync(1)).ReturnsAsync(default(Booking));

				var handler = new GetBookingCommandHandler(_rentalRepository.Object, _logger.Object);

				var result = await handler.Handle(request, CancellationToken.None);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<BookingDomainException>(ex);
			}
		}
	}
}
