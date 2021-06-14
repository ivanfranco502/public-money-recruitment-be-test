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
	public class UpdateRentalCommandHandlerTests
	{
		private Mock<IRentalRepository> _rentalRepository;
		private Mock<ILogger<UpdateRentalCommandHandler>> _logger;

		public UpdateRentalCommandHandlerTests()
		{
			_rentalRepository = new Mock<IRentalRepository>();
			_logger = new Mock<ILogger<UpdateRentalCommandHandler>>();
		}

		[Fact]
		public async Task
			GivenAValidRentalCreationRequest_WhenHandleCommand_ThenEntityPersistedAndIdReturned()
		{
			var request = new UpdateRentalCommand(4, 1, 2);
			_rentalRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(new Rental(1, RentalType.Room, 2));
			_rentalRepository.Setup(r => r.UnitOfWork.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

			var handler = new UpdateRentalCommandHandler(_rentalRepository.Object, _logger.Object);

			var result = await handler.Handle(request, CancellationToken.None);

			_rentalRepository.Verify(r => r.GetAsync(4), Times.Once);
			Assert.NotNull(result);
		}


		[Fact]
		public async Task
			GivenAnErrorInSaveEntities_WhenHandleCommand_ThenRentalDomainException()
		{
			try
			{
				var request = new UpdateRentalCommand(4, 1, 2);
				_rentalRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(default(Rental));
				_rentalRepository.Setup(r => r.UnitOfWork.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

				var handler = new UpdateRentalCommandHandler(_rentalRepository.Object, _logger.Object);

				var result = await handler.Handle(request, CancellationToken.None);

				_rentalRepository.Verify(r => r.GetAsync(4), Times.Once);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<RentalDomainException>(ex);
			}
		}
	}
}
