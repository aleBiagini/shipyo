using Moq;
using ShipYo.Application.Services;
using ShipYo.Core.Entities;
using ShipYo.Core.Interfaces;
using Xunit;

namespace ShipYo.Tests
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
            var result = await _service.GetAllEntitiesAsync();

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
            await _service.AddEntityAsync(entity);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(It.Is<ShipYoEntity>(e => e.Name == "New Entity")), Times.Once);
        }
    }
}
