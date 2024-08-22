using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CriarMovimento([FromBody] MovimentoCommand command)
        {
            try
            {
                var idMovimento = await _mediator.Send(command);
                return Ok(idMovimento);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { ex.Message, ex.Tipo });
            }
        }

        


    }
}
