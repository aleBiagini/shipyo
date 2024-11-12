using Moq;
using ShipYo.Application.Services;
using ShipYo.Core.Entities;
using ShipYo.Core.Interfaces;
using Xunit;

namespace ShipYo.Tests.UnitTests.Services
{
    public class ShipYoServiceTests
    {
        private readonly Mock<IShipYoRepository> _mockRepository;
        private readonly ShipYoService _service;

        public ShipYoServiceTests()
        {
            _mockRepository = new Mock<IShipYoRepository>();
            _service = new ShipYoService(_mockRepository.Object);
        }


        [Fact]
        public async Task GetAllEntitiesAsync_ShouldReturnEntities()
        {
            // Arrange
            var entities = new List<ShipYoEntity>
            {
                new ShipYoEntity { Id = 1, Name = "Entity 1" },
                new ShipYoEntity { Id = 2, Name = "Entity 2" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Entity 1", result.First().Name);
        }

        [Fact]
        public async Task AddEntityAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var entity = new ShipYoEntity { Name = "New Entity" };

            // Act
            await _service.AddAsync(entity);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(It.Is<ShipYoEntity>(e => e.Name == "New Entity")), Times.Once);
        }

        [Fact]
        public async Task GetEntityByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            var entity = new ShipYoEntity { Id = 1, Name = "Entity 1" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
            Assert.Equal("Entity 1", result.Name);
        }

        [Fact]
        public async Task GetEntityByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ShipYoEntity?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateEntityAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var entity = new ShipYoEntity { Id = 1, Name = "Updated Entity" };

            // Act
            await _service.UpdateAsync(entity);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(It.Is<ShipYoEntity>(e => e.Id == 1 && e.Name == "Updated Entity")), Times.Once);
        }

        [Fact]
        public async Task DeleteEntityAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            var entityId = 1;

            // Act
            await _service.DeleteAsync(entityId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(It.Is<int>(id => id == entityId)), Times.Once);
        }
    }
}
