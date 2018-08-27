using System;
using System.Windows.Forms;
using HotKeys;


namespace KeyboardLock
{
    public partial class MainForm : Form
    {
        private GlobalHotKey ghk;

        public MainForm()
        {
            InitializeComponent();
            ghk = new GlobalHotKey(Constants.SHIFT, Keys.L, this);
            var handler = new EventHandler(OnClick);
            this.Click += handler;
            label1.Click += handler;
            label2.Click += handler;
        }

        private void HandleHotKey()
        {
            MessageBox.Show("Hotkey pressed");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotKey();
            base.WndProc(ref m);
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Registering");
            if (ghk.Register())
                Console.WriteLine("Hotkey Registered");
            else
                Console.WriteLine("Hotkey Failed to Register");
        }

        private void MainForm_FormClosing(object sender, FormClosedEventArgs e)
        {
            if (!ghk.UnRegister())
                MessageBox.Show("Hotkey Failed to Unregister");
        }

        void OnClick(object sender, EventArgs e)
        {
            Close();
        }

    }
}
