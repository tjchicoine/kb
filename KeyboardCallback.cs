using System;
using KeyboardLockHost;
using System.ServiceModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;

namespace KeyboardLock
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    internal class KeyboardCallback : IKeyboardCallback
    {
        private SynchronizationContext _syncContext = AsyncOperationManager.SynchronizationContext;
        public event EventHandler<UpdatedListEventArgs> ServiceCallbackEvent;

        public void SendUpdatedList(List<string> items)
        {
            _syncContext.Post(new SendOrPostCallback(OnServiceCallbackEvent),
                new UpdatedListEventArgs(items));

            for (int i = 0; i < items.Count; i++) { Console.Write(items[i]+" "); }
            
        }

        private void OnServiceCallbackEvent(object state)
        {
            EventHandler<UpdatedListEventArgs> handler = ServiceCallbackEvent;
            UpdatedListEventArgs e = state as UpdatedListEventArgs;

            if(handler == null)
            {
                handler(this, e);
            }
        }
    }
}



