using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Dtos;

namespace Questao5.Application.Queries
{
    public class ConsultaSaldoContaCorrenteQuery : IRequest<Result<SaldoContaCorrenteDto>>
    {
        public string IdContaCorrente { get; set; }

        public ConsultaSaldoContaCorrenteQuery(string idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }
    }
}
