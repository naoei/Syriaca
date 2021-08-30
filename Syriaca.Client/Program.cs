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
            var proc = ProcessUtils.GetGdProcess(args.Length > 0 && args.Any(a => a == "--opengd" || a == "-o"));

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
    }
}