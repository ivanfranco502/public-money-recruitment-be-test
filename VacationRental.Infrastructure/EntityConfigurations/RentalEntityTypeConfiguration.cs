using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Domain.AggregatesModel.RentalAggregate;

namespace VacationRental.Infrastructure.EntityConfigurations
{
	public class RentalEntityTypeConfiguration : IEntityTypeConfiguration<Rental>
	{
		public void Configure(EntityTypeBuilder<Rental> rentalConfiguration)
		{
			rentalConfiguration.HasKey(o => o.Id);

			rentalConfiguration.Ignore(b => b.DomainEvents);

			//RentalType value object persisted as owned entity type supported since EF Core 2.0
			rentalConfiguration.OwnsOne(r => r.RentalType);

			rentalConfiguration.Property<int>("Units").IsRequired();
			rentalConfiguration.Property<int>("PreparationTimeInDays").IsRequired();

			var navigation = rentalConfiguration.Metadata.FindNavigation(nameof(Rental.Bookings));

			// DDD Patterns comment:
			//Set as field (New since EF 1.1) to access the Bookings collection property through its field
			navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			rentalConfiguration.HasOne(r => r.RentalType)
				.WithMany()
				.HasForeignKey("RentalTypeId");
		}
	}
}