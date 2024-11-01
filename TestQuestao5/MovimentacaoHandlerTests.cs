using NSubstitute;
using Questao5.Application.Commands;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;

namespace TestQuestao5
{
    public class MovimentacaoHandlerTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly MovimentacaoContaCorrenteHandler _handler;

        public MovimentacaoHandlerTests()
        {
            _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            _movimentoRepository = Substitute.For<IMovimentoRepository>();
            _idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            _handler = new MovimentacaoContaCorrenteHandler(_contaCorrenteRepository, _movimentoRepository, _idempotenciaRepository);
        }

        [Fact]
        public async Task Movimentacao_DeveRetornarErro_QuandoContaInativa()
        {
            // Arrange
            var command = new MovimentacaoContaCorrenteCommand { IdContaCorrente = Guid.NewGuid().ToString(), Valor = 100, TipoMovimento = "C" };
            var conta = await _contaCorrenteRepository.ObterPorIdAsync(command.IdContaCorrente);
            _contaCorrenteRepository.ObterPorIdAsync(conta?.IdContaCorrente).Returns(new ContaCorrente { Ativo = false });
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Conta inválida ou inativa.", result.ErrorMessage);
        }

        [Fact]
        public async Task Movimentacao_DeveRetornarErro_QuandoValorNegativo()
        {
            // Arrange
            var command = new MovimentacaoContaCorrenteCommand { IdContaCorrente = "f1b2db69-2808-42be-8263-97a3f0fca055", Valor = -50, TipoMovimento = "C" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Valor do movimento deve ser positivo.", result.ErrorMessage);
        }

        [Fact]
        public async Task Movimentacao_DeveRetornarErro_QuandoTipoMovimentoInvalido()
        {
            // Arrange
            var command = new MovimentacaoContaCorrenteCommand { IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9", Valor = 50, TipoMovimento = "x" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Tipo de movimento inválido.", result.ErrorMessage);
        }

        [Fact]
        public async Task Movimentacao_DeveRetornarErro_QuandoRequisicaoDuplicada()
        {
            // Arrange
            // Arrange
            var command = new MovimentacaoContaCorrenteCommand { IdContaCorrente = "f1b2db69-2808-42be-8263-97a3f0fca055", Valor = -50, TipoMovimento = "C", ChaveIdempotencia = Guid.NewGuid() };
           
            _idempotenciaRepository.VerificarIdempotenciaAsync(command.ChaveIdempotencia).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Requisição duplicada.", result.ErrorMessage);
        }
    }
}