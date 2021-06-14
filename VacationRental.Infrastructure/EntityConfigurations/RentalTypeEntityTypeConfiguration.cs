using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationRental.Domain.AggregatesModel.RentalAggregate;

namespace VacationRental.Infrastructure.EntityConfigurations
{
	public class RentalTypeEntityTypeConfiguration : IEntityTypeConfiguration<RentalType>
	{
		public void Configure(EntityTypeBuilder<RentalType> rentalTypeConfiguration)
		{
			rentalTypeConfiguration.HasKey(o => o.Id);

			rentalTypeConfiguration.Property(o => o.Id)
				.ValueGeneratedNever()
				.IsRequired();

			rentalTypeConfiguration.Property(o => o.Name)
				.HasMaxLength(200)
				.IsRequired();
        }
	}
}