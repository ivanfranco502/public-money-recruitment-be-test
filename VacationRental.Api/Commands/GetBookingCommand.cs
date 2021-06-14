using MediatR;
using VacationRental.Api.Models;

namespace VacationRental.Api.Commands
{
	public class GetBookingCommand : IRequest<BookingViewModel>
	{
		public int BookingId { get; }

		public GetBookingCommand(int bookingId)
		{
			BookingId = bookingId;
		}
	}
}
