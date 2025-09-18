using AutoMapper;
using FluentAssertions;
using LogisticsAPI.Models.DTOs.Shipment;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Repositories.Interfaces;
using LogisticsAPI.Services;
using Moq;

namespace LogisticsAPI.Tests.Services
{
    public class ShipmentServiceTests
    {
        private readonly Mock<IShipmentRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ShipmentService _service;

        public ShipmentServiceTests()
        {
            _repositoryMock = new Mock<IShipmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ShipmentService(_repositoryMock.Object, _mapperMock.Object);
        }

        #region CreateAsync Method.

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDTOisNull()
        {
            // Arrange
            ShipmentCreateDTO dto = null;

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName("dto");

            _mapperMock.Verify(m => m.Map<Shipment>(It.IsAny<ShipmentCreateDTO>()), Times.Never);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Shipment>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnDTO_WhenDTOIsValid()
        {
            // Arrange
            var createDTO = new ShipmentCreateDTO { OrderId = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA" };
            var entity = new Shipment { Id = Guid.NewGuid(), OrderId = createDTO.OrderId, Status = createDTO.Status, DispatchDate = createDTO.DispatchDate, ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), TrackingCode = "TRK123456789", Carrier = createDTO.Carrier, ServiceLevel = createDTO.ServiceLevel, Street = createDTO.Street, City = createDTO.City, State = createDTO.State, PostalCode = createDTO.PostalCode, Country = createDTO.Country, CreatedAt = DateTime.UtcNow };
            var readDTO = new ShipmentReadDTO { Id = entity.Id, OrderId = entity.OrderId, Status = entity.Status, DispatchDate = entity.DispatchDate, ExpectedArrival = entity.ExpectedArrival, TrackingCode = entity.TrackingCode, Carrier = entity.Carrier, ServiceLevel = entity.ServiceLevel, Street = entity.Street, City = entity.City, State = entity.State, PostalCode = entity.PostalCode, Country = entity.Country, CreatedAt = entity.CreatedAt };

            _mapperMock.Setup(m => m.Map<Shipment>(createDTO)).Returns(entity);
            _mapperMock.Setup(m => m.Map<ShipmentReadDTO>(entity)).Returns(readDTO);
            _repositoryMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);

            // Act
            var act = await _service.CreateAsync(createDTO);

            // Assert
            act.Should().BeEquivalentTo(readDTO);

            _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
            _mapperMock.Verify(m => m.Map<Shipment>(createDTO), Times.Once);
            _mapperMock.Verify(m => m.Map<ShipmentReadDTO>(entity), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var dto = new ShipmentCreateDTO { OrderId = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA" };
            var entity = new Shipment { Id = Guid.NewGuid(), OrderId = dto.OrderId, Status = dto.Status, DispatchDate = dto.DispatchDate, ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), TrackingCode = "TRK123456789", Carrier = dto.Carrier, ServiceLevel = dto.ServiceLevel, Street = dto.Street, City = dto.City, State = dto.State, PostalCode = dto.PostalCode, Country = dto.Country, CreatedAt = DateTime.UtcNow };

            _mapperMock.Setup(m => m.Map<Shipment>(dto)).Returns(entity);
            _repositoryMock.Setup(r => r.AddAsync(entity)).ThrowsAsync(new Exception("Repository failure."));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
            _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
        }

        #endregion

        #region GetAllAsync Method.

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoRecordsExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Shipment>());

            // Act
            var act = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(act);
            Assert.Empty(act);

            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDTOs_WhenRecordsExist()
        {
            // Arrange
            var entities = new List<Shipment>
            {
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Oak Ave", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow },
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Delivered", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), TrackingCode = "TRK555666777", Carrier = "DHL", ServiceLevel = "Overnight", Street = "789 Pine Rd", City = "Los Angeles", State = "CA", PostalCode = "90001", Country = "USA", CreatedAt = DateTime.UtcNow }
            };

            var dtos = new List<ShipmentReadDTO>
            {
                new ShipmentReadDTO { OrderId = entities[0].OrderId, Status = entities[0].Status, DispatchDate = entities[0].DispatchDate, ExpectedArrival = entities[0].ExpectedArrival, TrackingCode = entities[0].TrackingCode, Carrier = entities[0].Carrier, ServiceLevel = entities[0].ServiceLevel, Street = entities[0].Street, City = entities[0].City, State = entities[0].State, PostalCode = entities[0].PostalCode, Country = entities[0].Country, CreatedAt = entities[0].CreatedAt },
                new ShipmentReadDTO { OrderId = entities[1].OrderId, Status = entities[1].Status, DispatchDate = entities[1].DispatchDate, ExpectedArrival = entities[1].ExpectedArrival, TrackingCode = entities[1].TrackingCode, Carrier = entities[1].Carrier, ServiceLevel = entities[1].ServiceLevel, Street = entities[1].Street, City = entities[1].City, State = entities[1].State, PostalCode = entities[1].PostalCode, Country = entities[1].Country, CreatedAt = entities[1].CreatedAt }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<ShipmentReadDTO>>(entities)).Returns(dtos);

            // Act
            var act = await _service.GetAllAsync();

            // Assert 
            act.Should().BeEquivalentTo(dtos);
        }

