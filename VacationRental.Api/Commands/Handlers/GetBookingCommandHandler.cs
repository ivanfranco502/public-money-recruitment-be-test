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
	public class GetBookingCommandHandler : IRequestHandler<GetBookingCommand, BookingViewModel>
	{
		private readonly IRentalRepository _rentalRepository;
		private readonly ILogger<GetBookingCommandHandler> _logger;

		public GetBookingCommandHandler(
			IRentalRepository rentalRepository,
			ILogger<GetBookingCommandHandler> logger)
		{
			_rentalRepository = rentalRepository ?? throw new ArgumentException(nameof(rentalRepository));
			_logger = logger ?? throw new ArgumentException(nameof(logger));
		}

		public async Task<BookingViewModel> Handle(GetBookingCommand request, CancellationToken cancellationToken)
		{
			var bookingToBeFound = await _rentalRepository.GetBookingAsync(request.BookingId);

			if (bookingToBeFound == default(Booking))
				throw new BookingDomainException("Booking not found");

			return new BookingViewModel(bookingToBeFound.RentalId, bookingToBeFound.Start, bookingToBeFound.Nights);
		}
	}
}