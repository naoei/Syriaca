using System;
using Syriaca.Plugin.Bp.Utils;

namespace Syriaca.Plugin.Bp.Patterns
{
    public class Height : PatternHandler
    {
        public override PatternType Type => PatternType.Height;

        private const int max_height = 2460;
        private const int min_height = 105;
        private readonly float multiplier;

        public Height(uint motor, bool isOpposite, float multiplier) 
            : base(motor, isOpposite)
        {
            this.multiplier = multiplier;
        }

        public override double GetValue(BpPlugin plugin)
        {
            plugin.LevelInfo.Update();
            plugin.PlayerInfo.Update();
            
            if (plugin.LevelInfo.Id == -882)
                return 0;
            
            var percentage = Interpolation.ValueAt(plugin.PlayerInfo.Y, 0, 1, min_height, max_height);

            if (IsOpposite)
                percentage = 1 - percentage;
            
            return percentage * multiplier;
        }
    }
}


/*
Jumping and hitting the circles or how fast you move vertically
Probably also when moving through
portals
and each player mode probably needs different 'feels' for jumping
like the plane would be more constant
and the spider would be more spiky
*/