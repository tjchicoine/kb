using System;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using KeyboardLock.kb;
using KeyboardLockHost;
using System.Collections.Generic;

namespace KeyboardLock
{
    public class Program
    {
        //private static KeyboardlockClient _proxy;
        [STAThread]
        public static void Main(string[] args)
        {


            //InitializeClient();
            //additem();

            Application.EnableVisualStyles();
            Application.Run(new MainForm());
#if DEBUG
            Console.WriteLine("Press <Enter> to continue . . .");
            Console.ReadLine();

            //_proxy.Disconnect();
#endif
        }
        //private static void HandleServiceCallbackEvent(object sender, UpdatedListEventArgs e)
        //{
        //    List<string> groceryList = e.ItemList;
        //    if(groceryList != null && groceryList.Count > 0)
        //    {
        //        for(int i = 0; i < groceryList.Count; i++)
        //        {
        //            Console.WriteLine(groceryList[i]);
        //        }
        //    }
        //}

        //private static void InitializeClient()
        //{
        //    #region localip
        //    string localIP;
        //    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        //    {
        //        socket.Connect("8.8.8.8", 65530);
        //        IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
        //        localIP = endPoint.Address.ToString();
        //    }
        //    #endregion

        //    if (_proxy != null)
        //    {
        //        try
        //        {
        //            _proxy.Close();
        //        }
        //        catch
        //        {
        //            _proxy.Abort();
        //        }
        //        finally
        //        {
        //            _proxy = null;
        //        }
        //    }

        //    Uri local = new Uri("http://"+localIP+":8080/E1");

        //    var KeyboardDuplexCallback = new KeyboardCallback();
        //    KeyboardDuplexCallback.ServiceCallbackEvent += HandleServiceCallbackEvent;

        //    var instanceContext = new InstanceContext(KeyboardDuplexCallback);
        //    var binding = new WSDualHttpBinding();
        //    var endpoint = new EndpointAddress(local);
        //    _proxy = new KeyboardlockClient(instanceContext, binding, endpoint);
        //    _proxy.Open();
        //    _proxy.Connect();
        //}

        //private static void additem()
        //{
        //    Console.WriteLine("What do you want to add?");
        //    string item = Console.ReadLine();
        //    _proxy.AddItem(item);
        //}
    }
}