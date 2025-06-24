using backend.Models.DTOs;
using System.Data;
using Dapper;

namespace backend.Repositories
{
    public class CarteiraRepository : ICarteiraRepository
    {
        private readonly IDbConnection _connection;

        public CarteiraRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<ClienteSimplificadoDto>> GetClientesByRiscoDeclaradoAsync(string tipoRisco)
        {
            string sqlBase = @"
                SELECT DISTINCT
                    c.id as ClienteId,
                    c.nome as ClienteNome
                FROM clientes c
                INNER JOIN historico_risco_declarado hrd ON c.id = hrd.clientes_id
                INNER JOIN (
                    SELECT clientes_id, MAX(created_at) as max_created_at
                    FROM historico_risco_declarado
                    GROUP BY clientes_id
                ) latest_declared_risk ON hrd.clientes_id = latest_declared_risk.clientes_id 
                                      AND hrd.created_at = latest_declared_risk.max_created_at
                WHERE c.is_active = true";

            // Declarar parametros como object para permitir diferentes tipos anônimos
            object parametros = null;

            if (!string.Equals(tipoRisco, "geral", StringComparison.OrdinalIgnoreCase))
            {
                // Usar UPPER para garantir compatibilidade com o ENUM
                sqlBase += " AND UPPER(hrd.risco::text) = UPPER(@TipoRisco)";
                parametros = new { TipoRisco = tipoRisco };
            }

            sqlBase += " ORDER BY c.nome";

            var clientes = await _connection.QueryAsync<ClienteSimplificadoDto>(sqlBase, parametros);
            return clientes.ToList();
        }
    }
}