using System;
using FluentValidation;
using VacationRental.Api.Commands;

namespace VacationRental.Api.Validations
{
	public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
	{
		public CreateBookingCommandValidator()
		{
			RuleFor(command => command.Nights).GreaterThan(0).WithMessage("Nights must be positive");
			RuleFor(command => command.RentalId).GreaterThan(0).WithMessage("Rental not found");
			RuleFor(command => command.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("The date should be greater than or equal to today");
		}
	}
}