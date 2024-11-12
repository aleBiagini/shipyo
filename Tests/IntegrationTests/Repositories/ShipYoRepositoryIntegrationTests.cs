using Microsoft.EntityFrameworkCore;
using ShipYo.Core.Entities;
using ShipYo.Infrastructure.Persistence;
using ShipYo.Infrastructure.Repositories;
using ShipYo.Tests.TestHelpers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShipYo.Tests.IntegrationTests.Repositories
{
    public class ShipYoRepositoryIntegrationTests
    {
        [Fact]
        public async Task AddEntity_ShouldSetCreatedAt_AndLeaveUpdatedAtNull()
        {
            // Arrange
            var context = InMemoryDbContextFactory.Create();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Test Entity" };

            // Act
            await repository.AddAsync(entity);

            // Assert
            var savedEntity = await context.ShipYoEntities.FirstOrDefaultAsync(e => e.Name == "Test Entity");
            Assert.NotNull(savedEntity);
            Assert.Equal("Test Entity", savedEntity!.Name);
            Assert.True(savedEntity.CreatedAt <= DateTime.UtcNow);
            Assert.Null(savedEntity.UpdatedAt);
        }

        [Fact]
        public async Task GetAllEntities_ShouldReturnAllEntities()
        {
            // Arrange
            var context = InMemoryDbContextFactory.Create();
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
            var context = InMemoryDbContextFactory.Create();
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
            Assert.Equal(entity.CreatedAt, result.CreatedAt);
            Assert.Equal(entity.UpdatedAt, result.UpdatedAt);
        }

        [Fact]
        public async Task UpdateEntity_ShouldUpdateUpdatedAt_AndKeepCreatedAtUnchanged()
        {
            // Arrange
            var context = InMemoryDbContextFactory.Create();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Old Name" };
            context.ShipYoEntities.Add(entity);
            await context.SaveChangesAsync();

            var originalCreatedAt = entity.CreatedAt;
            entity.Name = "Updated Name";
            entity.UpdatedAt = DateTime.UtcNow;

            // Act
            await repository.UpdateAsync(entity);

            // Assert
            var updatedEntity = await context.ShipYoEntities.FirstOrDefaultAsync(e => e.Id == entity.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal("Updated Name", updatedEntity!.Name);
            Assert.Equal(originalCreatedAt, updatedEntity.CreatedAt); // Invariato
            Assert.NotNull(updatedEntity.UpdatedAt);
            Assert.True(updatedEntity.UpdatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public async Task DeleteEntity_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            var context = InMemoryDbContextFactory.Create();
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
