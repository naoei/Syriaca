using System;
using Syriaca.Plugin.Bp.Utils;

namespace Syriaca.Plugin.Bp.Patterns
{
    public class Attempts : PatternHandler
    {
        public override PatternType Type => PatternType.Attempts;

        private int threshold;

        public Attempts(uint motor, bool isOpposite, int threshold) 
            : base(motor, isOpposite)
        {
            this.threshold = threshold;
        }

        public override double GetValue(BpPlugin plugin)
        {
            plugin.LevelInfo.Update();
            plugin.PlayerInfo.Update();
            
            if (plugin.LevelInfo.Id == -882)
                return 0;

            var percentage = Interpolation.ValueAt(plugin.PlayerInfo.CurrentAttempt, 0, 1, 0, threshold);

            if (IsOpposite)
                percentage = 1 - percentage;

            return percentage;
        }
    }
}