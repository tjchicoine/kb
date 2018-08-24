using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardLock
{
    public partial class Program : Form
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [STAThread]
        static void Main()
        {
            const int WH_KEYBOARD_LL = 13;
            IntPtr hHook = SetWindowsHookEx(WH_KEYBOARD_LL, Callback, LoadLibrary("User32"), 0);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            UnhookWindowsHookEx(hHook);
        }



        private delegate IntPtr LowLevelKeyboardProc();

        private static IntPtr Callback()
        {
            return (IntPtr)1;
        }
    }
}
