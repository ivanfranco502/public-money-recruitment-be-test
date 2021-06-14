using MediatR;
using VacationRental.Api.Models;

namespace VacationRental.Api.Commands
{
	public class UpdateRentalCommand : IRequest<ResourceIdViewModel>
	{
		public int RentalId { get; }
		public int Units { get; }

		public int PreparationTimeInDays { get; }

		public UpdateRentalCommand(int rentalId, int units, int preparationTimeInDays)
		{
			RentalId = rentalId;
			Units = units;
			PreparationTimeInDays = preparationTimeInDays;
		}
	}
}
