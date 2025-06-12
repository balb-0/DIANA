namespace backend.Models
{
    public class Investment
    {
        public string Id { get; set; } = string.Empty;
        public string AtivosId { get; set; } = string.Empty;
        public string ClientesId { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }
        public DateTime Data { get; set; }
        
        // Navigation property
        public Asset? Ativo { get; set; }
    }

    public class Asset
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Subgrupo { get; set; } = string.Empty;
    }
} 