using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sender.Services
{
    public class ReplyObserverHandler // : IObservable<int>
    {
        ConcurrentDictionary<String, IObserver<String>> observers;
        // List<IObserver<int>> observers;

        public ReplyObserverHandler()
        {
            observers = new ConcurrentDictionary<String, IObserver<String>>();
        }

        public void Unsubsribe(String guid)
        {
            observers.TryRemove(guid, out _);
        }

        public void Subscribe(String guid, IObserver<String> observer)
        {
            String p = "Subsribing observer: " + guid;
            if (!observers.ContainsKey(guid))
                observers[guid] = observer;
        }

        public void SetObserver(String guid, String val)
        {
            String p = "Setting observer for: " + guid;
            ReplyObserver observer = (ReplyObserver) observers[guid];
            observer.SetValue(val);
            Unsubsribe(guid);
        }
    }
}