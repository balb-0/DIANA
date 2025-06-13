using backend.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities
{
    [Table("clientes")]
    public class Cliente
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("rg")]
        public string Rg { get; set; }

        [Column("endereco")]
        public string Endereco { get; set; }

        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Column("categoria")]
        public CategoriaEnum Categoria { get; set; }

        [Column("assessores_id")]
        public string? AssessoresId { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("inactive_datetime")]
        public DateTime? InactiveDatetime { get; set; }

        // Navigation properties
        public virtual Assessor? Assessor { get; set; }
        public virtual ICollection<Investimento> Investimentos { get; set; } = new List<Investimento>();
        public virtual ICollection<HistoricoRiscoDeclarado> HistoricoRiscoDeclarado { get; set; } = new List<HistoricoRiscoDeclarado>();
        public virtual ICollection<HistoricoRiscoCalculado> HistoricoRiscoCalculado { get; set; } = new List<HistoricoRiscoCalculado>();
    }
}