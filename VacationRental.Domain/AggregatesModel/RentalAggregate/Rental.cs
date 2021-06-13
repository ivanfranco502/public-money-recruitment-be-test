using VacationRental.Domain.SeedWork;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class Rental : Entity, IAggregateRoot
	{
		public int Units { get; }
		public RentalType Type { get; }
		public int PreparationTimeInDays { get; }

		public Rental(int units, RentalType type, int preparationTimeInDays)
		{
			Units = units;
			Type = type;
			PreparationTimeInDays = preparationTimeInDays;
		}
	}
}