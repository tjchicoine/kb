using System;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using KeyboardLock.kb;

namespace KeyboardLock
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            #region localip
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            #endregion

            Console.WriteLine("Enter a message and press <enter>:");
            for (int i = 0; i < 10; i++)
            {
                string msg = Console.ReadLine();
                Duplex(localIP, msg);
            }    

            //Application.EnableVisualStyles();
            //Application.Run(new MainForm());
#if DEBUG
            Console.WriteLine("Press <Enter> to continue . . .");
            Console.ReadLine();
#endif
            }

        private async static void Duplex(string localIP, string npt)
        {
            var binding = new WSDualHttpBinding();
            var address = new EndpointAddress(new Uri("http://"+localIP+":8080/E1"));
            var clientCallback = new CallbackHandler();
            var context = new InstanceContext(clientCallback);
            var factory = new DuplexChannelFactory<nwKeyboardLock.IKeyboard>(clientCallback, binding, address);
            nwKeyboardLock.IKeyboard keyboard = factory.CreateChannel();
            await Task.Run(() => keyboard.IO(npt));
        }
    }
}