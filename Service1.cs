using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;


namespace KeyboardLock
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class KeyBoardService : IKeyboard
    {
        public bool IO(bool command)
        {
            if (command == true)
            {
                return true;
            }
            else return false;
        }
    }
}
