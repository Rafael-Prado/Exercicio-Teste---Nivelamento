using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Repositories
{
    public interface IIdempotenciaRepository
    {
        Task<Idempotencia> GetIdempotenciaIdRequisicao(string idRequisicao);
        void InserirIdempotencia(string idMovimento, MovimentoCommand request);
    }
}
