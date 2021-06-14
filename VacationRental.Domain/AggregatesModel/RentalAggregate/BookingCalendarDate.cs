using System;
using System.Collections.Generic;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class BookingCalendarDate
	{
		private List<BookingUnit> _bookings;
		private List<int> _preparationTimes;

		public DateTime Date { get; }

		public IReadOnlyCollection<BookingUnit> Bookings => _bookings;

		public IReadOnlyCollection<int> PreparationTimes => _preparationTimes;

		protected BookingCalendarDate()
		{
			_bookings = new List<BookingUnit>();
		}

		public BookingCalendarDate(DateTime date, List<BookingUnit> bookings, List<int> preparationTimes): this()
		{
			Date = date;
			_bookings = bookings;
			_preparationTimes = preparationTimes;
		}
	}
}