using System;
using System.Runtime.InteropServices.ComTypes;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class BookingUnit
	{
		public int Id { get; }

		public int Unit { get; }

		public BookingUnit(int id, int unit)
		{
			Id = id > 0 ? id: throw new ArgumentException(nameof(id));
			Unit = unit > 0 ? unit :throw new ArgumentException(nameof(unit));
		}
	}
}