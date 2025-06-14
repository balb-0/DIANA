using backend.Models;
using Npgsql;

namespace backend.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly string _connectionString;

        public PerfilService(string connectionString)
        {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public PerfilDto ObterPerfilDistribuicao(string clienteId)
        {
            var perfil = ObterPerfilDeRisco(clienteId);
            var distribuicao = ObterDistribuicao(clienteId);

            return new PerfilDto
            {
                Perfil = perfil,
                Distribuicao = distribuicao,
            };
        }

        private string ObterPerfilDeRisco(string id)
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

            return result?.ToString() ?? "Desconhecido";
        }

        private DistribuicaoDto ObterDistribuicao(string clienteId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                SELECT tipo, SUM(valor) as total
                FROM investimentos
                WHERE clientes_id = @id
                GROUP BY tipo;
            ", conn);

            cmd.Parameters.AddWithValue("id", clienteId);
            using var reader = cmd.ExecuteReader();

            double rendaFixa = 0, rendaVariavel = 0;
            while (reader.Read())
            {
                var tipo = reader.GetString(0);
                var valor = reader.GetDouble(1);

                if (tipo == "RendaFixa") rendaFixa += valor;
                else if (tipo == "RendaVariavel") rendaVariavel += valor;
            }

            return new DistribuicaoDto
            {
                RendaFixa = rendaFixa,
                RendaVariavel = rendaVariavel
            };
        }
    }
}
