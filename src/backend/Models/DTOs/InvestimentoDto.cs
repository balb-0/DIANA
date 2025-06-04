using backend.Models.Enums;

namespace bakend.Models.DTOs
{
    public class InvestimentoDto
    {
        public string AtivoId { get; set; }
        public string AtivoNome { get; set; }
        public string AtivoTipo { get; set; }
        public string AtivoSubgrupo { get; set; }
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataInvestimento { get; set; }
    }
}