        [Fact]
        public async Task GetAllAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var exception = new Exception("Database connection failed.");

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.GetAllAsync();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database connection failed.");
        }

        #endregion

        #region GetById Method.

        [Fact]
        public async Task GetByIdAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<Task> act = async () => await _service.GetByIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid ID (Parameter 'id')");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDTO_WhenRecordExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            var entity = new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Oak Ave", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow };
            var dto = new ShipmentReadDTO { OrderId = entity.OrderId, Status = entity.Status, DispatchDate = entity.DispatchDate, ExpectedArrival = entity.ExpectedArrival, TrackingCode = entity.TrackingCode, Carrier = entity.Carrier, ServiceLevel = entity.ServiceLevel, Street = entity.Street, City = entity.City, State = entity.State, PostalCode = entity.PostalCode, Country = entity.Country, CreatedAt = entity.CreatedAt };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ShipmentReadDTO>(entity)).Returns(dto);

            // Act
            var act = await _service.GetByIdAsync(id);

            // Assert
            act.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new Exception("Repository failure.");

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.GetByIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Repository failure.");
        }

        #endregion

        #region UpdateAsync Method.

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenDTOisNullOrIdIsEmpty()
        {
            // Arrange
            ShipmentUpdateDTO nullDTO = null;
            var emptyIdDTO = new ShipmentUpdateDTO { Id = Guid.Empty, Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA" };

            // Act
            Func<Task> actWithNull = async () => await _service.UpdateAsync(nullDTO);
            Func<Task> actWithEmptyId = async () => await _service.UpdateAsync(emptyIdDTO);

            // Assert
            await actWithEmptyId.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid update data.");
            await actWithEmptyId.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid update data.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var dto = new ShipmentUpdateDTO { Id = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA" };
            var existing = new Shipment { Id = dto.Id, OrderId = Guid.NewGuid(), Status = dto.Status, DispatchDate = dto.DispatchDate, ExpectedArrival = dto.ExpectedArrival, TrackingCode = "TRK987654321", Carrier = dto.Carrier, ServiceLevel = dto.ServiceLevel, Street = dto.Street, City = dto.City, State = dto.State, PostalCode = dto.PostalCode, Country = dto.Country, CreatedAt = DateTime.UtcNow };

            _repositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(dto, existing));
            _repositoryMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(true);

            // Act
            var act = await _service.UpdateAsync(dto);

            // Assert
            act.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var dto = new ShipmentUpdateDTO { Id = Guid.NewGuid(), Status = "Pending", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Carrier = "FedEx", ServiceLevel = "Express", Street = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", Country = "USA" };

            var existing = new Shipment { Id = dto.Id, OrderId = Guid.NewGuid(), Status = dto.Status, DispatchDate = dto.DispatchDate, ExpectedArrival = dto.ExpectedArrival, TrackingCode = "TRK987654321", Carrier = dto.Carrier, ServiceLevel = dto.ServiceLevel, Street = dto.Street, City = dto.City, State = dto.State, PostalCode = dto.PostalCode, Country = dto.Country, CreatedAt = DateTime.UtcNow };

            var exception = new Exception("Repository failure");

            _repositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(dto, existing));
            _repositoryMock.Setup(r => r.UpdateAsync(existing)).ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Repository failure");
        }

        #endregion

        #region DeleteAsync Method.

        [Fact]
        public async Task DeleteAsync_ShouldCallRepository_WithCorrectId()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var act = await _service.DeleteAsync(id);

            // Assert
            act.Should().BeTrue();
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new Exception("Repository delete failed");

            _repositoryMock
                .Setup(r => r.DeleteAsync(id))
                .ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.DeleteAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Repository delete failed");
        }

        #endregion
    }
}