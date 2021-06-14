using System;
using MediatR;
using VacationRental.Api.Models;

namespace VacationRental.Api.Commands
{
	public class CreateBookingCommand : IRequest<ResourceIdViewModel>
	{
		public int RentalId { get; }

		public DateTime Date { get; }
	
		public int Nights { get; }

		public CreateBookingCommand(int rentalId, DateTime date, int nights)
		{
			RentalId = rentalId;
			Date = date;
			Nights = nights;
		}
	}
}