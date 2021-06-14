using System;
using VacationRental.Domain.AggregatesModel.RentalAggregate;
using VacationRental.Domain.Exceptions;
using Xunit;

namespace VacationRental.UnitTests.Domain
{
	public class RentalAggregateTests
	{
		public RentalAggregateTests()
		{
			
		}

		[Fact]
		public void GivenValidRentalParameters_WhenCreatingRental_ThenShouldCreateRentalEntity()
		{
			var rental = new Rental(1, RentalType.Room, 2);

			Assert.NotNull(rental);
		}

		[Fact]
		public void GivenAnEmptyRentalType_WhenCreatingRental_ThenShouldBeRoomByDefault()
		{
			var rental = new Rental(1, null, 2);

			Assert.NotNull(rental);
			Assert.Equal(RentalType.Room, rental.RentalType);
		}

		[Fact]
		public void GivenInvalidUnits_WhenCreatingRental_ThenThrowException()
		{
			Assert.Throws<ArgumentException>(() => new Rental(-1, RentalType.Room, 2));
		}

		[Fact]
		public void GivenInvalidPreparationTimeInDays_WhenCreatingRental_ThenThrowException()
		{
			Assert.Throws<ArgumentException>(() => new Rental(2, RentalType.Room, -1));
		}

		[Fact]
		public void GivenValidBookingParameters_WhenAddBooking_ThenShouldAddToRental()
		{
			var rental = new Rental(1, RentalType.Room, 2);

			var booking = rental.AddBooking(1, 2, DateTime.Today);

			Assert.NotNull(booking);
			Assert.Single(rental.Bookings);
		}

		[Fact]
		public void GivenInvalidRentalId_WhenAddBooking_ThenShouldThrowError()
		{
			var rental = new Rental(1, RentalType.Room, 2);
			Assert.Throws<ArgumentException>(() => rental.AddBooking(-1, 2, DateTime.Today));
		}

		[Fact]
		public void GivenInvalidNights_WhenAddBooking_ThenShouldThrowError()
		{
			var rental = new Rental(1, RentalType.Room, 2);
			Assert.Throws<ArgumentException>(() => rental.AddBooking(1, -2, DateTime.Today));
		}

		[Fact] 
		public void GivenInvalidStartDate_WhenAddBooking_ThenShouldThrowError()
		{
			var rental = new Rental(1, RentalType.Room, 2);
			var yesterday = DateTime.Today.AddDays(-1);
			Assert.Throws<ArgumentException>(() =>
			{
				return rental.AddBooking(-1, 2, yesterday);
			});
		}

		[Fact]
		public void GivenANewBookingWithoutConflictWithPrevious_WhenAddBooking_ThenShouldAddToRental()
		{
			var rental = new Rental(1, RentalType.Room, 2);
			rental.AddBooking(1, 2, DateTime.Today);

			var newBooking = rental.AddBooking(1, 3, DateTime.Today.AddMonths(1));

			Assert.NotNull(newBooking);
			Assert.Equal(2, rental.Bookings.Count);
		}

		[Fact]
		public void GivenANewBookingInConflictWithPrevious_WhenAddBooking_ThenShouldThrowError()
		{
			var rental = new Rental(1, RentalType.Room, 2);
			rental.AddBooking(1, 2, DateTime.Today);

			Assert.Throws<BookingDomainException>(() => rental.AddBooking(1, 3, DateTime.Today.AddDays(1)));
			Assert.Single(rental.Bookings);
		}

		[Fact]
		public void GivenANewBookingInConflictWithPreparationTime_WhenAddBooking_ThenShouldThrowError()
		{
			var rental = new Rental(1, RentalType.Room, 2);
			rental.AddBooking(1, 2, DateTime.Today);

			Assert.Throws<BookingDomainException>(() => rental.AddBooking(1, 3, DateTime.Today.AddDays(3)));
			Assert.Single(rental.Bookings);
		}

		[Fact]
		public void GivenARentalPreparationTimeUpdateWithoutConflict_WhenVerifyRentalUpdate_ThenShouldRunWithoutError()
		{
			var rental = new Rental(1, RentalType.Room, 2);
			rental.AddBooking(1, 2, DateTime.Today);
			rental.AddBooking(1, 2, DateTime.Today.AddDays(4));

			rental.ApplyUpdate(1, 1);
		}

		[Fact]
		public void GivenARentalPreparationTimeUpdateWithConflict_WhenVerifyRentalUpdate_ThenShouldThrowError()
		{
			var rental = new Rental(1, RentalType.Room, 1);
			rental.AddBooking(1, 2, DateTime.Today);
			rental.AddBooking(1, 2, DateTime.Today.AddDays(3));


			Assert.Throws<BookingDomainException>(() => rental.ApplyUpdate(1, 2, false));
		}

		[Fact]
		public void GivenARentalUnitUpdateWithoutConflict_WhenVerifyRentalUpdate_ThenShouldRunWithoutError()
		{
			var rental = new Rental(2, RentalType.Room, 1);
			rental.AddBooking(1, 2, DateTime.Today);
			rental.AddBooking(1, 2, DateTime.Today);

			rental.ApplyUpdate(3, 1, false);
		}

		[Fact]
		public void GivenARentalUnitUpdateWithConflict_WhenVerifyRentalUpdate_ThenShouldThrowError()
		{
			var rental = new Rental(2, RentalType.Room, 1);
			rental.AddBooking(1, 2, DateTime.Today);
			rental.AddBooking(1, 2, DateTime.Today);

			Assert.Throws<BookingDomainException>(() => rental.ApplyUpdate(1, 2, false));
		}
	}
}
