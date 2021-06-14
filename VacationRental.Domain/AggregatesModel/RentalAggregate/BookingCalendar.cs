using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class BookingCalendar
	{
		private readonly int _rentalId;
		private List<BookingCalendarDate> _dates;

		public int RentalId { get; }

		public IReadOnlyCollection<BookingCalendarDate> Dates => _dates;

		public BookingCalendar(int rentalId)
		{
			RentalId = rentalId;
			_dates = new List<BookingCalendarDate>();
		}

		public void AddCalendarDate(BookingCalendarDate calendarByDate)
		{
			_dates.Add(calendarByDate);
		}
	}
}