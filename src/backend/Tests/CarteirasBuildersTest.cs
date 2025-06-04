using backend.Models.DTOs;
using backend.Models.Enums;

namespace backend.Tests.Builders
{
    public class CarteiraDtoBuilder
    {
        private CarteiraDto _carteira = new CarteiraDto();

        public CarteiraDtoBuilder WithId(string id)
        {
            _carteira.ClienteId = id;
            return this;
        }

        public CarteiraDtoBuilder WithNome(string nome)
        {
            _carteira.ClienteNome = nome;
            return this;
        }

        public CarteiraDtoBuilder WithRisco(RiscoEnum risco)
        {
            _carteira.RiscoCalculado = risco;
            return this;
        }

        public CarteiraDtoBuilder WithRiscoDeclarado(RiscoEnum risco)
        {
            _carteira.RiscoDeclarado = risco;
            return this;
        }

        public CarteiraDtoBuilder WithCpf(string cpf)
        {
            _carteira.ClienteCpf = cpf;
            return this;
        }

        public CarteiraDtoBuilder WithAssessor(string nome, string unidade)
        {
            _carteira.AssessorNome = nome;
            _carteira.AssessorUnidade = unidade;
            return this;
        }

        public CarteiraDtoBuilder WithPercentuais(double? rv, double? rf)
        {
            _carteira.RVPercentual = rv;
            _carteira.RFPercentual = rf;
            return this;
        }

        public CarteiraDtoBuilder WithValores(double valorCalculado, double valorCarteira)
        {
            _carteira.ValorTotalCalculado = valorCalculado;
            _carteira.ValorTotalCarteira = valorCarteira;
            return this;
        }

        public CarteiraDtoBuilder WithInvestimentos(List<InvestimentoDto> investimentos)
        {
            _carteira.Investimentos = investimentos;
            return this;
        }

        public CarteiraDto Build() => _carteira;
    }

    public class InvestimentoDtoBuilder
    {
        private InvestimentoDto _investimento = new InvestimentoDto();

        public InvestimentoDtoBuilder WithId(string id)
        {
            _investimento.InvestimentoId = id;
            return this;
        }

        public InvestimentoDtoBuilder WithAtivo(string nome)
        {
            _investimento.AtivoNome = nome;
            return this;
        }

        public InvestimentoDtoBuilder WithValor(decimal valor)
        {
            _investimento.ValorTotal = valor;
            return this;
        }

        public InvestimentoDto Build() => _investimento;
    }
}