using VacationRental.Domain.SeedWork;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class RentalType : Enumeration
	{
		public RentalType() : base(1, "Room")
		{
		}

		public RentalType(int id, string name) : base(id, name)
		{
		}
	}
}