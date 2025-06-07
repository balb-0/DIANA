using SomaConformeApi.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomaConformeApi.Services
{
    public class SomaConformeService : ISomaConformeService
    {
        private readonly IConfiguration _configuration;

        public SomaConformeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<SomaConformeCategoriaDto>> SomaConformePorCategoriaAsync()
        {
            var result = new List<SomaConformeCategoriaDto>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = @"
                -- Sua query SQL aqui, por exemplo:
                SELECT 'Conservador' AS perfil_investidor, 2 AS total_conformes
                UNION ALL
                SELECT 'Moderado', 1
            ";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new SomaConformeCategoriaDto
                {
                    PerfilInvestidor = reader.GetString(0),
                    TotalConformes = reader.GetInt32(1)
                });
            }

            return result;
        }
    }
}
