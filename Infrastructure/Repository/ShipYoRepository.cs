using ShipYo.Core.Entities;
using ShipYo.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using ShipYo.Infrastructure.Persistence;

namespace ShipYo.Infrastructure.Repositories
{
    public class ShipYoRepository : IShipYoRepository
    {
        private readonly ApplicationDbContext _context;

        public ShipYoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShipYoEntity>> GetAllAsync()
        {
            return await _context.ShipYoEntities.ToListAsync();
        }

        public async Task<ShipYoEntity?> GetByIdAsync(int id)
        {
            return await _context.ShipYoEntities.FindAsync(id);
        }

        public async Task AddAsync(ShipYoEntity entity)
        {
            await _context.ShipYoEntities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ShipYoEntity entity)
        {
            _context.ShipYoEntities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ShipYoEntities.FindAsync(id);
            if (entity != null)
            {
                _context.ShipYoEntities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
