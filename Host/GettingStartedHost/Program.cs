using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using NetFwTypeLib;
using System.Net;
using System.Net.Sockets;



namespace KeyboardLockHost
{
    class Program
    {
        internal static ServiceHost host = null;

        static void Main(string[] args)
        {
            #region portcheck_init
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            Console.WriteLine(localIP);
            int port = 8080;
            bool open = portcheck(port);
            Console.WriteLine("Port " + port + " is " + ((open) ? "open" : "closed"));

            //if (!open) { Fwr(); }

            #endregion


            StartService(localIP);

            Console.WriteLine("Press <Enter> to stop the service.");
            Console.ReadLine();

            StopService();
        }

        private static void StartService(string localIP)
        {
            Uri baseAddress = new Uri("http://" + localIP + ":8080");

            WSDualHttpBinding binding1 = new WSDualHttpBinding();

            host = new ServiceHost(typeof(KeyboardService),baseAddress);

            //host.AddServiceEndpoint(typeof(IKeyboard), new WSDualHttpBinding(), "E1");

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);

            //host.Description.Behaviors.Remove<ServiceDebugBehavior>();

            host.Open();
            Console.WriteLine("The service is ready at {0}", baseAddress);
        }

         private static void StopService()
        {
            host.Close();
        }
        #region portcheck
        static bool portcheck(int port)
        {
            bool exists = false;
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            var currentProfiles = fwPolicy2.CurrentProfileTypes;
            List<INetFwRule> RuleList = new List<INetFwRule>();
            string i = Convert.ToString(port);
            foreach (INetFwRule rule in fwPolicy2.Rules)
            {
                if (rule.LocalPorts == i)
                {
                    exists = true;
                    return exists;
                }
            }
            return exists;
        }

        static void Fwr()
        {
            INetFwRule firewallrule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallrule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallrule.Description = ("Enables port 8080");
            firewallrule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            firewallrule.Enabled = true;
            firewallrule.InterfaceTypes = "All";
            firewallrule.Name = "Test";
            firewallrule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            firewallrule.LocalPorts = "8080";
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Add(firewallrule);
        }
        #endregion
    }
}
