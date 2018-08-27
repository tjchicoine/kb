using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeyboardLock
{
    public partial class Keylocks : Form
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);
        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);


        const int WH_KEYBOARD_LL = 13;
        const int WM_HOTKEY = 0x312;
        const int WM_KEYDOWN = 0x100;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private LowLevelKeyboardProc _proc = HookProc;
        private static IntPtr hhook = IntPtr.Zero;

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(hhook);
        }

        public static IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == 0xA2)
                {
                    MessageBox.Show("Control PresseD");
                }
                return (IntPtr)1;
            }
            if (code >= 0 && wParam == (IntPtr)WM_HOTKEY)
            {
                MessageBox.Show("Hotkey");
                return (IntPtr)1;
            }
            else
                return CallNextHookEx(hhook, code, wParam, lParam);
        }

    }
}
