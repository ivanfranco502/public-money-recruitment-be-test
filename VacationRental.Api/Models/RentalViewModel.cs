namespace VacationRental.Api.Models
{
    public class RentalViewModel
    {
        public int Id { get; }
        public int Units { get; }

		public RentalViewModel(int units)
		{
			Units = units;
		}
	}
}
