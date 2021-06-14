using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace VacationRental.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseSerilog()
				.ConfigureLogging((hostingContext, logging) =>
				{
					Log.Logger = new LoggerConfiguration()
						.ReadFrom.Configuration(hostingContext.Configuration, sectionName: "Serilog")
						.CreateLogger();

					logging.AddSerilog(Log.Logger, dispose: true);
				});
	}
}
