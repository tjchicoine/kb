using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

namespace nwKeyboardLock
{
    [ServiceBehavior(ConfigurationName = "foo")]
    public class KeyBoardService : IKeyboard , IMyHelpPageContract
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
        public Message Help()
        {
            return new MyHelpPageMessage();
        }
    }


    abstract class ContentOnlyMessage : Message
    {
        MessageHeaders headers;
        MessageProperties properties;

        protected ContentOnlyMessage()
        {
            this.headers = new MessageHeaders(MessageVersion.None);
        }

        public override MessageHeaders Headers
        {
            get
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException("blah");
                }
                return this.headers;
            }
        }

        public override MessageProperties Properties
        {
            get
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException("blah");
                }
                if (this.properties == null)
                {
                    this.properties = new MessageProperties();

                }
                return this.properties;
            }
        }

        public override MessageVersion Version
        {
            get
            {
                return headers.MessageVersion;
            }
        }

        protected override void OnBodyToString(XmlDictionaryWriter writer)
        {
            OnWriteBodyContents(writer);
        }

    }
    class MyHelpPageMessage : ContentOnlyMessage
    {
        public MyHelpPageMessage() : base() { }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("HTML");
            writer.WriteStartElement("HEAD");
            writer.WriteRaw("");
            writer.WriteEndElement();
            writer.WriteRaw(@"future information implementiation");
            writer.WriteEndElement();
        }
    }

}
