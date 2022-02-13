using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Syriaca.Plugin.Bp.Patterns;
using Syriaca.Plugin.Bp.Utils;

namespace Syriaca.Plugin.Bp.Tcp.Handlers
{
    public class Configuration : TcpHandler
    {
        public override OpCodes OpCode => OpCodes.Configuration;

        public override void HandleData(byte[] data)
        {
            var conf = new TcpReader(data).ReadBson();
            
            if (conf.DeviceList == null)
                return;
            
            BpPlugin.Devices.AddRange(((JArray)conf.DeviceList).ToObject<dynamic[]>()!);
            
            if (conf.Patterns == null)
                return;
            
            foreach (var (index, pattern) in ((JArray)conf.Patterns).ToObject<List<(uint, dynamic)>>()!)
            {
                PatternHandler handler = (PatternType)pattern.Type switch
                {
                    PatternType.LevelProgression => new LevelProgress((uint) pattern.Motor, (bool) pattern.IsOpposite),
                    PatternType.Attempts => new Attempts((uint) pattern.Motor, (bool) pattern.IsOpposite, (int) pattern.Threshold),
                    PatternType.Height => new Height((uint) pattern.Motor, (bool) pattern.IsOpposite, (float) pattern.Multiplier),
                    _ => null
                };
                
                BpPlugin.CurrentPatterns.Add((index, handler));
            }
        }
        
        // private class Config
        // {
        //     public dynamic[] DeviceList { get; set; }
        //
        //     public List<(uint, PatternData)> Patterns { get; set; }
        //     
        //     public class PatternData
        //     {
        //         public PatternType Type { get; set; }
        //         public bool IsOpposite { get; set; }
        //         public uint Motor { get; set; }
        //         public int Threshold { get; set; }
        //         public float Multiplier { get; set; }
        //     }
        // }
    }
}