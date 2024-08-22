using MediatR;
using Questao5.Domain.Dto;

namespace Questao5.Application.Queries.Responses
{
    public class ObterSaldoQuery: IRequest<SaldoDto>
    {
        public string IdContaCorrente { get; set; }
    }
}
