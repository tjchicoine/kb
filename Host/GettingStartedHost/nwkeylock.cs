using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using NetFwTypeLib;




namespace nwKeyboardLock
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8080;
            bool open = portcheck(port);
            Console.WriteLine("Port " + port + " is " + ((open)? "open" : "closed"));
            if (!open) { Fwr(); }

            Uri baseAddress = new Uri("http://192.168.100.139:8080/");

            // Create the ServiceHost.
            using (ServiceHost host = new ServiceHost(typeof(KeyBoardService), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }
        }
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
    }
}
