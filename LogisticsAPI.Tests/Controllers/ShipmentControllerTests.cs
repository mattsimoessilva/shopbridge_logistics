using FluentAssertions;
using LogisticsAPI.Controllers;
using LogisticsAPI.Models.DTOs.Shipment;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace  LogisticsAPI.Tests.Controllers
{
    public class ShipmentControllerTests
    {
        private readonly Mock<IShipmentService> _mockService;
        private readonly ShipmentController _controller;

        public ShipmentControllerTests()
        {
            _mockService = new Mock<IShipmentService>();
            _controller = new ShipmentController(_mockService.Object);
        }

        #region Create Method.

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRecordIsCreated()
        {
            // Arrange
            var dto = new ShipmentCreateDTO { OrderId = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA" };
            var created = new ShipmentReadDTO { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), TrackingCode = "TRK123456789", Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA", CreatedAt = DateTime.UtcNow };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            // Act
            var act = await _controller.Create(dto);

            // Assert
            var createdResult = act as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.Value.Should().BeEquivalentTo(created);
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            _mockService.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        #endregion

        #region GetAll Method.

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithRecordList()
        {
            // Arrange
            var entities = new List<ShipmentReadDTO>
            {
                new() { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), TrackingCode = "TRK123456789", Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA", CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Elm St", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow.AddDays(-3) },
                new() { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Delivered", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), TrackingCode = "TRK555666777", Carrier = "DHL", ServiceLevel = "Priority", Street = "789 Oak Ave", City = "Los Angeles", State = "CA", PostalCode = "90001", Country = "USA", CreatedAt = DateTime.UtcNow.AddDays(-8) }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var act = await _controller.GetAll();

            // Assert
            var okResult = act as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(entities);
        }

        #endregion

        #region GetById Method.

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new ShipmentReadDTO { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), TrackingCode = "TRK123456789", Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA", CreatedAt = DateTime.UtcNow };

            _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            var act = await _controller.GetById(id);

            // Assert
            var okResult = act as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ShipmentReadDTO?)null);

            // Act
            var act = await _controller.GetById(id);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Update Method.

        [Fact]
        public async Task Update_ShouldReturnOk_WithUpdatedRecord()
        {
            // Arrange
            var dto = new ShipmentUpdateDTO { Id = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA" };
            var updated = new ShipmentReadDTO { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), TrackingCode = "TRK123456789", Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA", CreatedAt = DateTime.UtcNow };

            _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(true);

            // Act
            var act = await _controller.Update(dto);

            // Assert
            act.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenUpdateFails()
        {
            // Arrange
            var dto = new ShipmentUpdateDTO { Id = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA" };

            _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(false);

            // Act
            var act = await _controller.Update(dto);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Delete Method.

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenRecordIsDeleted()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var act = await _controller.Delete(id);

            // Assert
            act.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var act = await _controller.Delete(id);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        #endregion
    }
}