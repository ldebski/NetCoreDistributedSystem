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
        //private readonly IMessageService _messageService;
        //private readonly IReplyMessageService _replyService;
        // public SendController(IMessageService messageService, IReplyMessageService replyService)
        // {
        //     _messageService = messageService;
        //     _replyService = replyService;
        // }

        private LoggingService _loggingService;

        public SendController()
        {
            _loggingService = new LoggingService();
        }

        [HttpGet("przelew/{from}/{to}/{amount}")]
        public async Task<ActionResult<string>> Przelew(string from, string to, string amount)
        {
            // Console.WriteLine("Got message");
            // Guid guid = Guid.NewGuid();
            // string message = guid.ToString() + "." + from + "." + to + "." + amount;
            // ReplyObserver observer = new ReplyObserver();
            // _replyService.addObserver(guid.ToString(), observer);
            // _messageService.Enqueue(message, "przelew"); // wysyla do serwerow
            // var reply = await observer.WaitForReply(); // czekam na odp z serwerow
            // // Console.WriteLine("Got reply: " + reply);
            // return reply.ToString();
            return "123";
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<string>> Get(string id)
        {
            Guid guid = Guid.NewGuid();
            string message = guid.ToString() + "." + id;
            //_messageService.Enqueue(message, "get");
            //ReplyObserver observer = new ReplyObserver();
            //_replyService.addObserver(guid.ToString(), observer);
            //var reply = await observer.WaitForReply();
            //return reply.ToString();
            // string reply = _replyService.GetFromDictionary(guid);
            // return reply;

            _loggingService.toFile("Info 1");
            _loggingService.toFile("Info 2");
            _loggingService.toFile("Info 3");

            return "1";
        }
    }
}
