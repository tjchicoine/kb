using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.ServiceModel.Channels;


namespace nwKeyboardLock
{
    [ServiceContract(SessionMode=SessionMode.Required,CallbackContract =typeof(IKeyboardCallback))]
    public interface IKeyboard
    {
        [OperationContract(IsOneWay = true)]
        void IO(string command);
    }
    public interface IKeyboardCallback
    {
        [OperationContract(IsOneWay =true)]
        void ret(string answer);
    }

    [ServiceContract]
    public interface IMyHelpPageContract
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message Help();
    }
}
