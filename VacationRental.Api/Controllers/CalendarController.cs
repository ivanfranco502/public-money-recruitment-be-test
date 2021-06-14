using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VacationRental.Api.Commands;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
	[Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
		private readonly IMediator _mediator;
		private readonly ILogger<CalendarController> _logger;

        public CalendarController(
			IMediator mediator,
			ILogger<CalendarController> logger)
        {
			_mediator = mediator;
			_logger = logger;
        }

        [HttpGet]
        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
			var getCalendarCommand = new GetCalendarCommand(rentalId, start, nights);

			CalendarViewModel calendar = await _mediator.Send(getCalendarCommand);

			if (calendar == default(CalendarViewModel))
				throw new ApplicationException("Calendar not found");

			return calendar;
		}
    }
}
