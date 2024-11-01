using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Application.Queries.Responses.ExemploRequestResponse
{
    public class SaldoResponseExample : IExamplesProvider<SaldoResponse>
    {
        public SaldoResponse GetExamples()
        {
            return new SaldoResponse
            {
                NumeroConta = 123,
                NomeTitular = "Katherine Sanchez",
                DataHoraConsulta = DateTime.Now,
                SaldoAtual = 2000.00m
            };

        }
    }
}
