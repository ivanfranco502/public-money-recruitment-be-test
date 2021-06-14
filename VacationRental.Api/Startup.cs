using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Api.Modules;
using VacationRental.Infrastructure;

namespace VacationRental.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services
				.AddSwaggerConfiguration(Configuration)
				.AddDbContextConfiguration(Configuration)
				.AddMediatorConfiguration(Configuration)
				.AddRepositoryConfiguration(Configuration)
				.AddMediatR(typeof(Startup).GetTypeInfo().Assembly)
				.AddMediatR(typeof(IValidator<>))
				.AddMediatR(typeof(IRequestHandler<,>))
				.AddMediatR(typeof(IRequest<>))
				.AddMediatR(typeof(INotificationHandler<>));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
		}
	}
}
