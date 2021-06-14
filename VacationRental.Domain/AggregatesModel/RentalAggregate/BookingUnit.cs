using System.Runtime.InteropServices.ComTypes;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class BookingUnit
	{
		public int Id { get; }

		public int Unit { get; }

		public BookingUnit(int id, int unit)
		{
			Id = id;
			Unit = unit;
		}
	}
}