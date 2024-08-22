using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Dto;
using Questao5.Infrastructure.Repositories;

namespace Questao5.Application.Handlers
{
    public class ObterSaldoQueryHandler : IRequestHandler<ObterSaldoQuery, SaldoDto>
    {
        private readonly IMovimentoRepository _repository;

        public ObterSaldoQueryHandler(IMovimentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<SaldoDto> Handle(ObterSaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _repository.GetContaConrrenteId(request.IdContaCorrente);

            if (conta == null)
                throw new BusinessException("Conta não encontrada", "INVALID_ACCOUNT");

            if (!conta.Ativo)
                throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");


            var saldo = await _repository.GetSaldoAtual(request.IdContaCorrente);

            return new SaldoDto
            {
                Numero = conta.Numero,
                Nome = conta.Nome,
                DataHoraConsulta = DateTime.Now,
                SaldoAtual = saldo.ToString("F2")
            };
        }
    }
}
