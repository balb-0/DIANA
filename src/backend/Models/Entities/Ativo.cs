using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities
{
    [Table("ativos")]
    public class Ativo
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("subgrupo")]
        public string Subgrupo { get; set; }

        // Navigation properties
        public virtual ICollection<Investimento> Investimentos { get; set; } = new List<Investimento>();
    }
}