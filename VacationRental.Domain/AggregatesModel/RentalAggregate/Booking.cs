using System;
using VacationRental.Domain.SeedWork;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class Booking : Entity
	{
		private int _rentalId;
		private int _nights;
		private DateTime _start;

		public int RentalId => _rentalId;

		public int Nights => _nights;

		public DateTime Start => _start;

		public Booking(int rentalId, int nights, DateTime start)
		{
			_rentalId = rentalId;
			_nights = nights;
			_start = start;
		}

		public bool IsBookingRequestInConflict(in DateTime start, int nights)
		{
			return (Start <= start.Date && Start.AddDays(Nights) > start.Date)
					|| (Start < start.AddDays(nights) &&
						Start.AddDays(Nights) >= start.AddDays(nights))
					|| (Start > start && Start.AddDays(Nights) < start.AddDays(nights));
		}

		public bool IsBookedFor(DateTime startDate)
		{
			return Start <= startDate && Start.AddDays(Nights) > startDate;
		}
	}
}