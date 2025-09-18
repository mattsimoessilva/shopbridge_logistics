using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using LogisticsAPI.Data;
using LogisticsAPI.Models.DTOs.Shipment;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Repositories;
using System.Runtime.Intrinsics.Arm;

namespace LogisticsAPI.Tests.Repositories
{
    public class ShipmentRepositoryTests
    {
        private readonly Mock<IMapper> _mapperMock;

        public ShipmentRepositoryTests()
        {
            _mapperMock = new Mock<IMapper>();
        }

        private LogisticsAppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<LogisticsAppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            return new LogisticsAppDbContext(options);
        }

        private ShipmentRepository GetRepository(LogisticsAppDbContext context)
        {
            return new ShipmentRepository(context, _mapperMock.Object);
        }

        #region AddAsync Method.

        [Fact]
        public async Task AddAsync_ShouldPersistAndReturnEntity()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var entity = new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Oak Ave", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow };

            // Act
            var act = await repository.AddAsync(entity);

            // Assert
            act.Should().NotBeNull();
            Assert.NotNull(act);
            act.Should().BeEquivalentTo(entity);

            var saved = await context.Shipments.FirstOrDefaultAsync(p => p.Id == entity.Id);
            Assert.NotNull(saved);
            saved.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async Task AddAsync_ShouldHandleNullRecordGracefully()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            // Act
            Func<Task> act = () => repository.AddAsync(null);

            // Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName("entity");

            var allEntities = await context.Shipments.ToListAsync();
            allEntities.Should().BeEmpty();
        }

        #endregion

        #region GetAllAsync Method.

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_IfNoRecords()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            // Act
            var act = await repository.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCorrectType()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var entities = new List<Shipment>
            {
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Oak Ave", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow },
                new Shipment { Id = Guid.NewGuid(), OrderId = Guid.NewGuid(), Status = "Delivered", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), TrackingCode = "TRK555666777", Carrier = "DHL", ServiceLevel = "Overnight", Street = "789 Pine Rd", City = "Los Angeles", State = "CA", PostalCode = "90001", Country = "USA", CreatedAt = DateTime.UtcNow }
            };

            await context.SaveChangesAsync();

            // Act
            var act = await repository.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().BeAssignableTo<IEnumerable<Shipment>>();
            act.Should().AllBeOfType<Shipment>();
        }

        #endregion

        #region GetByIdAsync Method.

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord_WithMatchingId()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new Shipment { Id = id, OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Oak Ave", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow };

            context.Shipments.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.GetByIdAsync(id);

            // Assert
            Assert.NotNull(act);
            act.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullIfNotFound()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var id = Guid.NewGuid();

            // Act
            var act = await repository.GetByIdAsync(id);

            // Assert
            act.Should().BeNull();
        }

        #endregion

        #region UpdateAsync Method.

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_IfShipmentNotFound()
        {
            // Arrange 
            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new Shipment { Id = id, OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Oak Ave", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow };

            // Act
            var act = await repository.UpdateAsync(entity);

            // Assert
            act.Should().BeFalse();

            var allEntities = await context.Shipments.ToListAsync();
            allEntities.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_ShouldPersistChanges()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new Shipment { Id = id, OrderId = Guid.NewGuid(), Status = "Shipped", DispatchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)), ExpectedArrival = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)), TrackingCode = "TRK987654321", Carrier = "UPS", ServiceLevel = "Standard", Street = "456 Oak Ave", City = "Chicago", State = "IL", PostalCode = "60601", Country = "USA", CreatedAt = DateTime.UtcNow };

            context.Shipments.Add(entity);
            await context.SaveChangesAsync();

            // Act
            entity.Name = "New Name";
            entity.ShortDescription = "New Short Description";
            entity.FullDescription = "New Full Description";
            entity.Price = 0;
            entity.DiscountPrice = 0;
            entity.IsActive = false;
            entity.IsFeatured = false;
            entity.StockQuantity = 0;
            entity.MinimumStockThreshold = 0;
            entity.AllowBackorder = false;
            entity.Brand = "New Brand";
            entity.Category = "New Category";
            entity.Tags = "new, more_new";
            entity.ImageUrl = "/images/new_stuff/new-image.jpg";
            entity.ThumbnailUrl = "/images/new_stuff/new-thumb.jpg";
            entity.SeoTitle = "New Seo Title";
            entity.Slug = "New Slug";

            var act = await repository.UpdateAsync(entity);

            // Assert
            act.Should().BeTrue();

            var updated = await context.Shipments.FindAsync(id);
            updated.Should().BeEquivalentTo(entity);
        }

        #endregion

        #region DeleteAsync Method.

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfShipmentNotFound()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var id = Guid.NewGuid();

            // Act
            var act = await repository.DeleteAsync(id);

            // Assert
            act.Should().BeFalse();
            (await context.Shipments.CountAsync()).Should().Be(0);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRecord()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var entity = new Shipment { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ShipmentVariant>(), Reviews = new List<ShipmentReview>() };

            context.Shipments.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.DeleteAsync(entity.Id);

            // Assert
            act.Should().BeTrue();
            (await context.Shipments.FindAsync(entity.Id)).Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldPersistChanges()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var entity = new Shipment { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ShipmentVariant>(), Reviews = new List<ShipmentReview>() };

            context.Shipments.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.DeleteAsync(entity.Id);

            // Assert
            act.Should().BeTrue();

            var existsInDb = await context.Shipments.AnyAsync(p => p.Id == entity.Id);
            existsInDb.Should().BeFalse();
        }

        #endregion
    }
}