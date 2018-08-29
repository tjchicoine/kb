using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace nwKeyboardLock
{
    public class KeyBoardService : IKeyboard
    {
        public bool IO(bool command)
        {
            if (command == true)
            {
                Console.WriteLine("Received Command");
                return true;
            }
            else return false;
        }
        public string msg(string msg)
        {
            Console.WriteLine(msg);
            return msg;
        }
    }
}
