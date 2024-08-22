using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaldoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{idContaCorrente}")]
        public async Task<IActionResult> ObterSaldo(string idContaCorrente)
        {
            try
            {
                var saldo = await _mediator.Send(new ObterSaldoQuery { IdContaCorrente = idContaCorrente });
                return Ok(saldo);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { ex.Message, ex.Tipo });
            }
        }
    }
}
