using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace WorkBench
{
    internal class f
    {

        internal static string GetSkiConnectionString()
        {
             string server = "SQL-SERVER";
        string instance = "GILBOA";
        string database = "SKI";
        string user = "Michael";
        string password = "2585";
        return $"Server={server}\\{instance};Database={database};User Id={user};Password={password};Encrypt=False;TrustServerCertificate=True;";
    }

        internal static void OpenForm(Form form)
        {
            form.ShowDialog();
            form.Close();
        }

        public static void TypeWithLanguageAutoSwitch(string text)
    {
        // Check for Hebrew character
        bool containsHebrew = Regex.IsMatch(text, @"[\u0590-\u05FF]");

        // Check current keyboard language
        bool isHebrewKeyboard = IsKeyboardHebrew();

        if (containsHebrew)
        {
            if (!isHebrewKeyboard)
            {
                // Switch to Hebrew (Alt+Shift)
                Keyboard.Press(VirtualKeyShort.LMENU);
                Keyboard.Press(VirtualKeyShort.SHIFT);
                Keyboard.Release(VirtualKeyShort.SHIFT);
                Keyboard.Release(VirtualKeyShort.LMENU);
            }
        }
        else
        {
            if (isHebrewKeyboard)
            {
                // Switch to English (Alt+Shift)
                Keyboard.Press(VirtualKeyShort.LMENU);
                Keyboard.Press(VirtualKeyShort.SHIFT);
                Keyboard.Release(VirtualKeyShort.SHIFT);
                Keyboard.Release(VirtualKeyShort.LMENU);
            }
        }

        // Type the text
        Keyboard.Type(text);
    }

    // Helper method to check if keyboard is in Hebrew mode
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

    [DllImport("user32.dll")]
    static extern IntPtr GetKeyboardLayout(uint thread);

    public static bool IsKeyboardHebrew()
    {
        IntPtr hwnd = GetForegroundWindow();
        uint threadId = GetWindowThreadProcessId(hwnd, IntPtr.Zero);
        IntPtr hkl = GetKeyboardLayout(threadId);
        int langId = hkl.ToInt32() & 0xFFFF;
        var culture = new CultureInfo(langId);
        return culture.TwoLetterISOLanguageName == "he";
    }
}
}
