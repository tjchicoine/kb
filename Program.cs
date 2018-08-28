using System;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;




namespace KeyboardLock
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8080/hello");

            using (ServiceHost host = new ServiceHost(typeof(HelloWorldService), baseAddress))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service");
                Console.ReadLine();

                host.Close();
            }

            Application.EnableVisualStyles();
            Application.Run(new MainForm());
#if DEBUG
            Console.WriteLine("Press <Enter> to continue . . .");
            Console.ReadLine();
#endif
        }
    }
    [ServiceContract]
    public interface IHelloWorldService
    {
        [OperationContract]
        string SayHello(string name);
    }
    public class HelloWorldService : IHelloWorldService
    {
        public string SayHello(string name)
        {
            return string.Format("Hello,{0}", name);
        }
    }
}