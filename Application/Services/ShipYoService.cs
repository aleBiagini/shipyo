using ShipYo.Core.Entities;
using ShipYo.Core.Interfaces;

namespace ShipYo.Application.Services
{
    public class ShipYoService
    {
        private readonly IShipYoRepository _repository;

        public ShipYoService(IShipYoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ShipYoEntity>> GetAllEntitiesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ShipYoEntity?> GetEntityByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddEntityAsync(ShipYoEntity entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task UpdateEntityAsync(ShipYoEntity entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteEntityAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
