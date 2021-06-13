using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VacationRental.Api.Models;
using VacationRental.Domain.AggregatesModel.RentalAggregate;

namespace VacationRental.Api.Commands
{
	public class CreateRentalCommandHandler: IRequestHandler<CreateRentalCommand, ResourceIdViewModel>
	{
		private readonly IRentalRepository _rentalRepository;
		private readonly ILogger<CreateRentalCommandHandler> _logger;

		public CreateRentalCommandHandler(
			IRentalRepository rentalRepository,
			ILogger<CreateRentalCommandHandler> logger)
		{
			_rentalRepository = rentalRepository ?? throw new ArgumentException(nameof(rentalRepository));
			_logger = logger ?? throw new ArgumentException(nameof(logger));
		}

		public async Task<ResourceIdViewModel> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
		{
			var rentalType = new RentalType();
			var rental = new Rental(request.Units, rentalType, request.PreparationTimeInDays);

			_rentalRepository.Add(rental);

			var rentalPersisted = await _rentalRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

			

		}
	}
}