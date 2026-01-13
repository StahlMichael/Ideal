using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace WorkBench
{
    public class GilboaAutomation
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        const int SW_MAXIMIZE = 3;
        const int SW_RESTORE = 9;

        private FlaUI.Core.Application? app;
        private int currentDoket;

        public int CurrentDoket => currentDoket;
        public FlaUI.Core.Application? Application => app;

        // Ensure we're attached to glbook process
        private void EnsureGlbookAttached()
        {
            if (app == null)
            {
                var glbook = Process.GetProcessesByName("glbook");
                if (glbook.Length > 0)
                {
                    AttachToExistingGlbook(glbook[0]);
                }
                else
                {
                    throw new InvalidOperationException("Gilboa (glbook) is not running. Please open Gilboa first.");
                }
            }
        }

        public void OpenOrAttachGilboa()
        {
            var gilboamn = Process.GetProcessesByName("gilboamn");
            var glbook = Process.GetProcessesByName("glbook");

            if (glbook.Length > 0)
            {
                AttachToExistingGlbook(glbook[0]);
            }
            else if (gilboamn.Length > 0)
            {
                AttachToGilboamnAndOpenGlbook(gilboamn[0]);
            }
            else
            {
                LaunchNewGilboaInstance();
            }
        }

        private void AttachToExistingGlbook(Process glbookProcess)
        {
            app = FlaUI.Core.Application.Attach(glbookProcess);
            using (var automation = new UIA3Automation())
            {
                var allWindows = app.GetAllTopLevelWindows(automation);
                var window = allWindows.FirstOrDefault(w => w.Title.StartsWith("Gilboa -"));
                if (window != null)
                {
                    string title = window.Title;
                    var match = Regex.Match(title, @"#(\d+)");
                    if (match.Success)
                    {
                        currentDoket = Convert.ToInt32(match.Groups[1].Value);
                    }
                }
            }
        }

        private void AttachToGilboamnAndOpenGlbook(Process gilboamnProcess)
        {
            app = FlaUI.Core.Application.Attach(gilboamnProcess);
            using (var automation = new UIA3Automation())
            {
                var chocolate = app?.GetMainWindow(automation, TimeSpan.FromSeconds(5));
                if (chocolate != null)
                {
                    var rect = chocolate.BoundingRectangle;
                    SetForegroundWindow((IntPtr)chocolate.Properties.NativeWindowHandle.Value);
                    Mouse.MoveTo(rect.Left + 50, rect.Top + 100);
                    Mouse.Click();
                    Thread.Sleep(300);
                    var prog = Process.GetProcessesByName("glbook");
                    if (prog.Length > 0)
                    {
                        app = FlaUI.Core.Application.Attach(prog[0]);
                    }
                }
            }
        }

        private void LaunchNewGilboaInstance()
        {
            app = FlaUI.Core.Application.Launch(@"G:\G4WExe\gilboamn.exe");
            Thread.Sleep(2000);
            
            using (var automation = new UIA3Automation())
            {
                var window_ = app.GetMainWindow(automation, TimeSpan.FromSeconds(10));
                if (window_ == null)
                {
                    throw new Exception("Main window not found.");
                }

                var loginWindow = app.GetAllTopLevelWindows(automation)
                    .FirstOrDefault(w => w.Title == "Gilboa Log-in");
                if (loginWindow == null)
                {
                    throw new Exception("Login window not found.");
                }

                PerformLogin(loginWindow);
                Thread.Sleep(300);

                CheckAndHandleLicenseRenewal(automation);
                ClickChocolateMenu(automation);
            }
        }

        private void PerformLogin(Window loginWindow)
        {
            Thread.Sleep(300); // Wait for window to be fully ready
            
            var usernameBox = loginWindow.FindFirstDescendant(cf => cf.ByAutomationId("14760"))?.AsTextBox();
            var passwordBox = loginWindow.FindFirstDescendant(cf => cf.ByAutomationId("14761"))?.AsTextBox();
            
            if (usernameBox != null && usernameBox.IsEnabled)
            {
                usernameBox.Focus();
                Thread.Sleep(100);
                usernameBox.Text = "stahl";
            }
            
            if (passwordBox != null && passwordBox.IsEnabled)
            {
                passwordBox.Focus();
                Thread.Sleep(100);
                passwordBox.Text = "3913";
            }

            Thread.Sleep(100);
            var okButton = loginWindow.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
            if (okButton != null && okButton.IsEnabled)
            {
                okButton.Invoke();
            }
        }

        private void CheckAndHandleLicenseRenewal(UIA3Automation automation)
        {
            var allWindows = app?.GetAllTopLevelWindows(automation);
            var window = allWindows?.LastOrDefault();
            
            if (window != null && window.Title.StartsWith("Please Read"))
            {
                var button = window.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
                if (button != null && button.IsEnabled)
                {
                    button.Invoke();
                }
            }
        }

        private void ClickChocolateMenu(UIA3Automation automation)
        {
            Thread.Sleep(2000);
            var chocolate = app?.GetMainWindow(automation, TimeSpan.FromSeconds(10));
            if (chocolate != null)
            {
                var rect = chocolate.BoundingRectangle;
                Mouse.MoveTo(rect.Left + 50, rect.Top + 100);
                Mouse.Click();
                
                // Wait for glbook to open and attach to it
                Thread.Sleep(1000);
                var glbook = Process.GetProcessesByName("glbook");
                if (glbook.Length > 0)
                {
                    app = FlaUI.Core.Application.Attach(glbook[0]);
                }
            }
        }

        private void BringGilboaToFront(Window window)
        {
            var handle = (IntPtr)window.Properties.NativeWindowHandle.Value;
            ShowWindow(handle, SW_RESTORE);
            ShowWindow(handle, SW_MAXIMIZE);
            SetForegroundWindow(handle);
            Thread.Sleep(200);
        }

        public void OpenNewDoketWithPax(string agentName, List<string> passengers)
        {
            // Always ensure we're attached to glbook
            var glbook = Process.GetProcessesByName("glbook");
            if (glbook.Length == 0)
            {
                OpenOrAttachGilboa();
            }
            else
            {
                // Always reattach to ensure we have the correct instance
                AttachToExistingGlbook(glbook[0]);
            }

            using (var automation = new UIA3Automation())
            {
                var allWindows = app?.GetAllTopLevelWindows(automation);
                var window = allWindows?.FirstOrDefault(w => w.Title.StartsWith("Gilboa -"));
                if (window == null) return;

                BringGilboaToFront(window);

                var rect = window.BoundingRectangle;
                Mouse.MoveTo(rect.Left + 26, rect.Top + 60);
                Mouse.Click();
                Thread.Sleep(300);

                allWindows = app?.GetAllTopLevelWindows(automation);
                window = allWindows?.Last();

                var editControl = window.FindFirstDescendant(cf => cf.ByAutomationId("10418"))?.AsTextBox();
                if (editControl != null && editControl.IsEnabled)
                {
                    editControl.Focus();
                    FlaUI.Core.Input.Keyboard.Type(agentName);
                }
                Thread.Sleep(200);

                var button = window.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
                if (button != null && button.IsEnabled)
                {
                    button.Invoke();
                }

                for (int i = 0; i < passengers.Count; i++)
                {
                    AddPassenger(passengers[i], i == passengers.Count - 1);
                }
            }
        }

        private void AddPassenger(string pax, bool last)
        {
            string[] paxInfo = pax.Split('|');
            string name = paxInfo[0];
            string birthdate = paxInfo[1];
            string passport = paxInfo[2];
            using (var automation = new UIA3Automation())
            {
                 var allWindows = app?.GetAllTopLevelWindows(automation);
                var window = allWindows?.Last();
                MakeClicks("birthday");
                FlaUI.Core.Input.Keyboard.Type(birthdate);
               
                var editControl = window.FindFirstDescendant(cf => cf.ByAutomationId("10121"))?.AsTextBox();
                if (editControl != null && editControl.IsEnabled)
                {
                    editControl.Focus();
                    FlaUI.Core.Input.Keyboard.Type(name);
                }
                  MakeClicks("passport1");

                allWindows = app?.GetAllTopLevelWindows(automation);
                var windowNext = allWindows?.Last();
                 
                var button = windowNext.FindFirstDescendant(cf => cf.ByAutomationId("6"))?.AsButton();
                if (button != null && button.IsEnabled)
                {
                    button.Invoke();
                }
               MakeClicks("passport2");
                FlaUI.Core.Input.Keyboard.Type(passport);
                  button = window.FindFirstDescendant(cf => cf.ByAutomationId(last ? "1" : "10145"))?.AsButton();
                if (button != null && button.IsEnabled)
                {
                    button.Invoke();
                }

            }
        }

        public void OpenExistingDoket(int doketNumber)
        {
            // Always ensure we're attached to glbook
            var glbook = Process.GetProcessesByName("glbook");
            if (glbook.Length == 0)
            {
                OpenOrAttachGilboa();
            }
            else
            {
                // Always reattach to ensure we have the correct instance
                AttachToExistingGlbook(glbook[0]);
            }

            currentDoket = doketNumber;

            using (var automation = new UIA3Automation())
            {
                var allWindows = app?.GetAllTopLevelWindows(automation);
                var window = allWindows?.FirstOrDefault(w => w.Title.StartsWith("Gilboa -"));
                if (window == null) return;

                BringGilboaToFront(window);

                var rect = window.BoundingRectangle;
                Mouse.MoveTo(rect.Left + 205, rect.Top + 60);
                Mouse.Click();
                Thread.Sleep(200);

                allWindows = app?.GetAllTopLevelWindows(automation);
                window = allWindows?.Last();

                var editControl = window?.FindFirstDescendant(cf => cf.ByAutomationId("10455"))?.AsTextBox();
                if (editControl != null && editControl.IsEnabled)
                {
                    editControl.Focus();
                    FlaUI.Core.Input.Keyboard.Type(doketNumber.ToString());
                }
                Thread.Sleep(200);

                var button = window?.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
                if (button != null && button.IsEnabled)
                {
                    button.Invoke();
                }
                //Thread.Sleep(200);

                //allWindows = app?.GetAllTopLevelWindows(automation);
                //window = allWindows?.Last();
                //button = window?.FindFirstDescendant(cf => cf.ByAutomationId("2"))?.AsButton();
                //if (button != null && button.IsEnabled)
                //{
                //    button.Invoke();
                //}
            }
        }

        public void CreateVoucher(string packageName, string startDate, string endDate, string description1, string description2)
        {
            // Always ensure we're attached to glbook
            var glbook = Process.GetProcessesByName("glbook");
            if (glbook.Length == 0)
            {
                OpenOrAttachGilboa();
            }
            else
            {
                // Always reattach to ensure we have the correct instance
                AttachToExistingGlbook(glbook[0]);
            }

            using (var automation = new UIA3Automation())
            {
                var allWindows = app.GetAllTopLevelWindows(automation);
                string title = allWindows[0].Title;
                var window = allWindows.FirstOrDefault(w => w.Title.StartsWith("Gilboa -"));
                if (window == null) return;

                BringGilboaToFront(window);

                var rect = window.BoundingRectangle;
                Mouse.MoveTo(rect.Left + 180, rect.Top + 110);
                Mouse.Click();
                Thread.Sleep(200);

                allWindows = app?.GetAllTopLevelWindows(automation);
                window = allWindows?.Last();

                InputVoucherInfo(window, 11408, packageName);
                InputVoucherInfo(window, 11438, startDate);
                InputVoucherInfo(window, 11437, endDate);
                InputVoucherInfo(window, 11394, description1);
                InputVoucherInfo(window, 11434, description2);

                var button = window.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
                if (button != null && button.IsEnabled)
                {
                    button.Invoke();
                }
            }
        }

        public void CreatePayment(PaymentInfo paymentInfo)
        {
                // Always ensure we're attached to glbook
            var glbook = Process.GetProcessesByName("glbook");
            if (glbook.Length == 0)
            {
                OpenOrAttachGilboa();
            }
            else
            {
                AttachToExistingGlbook(glbook[0]);
            }
            try
            {
                using (var automation = new UIA3Automation())
                {
                    var allWindows = app.GetAllTopLevelWindows(automation);
                    string title = allWindows[0].Title;
                    var window = allWindows.FirstOrDefault(w => w.Title.StartsWith("Gilboa -"));
                    if (window == null) return;
                    BringGilboaToFront(window);

                    var rec = window.BoundingRectangle;
                    Mouse.MoveTo(rec.Left + 840, rec.Top + 680);
                    Mouse.Click();
                    Thread.Sleep(100);

                    // InputVoucherInfo(window, 11634, "C.C.");
                    MakeClicks("cardType");
                    InputVoucherInfo(window, 11654, paymentInfo.CardName);
                    InputVoucherInfo(window, 11663, paymentInfo.ExpiryDate);
                    InputVoucherInfo(window, 11664, paymentInfo.Token);
                    InputVoucherInfo(window, 11603, paymentInfo.Amount);
                    InputVoucherInfo(window, 11662, paymentInfo.ApprovalNum);
                    InputVoucherInfo(window, 11661, paymentInfo.CardName);
                    MakeClicks("currency");
                    var button = window.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
                    if (button != null && button.IsEnabled)
                    {
                        button.Invoke();
                    }
                    Thread.Sleep(100);
                    var lastWindow = GetLastOpenedWindow();
                    button = lastWindow.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
                    if (button != null && button.IsEnabled)
                    {
                        button.Invoke();
                    }
                    Thread.Sleep(100);
                    lastWindow = GetLastOpenedWindow();
                    button = lastWindow.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
                    if (button != null && button.IsEnabled)
                    {
                        button.Invoke();
                    }
                    Thread.Sleep(100);
                    lastWindow = GetLastOpenedWindow();
                    button = window.FindFirstDescendant(cf => cf.ByAutomationId("1"))?.AsButton();
                    if (button != null && button.IsEnabled)
                    {
                        button.Invoke();
                    }
                }
            }
            catch(Exception ex) { }
  }
        private void MakeClicks(string type)
        {
            var lastWindow = GetLastOpenedWindow();
            var windowHandle = (IntPtr)lastWindow.Properties.NativeWindowHandle.Value;
            if (windowHandle != IntPtr.Zero)
            {
                RECT rect;
                if (GetWindowRect(windowHandle, out rect))
                {
                    int x = rect.Left;
                    int y = rect.Top;
                    int width = rect.Right - rect.Left;
                    int height = rect.Bottom - rect.Top;
                    switch (type)
                    {
                        case "cardType":
                            {
                                Click(x, y, 110, 105);
                                Click(x, y, 110, 175);
                            }
                            break;
                        case "currency":
                            {
                                Click(x, y, 530, 245);
                                Click(x, y, 530, 265);
                                Click(x, y, 500, 265);
                            }
                            break;
                        case "birthday":
                            {
                                Click(x, y, 120, 300);
                            }
                            break;
                        case "passport1":
                            {
                                Click(x, y, 375, 205);
                             }
                            break;
                        case "passport2":
                            {
                                Click(x, y, 375, 280);
                            }
                            break;
                    }
                }
            }
        }

        private void Click(int x, int y, int width, int height)
        {
            Mouse.MoveTo(x + width, y + height);
            Mouse.Click();
            Thread.Sleep(100);
        }

        private void InputVoucherInfo(Window window, int automationId, string text)
        {
            var editControl = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId.ToString()))?.AsTextBox();
           if (editControl != null && editControl.IsEnabled)
            {
                editControl.Focus();
                f.TypeWithLanguageAutoSwitch(text);
            }
            Thread.Sleep(100);
        }

        // Window enumeration methods
        public Window GetLastOpenedWindow()
        {
            using (var automation = new UIA3Automation())
            {
                // Get all windows including child windows, not just top-level
                var allTopLevel = app?.GetAllTopLevelWindows(automation) ?? new AutomationElement[0];
                var allWindows = new List<Window>();
                
                foreach (var topWindow in allTopLevel)
                {
                    allWindows.Add(topWindow.AsWindow());
                    // Add all descendant windows
                    var descendants = topWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window));
                    foreach (var descendant in descendants)
                    {
                        allWindows.Add(descendant.AsWindow());
                    }
                }
                
                return allWindows.Last();
            }
        }

        public List<(IntPtr hWnd, string Title)> GetOpenWindows()
        {
            var winList = new List<(IntPtr, string)>();
            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd))
                {
                    var sb = new StringBuilder(256);
                    GetWindowText(hWnd, sb, sb.Capacity);
                    string title = sb.ToString();
                    if (!string.IsNullOrWhiteSpace(title))
                        winList.Add((hWnd, title));
                }
                return true;
            }, IntPtr.Zero);
            return winList;
        }

        public string GetControlsInfo(IntPtr windowHandle)
        {
            string info = "";
            using (var automation = new UIA3Automation())
            {
                try
                {
                    uint processId;
                    GetWindowThreadProcessId(windowHandle, out processId);
                    var app = FlaUI.Core.Application.Attach((int)processId);
                    var window = app.GetMainWindow(automation);
                    var allControls = window?.FindAllDescendants();
                                        if (allControls != null)                    foreach (var control in allControls)
                    {
                        string automationId, className;
                        try { automationId = control.AutomationId; }
                        catch { automationId = "[Not Supported]"; }

                        try { className = control.ClassName; }
                        catch { className = "[Not Supported]"; }

                        info += $"AutomationId: {automationId}, Name: {control.Name}, ControlType: {control.ControlType}, ClassName: {className}, BoundingRectangle: {control.BoundingRectangle}, IsEnabled: {control.IsEnabled}\n";
                    }
                }
                catch { }
            }
            return info;
        }
    }
}
