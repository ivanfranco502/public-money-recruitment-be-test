using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VacationRental.Domain.AggregatesModel.RentalAggregate;

namespace VacationRental.Api.Models
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }

		public CalendarViewModel(int rentalId, IEnumerable<BookingCalendarDate> bookingCalendarDates)
		{
			RentalId = rentalId;
			Dates = bookingCalendarDates != null
				? bookingCalendarDates.Select(bcd => new CalendarDateViewModel(bcd.Date, bcd.Bookings)).ToList()
				: new List<CalendarDateViewModel>();
		}
    }
}
