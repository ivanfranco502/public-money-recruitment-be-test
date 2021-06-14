using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationRental.Domain.AggregatesModel.RentalAggregate;
using VacationRental.Domain.SeedWork;

namespace VacationRental.Infrastructure.Repositories
{
	public class RentalRepository : IRentalRepository
	{
		private readonly VacationRentalContext _context;

		public IUnitOfWork UnitOfWork => _context;

		public RentalRepository(VacationRentalContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));

		}

		public async Task<Rental> GetAsync(int rentalId)
		{
			var rental = await _context.Rentals.FindAsync(rentalId);
			if (rental != null)
			{
				await _context.Entry(rental)
					.Collection(i => i.Bookings).LoadAsync();
				await _context.Entry(rental)
					.Reference(i => i.RentalType).LoadAsync();
			}

			return rental;
		}

		public Rental Add(Rental rental)
		{
			return _context.Rentals.Add(rental).Entity;
		}

		public void Update(Rental rental)
		{
			_context.Entry(rental).State = EntityState.Modified;
		}

		public async Task<Booking> GetBookingAsync(int bookingId)
		{
			return await _context.Bookings.FindAsync(bookingId);
		}
	}
}