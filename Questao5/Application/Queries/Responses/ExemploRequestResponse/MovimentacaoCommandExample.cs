using Questao5.Application.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Application.Queries.Responses.ExemploRequestResponse
{
    public class MovimentacaoCommandExample : IExamplesProvider<MovimentacaoContaCorrenteCommand>
    {
        public MovimentacaoContaCorrenteCommand GetExamples()
        {
            return new MovimentacaoContaCorrenteCommand
            {
                ChaveIdempotencia = Guid.NewGuid(),
                IdContaCorrente = Guid.NewGuid().ToString(),
                Valor = 100.00m,
                TipoMovimento = "C" // Crédito
            };
        }

    }
}
