using backend.Models;

public interface IInvestimentoRepository
{
    List<Investimento> ObterPorClienteId(string clienteId);
}
