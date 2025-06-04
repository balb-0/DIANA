using bakend.Models.DTOs;
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

        public async Task<List<CarteiraDto>> GetCarteirasByRiscoAsync(string tipoRisco)
        {
            string sqlBase = @"
        SELECT DISTINCT
            c.id as ClienteId,
            c.nome as ClienteNome,
            c.cpf,
            c.categoria,
            a.nome as AssessorNome,
            a.unidade_btg as UnidadeBTG,
            hrc.risco as RiscoCalculado,
            hrc.rv_percentual as RVPercentual,
            hrc.rf_percentual as RFPercentual,
            hrc.valortotal as ValorTotalCarteira,
            hrc.created_at as DataCalculoRisco
        FROM clientes c
        INNER JOIN assessores a ON c.assessores_id = a.id
        INNER JOIN historico_risco_calculado hrc ON c.id = hrc.clientes_id
        INNER JOIN (
            SELECT clientes_id, MAX(created_at) as max_created_at
            FROM historico_risco_calculado
            GROUP BY clientes_id
        ) latest_risk ON hrc.clientes_id = latest_risk.clientes_id 
                      AND hrc.created_at = latest_risk.max_created_at
        WHERE c.is_active = true
    ";

            if (!string.Equals(tipoRisco, "geral", StringComparison.OrdinalIgnoreCase))
            {
                sqlBase += " AND LOWER(hrc.risco) = LOWER(@TipoRisco)";
            }

            sqlBase += " ORDER BY c.nome";

            var carteiras = await _connection.QueryAsync<CarteiraDto>(sqlBase,
                string.Equals(tipoRisco, "geral", StringComparison.OrdinalIgnoreCase)
                    ? null
                    : new { TipoRisco = tipoRisco });

            // Buscar investimentos para cada carteira
            foreach (var carteira in carteiras)
            {
                carteira.Investimentos = await GetInvestimentosByClienteAsync(carteira.ClienteId);
            }

            return carteiras.ToList();
        }


        private async Task<List<InvestimentoDto>> GetInvestimentosByClienteAsync(string clienteId)
        {
            var sql = @"
            SELECT 
                i.id as InvestimentoId,
                i.quantidade,
                i.valortotal as ValorTotal,
                i.data as DataInvestimento,
                at.nome as AtivoNome,
                at.tipo as AtivoTipo,
                at.subgrupo as AtivoSubgrupo
            FROM investimento i
            INNER JOIN ativos at ON i.ativos_id = at.id
            WHERE i.clientes_id = @ClienteId
            ORDER BY i.data DESC";

            var investimentos = await _connection.QueryAsync<InvestimentoDto>(sql, new { ClienteId = clienteId });
            return investimentos.ToList();
        }
    }
}