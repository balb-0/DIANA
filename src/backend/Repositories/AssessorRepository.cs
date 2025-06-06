using backend.Models;
using Dapper;
using Npgsql;

namespace backend.Repositories
{
    public interface IAssessorRepository
    {
        Task<bool> CreateAsync(Assessor assessor);
        Task<Assessor?> GetByIdAsync(string id);
        Task<bool> ExistsAsync(string id);
    }

    public class AssessorRepository : IAssessorRepository
    {
        private readonly NpgsqlDataSource _dataSource;
        private readonly ILogger<AssessorRepository> _logger;

        // Construtor com NpgsqlDataSource (mais moderno)
        public AssessorRepository(NpgsqlDataSource dataSource, ILogger<AssessorRepository> logger)
        {
            _dataSource = dataSource;
            _logger = logger;
        }

        // Construtor alternativo com IConfiguration (caso prefira)
        public AssessorRepository(IConfiguration configuration, ILogger<AssessorRepository> logger)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentException("Connection string 'DefaultConnection' not found");
            _dataSource = NpgsqlDataSource.Create(connectionString);
            _logger = logger;
        }

        public async Task<bool> CreateAsync(Assessor assessor)
        {
            try
            {
                using var connection = await _dataSource.OpenConnectionAsync();

                const string sql = @"
                    INSERT INTO assessores (id, nome, unidade_btg) 
                    VALUES (@Id, @Nome, @UnidadeBtg)
                    ON CONFLICT (id) DO NOTHING";

                var parameters = new
                {
                    Id = assessor.Id,
                    Nome = assessor.Nome,
                    UnidadeBtg = assessor.UnidadeBtg
                };

                var rowsAffected = await connection.ExecuteAsync(sql, parameters);

                if (rowsAffected == 0)
                {
                    _logger.LogWarning("Assessor com ID {Id} já existe no banco", assessor.Id);
                    return false;
                }

                _logger.LogInformation("Assessor criado com sucesso: {Id} - {Nome}", assessor.Id, assessor.Nome);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar assessor no banco de dados. ID: {Id}, Nome: {Nome}",
                    assessor.Id, assessor.Nome);
                return false;
            }
        }

        public async Task<Assessor?> GetByIdAsync(string id)
        {
            try
            {
                using var connection = await _dataSource.OpenConnectionAsync();

                const string sql = @"
                    SELECT id as Id, nome as Nome, unidade_btg as UnidadeBtg 
                    FROM assessores 
                    WHERE id = @Id";

                var assessor = await connection.QuerySingleOrDefaultAsync<Assessor>(sql, new { Id = id });

                if (assessor != null)
                {
                    _logger.LogDebug("Assessor encontrado: {Id} - {Nome}", assessor.Id, assessor.Nome);
                }

                return assessor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar assessor por ID: {Id}", id);
                return null;
            }
        }

        public async Task<bool> ExistsAsync(string id)
        {
            try
            {
                using var connection = await _dataSource.OpenConnectionAsync();

                const string sql = "SELECT EXISTS(SELECT 1 FROM assessores WHERE id = @Id)";

                return await connection.QuerySingleAsync<bool>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar se assessor existe: {Id}", id);
                return false;
            }
        }
    }
}