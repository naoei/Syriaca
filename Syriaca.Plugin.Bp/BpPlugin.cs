using System;
using System.Collections.Generic;
using System.Linq;
using Syriaca.Client;
using Syriaca.Client.Information;
using Syriaca.Plugin.Bp.Patterns;
using Syriaca.Plugin.Bp.Tcp;
using Syriaca.Plugin.Bp.Tcp.Requests;
using Syriaca.Plugin.Bp.Utils;

namespace Syriaca.Plugin.Bp
{
    public class BpPlugin : Client.Plugins.Plugin
    {
        public static readonly TcpClient Client = new();
        public static List<dynamic> Devices = new();

        public static readonly List<(uint, PatternHandler)> CurrentPatterns = new();

        public override string Name => "Buttplug.io";

        public override Scheduler CreateScheduler() => new(5000);

        public override void OnScheduleCreated()
        {
            Client.Start();
            Scheduler.Add(testVibration);
        }

        private void testVibration()
        {
            if (Devices is { Count: <= 0 })
                return;
            
            if (CurrentPatterns is { Count: <= 0 })
                return;
            
            foreach (var (index, pattern) in CurrentPatterns)
            {
                Console.WriteLine(index);
                Console.WriteLine(pattern);
                
                new TcpBuilder().Start(OpCodes.SendCommand).Write(new Command
                {
                    Index = index,
                    Motor = pattern.Motor,
                    Speed = new Random().NextDouble()
                }).End();
            }
            
            if (State.Scene.Scene is not (GameScene.Play or GameScene.TheChallenge))
                return;
            
            if (State.Scene.SceneData["Level ID"] == (object) -882)
                return;
        }
    }
}