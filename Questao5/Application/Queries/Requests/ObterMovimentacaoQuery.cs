using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Queries.Requests
{
    public class ObterMovimentacaoQuery : IRequest<Movimento>
    {
        public Guid Id { get; set; }
        public DateTime DataMovimento { get; set; }
        public ObterMovimentacaoQuery(Guid id ,  DateTime dataMovimento)
        {
            Id = id;
            DataMovimento = dataMovimento;
        }
    }
}
