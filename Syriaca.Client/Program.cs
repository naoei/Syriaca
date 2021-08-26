using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Syriaca.Client.Memory;
using Syriaca.Client.Rpc;
using Syriaca.Client.Utils;

namespace Syriaca.Client
{
    public static class Program
    {
        private static GdReader reader;
        private static GdProcessState processState;
        private static RpcThread rpcThread;

        public static void Main(string[] args)
        {
            var proc = GetGdProcess(args);

            if (proc == null)
            {
                Logger.Error("Failed to fetch GeometryDash process");

                return;
            }

            reader = new GdReader(proc);

            processState = new GdProcessState(reader);
            rpcThread = new RpcThread(processState, reader);

            processState.StartScheduler();
            rpcThread.StartScheduler();

            new Thread(() => CreateLoop(processState.Scheduler)).Start();
            new Thread(() => CreateLoop(rpcThread.Scheduler)).Start();
        }

        private static void CreateLoop(Scheduler scheduler)
        {
            while (true)
            {
                if (scheduler.Stopwatch.ElapsedMilliseconds < scheduler.Delay)
                    continue;

                scheduler.Stopwatch.Restart();
                scheduler.Pulse();
            }
        }

        private static Process GetGdProcess(IReadOnlyCollection<string> args)
        {
            Process proc = null;

            if (args.Count > 0 && args.Any(a => a == "--opengd" || a == "-o"))
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
                try
                {
                    proc = Process.GetProcessesByName("GeometryDash")[0];
                }
                catch
                {
                    // ignored
                }
            }

            return proc;
        }
    }
}