using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Domain.AggregatesModel.RentalAggregate;
using VacationRental.Infrastructure;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Api.Modules
{
	public static class ApplicationModule
	{
		public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddSwaggerGen(opts =>
			{
				opts.DescribeAllEnumsAsStrings();
				opts.SwaggerDoc("v1", new Info {Title = "Vacation rental information", Version = "v1"});
			});

			return services;
		}

		public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddEntityFrameworkInMemoryDatabase()
				.AddDbContext<VacationRentalContext>(
					opt => { opt.UseInMemoryDatabase(databaseName: "VacationRental"); },
					ServiceLifetime.Scoped);

			return services;
		}


		public static IServiceCollection AddMediatorConfiguration(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddSingleton<IMediator, Mediator>();

			return services;
		}

		public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddScoped<IRentalRepository, RentalRepository>();

			return services;
		}
	}
}
