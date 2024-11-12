namespace ShipYo.Core.Entities
{
    public abstract class BaseEntity
    {
        // Identificatore univoco
        public int Id { get; set; }

        // Timestamp di creazione e aggiornamento
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        // Tracciamento degli utenti
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        // Token di concorrenza
        public string? ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
    }
}
