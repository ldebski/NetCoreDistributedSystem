using System;
using System.Threading.Tasks;

namespace sender.Services
{
    public class ReplyObserver : IObserver<string>
    {
        private IDisposable _unsubscriber;

        public ReplyObserver()
        {
            accountMoney = "";
        }

        public string accountMoney { get; set; }

        public virtual void OnCompleted()
        {
            // do nothing
        }

        public virtual void OnError(Exception error)
        {
            // do nothing
        }

        public virtual void OnNext(string value)
        {
            accountMoney = value;
        }

        public virtual void Subscribe(IObservable<string> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }

        public void SetValue(string val)
        {
            accountMoney = val;
        }

        public string GetValue()
        {
            return accountMoney;
        }

        public async Task<string> WaitForReply()
        {
            await Task.Run(async () =>
            {
                while (accountMoney.Equals("")) await Task.Delay(25);
            });
            return accountMoney;
        }
    }
}