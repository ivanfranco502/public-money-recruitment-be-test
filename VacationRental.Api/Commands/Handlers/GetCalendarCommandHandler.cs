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
	public class GetCalendarCommandHandler : IRequestHandler<GetCalendarCommand, CalendarViewModel>
	{
		private readonly IRentalRepository _rentalRepository;
		private readonly ILogger<GetCalendarCommandHandler> _logger;

		public GetCalendarCommandHandler(
			IRentalRepository rentalRepository,
			ILogger<GetCalendarCommandHandler> logger)
		{
			_rentalRepository = rentalRepository ?? throw new ArgumentException(nameof(rentalRepository));
			_logger = logger ?? throw new ArgumentException(nameof(logger));
		}

		public async Task<CalendarViewModel> Handle(GetCalendarCommand getCalendarCommand, CancellationToken cancellationToken)
		{
			var rentalToBeFound = await _rentalRepository.GetAsync(getCalendarCommand.RentalId);

			if (rentalToBeFound == default(Rental))
				throw new RentalDomainException("Rental not found");

			var calendar = rentalToBeFound.GetCalendar(getCalendarCommand.Start, getCalendarCommand.Nights);

			return new CalendarViewModel(calendar.RentalId, calendar.Dates);
		}
	}
}