using backend.Models;

namespace backend.Services
{
    public interface IPerfilService
    {
        PerfilDto ObterPerfilDistribuicao(string clienteId);
    }
}
