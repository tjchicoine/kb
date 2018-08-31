using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Hooks
{
    public class Keylocks
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        const int WH_KEYBOARD_LL = 13;
        const int WM_HOTKEY = 0x312;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;
        private IntPtr _hhook = IntPtr.Zero;

        private delegate int KeyboardHookProc(int nCode, int wParam, ref KeyboardHookStruct lParam);

        public struct KeyboardHookStruct
        {
            public int vkCode;
            public int scancode;
            public int flags;
            public int time;
            public int extra;
        }

        public List<Keys> HookedKeys = new List<Keys>();
        
        
        private event EventHandler<KeyPressedEventArgs> KeyPressed;

        public Keylocks()
        {
            this.SetHook();
            this.HookedKeys.Add(Keys.A);
            this.HookedKeys.Add(Keys.Control);
            this.HookedKeys.Add(Keys.ControlKey);
        }

        ~Keylocks()
        {
            this.UnHook();
        }

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            this._hhook = SetWindowsHookEx(WH_KEYBOARD_LL, this.HookProc, hInstance, 0);
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(this._hhook);
        }

        private int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                var key = (Keys)lParam.vkCode;

                if (this.HookedKeys.Contains(key))
                {
                    var handler = this.KeyPressed;
                    
                    if (((wParam == WM_KEYDOWN) || wParam == WM_SYSKEYDOWN)  && (handler != null))
                    {
                        ModifierKeys mods = 0;
                        if (Keyboard.IsKeyDown(Keys.Control) || Keyboard.IsKeyDown(Keys.ControlKey) || Keyboard.IsKeyDown(Keys.LControlKey) || Keyboard.IsKeyDown(Keys.RControlKey))
                        {
                            mods |= ModifierKeys.Control;
                            Console.WriteLine("Probably Unhook Now");
                        }

                        handler(this, new KeyPressedEventArgs(mods, key));
                    }
                }
                return 1;
            }
            else 
                return CallNextHookEx(this._hhook, code, wParam, ref lParam);
        }
    }

    static class Keyboard
    {
        [Flags]
        private enum KeyStates
        {
            None = 0,
            Down = 1,
            Toggled = 2
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        private static KeyStates GetKeyState(Keys key)
        {
            KeyStates state = KeyStates.None;

            short retVal = GetKeyState((int)key);

            if ((retVal & 0x8000) == 0x8000)
                state |= KeyStates.Down;

            if ((retVal & 1) == 1)
                state |= KeyStates.Toggled;

            return state;
        }

        public static bool IsKeyDown(Keys key)
        {
            return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
        }

        public static bool IsKeyToggled(Keys key)
        {
            return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled);
        }

    }
    class KeyPressedEventArgs : EventArgs
    {
        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            this.Modifier = modifier;
            this.Key = Key;
            this.Ctrl = (modifier & ModifierKeys.Control) != 0;
            this.Shift = (modifier & ModifierKeys.Shift) != 0;
            this.Win = (modifier & ModifierKeys.Win) != 0;
            this.Alt = (modifier & ModifierKeys.Alt) != 0;
        }

        public ModifierKeys Modifier { get; private set; }
        public Keys Key { get; private set; }
        public readonly bool Ctrl;
        public readonly bool Shift;
        public readonly bool Win;
        public readonly bool Alt;
    }
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
