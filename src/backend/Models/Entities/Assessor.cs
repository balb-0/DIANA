using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities
{
    [Table("assessores")]
    public class Assessor
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("unidade_btg")]
        public string UnidadeBtg { get; set; }

        // Navigation properties
        public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}