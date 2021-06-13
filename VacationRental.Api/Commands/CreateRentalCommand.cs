using MediatR;
using VacationRental.Api.Models;

namespace VacationRental.Api.Commands
{
	public class CreateRentalCommand : IRequest<ResourceIdViewModel>
	{
		public int Units { get; }

		public int PreparationTimeInDays { get; }

		public CreateRentalCommand(int units, int preparationTimeInDays)
		{
			Units = units;
			PreparationTimeInDays = preparationTimeInDays;
		}
	}
}