using System;
using MediatR;

namespace sender.Queries
{
    public class GetAccountByIdQuery : IRequest<string>
    {
        public GetAccountByIdQuery(string accountId)
        {
            AccountId = accountId;
        }

        public string AccountId { get; }
    }
}