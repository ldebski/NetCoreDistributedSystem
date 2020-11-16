using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sender.Services;
using sender.Models;

namespace sender.Controllers
{
    [Route("send")]
    [ApiController]
    public class SendController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IReplyMessageService _replyService;
        public SendController(IMessageService messageService, IReplyMessageService replyService)
        {
            _messageService = messageService;
            _replyService = replyService;
        }

        [HttpGet("przelew/{from}/{to}/{amount}")]
        public ActionResult<string> Przelew(string from, string to, string amount)
        {
            Guid guid = Guid.NewGuid();
            string message = guid.ToString() + "." + from + "." + to + "." + amount;
            _messageService.Enqueue(message, "przelew");
            string reply = _replyService.GetFromDictionary(guid);
            return reply;
        }

        [HttpGet("get/{id}")]
        public ActionResult<string> Get(string id)
        {
            Guid guid = Guid.NewGuid();
            string message = guid.ToString() + "." + id;
            _messageService.Enqueue(message, "get");
            string reply = _replyService.GetFromDictionary(guid);
            return reply;
        }
    }
}
