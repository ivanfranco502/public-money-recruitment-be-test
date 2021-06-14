using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VacationRental.Api.Models;
using VacationRental.Domain.AggregatesModel.RentalAggregate;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Api.Commands
{
	public class GetRentalCommandHandler: IRequestHandler<GetRentalCommand, RentalViewModel>
	{
		private readonly IRentalRepository _rentalRepository;
		private readonly ILogger<GetRentalCommandHandler> _logger;

		public GetRentalCommandHandler(
			IRentalRepository rentalRepository,
			ILogger<GetRentalCommandHandler> logger)
		{
			_rentalRepository = rentalRepository ?? throw new ArgumentException(nameof(rentalRepository));
			_logger = logger ?? throw new ArgumentException(nameof(logger));
		}
		public async Task<RentalViewModel> Handle(GetRentalCommand request, CancellationToken cancellationToken)
		{
			var rentalToBeFound = await _rentalRepository.GetAsync(request.RentalId);

			if (rentalToBeFound == default(Rental))
				throw new RentalDomainException("Rental not found");

			return new RentalViewModel(rentalToBeFound.Units, rentalToBeFound.PreparationTimeInDays);
		}
	}
}