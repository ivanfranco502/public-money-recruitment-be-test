namespace VacationRental.Api.Models
{
	public class CalendarPreparationTimesViewModel
	{
		public int Unit { get; set; }

		public CalendarPreparationTimesViewModel()
		{

		}

		public CalendarPreparationTimesViewModel(int unit)
		{
			Unit = unit;
		}
	}
}