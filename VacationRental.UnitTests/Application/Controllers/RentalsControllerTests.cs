using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Commands;
using VacationRental.Api.Controllers;
using VacationRental.Api.Models;
using Xunit;
using Xunit.Sdk;

namespace VacationRental.UnitTests.Application.Controllers
{
	public class RentalsControllerTests
	{
		private Mock<IMediator> _mediator;
		private Mock<ILogger<RentalsController>> _logger;
		private Mock<HttpContext> _context;

		public RentalsControllerTests()
		{
			_mediator = new Mock<IMediator>();
			_logger = new Mock<ILogger<RentalsController>>();
			_context = new Mock<HttpContext>();
		}

		[Fact]
		public async Task GivenAValidRentalId_WhenGet_ThenShouldReturnRentalEntity()
		{
			var rentalId = 1;

			var today = DateTime.Today;
			_mediator.Setup(m => m.Send(It.IsAny<GetRentalCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new RentalViewModel(3, 1));

			var rentalsController = new RentalsController(_mediator.Object, _logger.Object);
			rentalsController.ControllerContext.HttpContext = _context.Object;

			var response = await rentalsController.Get(rentalId);

			_mediator.Verify(
				m => m.Send(It.Is<GetRentalCommand>(grc => grc.RentalId == rentalId), It.IsAny<CancellationToken>()),
				Times.Once);

			Assert.NotNull(response);
			Assert.Equal(3, response.Units);
			Assert.Equal(1, response.PreparationTimeInDays);
		}

		[Fact]
		public async Task GivenAnInexistentRentalId_WhenGet_ThenShouldReturnError()
		{
			try
			{
				var rentalId = 1;
				_mediator.Setup(m => m.Send(It.IsAny<GetRentalCommand>(),
						It.IsAny<CancellationToken>()))
					.ReturnsAsync(default(RentalViewModel));

				var rentalsController = new RentalsController(_mediator.Object, _logger.Object);
				rentalsController.ControllerContext.HttpContext = _context.Object;

				await rentalsController.Get(rentalId);

				_mediator.Verify(
					m => m.Send(It.Is<GetRentalCommand>(grc => grc.RentalId == rentalId),
						It.IsAny<CancellationToken>()),
					Times.Once);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<ApplicationException>(ex);
			}
		}

		[Fact]
		public async Task GivenAValidRentalBindingModel_WhenPost_ThenShouldReturnPersistedId()
		{
			var rentalBindingModel = new RentalBindingModel{ PreparationTimeInDays = 3, Units = 1};

			_mediator.Setup(m => m.Send(It.IsAny<CreateRentalCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new ResourceIdViewModel(7));

			var rentalsController = new RentalsController(_mediator.Object, _logger.Object);
			rentalsController.ControllerContext.HttpContext = _context.Object;

			var response = await rentalsController.Post(rentalBindingModel);

			_mediator.Verify(
				m => m.Send(
					It.Is<CreateRentalCommand>(crc => crc.Units == 1 && crc.PreparationTimeInDays == 3),
					It.IsAny<CancellationToken>()),
				Times.Once);

			Assert.NotNull(response);
			Assert.Equal(7, response.Id);
		}

		[Fact]
		public async Task GivenAnInValidRentalBindingModel_WhenPost_ThenShouldReturnError()
		{
			try
			{
				var rentalBindingModel = new RentalBindingModel { PreparationTimeInDays = 3, Units = 1 };

				_mediator.Setup(m => m.Send(It.IsAny<CreateRentalCommand>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(default(ResourceIdViewModel));

				var rentalsController = new RentalsController(_mediator.Object, _logger.Object);
				rentalsController.ControllerContext.HttpContext = _context.Object;

				var response = await rentalsController.Post(rentalBindingModel);

				_mediator.Verify(
					m => m.Send(
						It.Is<CreateRentalCommand>(crc => crc.Units == 1 && crc.PreparationTimeInDays == 3),
						It.IsAny<CancellationToken>()),
					Times.Once);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<ApplicationException>(ex);
			}
		}

		[Fact]
		public async Task GivenAValidRentalBindingModel_WhenPut_ThenShouldReturnUpdated()
		{
			var rentalBindingModel = new RentalBindingModel { PreparationTimeInDays = 3, Units = 1 };

			_mediator.Setup(m => m.Send(It.IsAny<UpdateRentalCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new ResourceIdViewModel(7));

			var rentalsController = new RentalsController(_mediator.Object, _logger.Object);
			rentalsController.ControllerContext.HttpContext = _context.Object;

			var response = await rentalsController.Put(1, rentalBindingModel);

			_mediator.Verify(
				m => m.Send(
					It.Is<UpdateRentalCommand>(crc => crc.Units == 1 && crc.PreparationTimeInDays == 3),
					It.IsAny<CancellationToken>()),
				Times.Once);

			Assert.NotNull(response);
			Assert.Equal(7, response.Id);
		}

		[Fact]
		public async Task GivenAnInValidRentalBindingModel_WhenPut_ThenShouldReturnError()
		{
			try
			{
				var rentalBindingModel = new RentalBindingModel { PreparationTimeInDays = 3, Units = 1 };

				_mediator.Setup(m => m.Send(It.IsAny<UpdateRentalCommand>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(default(ResourceIdViewModel));

				var rentalsController = new RentalsController(_mediator.Object, _logger.Object);
				rentalsController.ControllerContext.HttpContext = _context.Object;

				var response = await rentalsController.Post(rentalBindingModel);

				_mediator.Verify(
					m => m.Send(
						It.Is<UpdateRentalCommand>(crc => crc.Units == 1 && crc.PreparationTimeInDays == 3),
						It.IsAny<CancellationToken>()),
					Times.Once);

				throw new XunitException();
			}
			catch (Exception ex)
			{
				Assert.IsType<ApplicationException>(ex);
			}
		}


	}
}