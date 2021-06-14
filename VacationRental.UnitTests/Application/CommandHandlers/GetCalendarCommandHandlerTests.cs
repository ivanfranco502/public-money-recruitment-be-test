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
	public class GetCalendarCommandHandlerTests
	{
		private Mock<IRentalRepository> _rentalRepository;
		private Mock<ILogger<GetCalendarCommandHandler>> _logger;

		public GetCalendarCommandHandlerTests()
		{
			_rentalRepository = new Mock<IRentalRepository>();
			_logger = new Mock<ILogger<GetCalendarCommandHandler>>();
		}

		[Fact]
		public async Task GivenAValidCalendarRequest_WhenHandleCommand_ThenEntityPersistedAndIdReturned()
		{
			var today = DateTime.Today.Date;
			var request = new GetCalendarCommand(1, today, 2);
			_rentalRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(new Rental(2, RentalType.Room, 1));

			var handler = new GetCalendarCommandHandler(_rentalRepository.Object, _logger.Object);

			var result = await handler.Handle(request, CancellationToken.None);

			Assert.NotNull(result);
			Assert.Equal(0, result.RentalId);
			Assert.NotEmpty(result.Dates);
			Assert.Equal(3, result.Dates.Count);
			
			Assert.True(result.Dates[0].Date == today);
			Assert.Empty(result.Dates[0].Bookings);
			Assert.Empty(result.Dates[0].PreparationTimes);

			Assert.True(result.Dates[1].Date == today.AddDays(1));
			Assert.Empty(result.Dates[1].Bookings);
			Assert.Empty(result.Dates[1].PreparationTimes);

			Assert.True(result.Dates[2].Date == today.AddDays(2));
			Assert.Empty(result.Dates[2].Bookings);
			Assert.Empty(result.Dates[2].PreparationTimes);
		}

		[Fact]
		public async Task
			GivenAnInexistentRental_WhenHandleCommand_ThenRentalDomainException()
		{
			try
			{
				var request = new GetCalendarCommand(1, DateTime.Today, 2);
				_rentalRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(default(Rental));

				var handler = new GetCalendarCommandHandler(_rentalRepository.Object, _logger.Object);

				var result = await handler.Handle(request, CancellationToken.None);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<RentalDomainException>(ex);
			}
		}
	}
}
