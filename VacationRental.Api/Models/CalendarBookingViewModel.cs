namespace VacationRental.Api.Models
{
    public class CalendarBookingViewModel
    {
		public int Id { get; set; }
		public int Unit { get; set; }

		public CalendarBookingViewModel()
		{
			
		}

		public CalendarBookingViewModel(int bookingId, int unit)
		{
			Id = bookingId;
			Unit = unit;
		}
	}
}
