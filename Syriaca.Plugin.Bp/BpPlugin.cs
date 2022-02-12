using System.Collections.Generic;
using Syriaca.Client;

namespace Syriaca.Plugin.Bp
{
    public class BpPlugin : Client.Plugins.Plugin
    {
        public static readonly TcpClient Client = new();
        public static IList<dynamic> Devices;

        public override string Name => "Buttplug.io";

        public override Scheduler CreateScheduler() => new(20);

        public override void OnScheduleCreated()
        {
            Client.Start();
            Scheduler.Add(testVibration);
        }

        private void testVibration()
        {
            if (Devices is { Count: > 0 })
            {
                
            }
        }
    }
}