using System;
using System.Collections.Concurrent;

namespace sender.Services
{
    public class ReplyObserverHandler // : IObservable<int>
    {
        private readonly ConcurrentDictionary<string, IObserver<string>> observers;

        public ReplyObserverHandler()
        {
            observers = new ConcurrentDictionary<string, IObserver<string>>();
        }

        public void Unsubsribe(string guid)
        {
            observers.TryRemove(guid, out _);
        }

        public void Subscribe(string guid, IObserver<string> observer)
        {
            var p = "Subsribing observer: " + guid;
            if (!observers.ContainsKey(guid))
                observers[guid] = observer;
        }

        public void SetObserver(string guid, string val)
        {
            var p = "Setting observer for: " + guid;
            var observer = (ReplyObserver) observers[guid];
            observer.SetValue(val);
            Unsubsribe(guid);
        }
    }
}