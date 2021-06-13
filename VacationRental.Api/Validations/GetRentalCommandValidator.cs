using FluentValidation;
using VacationRental.Api.Commands;

namespace VacationRental.Api.Validations
{
	public class GetRentalCommandValidator : AbstractValidator<GetRentalCommand>
	{
		public GetRentalCommandValidator()
		{
			RuleFor(command => command.RentalId).GreaterThan(0);
		}
	}
}