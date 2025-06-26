using System.Data;
using Dapper;

namespace backend.Repositories
{
    public class ComplianceRepository : IComplianceRepository
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<ComplianceRepository> _logger;

        public ComplianceRepository(IDbConnection connection, ILogger<ComplianceRepository> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<int> GetConformingClientsCountAsync()
        {
            try
            {
                const string sql = "SELECT obter_quantidade_clientes_conformes()";
                
                var result = await _connection.QuerySingleAsync<int>(sql, commandTimeout: 120);
                
                return result;
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
                const string sql = "SELECT obter_quantidade_clientes_inconformes()";
                
                var result = await _connection.QuerySingleAsync<int>(sql, commandTimeout: 120);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving non-conforming clients count from database");
                throw;
            }
        }
    }
}