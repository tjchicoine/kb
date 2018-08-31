using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardLockHost;
using System.ServiceModel;

namespace KeyboardLock
{
    internal class KeyboardlockClient : DuplexClientBase<IKeyboard>, IKeyboard  
    {
        public KeyboardlockClient(InstanceContext callbackInstance, WSDualHttpBinding binding, EndpointAddress endpointAddress) 
            : base(callbackInstance, binding, endpointAddress) { }

        public void Connect()
        {
            Channel.Connect();
        }
        public void AddItem(string item)
        {
            Channel.AddItem(item);
        }
        public void Disconnect()
        {
            Channel.Disconnect();
        }
    }
}
