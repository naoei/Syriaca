using System;
using System.Collections.Generic;
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

        public LevelInfo LevelInfo;
        public PlayerInfo PlayerInfo;

        public static readonly List<(uint, PatternHandler)> CurrentPatterns = new();

        public override string Name => "Buttplug.io";

        public override Scheduler CreateScheduler() => new(100);

        public override void OnScheduleCreated()
        {
            LevelInfo = new LevelInfo(GdReader);
            PlayerInfo = new PlayerInfo(GdReader);

            Client.Start();
            Scheduler.Add(testVibration);
        }

        private void testVibration()
        {
            if (Devices is { Count: <= 0 })
                return;
            
            if (CurrentPatterns is { Count: <= 0 })
                return;

            if (State.Scene.Scene != GameScene.Play)
                return;

            foreach (var (index, pattern) in CurrentPatterns)
            {
                var percentage = pattern.GetValue(this);
                percentage = Math.Clamp(percentage, 0, 1);

                Console.WriteLine($"T: {pattern.Type.ToString()} | P: {percentage}");

                new TcpBuilder().Start(OpCodes.SendCommand).Write(new Command
                {
                    Index = index,
                    Motor = pattern.Motor,
                    Speed = percentage
                }).End();
            }
        }
    }
}