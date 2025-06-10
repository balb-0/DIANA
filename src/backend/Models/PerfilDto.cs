namespace backend.Models
{
    public class PerfilDto
    {
        public string Perfil { get; set; } = string.Empty;
        public DistribuicaoDto Distribuicao { get; set; } = new DistribuicaoDto();
    }
}