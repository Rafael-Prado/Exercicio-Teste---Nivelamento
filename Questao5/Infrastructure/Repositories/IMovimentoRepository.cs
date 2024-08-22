using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Repositories
{
    public interface IMovimentoRepository
    {
        Task<ContaCorrente> GetContaConrrenteId(string idContaCorrente);
        void InserirMovimento(string idMovimento, MovimentoCommand request);
        Task<decimal> GetSaldoAtual(string idContaCorrente);
    }
}
