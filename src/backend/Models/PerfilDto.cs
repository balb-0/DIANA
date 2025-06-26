namespace backend.Models
{
    public class PerfilDto
    {
        public string Perfil { get; set; } = string.Empty;
        public DistribuicaoDto Distribuicao { get; set; } = new DistribuicaoDto();
        public double ValorTotal { get; set; }
        public DateTime DataCalculo { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string ClienteId { get; set; } = string.Empty;
    }
}
