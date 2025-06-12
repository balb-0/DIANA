using SomaConformeApi.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

            // Consulta real: conta clientes em conformidade por perfil
            var sql = @"
                SELECT d.risco AS perfil_investidor, COUNT(*) AS total_conformes
                FROM historico_risco_declarado d
                JOIN historico_risco_calculado c ON d.clientes_id = c.clientes_id
                WHERE d.risco = c.risco
                AND d.created_at = (
                    SELECT MAX(d2.created_at) FROM historico_risco_declarado d2 WHERE d2.clientes_id = d.clientes_id
                )
                AND c.created_at = (
                    SELECT MAX(c2.created_at) FROM historico_risco_calculado c2 WHERE c2.clientes_id = c.clientes_id
                )
                GROUP BY d.risco
            ";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            // Inicializa todos os perfis com zero
            var perfis = new[] { "Conservador", "Moderado", "Sofisticado" };
            var dict = perfis.ToDictionary(p => p, p => 0);

            while (await reader.ReadAsync())
            {
                var perfil = reader.GetString(0);
                var total = reader.GetInt32(1);
                dict[perfil] = total;
            }

            foreach (var perfil in perfis)
            {
                result.Add(new SomaConformeCategoriaDto
                {
                    PerfilInvestidor = perfil,
                    TotalConformes = dict[perfil]
                });
            }

            return result;
        }
    }
}
