using backend.Models.Entities;
using backend.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities
{
    [Table("historico_risco_declarado")]
    public class HistoricoRiscoDeclarado
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("risco")]
        public RiscoEnum Risco { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("clientes_id")]
        public string ClientesId { get; set; }

        // Navigation properties
        public virtual Cliente Cliente { get; set; }
    }
}
