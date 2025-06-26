namespace backend.Models
{
    public class Investimento
    {
        public string Tipo { get; set; } = string.Empty; // "RendaFixa" ou "RendaVariavel"
        public double Valor { get; set; }
    }
}
