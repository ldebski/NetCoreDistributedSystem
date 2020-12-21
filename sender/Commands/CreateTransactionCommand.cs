using MediatR;

namespace sender.Commands
{
    public class CreateTransactionCommand : IRequest<string>
    {
        public string FromId { get; set; }
        
        public string ToId { get; set; }
        
        public int Amount { get; set; }
    }
}