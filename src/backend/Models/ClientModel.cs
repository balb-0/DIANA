namespace backend.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Segmento { get; set; } = "";
        public string PerfilRisco { get; set; } = "";
    }
}