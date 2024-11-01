using Questao5.Domain.Entities;

namespace Questao5.Domain.Repository
{
    public interface IMovimentoRepository
    {
        Task<IEnumerable<Movimento>> ObterPorContaIdAsync(string contaId);
        Task InserirAsync(Movimento movimento);
    }
}
