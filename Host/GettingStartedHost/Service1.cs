using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Xml;
using HtmlAgilityPack;

namespace nwKeyboardLock
{
    [ServiceBehavior(ConfigurationName ="foo",AddressFilterMode =AddressFilterMode.Any)]
    public class KeyboardService : IKeyboard , IMyHelpPageContract
    {
        public void IO(string mes)
        {
            Console.WriteLine(mes);
            Callback.ret(mes);
        }

        IKeyboardCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IKeyboardCallback>();
            }
        }
        public Message Help()
        {
            var context = WebOperationContext.Current;
            context.OutgoingResponse.ContentType = "text/html";
            return new LandingPageMessage();
        }
    }
    #region LandingPage
    public class LandingPageMessage : Message
    {
        private readonly MessageHeaders _headers;
        private readonly MessageProperties _properties;

        public LandingPageMessage()
        {
            this._headers = new MessageHeaders(MessageVersion.None);
            this._properties = new MessageProperties();
        }

        public override MessageHeaders Headers
        {
            get { return this._headers; }
        }
        public override MessageProperties Properties
        {
            get { return this._properties; }
        }
        public override MessageVersion Version
        {
            get { return this._headers.MessageVersion; }
        }
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            var document = new HtmlDocument();
            document.Load("webpage.htm");
            var bodyNode = document.DocumentNode.SelectSingleNode("//html");
            writer.WriteStartElement("HTML");
            writer.WriteRaw(bodyNode.InnerHtml);
            writer.WriteEndElement();
        }
    }
    #endregion

}
