using System;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;
using KeyboardLock.kb;

namespace KeyboardLock
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            KeyboardClient client = new KeyboardClient();
            bool value1 = true;
            client.IO(value1);

            Console.WriteLine("Enter a message and press <enter>:");
            string tosend = Console.ReadLine();
            client.msg(tosend);
                    

            //Application.EnableVisualStyles();
            //Application.Run(new MainForm());
#if DEBUG
            Console.WriteLine("Press <Enter> to continue . . .");
            Console.ReadLine();
#endif
            }
    }
}