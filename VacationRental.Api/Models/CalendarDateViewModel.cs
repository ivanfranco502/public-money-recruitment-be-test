using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Api.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }

		protected CalendarDateViewModel()
		{
			Bookings = new List<CalendarBookingViewModel>();
		}

		public CalendarDateViewModel(DateTime date, IEnumerable<int> bookings):this()
		{
			Date = date;
			Bookings = bookings != null
				? bookings.Select(b => new CalendarBookingViewModel(b)).ToList()
				: new List<CalendarBookingViewModel>();
		}
    }
}
