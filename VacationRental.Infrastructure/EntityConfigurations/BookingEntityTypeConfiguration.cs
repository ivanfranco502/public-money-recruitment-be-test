using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Domain.AggregatesModel.RentalAggregate;

namespace VacationRental.Infrastructure.EntityConfigurations
{
	public class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
	{
		public void Configure(EntityTypeBuilder<Booking> bookingConfiguration)
		{
			bookingConfiguration.HasKey(o => o.Id);

			bookingConfiguration.Ignore(b => b.DomainEvents);

			//Address value object persisted as owned entity type supported since EF Core 2.0
			bookingConfiguration.Property<int>("RentalId").IsRequired();
			bookingConfiguration.Property<int>("Nights").IsRequired();
			bookingConfiguration.Property<DateTime>("Start").IsRequired();
		}
	}
}