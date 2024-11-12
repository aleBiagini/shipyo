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
        public async Task AddEntity_ShouldSetCreatedAt_AndLeaveUpdatedAtAndDeletedAtNull()
        {
            // Arrange
            var context = InMemoryDbContextFactory.Create();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Test Entity", CreatedBy = "TestUser" };

            // Act
            await repository.AddAsync(entity);

            // Assert
            var savedEntity = await context.ShipYoEntities.FirstOrDefaultAsync(e => e.Name == "Test Entity");
            Assert.NotNull(savedEntity);
            Assert.Equal("Test Entity", savedEntity!.Name);
            Assert.Equal("TestUser", savedEntity.CreatedBy);
            Assert.True(savedEntity.CreatedAt <= DateTime.UtcNow);
            Assert.Null(savedEntity.UpdatedAt);
            Assert.Null(savedEntity.DeletedAt);
            Assert.False(savedEntity.IsDeleted);
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

            var entity = new ShipYoEntity { Name = "Entity 1", CreatedBy = "TestUser" };
            context.ShipYoEntities.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result!.Id);
            Assert.Equal("Entity 1", result.Name);
            Assert.Equal(entity.CreatedBy, result.CreatedBy);
            Assert.Equal(entity.CreatedAt, result.CreatedAt);
            Assert.Equal(entity.UpdatedAt, result.UpdatedAt);
        }

        [Fact]
        public async Task UpdateEntity_ShouldUpdateUpdatedAt_AndKeepCreatedAtUnchanged()
        {
            // Arrange
            var context = InMemoryDbContextFactory.Create();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Old Name", CreatedBy = "TestUser" };
            context.ShipYoEntities.Add(entity);
            await context.SaveChangesAsync();

            var originalCreatedAt = entity.CreatedAt;
            var originalCreatedBy = entity.CreatedBy;
            entity.Name = "Updated Name";
            entity.UpdatedBy = "AdminUser";
            entity.UpdatedAt = DateTime.UtcNow;

            // Act
            await repository.UpdateAsync(entity);

            // Assert
            var updatedEntity = await context.ShipYoEntities.FirstOrDefaultAsync(e => e.Id == entity.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal("Updated Name", updatedEntity!.Name);
            Assert.Equal(originalCreatedAt, updatedEntity.CreatedAt); // CreatedAt invariato
            Assert.Equal(originalCreatedBy, updatedEntity.CreatedBy); // CreatedBy invariato
            Assert.NotNull(updatedEntity.UpdatedAt);
            Assert.True(updatedEntity.UpdatedAt <= DateTime.UtcNow);
            Assert.Equal("AdminUser", updatedEntity.UpdatedBy);
        }

        [Fact]
        public async Task DeleteEntity_ShouldSetIsDeletedAndDeletedAt()
        {
            // Arrange
            var context = InMemoryDbContextFactory.Create();
            var repository = new ShipYoRepository(context);

            var entity = new ShipYoEntity { Name = "Entity to Delete" };
            context.ShipYoEntities.Add(entity);
            await context.SaveChangesAsync();

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            entity.DeletedBy = "AdminUser";

            // Act
            await repository.UpdateAsync(entity);

            // Assert
            var deletedEntity = await context.ShipYoEntities.FirstOrDefaultAsync(e => e.Id == entity.Id);
            Assert.NotNull(deletedEntity);
            Assert.True(deletedEntity!.IsDeleted);
            Assert.NotNull(deletedEntity.DeletedAt);
            Assert.Equal("AdminUser", deletedEntity.DeletedBy);
        }
    }
}
