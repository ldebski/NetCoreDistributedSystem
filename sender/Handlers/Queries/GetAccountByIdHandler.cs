using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using sender.Queries;
using sender.Services;

namespace sender.Handlers.Queries
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, string>
    {
        
        private readonly IMessageService _messageService;
        private readonly IReplyMessageService _replyService;

        public GetAccountByIdHandler(IMessageService messageService, IReplyMessageService replyService)
        {
            _messageService = messageService;
            _replyService = replyService;
        }

        public async Task<string> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var guid = Guid.NewGuid();
            var message = guid + "." + request.AccountId;
            _messageService.Enqueue(message, "get");
            var observer = new ReplyObserver();
            _replyService.addObserver(guid.ToString(), observer);
            var reply = await observer.WaitForReply();
            return reply;
        }
    }
}