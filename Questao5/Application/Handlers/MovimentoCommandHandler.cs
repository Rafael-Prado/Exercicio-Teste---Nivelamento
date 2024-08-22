using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Repositories;

namespace Questao5.Application.Handlers
{
    public class MovimentoCommandHandler : IRequestHandler<MovimentoCommand, string>
    {
        private readonly IMovimentoRepository _repository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentoCommandHandler(IMovimentoRepository repository, IIdempotenciaRepository idempotenciaRepository)
        {
            _repository = repository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<string> Handle(MovimentoCommand request, CancellationToken cancellationToken)
        {

            var conta = await _repository.GetContaConrrenteId(request.IdContaCorrente);
            //Validar negocio conta corrente.
            if (conta == null)
                throw new BusinessException("Conta não encontrada", "INVALID_ACCOUNT");

            if (!conta.Ativo)
                throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");

            if (request.Valor <= 0)
                throw new BusinessException("Valor inválido", "INVALID_VALUE");

            if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
                throw new BusinessException("Tipo de movimento inválido", "INVALID_TYPE");

            // Verifica idempotência
            var idempotencia = await _idempotenciaRepository.GetIdempotenciaIdRequisicao(request.IdRequisicao);

            if (idempotencia != null)
                return idempotencia.Resultado;

            var idMovimento = Guid.NewGuid().ToString();

            _repository.InserirMovimento(idMovimento, request);

            _idempotenciaRepository.InserirIdempotencia(idMovimento, request);
            return idMovimento;


        }
    }
}
