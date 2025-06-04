using backend.Models.Enums;

namespace backend.Models.DTOs
{
    public class CarteiraDto
    {
        public string ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteCpf { get; set; }
        public RiscoEnum RiscoDeclarado { get; set; }
        public RiscoEnum? RiscoCalculado { get; set; }
        public double? RVPercentual { get; set; }
        public double? RFPercentual { get; set; }
        public double? ValorTotalCalculado { get; set; }
        public List<InvestimentoDto> Investimentos { get; set; } = new List<InvestimentoDto>();
        public double ValorTotalCarteira { get; set; }
        public string AssessorNome { get; set; }
        public string AssessorUnidade { get; set; }
    }
}