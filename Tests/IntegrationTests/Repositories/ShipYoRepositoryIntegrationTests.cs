using Microsoft.EntityFrameworkCore;
using ShipYo.Core.Entities;
using ShipYo.Infrastructure.Persistence;
using ShipYo.Infrastructure.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShipYo.Tests.IntegrationTests.Repositories
{
    public class ShipYoRepositoryIntegrationTests
    {
        private ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}") // Nome univoco per evitare conflitti
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddEntity_ShouldAddEntityToDatabase()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Test Entity" };

            // Act
            await repository.AddAsync(entity);

            // Assert
            var savedEntity = await context.ShipYoEntities.FirstOrDefaultAsync(e => e.Name == "Test Entity");
            Assert.NotNull(savedEntity);
            Assert.Equal("Test Entity", savedEntity.Name);
        }

        [Fact]
        public async Task GetAllEntities_ShouldReturnAllEntities()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var repository = new ShipYoRepository(context);

            context.ShipYoEntities.AddRange(
                new ShipYoEntity { Name = "Entity 1" },
                new ShipYoEntity { Name = "Entity 2" }
            );
            await context.SaveChangesAsync();

            // Act
            var entities = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(entities);
            Assert.Equal(2, entities.Count());
        }

        [Fact]
        public async Task GetEntityById_ShouldReturnCorrectEntity()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Entity 1" };
            context.ShipYoEntities.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result!.Id);
            Assert.Equal("Entity 1", result.Name);
        }

        [Fact]
        public async Task UpdateEntity_ShouldModifyEntityInDatabase()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Old Name" };
            context.ShipYoEntities.Add(entity);
            await context.SaveChangesAsync();

            entity.Name = "Updated Name";

            // Act
            await repository.UpdateAsync(entity);

            // Assert
            var updatedEntity = await context.ShipYoEntities.FirstOrDefaultAsync(e => e.Id == entity.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal("Updated Name", updatedEntity!.Name);
        }

        [Fact]
        public async Task DeleteEntity_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Entity to Delete" };
            context.ShipYoEntities.Add(entity);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(entity.Id);

            // Assert
            var deletedEntity = await context.ShipYoEntities.FirstOrDefaultAsync(e => e.Id == entity.Id);
            Assert.Null(deletedEntity);
        }
    }
}
