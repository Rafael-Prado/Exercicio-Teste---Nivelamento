using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries;
using Questao5.Domain.Entities;
using Questao5.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuestao5
{
    public class ConsultarSaldoHandlerTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly ConsultaSaldoContaCorrenteHandler _handler;

        public ConsultarSaldoHandlerTests()
        {
            _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            _movimentoRepository = Substitute.For<IMovimentoRepository>();
            _handler = new ConsultaSaldoContaCorrenteHandler(_contaCorrenteRepository, _movimentoRepository);
        }

        [Fact]
        public async Task ConsultarSaldo_DeveRetornarErro_QuandoContaInativa()
        {
            // Arrange
            var query = new ConsultaSaldoContaCorrenteQuery (Guid.NewGuid().ToString());
            _contaCorrenteRepository.ObterPorIdAsync(query.IdContaCorrente).Returns(new ContaCorrente { Ativo = false });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Conta inválida ou inativa.", result.ErrorMessage);
        }

        [Fact]
        public async Task ConsultarSaldo_DeveRetornarZero_QuandoSemMovimentacoes()
        {
            // Arrange
            var query = new ConsultaSaldoContaCorrenteQuery(Guid.NewGuid().ToString());
            _contaCorrenteRepository.ObterPorIdAsync(query.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });
            _movimentoRepository.ObterPorContaIdAsync(query.IdContaCorrente).Returns(new List<Movimento>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Saldo);
        }

        [Fact]
        public async Task ConsultarSaldo_DeveCalcularSaldoCorretamente()
        {
            // Arrange
            var query = new ConsultaSaldoContaCorrenteQuery("B6BAFC09-6967-ED11-A567-055DFA4A16C9"); 
            _contaCorrenteRepository.ObterPorIdAsync(query.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });

            _movimentoRepository.ObterPorContaIdAsync(query.IdContaCorrente).Returns(new List<Movimento>
        {
            new Movimento { TipoMovimento = "C", Valor = 100 },
            new Movimento { TipoMovimento = "D", Valor = 50 },
            new Movimento { TipoMovimento = "C", Valor = 25 }
        });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(75, result.Value.Saldo); // 100 - 50 + 25 = 75
        }
    }
}
