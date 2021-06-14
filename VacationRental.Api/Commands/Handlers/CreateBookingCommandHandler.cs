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
	public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ResourceIdViewModel>
	{
		private readonly IRentalRepository _rentalRepository;
		private readonly ILogger<CreateBookingCommandHandler> _logger;

		public CreateBookingCommandHandler(
			IRentalRepository rentalRepository,
			ILogger<CreateBookingCommandHandler> logger)
		{
			_rentalRepository = rentalRepository ?? throw new ArgumentException(nameof(rentalRepository));
			_logger = logger ?? throw new ArgumentException(nameof(logger));
		}

		public async Task<ResourceIdViewModel> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
		{
			var rental = await _rentalRepository.GetAsync(request.RentalId);

			var bookingAdded = rental.AddBooking(request.RentalId, request.Nights, request.Date);

			var rentalUpdated = await _rentalRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

			if(!rentalUpdated)
				throw new RentalDomainException("The booking was not created");

			return new ResourceIdViewModel(bookingAdded.Id);
		}
	}
}