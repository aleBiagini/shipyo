using Microsoft.EntityFrameworkCore;
using ShipYo.Infrastructure.Persistence;

namespace ShipYo.Tests.TestHelpers
{
    public static class InMemoryDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}") // Nome univoco per evitare conflitti
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
