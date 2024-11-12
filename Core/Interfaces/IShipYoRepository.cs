using ShipYo.Core.Entities;

namespace ShipYo.Core.Interfaces
{
    public interface IShipYoRepository
    {
        Task<IEnumerable<ShipYoEntity>> GetAllAsync();
        Task<ShipYoEntity?> GetByIdAsync(int id);
        Task AddAsync(ShipYoEntity entity);
        Task UpdateAsync(ShipYoEntity entity);
        Task DeleteAsync(int id);
    }
}
