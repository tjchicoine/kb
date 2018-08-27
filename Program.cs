using System;
using System.Windows.Forms;


namespace KeyboardLock
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            //Keylocks kl = new Keylocks();
            //kl.SetHook();
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
            //kl.UnHook();
        }              
    }
}
