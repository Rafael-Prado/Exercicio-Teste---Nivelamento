using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Commands
{
    public class MovimentacaoContaCorrenteCommand : IRequest<Result<String>>
    {
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; } // "C" ou "D"
        public Guid ChaveIdempotencia { get; set; }
    }
}
