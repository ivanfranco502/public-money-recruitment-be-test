using System;
using MediatR;
using VacationRental.Api.Models;

namespace VacationRental.Api.Commands
{
	public class GetCalendarCommand : IRequest<CalendarViewModel>
	{
		public int RentalId { get; }
		public DateTime Start { get; }
		public int Nights { get; }

		public GetCalendarCommand(int rentalId, DateTime start, int nights)
		{
			RentalId = rentalId;
			Start = start;
			Nights = nights;
		}
	}
}
