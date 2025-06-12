using Npgsql;

namespace backend.Repositories
{
    public class ComplianceRepository : IComplianceRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ComplianceRepository> _logger;

        public ComplianceRepository(IConfiguration configuration, ILogger<ComplianceRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<int> GetConformingClientsCountAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT obter_quantidade_clientes_conformes()", connection);
                var result = await command.ExecuteScalarAsync();

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving conforming clients count from database");
                throw;
            }
        }

        public async Task<int> GetNonConformingClientsCountAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT obter_quantidade_clientes_inconformes()", connection);
                var result = await command.ExecuteScalarAsync();

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving non-conforming clients count from database");
                throw;
            }
        }
    }
}
