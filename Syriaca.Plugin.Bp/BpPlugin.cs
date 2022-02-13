using System;
using System.Collections.Generic;
using Syriaca.Client;
using Syriaca.Plugin.Bp.Tcp;
using Syriaca.Plugin.Bp.Tcp.Requests;
using Syriaca.Plugin.Bp.Utils;

namespace Syriaca.Plugin.Bp
{
    public class BpPlugin : Client.Plugins.Plugin
    {
        public static readonly TcpClient Client = new();
        public static IList<dynamic> Devices;

        public override string Name => "Buttplug.io";

        public override Scheduler CreateScheduler() => new(5000);

        public override void OnScheduleCreated()
        {
            Client.Start();
            Scheduler.Add(testVibration);
        }

        private void testVibration()
        {
            if (Devices is { Count: > 0 })
            {
                new TcpBuilder().Start(OpCodes.SendCommand).Write(new Command
                {
                    Index = 0,
                    Motor = 0,
                    Speed = new Random().NextDouble()
                }).End();
                
                new TcpBuilder().Start(OpCodes.SendCommand).Write(new Command
                {
                    Index = 0,
                    Motor = 1,
                    Speed = new Random().NextDouble()
                }).End();
            }
        }
    }
}