using FluentValidation;
using VacationRental.Api.Commands;

namespace VacationRental.Api.Validations
{
	public class GetCalendarCommandValidator : AbstractValidator<GetCalendarCommand>
	{
		public GetCalendarCommandValidator()
		{
			RuleFor(command => command.RentalId).GreaterThan(0).WithMessage("Rental not found");
			RuleFor(command => command.Nights).GreaterThan(0).WithMessage("Nights must be positive");
		}
	}
}