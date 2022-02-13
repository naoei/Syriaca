using System;

namespace Syriaca.Plugin.Bp.Patterns
{
    public class LevelProgress : PatternHandler
    {
        public override PatternType Type => PatternType.LevelProgression;

        public LevelProgress(uint motor, bool isOpposite) 
            : base(motor, isOpposite)
        {
        }

        public override double GetValue(BpPlugin plugin)
        {
            plugin.LevelInfo.Update();
            plugin.PlayerInfo.Update();
            
            if (plugin.LevelInfo.Id == -882)
                return 0;

            var percentage = plugin.PlayerInfo.X / plugin.LevelInfo.Length;

            if (IsOpposite)
                percentage = 1 - percentage;
            
            return percentage;
        }
    }
}