using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.ServiceModel.Channels;


namespace KeyboardLockHost
{
    [ServiceContract(SessionMode=SessionMode.Required,CallbackContract =typeof(IKeyboardCallback))]
    public interface IKeyboard
    {
        [OperationContract(IsOneWay = true)]
        void Connect();

        [OperationContract(IsOneWay = true)]
        void AddItem(string item);

        [OperationContract(IsOneWay = true)]
        void Disconnect();
    }
    public interface IKeyboardCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendUpdatedList(List<string> items);
    }

    [ServiceContract]
    public interface IMyHelpPageContract
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message Help();
    }
}
