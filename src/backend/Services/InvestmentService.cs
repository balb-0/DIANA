using backend.Models;
using Npgsql;

namespace backend.Services
{
    public class InvestmentService : IInvestmentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<InvestmentService> _logger;
        private readonly string _connectionString;

        public InvestmentService(IConfiguration configuration, ILogger<InvestmentService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<IEnumerable<Investment>> GetInvestmentsByClientIdAsync(string clientId)
        {
            var investments = new List<Investment>();

            try
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"
                    SELECT 
                        i.id, 
                        i.quantidade, 
                        i.valorTotal, 
                        i.data,
                        a.id as ativo_id,
                        a.nome as ativo_nome,
                        a.tipo as ativo_tipo,
                        a.subgrupo as ativo_subgrupo
                    FROM investimento i
                    JOIN ativos a ON i.ativos_id = a.id
                    WHERE i.clientes_id = @clientId";

                await using var cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("clientId", clientId);

                await using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    investments.Add(new Investment
                    {
                        Id = reader.GetString(0),
                        Quantidade = reader.GetInt32(1),
                        ValorTotal = reader.GetDouble(2),
                        Data = reader.GetDateTime(3),
                        Ativo = new Asset
                        {
                            Id = reader.GetString(4),
                            Nome = reader.GetString(5),
                            Tipo = reader.GetString(6),
                            Subgrupo = reader.GetString(7)
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching investments for client {ClientId}", clientId);
                throw;
            }

            return investments;
        }
    }
}