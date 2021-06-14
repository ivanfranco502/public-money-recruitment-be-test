using VacationRental.Domain.SeedWork;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class RentalType : Enumeration
	{
		public static RentalType Room = new RentalType(1, nameof(Room).ToLowerInvariant());
		public static RentalType Bed = new RentalType(2, nameof(Bed).ToLowerInvariant());
		public static RentalType Flat = new RentalType(3, nameof(Flat).ToLowerInvariant());
		public static RentalType House = new RentalType(4, nameof(House).ToLowerInvariant());

		public RentalType(int id = 1, string name = "room") : base(id, name)
		{
		}
	}
}