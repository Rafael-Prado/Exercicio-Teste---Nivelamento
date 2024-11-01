using Questao5.Domain.Entities;

namespace Questao5.Domain.Repository
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> ObterPorIdAsync(string id);
        Task<ContaCorrente> ObterPorNumeroAsync(int numero);
    }
}
