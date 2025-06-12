using backend.Models.Entities;
using backend.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities
{
    [Table("historico_risco_calculado")]
    public class HistoricoRiscoCalculado
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("risco")]
        public RiscoEnum Risco { get; set; }

        [Column("rv_percentual")]
        public double RVPercentual { get; set; }

        [Column("rf_percentual")]
        public double RFPercentual { get; set; }

        [Column("valortotal")]
        public double ValorTotal { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("clientes_id")]
        public string ClientesId { get; set; }

        // Navigation properties
        public virtual Cliente Cliente { get; set; }
    }
}