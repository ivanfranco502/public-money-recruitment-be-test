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
	public class CreateBookingCommandHandlerTests
	{
		private Mock<IRentalRepository> _rentalRepository;
		private Mock<ILogger<CreateBookingCommandHandler>> _logger;

		public CreateBookingCommandHandlerTests()
		{
			_rentalRepository = new Mock<IRentalRepository>();
			_logger = new Mock<ILogger<CreateBookingCommandHandler>>();
		}

		[Fact]
		public async Task
			GivenAValidBookingCreationRequest_WhenHandleCommand_ThenEntityPersistedAndIdReturned()
		{
			var request = new CreateBookingCommand(1, DateTime.Today, 2);
			_rentalRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(new Rental(2, RentalType.Room, 1));
			_rentalRepository.Setup(r => r.UnitOfWork.SaveEntitiesAsync(CancellationToken.None)).ReturnsAsync(true);

			var handler = new CreateBookingCommandHandler(_rentalRepository.Object, _logger.Object);

			var result = await handler.Handle(request, CancellationToken.None);

			Assert.NotNull(result);
			Assert.Equal(0, result.Id);
		}


		[Fact]
		public async Task
			GivenAnErrorInSaveEntities_WhenHandleCommand_ThenRentalDomainException()
		{
			try
			{
				var request = new CreateBookingCommand(1, DateTime.Today, 2);
				_rentalRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(new Rental(2, RentalType.Room, 1));
				_rentalRepository.Setup(r => r.UnitOfWork.SaveEntitiesAsync(CancellationToken.None)).ReturnsAsync(false);

				var handler = new CreateBookingCommandHandler(_rentalRepository.Object, _logger.Object);

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
