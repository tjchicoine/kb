using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Xml;
using HtmlAgilityPack;

namespace KeyboardLockHost
{
    [ServiceBehavior(ConfigurationName = "foo", InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class KeyboardService : IKeyboard, IMyHelpPageContract
    {
        private static List<IKeyboardCallback> _callbackChannels = new List<IKeyboardCallback>();
        private static readonly object _sycnRoot = new Object();
        private static List<string> _groceryList = new List<string>();

        #region Connect
        public void Connect()
        {
            try
            {
                IKeyboardCallback callbackChannel = OperationContext.Current.GetCallbackChannel<IKeyboardCallback>();

                lock (_sycnRoot)
                {
                    if (!_callbackChannels.Contains(callbackChannel))
                    {
                        _callbackChannels.Add(callbackChannel);
                        Console.WriteLine("Added Callback Channel: {0}", callbackChannel.GetHashCode());
                        callbackChannel.SendUpdatedList(_groceryList);

                    }
                }
            }
            catch { }
        }
        #endregion

        #region Disconnect
        public void Disconnect()
        {
            IKeyboardCallback callbackChannel = 
                OperationContext.Current.GetCallbackChannel<IKeyboardCallback>();

            try
            {
                lock (_sycnRoot)
                {
                    if (_callbackChannels.Remove(callbackChannel))
                    {
                        Console.WriteLine("Removed Callback Channel: {0}", callbackChannel.GetHashCode());
                    }
                }
            }
            catch { }
        }
        #endregion

        #region AddItem
        public void AddItem(string item)
        {
            lock (_sycnRoot)
            {
                _groceryList.Add(item);
                Console.WriteLine("-- Grocery List--");
                _groceryList.ForEach(listItem => Console.WriteLine(listItem));
                Console.WriteLine("-------------------");

                for(int i = _callbackChannels.Count -1; i >=0; i--)
                {
                    if(((ICommunicationObject)_callbackChannels[i]).State != CommunicationState.Opened)
                    {
                        Console.WriteLine("Detected Non-Open Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                        _callbackChannels.RemoveAt(i);
                        continue;
                    }
                    try
                    {
                        _callbackChannels[i].SendUpdatedList(_groceryList);
                        Console.WriteLine("Pushed Updated List on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Service threw exception while communication on Callback Channel: {0}", _callbackChannels[i].GetHashCode());
                        Console.WriteLine("Exception Type: {0} Description: {1}", ex.GetType(), ex.Message);
                        _callbackChannels.RemoveAt(i);
                    }
                }
            }
        }
        #endregion

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
