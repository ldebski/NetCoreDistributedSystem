using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sender.Services
{
    public class ReplyObserver : IObserver<String>
    {
        private IDisposable unsubscriber;
        public String accountMoney { get; set; }

        public ReplyObserver() { accountMoney = ""; }

        public virtual void Subscribe(IObservable<String> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
        public virtual void OnCompleted()
        {
            // do nothing
        }
        public virtual void OnError(Exception error)
        {
            // do nothing
        }

        public virtual void OnNext(String value)
        {
            accountMoney = value;
        }

        public void SetValue(String val)
        {
            accountMoney = val;
        }
        
        public String GetValue()
        {
            return accountMoney;
        }

        public async Task<String> WaitForReply()
        {
            await Task.Run(async () =>
             {
                 while (accountMoney.Equals("")) { await Task.Delay(25);
                 }
             });
            return accountMoney;
        }
    }
}
