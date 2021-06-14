using System;
using VacationRental.Domain.SeedWork;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class Booking : Entity
	{
		private int _rentalId;
		private int _nights;
		private DateTime _start;
		private int _unit;
		private readonly int _preparationDays;

		public int RentalId => _rentalId;

		public int Nights => _nights;

		public DateTime Start => _start;

		public int Unit => _unit;

		public int PreparationDays => _preparationDays;

		public Booking(int rentalId, int nights, int preparationDays, DateTime start, int unit)
		{
			_preparationDays = preparationDays;
			_rentalId = rentalId;
			_nights = nights;
			_start = start;
			_unit = unit;
		}

		public bool IsExistingBookingInConflictWithBookingRequest(in DateTime start, int nights)
		{
			return Start <= start.Date && Start.AddDays(Nights + PreparationDays) > start.Date 
					|| Start < start.AddDays(nights) && Start.AddDays(Nights + PreparationDays) >= start.AddDays(nights)
					|| Start > start && Start.AddDays(Nights + PreparationDays) < start.AddDays(nights);
		}

		public bool IsBookedFor(DateTime startDate)
		{
			return Start <= startDate && Start.AddDays(Nights) > startDate;
		}

		public bool IsUnavailable(DateTime startDate)
		{
			return Start <= startDate && Start.AddDays(Nights + PreparationDays) > startDate;
		}
	}
}