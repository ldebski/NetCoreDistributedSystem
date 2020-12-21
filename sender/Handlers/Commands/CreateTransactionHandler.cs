using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using sender.Commands;
using sender.Services;

namespace sender.Handlers.Commands
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, string>
    {
        private readonly IMessageService _messageService;
        private readonly IReplyMessageService _replyService;

        public CreateTransactionHandler(IMessageService messageService, IReplyMessageService replyService)
        {
            _messageService = messageService;
            _replyService = replyService;
        }

        public async Task<string> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var guid = Guid.NewGuid();
            var message = guid + "." + request.FromId + "." + request.ToId + "." + request.Amount;
            var observer = new ReplyObserver();
            _replyService.addObserver(guid.ToString(), observer);
            _messageService.Enqueue(message, "przelew");
            var reply = await observer.WaitForReply();
            return reply;
        }
    }
}