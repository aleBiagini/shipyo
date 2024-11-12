using ShipYo.Core.Entities;
using ShipYo.Core.Interfaces;

namespace ShipYo.Application.Services
{
    public class ShipYoService : BaseService<ShipYoEntity>, IBaseService<ShipYoEntity>
    {
        public ShipYoService(IShipYoRepository repository) : base(repository)
        {
        }

        // Metodi specifici per ShipYoEntity
        public async Task ConfigureEntityAsync(ShipYoEntity entity)
        {
            // Logica specifica per configurare l'entità
            entity.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(entity);
        }
    }
}
