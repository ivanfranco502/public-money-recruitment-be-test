using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Exceptions;
using VacationRental.Domain.SeedWork;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public class Rental : Entity, IAggregateRoot
	{
		private readonly List<Booking> _bookings;
		private int _units;
		private RentalType _rentalType;
		private int _preparationTimeInDays;

		public int Units => _units;

		public RentalType RentalType => _rentalType;

		public int PreparationTimeInDays => _preparationTimeInDays;

		public IReadOnlyCollection<Booking> Bookings => _bookings;

		protected Rental()
		{
			_bookings = new List<Booking>();
		}

		public Rental(int units, RentalType rentalType, int preparationTimeInDays) : this()
		{
			_units = units;
			_rentalType = rentalType;
			_preparationTimeInDays = preparationTimeInDays;
		}

		public bool IsAvailableBookingRequest(DateTime start, int nights)
		{
			var count = 0;
			for (var i = 0; i < nights; i++)
			{
				count = _bookings.Count(booking => booking.IsBookingRequestInConflict(start, nights));
			}

			return count < Units;
		}

		public Booking AddBooking(int rentalId, int nights, DateTime date)
		{
			if (IsAvailableBookingRequest(date, nights))
			{
				var booking = new Booking(rentalId, nights, date);

				_bookings.Add(booking);

				return booking;
			}
			else
			{
				throw new BookingDomainException(
					"The booking is not possible to be created. There is no availability.");
			}
		}

		public BookingCalendar GetCalendar(DateTime start, int nights)
		{
			var calendar = new BookingCalendar(Id);
			for (var i = 0; i < nights; i++)
			{
				var dateTime = start.AddDays(i).Date;
				var calendarByDate =
					new BookingCalendarDate(dateTime, GetBookingIdsByDate(dateTime));

				calendar.AddCalendarDate(calendarByDate);
			}

			return calendar;
		}

		private List<int> GetBookingIdsByDate(DateTime startDate)
		{
			return _bookings.Where(b => b.IsBookedFor(startDate)).Select(filtered => filtered.Id).ToList();
		}
	}
}