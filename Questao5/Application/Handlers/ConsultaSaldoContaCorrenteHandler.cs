using MediatR;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Dtos;
using Questao5.Domain.Repository;

namespace Questao5.Application.Handlers
{
    public class ConsultaSaldoContaCorrenteHandler : IRequestHandler<ConsultaSaldoContaCorrenteQuery, Result<SaldoContaCorrenteDto>>
    {
        private readonly IContaCorrenteRepository _contaRepository;
        private readonly IMovimentoRepository _movimentoRepository;

        public ConsultaSaldoContaCorrenteHandler(IContaCorrenteRepository contaRepository,
                                                 IMovimentoRepository movimentoRepository)
        {
            _contaRepository = contaRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<Result<SaldoContaCorrenteDto>> Handle(ConsultaSaldoContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.ObterPorIdAsync(request.IdContaCorrente);
            if (conta == null || !conta.Ativo)
                return Result<SaldoContaCorrenteDto>.Failure("Conta inválida ou inativa.");

            var movimentos = await _movimentoRepository.ObterPorContaIdAsync(request.IdContaCorrente);
            var saldo = movimentos.Where(m => m.TipoMovimento.ToString() == "C").Sum(m => m.Valor) - movimentos.Where(m => m.TipoMovimento.ToString() == "D").Sum(m => m.Valor);

            return Result<SaldoContaCorrenteDto>.Success(new SaldoContaCorrenteDto
            {
                Numero = conta.Numero,
                Nome = conta.Nome,
                DataConsulta = DateTime.UtcNow,
                Saldo = saldo
            });
        }
    }

}
