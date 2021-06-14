using FluentValidation;
using VacationRental.Api.Commands;

namespace VacationRental.Api.Validations
{
	public class CreateRentalCommandValidator: AbstractValidator<CreateRentalCommand>
	{
		public CreateRentalCommandValidator()
		{
			RuleFor(command => command.Units).GreaterThan(0).WithMessage("Units should be positive");
			RuleFor(command => command.PreparationTimeInDays).GreaterThanOrEqualTo(0).WithMessage("PreparationTimeInDays should be greater than or equal to zero");
		}
	}
}