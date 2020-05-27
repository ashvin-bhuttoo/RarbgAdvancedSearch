using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RarbgAdvancedSearch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "INSTALLER")
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                FileSystemAccessRule fsar = new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow);
                DirectorySecurity ds = null;

                ds = di.GetAccessControl();
                ds.AddAccessRule(fsar);
                di.SetAccessControl(ds);

                UsageStats.Log("installed", "", true);
                Process.Start(Application.ExecutablePath, "OVERRIDE_PROCESS_CHECK");
                return;
            }

            Thread.Sleep(1500);
            Process[] runningProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (runningProcesses.Length == 1 || (args.Length == 1 && args[0] == "OVERRIDE_PROCESS_CHECK")) // if its just me or OVERRIDE is set, let me run!
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Updater.Run(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                UsageStats.Log("open");
                try
                {
                    Application.Run(new Main());
                }
                catch(Exception e)
                {
                    UsageStats.Log("crash", e.Message + "\n" + e.StackTrace, true);
                }
            }
        }
    }
}
