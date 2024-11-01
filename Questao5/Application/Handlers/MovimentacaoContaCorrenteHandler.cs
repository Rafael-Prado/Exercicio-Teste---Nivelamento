using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoContaCorrenteHandler : IRequestHandler<MovimentacaoContaCorrenteCommand, Result<string>>
    {
        private readonly IContaCorrenteRepository _contaRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentacaoContaCorrenteHandler(IContaCorrenteRepository contaRepository,
                                                IMovimentoRepository movimentoRepository,
                                                IIdempotenciaRepository idempotenciaRepository)
        {
            _contaRepository = contaRepository;
            _movimentoRepository = movimentoRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<Result<string>> Handle(MovimentacaoContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            if (!await _idempotenciaRepository.VerificarIdempotenciaAsync(request.ChaveIdempotencia))
            {
                
                if (request.Valor <= 0)
                    return Result<string>.Failure("Valor do movimento deve ser positivo.");

                if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                    return Result<string>.Failure("Tipo de movimento inválido.");

                var conta = await _contaRepository.ObterPorIdAsync(request.IdContaCorrente);
                if (conta == null || !conta.Ativo)
                    return Result<string>.Failure("Conta inválida ou inativa.");                

                

                var movimento = new Movimento
                {
                    IdMovimento = Guid.NewGuid().ToString(),
                    IdContaCorrente = request.IdContaCorrente,
                    DataMovimento = DateTime.UtcNow,
                    TipoMovimento = request.TipoMovimento,
                    Valor = request.Valor
                };
                await _movimentoRepository.InserirAsync(movimento);
                await _idempotenciaRepository.RegistrarIdempotenciaAsync(request.ChaveIdempotencia, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(movimento.IdMovimento));

                return Result<string>.Success(movimento.IdMovimento);
            }

            return Result<string>.Failure("Requisição duplicada.");
        }
    }

}
