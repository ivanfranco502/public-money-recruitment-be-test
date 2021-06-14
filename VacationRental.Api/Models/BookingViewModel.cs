using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel
    {
        public int Id { get;}
        public int RentalId { get; }
        public DateTime Start { get; }
        public int Nights { get; }

		public BookingViewModel(int rentalId, DateTime start, int nights)
		{
			RentalId = rentalId;
			Start = start;
			Nights = nights;
		}
    }
}
