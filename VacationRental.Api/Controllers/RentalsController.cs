using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VacationRental.Api.Commands;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
	[Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<RentalsController> _logger;

		public RentalsController(
			IMediator mediator,
			ILogger<RentalsController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int rentalId)
		{
			var rental = default(RentalViewModel);

			if (rentalId > 0)
			{
				var getRentalCommand = new GetRentalCommand(rentalId);

				rental = await _mediator.Send(getRentalCommand);
			}

			if (rental == default(RentalViewModel))
                throw new ApplicationException("Rental not found");

            return rental;
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
		{
			var createdRental = default(ResourceIdViewModel);

			var createRentalCommand = new CreateRentalCommand(model.Units);
            
			if (createdRental == default(ResourceIdViewModel))
				throw new ApplicationException("Rental was not created");

			return createdRental;
		}
    }
}
