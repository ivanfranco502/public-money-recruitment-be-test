using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Domain.AggregatesModel.RentalAggregate;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Api.Commands
{
	public class UpdateRentalCommandHandler: IRequestHandler<UpdateRentalCommand, ResourceIdViewModel>
	{
		private readonly IRentalRepository _rentalRepository;
		private readonly ILogger<UpdateRentalCommandHandler> _logger;

		public UpdateRentalCommandHandler(
			IRentalRepository rentalRepository,
			ILogger<UpdateRentalCommandHandler> logger)
		{
			_rentalRepository = rentalRepository ?? throw new ArgumentException(nameof(rentalRepository));
			_logger = logger ?? throw new ArgumentException(nameof(logger));
		}

		public async Task<ResourceIdViewModel> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
		{
			var rentalToUpdate = await _rentalRepository.GetAsync(request.RentalId);

			if (rentalToUpdate == default(Rental))
				throw new RentalDomainException("The rental was not found");

			
			rentalToUpdate.ApplyUpdate(request.Units, request.PreparationTimeInDays);

			_rentalRepository.Update(rentalToUpdate);
			
			var rentalUpdated = await _rentalRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			if (rentalUpdated < 1)
				throw new RentalDomainException("The rental was not updated");

			return new ResourceIdViewModel(rentalToUpdate.Id);
		}
	}
}