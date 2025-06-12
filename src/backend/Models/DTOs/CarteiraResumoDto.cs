using backend.Models.Enums;

namespace backend.Models.DTOs
{
    public class CarteiraResumoDto
    {
        public RiscoEnum TipoRisco { get; set; }
        public int QuantidadeCarteiras { get; set; }
        public double ValorTotalGeral { get; set; }
        public double ValorMedio { get; set; }
        public double ValorMinimo { get; set; }
        public double ValorMaximo { get; set; }
    }
}