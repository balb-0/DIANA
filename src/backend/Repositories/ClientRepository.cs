using backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace backend.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(IConfiguration configuration, ILogger<ClientRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            var clients = new List<Client>();
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT id, nome, segmento, perfil_risco FROM clientes", connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    clients.Add(new Client
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Segmento = reader.GetString(2),
                        PerfilRisco = reader.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clients from database");
                throw;
            }
            return clients;
        }

        public async Task<IEnumerable<Client>> GetBySegmentAsync(string segment)
        {
            var clients = new List<Client>();
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT id, nome, segmento, perfil_risco FROM clientes WHERE segmento = @segment", connection);
                command.Parameters.AddWithValue("@segment", segment);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    clients.Add(new Client
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Segmento = reader.GetString(2),
                        PerfilRisco = reader.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clients by segment from database");
                throw;
            }
            return clients;
        }

        public async Task<IEnumerable<Client>> GetByRiskProfileAsync(string risk)
        {
            var clients = new List<Client>();
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT id, nome, segmento, perfil_risco FROM clientes WHERE perfil_risco = @risk", connection);
                command.Parameters.AddWithValue("@risk", risk);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    clients.Add(new Client
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Segmento = reader.GetString(2),
                        PerfilRisco = reader.GetString(3)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clients by risk from database");
                throw;
            }
            return clients;
        }
    }
}