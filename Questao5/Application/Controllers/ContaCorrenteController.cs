using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Controllers
{
    [ApiController]
    [Route("api/contacorrente")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza uma movimentação em uma conta corrente.
        /// </summary>
        /// <param name="command">Dados da movimentação da conta corrente.</param>
        /// <returns>Id do movimento gerado ou mensagem de erro.</returns>
        [HttpPost("movimentar")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Movimentar([FromBody] MovimentacaoContaCorrenteCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new ErroResponse(result.ErrorMessage));
        }

        /// <summary>
        /// Consulta o saldo de uma conta corrente.
        /// </summary>
        /// <param name="idContaCorrente">Id da conta corrente a ser consultada.</param>
        /// <returns>Informações da conta e saldo atual.</returns>
        [HttpGet("saldo")]
        [ProducesResponseType(typeof(SaldoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConsultarSaldo(string idcontaCorrente)
        {
            var query = new ConsultaSaldoContaCorrenteQuery(idcontaCorrente);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new ErroResponse(result.ErrorMessage));
        }
    }
}
