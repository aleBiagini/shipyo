using Microsoft.EntityFrameworkCore;
using ShipYo.Core.Entities;
using ShipYo.Core.Interfaces;
using ShipYo.Infrastructure.Persistence;
using ShipYo.Infrastructure.Repositories;

public class ShipYoRepository : BaseRepository<ShipYoEntity>, IShipYoRepository
{
    public ShipYoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ShipYoEntity>> GetEntitiesByNameAsync(string name)
    {
        return await _dbSet.Where(e => e.Name.Contains(name) && !e.IsDeleted).ToListAsync();
    }
}
