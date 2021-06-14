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
	public class GetRentalCommandHandlerTests
	{
		private Mock<IRentalRepository> _rentalRepository;
		private Mock<ILogger<GetRentalCommandHandler>> _logger;

		public GetRentalCommandHandlerTests()
		{
			_rentalRepository = new Mock<IRentalRepository>();
			_logger = new Mock<ILogger<GetRentalCommandHandler>>();
		}

		[Fact] 
		public async Task GivenAValidRentalRequest_WhenHandleCommand_ThenEntityPersistedAndIdReturned()
		{
			var request = new GetRentalCommand(1);
			_rentalRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(new Rental(2, RentalType.Room, 1));

			var handler = new GetRentalCommandHandler(_rentalRepository.Object, _logger.Object);

			var result = await handler.Handle(request, CancellationToken.None);

			Assert.NotNull(result);
			Assert.Equal(2, result.Units);
			Assert.Equal(1, result.PreparationTimeInDays);
		}


		[Fact] 
		public async Task GivenAnInexistentRental_WhenHandleCommand_ThenRentalDomainException()
		{
			try
			{
				var request = new GetRentalCommand(1);
				_rentalRepository.Setup(r => r.GetAsync(1)).ReturnsAsync(default(Rental));

				var handler = new GetRentalCommandHandler(_rentalRepository.Object, _logger.Object);

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
