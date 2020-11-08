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
        public SendController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("przelew/{from}/{to}/{amount}")]
        public ActionResult<string> Przelew(string from, string to, string amount)
        {
            string message = from + "." + to + "." + amount;
            _messageService.Enqueue(message, "przelew");
            return message;
        }

        [HttpGet("get/{id}")]
        public ActionResult<string> Get(string id)
        {
            _messageService.Enqueue(id, "get");
            return id;
        }
    }
}
