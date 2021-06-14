namespace VacationRental.Api.Models
{
    public class RentalViewModel
    {
        public int Id { get; }
        public int Units { get; }
		public int PreparationTimeInDays { get; }

		public RentalViewModel(int units, int preparationTimeInDays)
		{
			Units = units;
			PreparationTimeInDays = preparationTimeInDays;
		}
	}
}
