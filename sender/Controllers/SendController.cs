using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using sender.Commands;
using sender.Queries;
using sender.Services;

namespace sender.Controllers
{
    [Route("send")]
    [ApiController]
    public class SendController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SendController(IMessageService messageService, IReplyMessageService replyService, IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("przelew")]
        public async Task<ActionResult<string>> Przelew([FromBody] CreateTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? (ActionResult<string>) Ok(result) : BadRequest();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<string>> GetAccountByIdQuery(string id)
        {
            var query = new GetAccountByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? (ActionResult<string>) Ok(result) : NotFound();
        }
    }
}