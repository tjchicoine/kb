using System;
using nwKeyboardLock;

namespace KeyboardLock
{
    public class CallbackHandler : IKeyboardCallback
    {
        public void ret(string msg)
        {
            Console.WriteLine("Your Message Was : " + msg);
        }
    }
}



