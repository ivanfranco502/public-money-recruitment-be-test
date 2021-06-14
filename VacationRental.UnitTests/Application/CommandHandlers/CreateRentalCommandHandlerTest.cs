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
	public class CreateRentalCommandHandlerTests
	{
		private Mock<IRentalRepository> _rentalRepository;
		private Mock<ILogger<CreateRentalCommandHandler>> _logger;

		public CreateRentalCommandHandlerTests()
		{
			_rentalRepository = new Mock<IRentalRepository>();
			_logger = new Mock<ILogger<CreateRentalCommandHandler>>();
		}

		[Fact]
		public async Task
			GivenAValidRentalCreationRequest_WhenHandleCommand_ThenEntityPersistedAndIdReturned()
		{
			var request = new CreateRentalCommand(1, 2);
			_rentalRepository.Setup(r => r.UnitOfWork.SaveEntitiesAsync(CancellationToken.None)).ReturnsAsync(true);

			var handler = new CreateRentalCommandHandler(_rentalRepository.Object, _logger.Object);

			var result = await handler.Handle(request, CancellationToken.None);

			_rentalRepository.Verify(r => r.Add(It.Is<Rental>(r => r.PreparationTimeInDays == 2 && r.Units == 1)), Times.Once);
			Assert.NotNull(result);
			Assert.Equal(0, result.Id);
		}


		[Fact]
		public async Task
			GivenAnErrorInSaveEntities_WhenHandleCommand_ThenRentalDomainException()
		{
			try
			{
				var request = new CreateRentalCommand(1, 2);
				_rentalRepository.Setup(r => r.UnitOfWork.SaveEntitiesAsync(CancellationToken.None)).ReturnsAsync(false);

				var handler = new CreateRentalCommandHandler(_rentalRepository.Object, _logger.Object);

				var result = await handler.Handle(request, CancellationToken.None);

				_rentalRepository.Verify(r => r.Add(It.Is<Rental>(r => r.PreparationTimeInDays == 2 && r.Units == 1)), Times.Once);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<RentalDomainException>(ex);
			}
		}
	}
}
