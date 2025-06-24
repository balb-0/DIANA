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
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM obter_perfil_risco_cliente(@cliente_id)", conn);
            cmd.Parameters.AddWithValue("cliente_id", clienteId);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new PerfilDto
                {
                    Perfil = reader.GetString(reader.GetOrdinal("perfil")),
                    Distribuicao = new DistribuicaoDto
                    {
                        RendaFixa = reader.GetDouble(reader.GetOrdinal("percentual_renda_fixa")),
                        RendaVariavel = reader.GetDouble(reader.GetOrdinal("percentual_renda_variavel"))
                    },
                    ValorTotal = reader.GetDouble(reader.GetOrdinal("valor_total")),
                    DataCalculo = reader.GetDateTime(reader.GetOrdinal("data_calculo")),
                    NomeCliente = reader.GetString(reader.GetOrdinal("nome_cliente")),
                    ClienteId = reader.GetString(reader.GetOrdinal("id_cliente"))
                };
            }
            else
            {
                return null;
            }
        }
    }
}
