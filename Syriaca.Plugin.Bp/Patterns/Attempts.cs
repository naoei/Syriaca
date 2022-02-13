using System;

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

            var percentage = valueAt(plugin.PlayerInfo.CurrentAttempt, 0, 1, 0, threshold);

            if (IsOpposite)
                percentage = 1 - percentage;

            return percentage;
        }
        
        private double valueAt(double time, double val1, double val2, double startTime, double endTime)
        {
            if (val1 == val2)
                return val1;

            var current = time - startTime;
            var duration = endTime - startTime;

            if (current == 0)
                return val1;
            if (duration == 0)
                return val2;

            var t = current / duration;
            return val1 + t * (val2 - val1);
        }
    }
}