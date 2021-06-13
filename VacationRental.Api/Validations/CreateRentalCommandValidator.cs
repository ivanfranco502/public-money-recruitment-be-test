using FluentValidation;
using VacationRental.Api.Commands;

namespace VacationRental.Api.Validations
{
	public class CreateRentalCommandValidator: AbstractValidator<CreateRentalCommand>
	{
		public CreateRentalCommandValidator()
		{
			RuleFor(command => command.Units).GreaterThan(0);
			RuleFor(command => command.PreparationTimeInDays).GreaterThanOrEqualTo(0);
		}
	}
}