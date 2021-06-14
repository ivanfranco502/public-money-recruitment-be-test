using MediatR;
using VacationRental.Api.Models;

namespace VacationRental.Api.Commands
{
	public class GetRentalCommand : IRequest<RentalViewModel>
	{
		public int RentalId { get; }

		public GetRentalCommand(int rentalId)
		{
			RentalId = rentalId;
		}
	}
}
