using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Syriaca.Client.Utils
{
    public static class ProcessUtils
    {
        /// <summary>
        /// Checks if a process is open or not.
        /// </summary>
        /// <param name="processName">The name of the process to check/</param>
        public static bool IsProcessOpen(string processName)
        {
            var proc = Process.GetProcessesByName(processName);

            return proc.Any();
        }

        /// <summary>
        /// Gets the Geometry Dash process.
        /// </summary>
        /// <param name="openGd">optionally opens Geometry Dash automatically.</param>
        public static Process GetGdProcess(bool openGd = false)
        {
            Process proc;

            if (openGd)
            {
                // TODO: Allow custom installation paths
                const string path = @"C:\Program Files (x86)\Steam\steamapps\common\Geometry Dash";

                var processStartInfo = new ProcessStartInfo(path + @"\GeometryDash.exe")
                {
                    WorkingDirectory = path,
                    UseShellExecute = false
                };

                proc = Process.Start(processStartInfo);

                // doesn't open immediately so we will have to wait a bit
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            else
            {
                const string process_name = "GeometryDash";

                if (!IsProcessOpen(process_name))
                    return null;

                proc = Process.GetProcessesByName(process_name)[0];
            }

            return proc;
        }
    }
}