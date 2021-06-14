using FluentValidation;
using VacationRental.Api.Commands;

namespace VacationRental.Api.Validations
{
	public class GetBookingCommandValidator : AbstractValidator<GetBookingCommand>
	{
		public GetBookingCommandValidator()
		{
			RuleFor(command => command.BookingId).GreaterThan(0).WithMessage("Booking not found");
		}
	}
}