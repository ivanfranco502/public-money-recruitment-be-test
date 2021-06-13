using System.Threading.Tasks;
using VacationRental.Domain.SeedWork;

namespace VacationRental.Domain.AggregatesModel.RentalAggregate
{
	public interface IRentalRepository : IRepository<Rental>
	{
		Task<Rental> GetAsync(int rentalId);

		Rental Add(Rental rental);

		void Update(Rental rental);
	}
}