using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.AggregatesModel.RentalAggregate;

namespace VacationRental.Api.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; } 
		public List<CalendarPreparationTimesViewModel> PreparationTimes { get; set; }

		public CalendarDateViewModel()
		{
			Bookings = new List<CalendarBookingViewModel>();
			PreparationTimes = new List<CalendarPreparationTimesViewModel>();
		}

		public CalendarDateViewModel(DateTime date, IEnumerable<BookingUnit> bookings, IEnumerable<int> preparationTimes):this()
		{
			Date = date;
			Bookings = bookings != null
				? bookings.Select(b => new CalendarBookingViewModel(b.Id, b.Unit)).ToList()
				: new List<CalendarBookingViewModel>();
			PreparationTimes = preparationTimes != null
				? preparationTimes.Select(pt => new CalendarPreparationTimesViewModel(pt)).ToList()
				: new List<CalendarPreparationTimesViewModel>();
		}
    }
}
