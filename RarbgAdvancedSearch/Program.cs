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

                Process.Start(Application.ExecutablePath);
                return;
            }

            Thread.Sleep(2000);
            Process[] runningProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (runningProcesses.Length == 1 || (args.Length == 1 && args[0] == "OVERRIDE_PROCESS_CHECK")) // if its just me or OVERRIDE is set, let me run!
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Updater.Run("RarbgAdvancedSearch");
                Application.Run(new Main());
            }                
        }
    }
}
