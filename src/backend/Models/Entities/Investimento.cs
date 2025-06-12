using backend.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities
{
    [Table("investimento")]
    public class Investimento
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("ativos_id")]
        public string AtivosId { get; set; }

        [Column("clientes_id")]
        public string ClientesId { get; set; }

        [Column("quantidade")]
        public int Quantidade { get; set; }

        [Column("valortotal")]
        public double ValorTotal { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        // Navigation properties
        public virtual Ativo Ativo { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}