using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

namespace backend.Services
{
    public class ClientService : IClientService
    {
        private readonly string _connectionString;

        public ClientService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string? GetPerfilDeRisco(string id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                SELECT risco
                FROM historico_risco_declarado
                WHERE clientes_id = @id
                ORDER BY created_at DESC
                LIMIT 1;
            ", conn);

            cmd.Parameters.AddWithValue("id", id);

            var result = cmd.ExecuteScalar();
            return result?.ToString();
        }
    }
}