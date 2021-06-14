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
			var count = GetTotalUnitsOccupied(start, nights);

			return count < Units;
		}

		private int GetTotalUnitsOccupied(DateTime start, int nights)
		{
			var count = 0;
			for (var i = 0; i < nights; i++)
			{
				count = _bookings.Count(booking => booking.IsExistingBookingInConflictWithBookingRequest(start, nights));
			}

			return count;
		}

		public Booking AddBooking(int rentalId, int nights, DateTime date)
		{
			var totalDurationUnavailable = nights + PreparationTimeInDays;
			if (IsAvailableBookingRequest(date, totalDurationUnavailable))
			{
				var booking = new Booking(rentalId, nights, PreparationTimeInDays, date, GetTotalUnitsOccupied(date, totalDurationUnavailable) + 1);

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
			for (var i = 0; i < nights + PreparationTimeInDays; i++)
			{
				var dateTime = start.AddDays(i).Date;
				var calendarByDate =
					new BookingCalendarDate(dateTime, GetBookingIdsByDate(dateTime), GetPreparationDaysUnits(dateTime));

				calendar.AddCalendarDate(calendarByDate);
			}

			return calendar;
		}

		private List<int> GetPreparationDaysUnits(DateTime startDate)
		{
			return _bookings.Where(b => b.IsUnavailable(startDate) && !b.IsBookedFor(startDate))
				.Select(filtered => filtered.Unit).ToList();
		}

		private List<BookingUnit> GetBookingIdsByDate(DateTime startDate)
		{
			return _bookings.Where(b => b.IsBookedFor(startDate)).Select(filtered => new BookingUnit(filtered.Id, filtered.Unit)).ToList();
		}
	}
}