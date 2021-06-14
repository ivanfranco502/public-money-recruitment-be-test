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

	public class BookingCalendarDate
	{
		public DateTime Date { get; }

		public IReadOnlyCollection<int> Bookings { get; }

		protected BookingCalendarDate()
		{
			Bookings = new List<int>();
		}

		public BookingCalendarDate(DateTime date, List<int> bookings): this()
		{
			Date = date;
			Bookings = bookings;
		}
	}
}